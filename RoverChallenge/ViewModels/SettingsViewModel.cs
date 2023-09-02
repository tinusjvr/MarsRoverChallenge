using RoverChallenge.Classes;
using RoverChallenge.Enums;
using RoverChallenge.Models;
using RoverChallenge.Services;
using RoverChallenge.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RoverChallenge.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        #region Constructor
        public SettingsViewModel()
        {
            RoverCount = 2;
            MapHeight = 10;
            MapWidth = 10;

            Results = new ObservableCollection<string>();
            Intersections = new List<(int, int)>();
            tempPositions = new ObservableCollection<string>();
            CurrentView = new InstructionsView();

            MapDataStart = new ObservableCollection<string>();
            MapDataEnd = new ObservableCollection<string>();
            MapDataIntersections = new ObservableCollection<string>();
            ResultingMapData = new ObservableCollection<string>();

            InitializeCommands();
        }
        #endregion

        #region Commands
        public RelayCommand NextCommand { get; private set; }
        public RelayCommand BackCommand { get; private set; }
        public RelayCommand GetResultsCommand { get; private set; }
        public RelayCommand RestartCommand { get; private set; }
        public RelayCommand IntersectionCommand { get; private set; }
        public RelayCommand MapCommand { get; private set; }
        public RelayCommand StartMapSelectedCommand { get; private set; }
        public RelayCommand EndMapSelectedCommand { get; private set; }
        public RelayCommand IntersectionMapSelectedCommand { get; private set; }

        private void ExecuteStartMapSelectedCommand()
        {
            ResultingMapData = MapDataStart;
        }
        private void ExecuteEndMapSelectedCommand()
        {
            ResultingMapData = MapDataEnd;
        }
        private void ExecuteIntersectionMapSelectedCommand()
        {
            ResultingMapData = MapDataIntersections;
        }
        private void ExecuteMapCommand()
        {
            ResultViewContent = new ResultsViewIntersectionMapView();
        }
        private void ExecuteIntersectionCommand()
        {
            ResultViewContent = new ResultsViewIntersectionsListView();
        }
        private void ExecuteRestart()
        {
            foreach (Rover rover in Rovers)
            {
                rover.ProcessedInstructions = false;
                rover.PositionX = 0;
                rover.PositionY = 0;
                rover.StartingX = 0;
                rover.StartingY = 0;
                rover.Instructions = string.Empty;
                rover.Path.Clear();
            }
            Results.Clear();
            tempPositions.Clear();
            MapDataStart.Clear();
            MapDataEnd.Clear();
            MapDataIntersections.Clear();
            ResultingMapData.Clear();
            Intersections.Clear();
            ProcessedResults = false;
        }
        private void ExecuteBack()
        {
            //Make sure there are at least 2 rovers present..
            RoverCount = 2;
            NavigationServices.Instance.NavigateToPreviousPage();
        }
        public void ExecuteGetResults()
        {
            if (ProcessedResults == false)
            {
                CurrentView = new ResultsView();

                //Make sure the current positions are set equal to the starting positions
                foreach (Rover rover in Rovers)
                {
                    rover.PositionX = rover.StartingX;
                    rover.PositionY = rover.StartingY;
                }

                //Verify that all rovers are on the plane
                VerifyRoversStartingPositions();

                //Get map to display starting points
                MapDataStart = GetStartPositions(Rovers);

                //Carry out rover instructions
                foreach (Rover rover in Rovers)
                {
                    ProcessInstructions(rover);
                }

                //Get intersections - this is a list of all the intersections where paths crossed, not time dependent.
                var AllActions = Rovers.SelectMany(x => x.Path);

                //Get intersections
                Intersections = AllActions
                                .GroupBy(str => str)
                                .Where(group => group.Count() > 1)
                                .Select(group => group.Key)
                                .ToList();

                //Populate map displaying end positions.
                MapDataEnd = GetEndPositions(Rovers);

                //Populate map displaying intersections
                MapDataIntersections = GetIntersections(Rovers);

                //Populate intersections in result list
                int IntersetionCount = 0;
                foreach (var item in Intersections)
                {
                    IntersetionCount++;

                    string temp = $" Intersection {IntersetionCount}  detected at: [ X - {item.Item1}" + "," + " Y - " + $"{item.Item2} ]";

                    Results.Add(temp);
                }
                ProcessedResults = true;
            }
        }
        public void ExecuteNext()
        {
            Rovers = new ObservableCollection<Rover>();

            for (int i = 0; i < RoverCount; i++)
            {
                Rover rover = new Rover()
                {
                    Instructions = string.Empty,
                    PositionX = 0,
                    PositionY = 0,
                    Index = i,
                };

                Rovers.Add(rover);

            }

            NavigationServices.Instance.NavigateToPage(ApplicationViews.RoverSettingsView);
        }
        private void InitializeCommands()
        {
            GetResultsCommand = new RelayCommand(execute => ExecuteGetResults());
            NextCommand = new RelayCommand(execute => ExecuteNext());
            BackCommand = new RelayCommand(exeucte => ExecuteBack());
            RestartCommand = new RelayCommand(execute => ExecuteRestart());
            MapCommand = new RelayCommand(execute => ExecuteMapCommand());
            IntersectionCommand = new RelayCommand(execute => ExecuteIntersectionCommand());
            StartMapSelectedCommand = new RelayCommand(execute => ExecuteStartMapSelectedCommand());
            EndMapSelectedCommand = new RelayCommand(execute => ExecuteEndMapSelectedCommand());
            IntersectionMapSelectedCommand = new RelayCommand(execute => ExecuteIntersectionMapSelectedCommand());
        }
        #endregion

        #region Helper Methods
        private bool VerifyRoversStartingPositions()
        {
            bool result = true;

            //Check x minimum
            if (Rovers.Any(x => x.PositionX < 0))
            {
                result = false;
            }
            //Check x maximum
            if (Rovers.Any(x => x.PositionX > MapWidth))
            {
                result = false;
            }
            //Check y minimum
            if (Rovers.Any(x => x.PositionY < 0))
            {
                result = false;
            }
            if (Rovers.Any(x => x.PositionY > MapHeight))
            {
                result = false;
            }
            return result;
        }
        private void VerifyRoversEndPositions(Rover rover)
        {
            //Must still be on plane
            if (rover.PositionX > MapWidth || rover.PositionX < 0)
            {
                string message = $"Rover {rover.Index} drifted off map.";
                MessageBox.Show(message);
            }
            if (rover.PositionY > MapHeight || rover.PositionY < 0)
            {
                string message = $"Rover {rover.Index} drifted off map.";
                MessageBox.Show(message);
            }
        }
        private void MoveRover(Rover Rover, char direction)
        {
            switch (direction)
            {
                case 'N':
                    Rover.PositionY++;
                    break;
                case 'S':
                    Rover.PositionY--;
                    break;
                case 'E':
                    Rover.PositionX++;
                    break;
                case 'W':
                    Rover.PositionX--;
                    break;

                default:
                    throw new System.Exception();
            }

        }
        private void ProcessInstructions(Rover Rover)
        {
            foreach (char instruction in Rover.Instructions)
            {
                MoveRover(Rover, instruction);
                Rover.Path.Add((Rover.PositionX, Rover.PositionY));
                Rover.Coordinates.Add($"{Rover.PositionX}" + $"{Rover.PositionY}");
            }
        }

        public ObservableCollection<string> GetIntersections(ObservableCollection<Rover> rovers)
        {
            ObservableCollection<string> map = new ObservableCollection<string>();

            // Create a 2D array to represent the map
            char[,] grid = new char[MapWidth + 1, MapHeight + 1];

            // Initialize the grid with empty spaces
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    grid[x, y] = '.';
                }
            }

            //Check for intersections with each path in a rovers path list
            foreach (var rover in rovers)
            {
                foreach (var coordinate in rover.Path)
                {
                    if (Intersections.Contains(coordinate))
                    {
                        grid[coordinate.Item1, coordinate.Item2] = '*';
                    }
                }
            }

            for (int y = MapHeight - 1; y >= 0; y--)
            {
                string row = "";

                for (int x = 0; x <= MapWidth - 1; x++)
                {
                    row += grid[x, y];
                    row += "        ";
                }

                map.Add(row);
            }

            return map;
        }
        public ObservableCollection<string> GetEndPositions(ObservableCollection<Rover> rovers)
        {
            ObservableCollection<string> EndingMap = new ObservableCollection<string>();

            // Create a 2D array to represent the map
            char[,] grid = new char[MapWidth + 1, MapHeight + 1];

            // Initialize the grid with empty spaces
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    grid[x, y] = '.';
                }
            }

            foreach (Rover rover in rovers)
            {
                grid[rover.PositionX, rover.PositionY] = '0';
            }

            for (int y = MapHeight - 1; y >= 0; y--)
            {
                string row = "";

                for (int x = 0; x <= MapWidth - 1; x++)
                {
                    row += grid[x, y];
                    row += "        ";
                }

                EndingMap.Add(row);
            }

            return EndingMap;
        }
        public ObservableCollection<string> GetStartPositions(ObservableCollection<Rover> rovers)
        {
            ObservableCollection<string> startingMap = new ObservableCollection<string>();

            // Create a 2D array to represent the map
            char[,] grid = new char[MapWidth + 1, MapHeight + 1];

            // Initialize the grid with empty spaces
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    grid[x, y] = '.';
                }
            }

            foreach (Rover rover in rovers)
            {
                grid[rover.StartingX, rover.StartingY] = 'X';
            }

            for (int y = MapHeight - 1; y >= 0; y--)
            {
                string row = "";

                for (int x = 0; x <= MapWidth - 1; x++)
                {
                    row += grid[x, y];
                    row += "        ";
                }

                startingMap.Add(row);
            }

            return startingMap;

        }
        #endregion

        #region Public Properties

        public int RoverCount { get => _roverCount; set { _roverCount = value; OnPropertyChanged(); } }
        public int InstructionExecuted { get; set; } = 0;
        public byte MapWidth { get => _mapWidth; set { _mapWidth = value; OnPropertyChanged(); } }
        public byte MapHeight { get => _mapHeight; set { _mapHeight = value; OnPropertyChanged(); } }

        public Visibility InstructionsVisibility { get => _instructionsVisibility; set { _instructionsVisibility = value; OnPropertyChanged(); } }
        public Visibility ResultsVisibility { get => _resultsVisibility; set { _resultsVisibility = value; OnPropertyChanged(); } }

        public UserControl CurrentView { get => _currentView; set { _currentView = value; OnPropertyChanged(); } }
        public UserControl ResultViewContent { get => _resultsViewContent; set { _resultsViewContent = value; OnPropertyChanged(); } }
        public ObservableCollection<Rover> Rovers { get => _rovers; set { _rovers = value; OnPropertyChanged(); } }
        public ObservableCollection<string> Results { get => _results; set { _results = value; OnPropertyChanged(); } }
        public ObservableCollection<string> tempPositions { get => _tempPositions; set { _tempPositions = value; OnPropertyChanged(); } }
        public ObservableCollection<string> MapDataStart { get => _mapDataStart; set { _mapDataStart = value; OnPropertyChanged(); } }
        public ObservableCollection<string> MapDataEnd { get => _mapDataEnd; set { _mapDataEnd = value; OnPropertyChanged(); } }
        public ObservableCollection<string> MapDataIntersections { get => _mapDataIntersections; set { _mapDataIntersections = value; OnPropertyChanged(); } }
        public ObservableCollection<string> ResultingMapData { get => _resultingMapData; set { _resultingMapData = value; OnPropertyChanged(); } }
        public List<(int, int)> Intersections { get; set; }
        #endregion

        #region Private Properties

        private ObservableCollection<string> _mapDataStart;
        private ObservableCollection<string> _mapDataIntersections;
        private ObservableCollection<string> _mapDataEnd;
        private ObservableCollection<string> _resultingMapData;

        private ObservableCollection<string> _tempPositions;
        private ObservableCollection<string> _results;
        private ObservableCollection<Rover> _rovers;

        private UserControl _currentView;
        private UserControl _resultsViewContent;

        private bool ProcessedResults;

        private string _resultsCommand;

        private int _roverCount;

        private byte _mapWidth;
        private byte _mapHeight;

        public Visibility _instructionsVisibility;
        public Visibility _resultsVisibility;
        #endregion

        #region Singleton
        private static readonly object Lock = new();
        private static SettingsViewModel? _instance;
        public static SettingsViewModel Instance
        {
            get
            {
                if (_instance == null)
                    lock (Lock)
                    {
                        if (_instance == null) _instance = new SettingsViewModel();
                    }

                return _instance;
            }
        }
        #endregion

    }
}
