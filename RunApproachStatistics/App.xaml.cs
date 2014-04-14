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
            LoginViewModel loginViewModel = new LoginViewModel(this);
            _setContent(loginViewModel);
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
            DialogWindow dialogWindow = new DialogWindow();
            SettingsViewModel settingsViewModel = new SettingsViewModel(this);
            settingsViewModel.Content = settingsViewModel;
            dialogWindow.DataContext = settingsViewModel;
            dialogWindow.ShowDialog();
        }

        public void ShowVaultSelectorView()
        {
            VaultSelectorViewModel vaultSelectorViewModel = new VaultSelectorViewModel(this);
            _setContent(vaultSelectorViewModel);
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
