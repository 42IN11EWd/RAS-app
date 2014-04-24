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
        // private PortController          portController;

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
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand CalibrateMinimumDistance { get; private set; }
        public RelayCommand CalibrateMaximumDistance { get; private set; }

        #endregion

        public SettingsViewModel(IApplicationController app, VideoCameraController videoCameraController = null) : base()
        {
            _app = app;

            CameraView     = new CameraViewModel(_app);
            // portController = new PortController(); 
            
            // Set videocamera settings
            this.videoCameraController = videoCameraController;
            openVideoSource(this.videoCameraController.CameraWindow);
            Devices = videoCameraController.Devices;
        }

        public void selectedCameraIndexChanged()
        {
            CameraWindow cameraWindow = videoCameraController.OpenVideoSource(selectedCameraIndex);
            if (cameraWindow == null) 
            {
                MessageBox.Show("Camera could not be found", "Not Found", MessageBoxButton.OK, MessageBoxImage.Error, 
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
            _app.ShowLoginView();

            if (commandParams[6] != null)
            {
                //
                
                // Save selected videocamera
                int cameraIndex = (int)commandParams[6];
                videoCameraSettingsModule.saveVideocameraIndex(cameraIndex);
            }
        }
        private void CancelAction(object commandParam)
        {
            _app.CloseSettingsWindow();
        }

        private void calibrateMinimumDistance(object commandParam)
        {

        }

        private void calibrateMaximumDistance(object commandParam)
        {

        }

        #endregion

        protected override void initRelayCommands()
        {
            SaveSettingsCommand         = new RelayCommand(saveSettings);
            CancelCommand               = new RelayCommand(CancelAction);
            CalibrateMinimumDistance    = new RelayCommand(calibrateMinimumDistance);
            CalibrateMaximumDistance    = new RelayCommand(calibrateMaximumDistance);
        }
    }
}
