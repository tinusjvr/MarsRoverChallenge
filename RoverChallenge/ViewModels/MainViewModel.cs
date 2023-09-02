using RoverChallenge.Classes;
using RoverChallenge.Enums;
using System.Windows;

namespace RoverChallenge.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            InitializeCommands();

            NavigationService.NavigateToPage(ApplicationViews.None);
            NavigationService.NavigateToPage(ApplicationViews.SettingsView);
        }

        #region Public Properties
        public WindowState Fullscreen
        {
            get => _fullscreen;
            set { _fullscreen = value; OnPropertyChanged(); }
        }

        #endregion

        #region Private Properties
        private WindowState _fullscreen;
        #endregion

        #region Commands
        public RelayCommand CloseApplication { get; private set; }
        public RelayCommand MinimizeApplication { get; private set; }
        public RelayCommand NavigateToViewCommand { get; private set; }
        private void ExecuteNavigateToViewCommand(ApplicationViews view)
        {
            NavigationService.NavigateToPage(view);
        }
        private void ExecuteCloseApplication()
        {
            Application.Current.Shutdown();
        }
        private void ExecuteMinimizeApplication()
        {
            Fullscreen = (Fullscreen == WindowState.Minimized) ? Fullscreen : WindowState.Minimized;
        }
        private void InitializeCommands()
        {
            CloseApplication = new RelayCommand(execute => ExecuteCloseApplication());
            MinimizeApplication = new RelayCommand(execute => ExecuteMinimizeApplication());
            NavigateToViewCommand = new RelayCommand(execute => ExecuteNavigateToViewCommand((ApplicationViews)execute), canExecute => true);

        }
        #endregion

    }
}
