﻿using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.Services;
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
        private AbstractViewModel currentViewModel;
        private MainViewModel mainViewModel;
        private LoginDialog loginWindow;
        private DialogWindow settingsWindow;
        private PortController portController;

        private VideoCameraController videoCameraController;
        private IVideoCameraSettingsModule videoCameraSettingsModule = new SettingsModule();

        private Boolean isLocked;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // init VideoCameraController
            videoCameraController = new VideoCameraController();
            int videoCameraIndex = videoCameraSettingsModule.getVideocameraIndex();
            videoCameraController.OpenVideoSource(videoCameraIndex);

            // set portcontroller
            portController = new PortController();

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
            HomeViewModel homeViewModel = new HomeViewModel(this, videoCameraController);
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
            SettingsViewModel settingsViewModel = new SettingsViewModel(this, portController, videoCameraController);

            settingsWindow = new DialogWindow();
            settingsViewModel.Content = settingsViewModel;
            settingsWindow.DataContext = settingsViewModel;
            settingsWindow.ShowDialog();
        }

        private void settingsWindow_Closed(object sender, EventArgs e)
        {
            HomeViewModel homeViewModel = (HomeViewModel)currentViewModel;
            homeViewModel.pauseVideoSource(false);
        }

        public void ShowVaultSelectorView()
        {
            VaultSelectorViewModel vaultSelectorViewModel = new VaultSelectorViewModel(this);
            _setContent(vaultSelectorViewModel);
        }

        public void ShowLocationEditorView()
        {
            if (settingsWindow == null)
            {
                settingsWindow = new DialogWindow();
            }
            LocationEditorViewModel locationEditorViewModel = new LocationEditorViewModel(this);
            locationEditorViewModel.Content = locationEditorViewModel;
            settingsWindow.DataContext = locationEditorViewModel;
            settingsWindow.Content = locationEditorViewModel;
        }

        public void ShowVaultNumberEditorView()
        {
            if (settingsWindow == null)
            {
                settingsWindow = new DialogWindow();
            }
            VaultNumberEditorViewModel vaultNumberEditorViewModel = new VaultNumberEditorViewModel(this);
            vaultNumberEditorViewModel.Content = vaultNumberEditorViewModel;
            settingsWindow.DataContext = vaultNumberEditorViewModel;
            settingsWindow.Content = vaultNumberEditorViewModel;
        }

        public void ToggleLockScreen()
        {
            if (!isLocked)
            {
                LockViewModel lockViewModel = new LockViewModel(this);
                mainViewModel.Content = lockViewModel;
            }
            else
            {
                mainViewModel.Content = currentViewModel;
            }

            isLocked = !isLocked;
        }

        public void CloseLoginWindow()
        {
            if(loginWindow != null)
            {
                loginWindow.Close();
                loginWindow = null;
            }
        }

        public void CloseSettingsWindow()
        {
            if(settingsWindow != null)
            {
                settingsWindow.Close();
                settingsWindow = null;
            }
        }

        private void _setContent(AbstractViewModel viewModel)
        {
            if (currentViewModel != viewModel)
            {
                mainViewModel.Content = viewModel;
                currentViewModel = viewModel;
            }
        }
    }
}
