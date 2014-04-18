using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using RunApproachStatistics.Services;
using RunApproachStatistics.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.Integration;

namespace RunApproachStatistics.ViewModel
{
    public class SettingsViewModel : AbstractViewModel
    {
        private IApplicationController  _app;
        private PropertyChangedBase     content;

        private CameraViewModel         cameraView;
        private VideoCameraController   videoCameraController;
        private ReadPort                readPort;
        private WritePort               writePort;

        private int         selectedCameraIndex;
        private String[]    devices;

        #region Modules

        private IVideoCameraSettingsModule videoCameraSettingsModule = new SettingsModule();
        private ILaserCameraSettingsModule laserCameraSettingsModule = new SettingsModule();

        #endregion

        #region DataBinding

        public PropertyChangedBase Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged("Content");
            }
        }

        public CameraViewModel CameraView
        {
            get { return cameraView; }
            set
            {
                cameraView = value;
                OnPropertyChanged("CameraView");
            }
        }

        public int SelectedCameraIndex
        {
            get { return selectedCameraIndex; }
            set
            {
                if (value > -1)
                {
                    selectedCameraIndex = value;
                    OnPropertyChanged("SelectedCameraIndex");
                    selectedCameraIndexChanged();
                }
            }
        }

        public String[] Devices
        {
            get { return devices; }
            set
            {
                devices = value;
                OnPropertyChanged("Devices");
            }
        }


        public RelayCommand SaveSettingsCommand { get; private set; }

        #endregion

        public SettingsViewModel(IApplicationController app, VideoCameraController videoCameraController = null) : base()
        {
            _app = app;

            CameraView = new CameraViewModel(_app);
            readPort  = new ReadPort();
            writePort = new WritePort(); 
            
            // Set videocamera settings
            if (videoCameraController != null)
            {
                this.videoCameraController = videoCameraController;
                openVideoSource(this.videoCameraController.CameraWindow);
            } 
            else
            {
                videoCameraController = new VideoCameraController();

                Devices = videoCameraController.Devices;
                if (devices.Length == 0)
                {
                    MessageBox.Show("Er zijn geen camera's gevonden die aangesloten zijn op de computer", "Niet gevonden",
                        MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                }
                else
                {
                    SelectedCameraIndex = videoCameraSettingsModule.getVideocameraIndex();
                }
            }
        }

        public void selectedCameraIndexChanged()
        {
            CameraWindow cameraWindow = videoCameraController.OpenVideoSource(selectedCameraIndex);
            if (cameraWindow == null) 
            {
                MessageBox.Show("Camera is niet gevonden", "Niet gevonden", MessageBoxButton.OK, MessageBoxImage.Error, 
                    MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                openVideoSource(cameraWindow);
            }
        }

        public void openVideoSource(CameraWindow cameraWindow)
        {
            WindowsFormsHost host = new WindowsFormsHost();
            host.Child = cameraWindow;
            cameraView.CameraHost = host;
        }

        #region Relay Commands

        private void saveSettings(object commandParam)
        {
            Object[] commandParams = (Object[]) commandParam;


            if (commandParams[6] != null)
            {
                int cameraIndex = (int)commandParams[6];
                videoCameraSettingsModule.saveVideocameraIndex(cameraIndex);
            }
        }

        #endregion

        protected override void initRelayCommands()
        {
            SaveSettingsCommand = new RelayCommand(saveSettings);
        }
    }
}
