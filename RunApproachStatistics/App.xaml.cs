using RunApproachStatistics.Controllers;
using RunApproachStatistics.View;
using RunApproachStatistics.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RunApproachStatistics
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IApplicationController
    {
        // Fields
        private MainWindow mainWindow;
        private AbstractViewModel _currentViewModel;
        private MainViewModel mainViewModel;
        LoginDialog loginWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Add a function to show the first screen
            ShowMainScreen();
            ShowHomeView();
        }

        public void ShowMainScreen()
        {
            mainWindow = new MainWindow();
            mainViewModel = new MainViewModel(this);
            mainWindow.DataContext = mainViewModel;
            mainWindow.Show();
        }

        public void ShowHomeView()
        {
            HomeViewModel homeViewModel = new HomeViewModel(this);
            _setContent(homeViewModel);
        }

        public void ShowCameraView()
        {
            CameraViewModel cameraViewModel = new CameraViewModel(this);
            _setContent(cameraViewModel);
        }

        public void ShowCompareVaultsView()
        {
            CompareVaultsViewModel compareVaultsViewModel = new CompareVaultsViewModel(this);
            _setContent(compareVaultsViewModel);
        }

        public void ShowLoginView()
        {
            loginWindow = new LoginDialog();
            LoginViewModel loginViewModel = new LoginViewModel(this);
            loginViewModel.Content = loginViewModel;
            loginWindow.DataContext = loginViewModel;
            loginViewModel.PasswordBox = loginWindow.PasswordBox;
            loginWindow.ShowDialog();
        }

        public void ShowMeasurementView()
        {
            MeasurementViewModel measurementViewModel = new MeasurementViewModel(this);
            _setContent(measurementViewModel);
        }

        public void ShowPostMeasurementView()
        {
            PostMeasurementViewModel postMeasurementViewModel = new PostMeasurementViewModel(this);
            _setContent(postMeasurementViewModel);
        }

        public void ShowProfileView()
        {
            ProfileViewModel profileViewModel = new ProfileViewModel(this);
            _setContent(profileViewModel);
        }

        public void ShowSettingsView()
        {
            SettingsViewModel settingsViewModel = null;
            // If current view is HomeView then get the VideoCameraController from HomeView 
            // and pass it on to SettingsView
            if (_currentViewModel.GetType() == typeof(HomeViewModel))
            {
                HomeViewModel homeViewModel = (HomeViewModel)_currentViewModel;
                homeViewModel.pauseVideoSource(true);
                settingsViewModel = new SettingsViewModel(this, homeViewModel.VideoCameraController);
            }
            else
            {
                settingsViewModel = new SettingsViewModel(this);
            }

            DialogWindow dialogWindow = new DialogWindow();
            settingsViewModel.Content = settingsViewModel;
            dialogWindow.DataContext = settingsViewModel;
            dialogWindow.Closed += settingsWindow_Closed;
            dialogWindow.ShowDialog();
        }

        private void settingsWindow_Closed(object sender, EventArgs e)
        {
            HomeViewModel homeViewModel = (HomeViewModel)_currentViewModel;
            homeViewModel.pauseVideoSource(false);
        }

        public void ShowVaultSelectorView()
        {
            VaultSelectorViewModel vaultSelectorViewModel = new VaultSelectorViewModel(this);
            _setContent(vaultSelectorViewModel);
        }

        public void CloseLoginWindow()
        {
            if(loginWindow != null)
            {
                loginWindow.Close();
            }
        }
        private void _setContent(AbstractViewModel viewModel)
        {
            if (_currentViewModel != viewModel)
            {
                mainViewModel.Content = viewModel;
                _currentViewModel = viewModel;
            }
        }
    }
}
