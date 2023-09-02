using RoverChallenge.Classes;
using System.Collections.Generic;

namespace RoverChallenge.Models
{
    public class Rover : ObservableObject
    {
        public Rover()
        {
            PositionX = 0;
            PositionY = 0;
            StartingX = 0;
            StartingY = 0;
            Instructions = string.Empty;
            Index = 0;
        }
        #region Public Properties
        public int Index { get; set; }
        public int StartingX { get => _startingX; set { _startingX = value; OnPropertyChanged(); } }
        public int StartingY { get => _startingY; set { _startingY = value; OnPropertyChanged(); } }
        public int PositionX { get => _positionX; set { _positionX = value; OnPropertyChanged(); } }
        public int PositionY { get => _positionY; set { _positionY = value; OnPropertyChanged(); } }

        public HashSet<(int, int)> Path { get; set; } = new HashSet<(int, int)>();
        public string Instructions { get => _instructions; set { _instructions = value; OnPropertyChanged(); } }
        public List<string> Coordinates { get; set; } = new List<string>();
        public List<char> Map { get; set; } = new List<char>();
        public bool ProcessedInstructions { get; set; } = false;
        #endregion

        #region Private Properties
        private int _positionX;
        private int _positionY;
        private int _startingX;
        private int _startingY;
        private string _instructions;
        #endregion
    }
}
