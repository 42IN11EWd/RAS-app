﻿using RunApproachStatistics.Controllers;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.Services;
using RunApproachStatistics.View;
using RunApproachStatistics.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

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
        public static volatile PortController portController;

        private VideoCameraController videoCameraController;
        private IVideoCameraSettingsModule videoCameraSettingsModule = new SettingsModule();

        private Boolean isLocked;
        private Boolean isLoggedIn;
        private Boolean isLoginWindowOpen;

        public Boolean IsLoggedIn
        {
            get { return isLoggedIn; }
            set
            {
                isLoggedIn = value;

                if (currentViewModel.Menu != null)
                {
                    MenuViewModel menu = (MenuViewModel)currentViewModel.Menu;
                    if (isLoggedIn)
                    {
                        menu.LogName = "Logout";
                    }
                    else
                    {
                        menu.LogName = "Login";
                    }
                }
            }
        }

        public Boolean IsLoginWindowOpen
        {
            get { return isLoginWindowOpen; }
            set { isLoginWindowOpen = value; }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // check connectivity with internet
            CheckNetworkConnection();

            // init VideoCameraController
            videoCameraController = new VideoCameraController();
            int videoCameraIndex = videoCameraSettingsModule.getVideocameraIndex();
            videoCameraController.OpenVideoSource(videoCameraIndex);

            // set portcontroller
            Thread portThread = new Thread(() => { portController = new PortController(); });
            portThread.Start();
            portThread.Join();

            // Pre-load entity (workaround?)
            Thread entityPreLoadThread = new Thread(() => { new EditorModule().readLocations(); });
            entityPreLoadThread.Start();
            entityPreLoadThread.Join();

            //Add a function to show the first screen
            ShowMainScreen();
            ShowHomeView();

            isLoggedIn = false;
            isLoginWindowOpen = false;

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
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

        public void ShowCompareVaultsView(List<vault> vaults)
        {
            CompareVaultsViewModel compareVaultsViewModel = new CompareVaultsViewModel(this);
            _setContent(compareVaultsViewModel);
            compareVaultsViewModel.setVaultsToCompare(vaults);
        }

        public void ShowLoginView()
        {
            isLoginWindowOpen = true;

            loginWindow = new LoginDialog();
            LoginViewModel loginViewModel = new LoginViewModel(this);
            loginViewModel.Content = loginViewModel;
            loginWindow.DataContext = loginViewModel;
            loginWindow.Closed += CloseLoginWindow;
            loginViewModel.PasswordBox = loginWindow.PasswordBox;
            loginWindow.ShowDialog();
        }

        public void ShowMeasurementView()
        {
            MeasurementViewModel measurementViewModel = new MeasurementViewModel(this, portController, videoCameraController);
            _setContent(measurementViewModel);
        }

        public void ShowPostMeasurementView()
        {
            PostMeasurementViewModel postMeasurementViewModel = new PostMeasurementViewModel(this);
            ModifyVaultViewModel modifyVaultViewModel = new ModifyVaultViewModel(this,"POST");
            _setContent(postMeasurementViewModel);
            postMeasurementViewModel.Content = modifyVaultViewModel;
        }

        public void ShowProfileView()
        {
            ProfileViewModel profileViewModel = new ProfileViewModel(this);
            _setContent(profileViewModel);
        }

        public void ShowSettingsView()
        {
            SettingsViewModel settingsViewModel = new SettingsViewModel(this, portController, videoCameraController);

            if (settingsWindow == null)
            {
                settingsWindow = new DialogWindow();
            }
            settingsViewModel.Content = settingsViewModel;
            settingsWindow.DataContext = settingsViewModel;
            settingsWindow.Content = settingsViewModel;
            settingsWindow.Closed += settingsWindow_Closed;
            
            if (!settingsWindow.IsVisible)
            {
                settingsWindow.ShowDialog();
            }
        }

        private void settingsWindow_Closed(object sender, EventArgs e)
        {
            CloseSettingsWindow(true);
        }

        public void ShowVaultSelectorView()
        {
            VaultSelectorViewModel vaultSelectorViewModel = new VaultSelectorViewModel(this);
            //ModifyVaultViewModel modifyVaultViewModel = new ModifyVaultViewModel(this, "SELECT");
            _setContent(vaultSelectorViewModel);
            //vaultSelectorViewModel.Content = modifyVaultViewModel;
        }

        public void ShowVaultSelectorView(gymnast gymnast)
        {
            VaultSelectorViewModel vaultSelectorViewModel = new VaultSelectorViewModel(this);
            ModifyVaultViewModel modifyVaultViewModel = new ModifyVaultViewModel(this, "SELECT");
            //modifyVaultViewModel.setFilter(gymnast);
            _setContent(vaultSelectorViewModel);
            vaultSelectorViewModel.Content = modifyVaultViewModel;
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

        public void ShowVaultKindEditorView()
        {
            if (settingsWindow == null)
            {
                settingsWindow = new DialogWindow();
            }
            VaultKindEditorViewModel vaultKindEditorViewModel = new VaultKindEditorViewModel(this);
            vaultKindEditorViewModel.Content = vaultKindEditorViewModel;
            settingsWindow.DataContext = vaultKindEditorViewModel;
            settingsWindow.Content = vaultKindEditorViewModel;
        }

        public void ShowVideoPlaybackView(vault selectedVault)
        {
            VideoPlaybackViewModel videoPlaybackViewModel = new VideoPlaybackViewModel(this);
            _setContent(videoPlaybackViewModel);
            videoPlaybackViewModel.setVaultToPlay(selectedVault);
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

        public void CloseLoginWindow(object sender = null, EventArgs e = null)
        {
            if (loginWindow != null)
            {
                isLoginWindowOpen = false;
                if (sender == null)
                {
                    loginWindow.Close();
                    loginWindow = null;
                }
            }
        }

        public void CloseSettingsWindow(Boolean isClosed = false)
        {
            if(settingsWindow != null)
            {
                if(!isClosed)
                {
                    settingsWindow.Close();
                }
                settingsWindow = null;

                if(currentViewModel.GetType() == typeof(HomeViewModel))
                {
                    HomeViewModel homeviewModel = (HomeViewModel)currentViewModel;
                    homeviewModel.VideoCameraController = videoCameraController;
                }
            }
        }

        public void CheckNetworkConnection()
        {
            bool isConnected = true;

            try
            {
                if (new System.Net.NetworkInformation.Ping().Send("www.google.com").Status != IPStatus.Success)
                    isConnected = false;
            }
            catch (PingException pe)
            {
                isConnected = false;
            }
            finally
            {
                if (!isConnected)
                {
                    MessageBoxResult result = MessageBox.Show("No active network connection could be found. The application will shut down.\n\nPlease connect to the internet then restart the application.", "Not connected to the internet", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    if (result == MessageBoxResult.OK)
                    {
                        Environment.Exit(0);
                    }
                }
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
