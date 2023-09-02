using RoverChallenge.Classes;
using RoverChallenge.Services;

namespace RoverChallenge.ViewModels
{
    public class BaseViewModel : ObservableObject
    {

        public BaseViewModel()
        {
            //Get singleton instances
            NavigationService = NavigationServices.Instance;
            Settings = SettingsViewModel.Instance;
        }

        #region Public Properties
        public SettingsViewModel Settings { get; set; }
        public NavigationServices NavigationService { get; set; }
        #endregion
    }
}
