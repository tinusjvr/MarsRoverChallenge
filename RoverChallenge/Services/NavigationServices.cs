using RoverChallenge.Classes;
using RoverChallenge.Enums;
using RoverChallenge.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace RoverChallenge.Services;

public class NavigationServices : ObservableObject
{
    #region Private Fields
    private UserControl? _currentViewUserControl;
    private ApplicationViews _currentView;

    #endregion

    #region Public Properties
    public ApplicationViews CurrentView { get { return _currentView; } set { _currentView = value; OnPropertyChanged(); } }
    public ApplicationViews PreviousView { get; set; }
    public UserControl? CurrentViewUserControl
    {
        get
        {
            return _currentViewUserControl;
        }
        set
        {
            _currentViewUserControl = value;
            OnPropertyChanged();
            CurrentViewUserControlChanged?.Invoke(null, EventArgs.Empty);
        }
    }
    #endregion

    #region Public Events
    public event EventHandler? CurrentViewUserControlChanged;
    #endregion

    #region Singleton Instance
    private static readonly object Lock = new();
    private static NavigationServices? _instance;
    public static NavigationServices Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    _instance ??= new NavigationServices();
                }
            }
            return _instance;
        }
    }
    #endregion

    #region Navigate To Page
    public void NavigateToPage(ApplicationViews view)
    {
        // Just return if we are already on the correct view
        if (view == CurrentView)
        {
            return;
        }
        // Navigate to the desired view
        PreviousView = CurrentView;
        CurrentView = view;
        Application.Current.Dispatcher.Invoke(delegate
        {
            UserControl? tempUserControl = CurrentViewUserControl;
            CurrentViewUserControl = GetUserControlFromView(CurrentView);
            if (tempUserControl?.DataContext is IDisposable disposableUserControlDataContext)
            {
                disposableUserControlDataContext.Dispose();
            }
        });

        return;
    }

    public void NavigateToPreviousPage()
    {
        // Just return if we are already on the correct view
        if (PreviousView == CurrentView)
        {
            return;
        }
        // Navigate to the previous vies
        ApplicationViews temp = PreviousView;
        PreviousView = CurrentView;
        CurrentView = temp;
        Application.Current.Dispatcher.Invoke(delegate
        {
            CurrentViewUserControl = GetUserControlFromView(CurrentView);
        });

        return;
    }
    #endregion

    #region Views Control
    private static UserControl? GetUserControlFromView(ApplicationViews view)
    {
        switch (view)
        {

            case ApplicationViews.SettingsView:
                return new SettingsView();

            case ApplicationViews.HelpView:
                return new HelpView();

            case ApplicationViews.MapView:
                return new HomeView();

            case ApplicationViews.RoverSettingsView:
                return new RoverSettingsView();

            case ApplicationViews.ResultsView:
                return new ResultsView();

            case ApplicationViews.InstructionsView:
                return new InstructionsView();

            case ApplicationViews.ResultContentMapData:
                return new ResultsViewIntersectionMapView();

            case ApplicationViews.ResultContentRawData:
                return new ResultsViewIntersectionsListView();


            default:
                return null;
        }
    }
    #endregion
}