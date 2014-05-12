using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using RunApproachStatistics.Services;
using RunApproachStatistics.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.Integration;

namespace RunApproachStatistics.ViewModel
{
    public class SettingsViewModel : ValidationViewModel
    {
        private IApplicationController  _app;
        private PropertyChangedBase     content;

        private CameraViewModel         cameraView;
        private VideoCameraController   videoCameraController;
        private PortController          portController;

        private int         selectedCameraIndex;
        private String[]    devices;

        private String measurementFrequency;
        private String meanValue;
        private String measurementWindowMax;
        private String measurementWindowMin;
        private int   pilotLaser;
        private int   measurementIndex;

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

        [Required]
        [Range(1, 2000, ErrorMessage="Invalid Frequency, must be between 1 and 2000")]
        public String MeasurementFrequency
        {
            get { return measurementFrequency; }
            set 
            {
                measurementFrequency = value;
                ValidateProperty(value);
                OnPropertyChanged("MeasurementFrequency");
            }
        }

        [Required]
        [Range(1, 10000, ErrorMessage="Invalid Mean Value, must be between 1 and 10.000")]
        public String MeanValue
        {
            get { return meanValue; }
            set 
            { 
                meanValue = value;
                ValidateProperty(value);
                OnPropertyChanged("MeanValue");
            }
        }

        [Required]
        [Range(0, 5000, ErrorMessage = "Number is outside laser range, must be between 1 and 5000")]
        public String MeasurementWindowMax
        {
            get { return measurementWindowMax; }
            set 
            { 
                measurementWindowMax = value;
                ValidateProperty(value);
                OnPropertyChanged("MeasurementWindowMax");
            }
        }

        [Required]
        [Range(0, 5000, ErrorMessage = "Number is outside laser range, must be between 1 and 5000")]
        public String MeasurementWindowMin
        {
            get { return measurementWindowMin; }
            set 
            {
                measurementWindowMin = value;
                ValidateProperty(value);
                OnPropertyChanged("MeasurementWindowMin");
            }
        }

        public int PilotLaser
        {
            get { return pilotLaser; }
            set 
            { 
                pilotLaser = value;
                portController.PilotLaser = value;
                OnPropertyChanged("PilotLaser");
            }
        }

        public int MeasurementIndex
        {
            get { return measurementIndex; }
            set
            {
                pilotLaser = value;
                OnPropertyChanged("MeasurementIndex");
            }
        }

        public RelayCommand SaveSettingsCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand CalibrateMinimumDistance { get; private set; }
        public RelayCommand CalibrateMaximumDistance { get; private set; }

        public RelayCommand ShowLocationEditerCommand { get; private set; }
        public RelayCommand ShowVaultNumberEditerCommand { get; private set; }

        #endregion

        public SettingsViewModel(IApplicationController app, 
            PortController portController,
            VideoCameraController videoCameraController = null) : base()
        {
            _app = app;

            CameraView     = new CameraViewModel(_app);
            
            // Set PortController
            this.portController = portController;

            // Set videocamera settings
            this.videoCameraController = videoCameraController;
            openVideoSource(this.videoCameraController.CameraWindow);
            Devices = videoCameraController.Devices;

            setSettingsProperties();
        }

        private void setSettingsProperties()
        {
            MeasurementFrequency = String.Format("{0:####.###}", portController.MeasurementFrequency);
            MeanValue            = String.Format("{0:####.###}", portController.MeanValue);
            MeasurementWindowMax = String.Format("{0:####.###}", portController.MeasurementWindowMax);
            MeasurementWindowMin = String.Format("{0:####.###}", portController.MeasurementWindowMin);

            MeasurementIndex     = laserCameraSettingsModule.getMeasurementIndex();
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

            if (!_app.IsLoggedIn)
            {
                _app.ShowLoginView();
            }

            while (_app.IsLoginWindowOpen)
            {
                //wait for closing of the login window
            }

            if (commandParams[6] != null && _app.IsLoggedIn)
            {
                if (IsValid)
                {
                    // 0: Frequency
                    // 1: Meanvalue
                    // 2: camera position
                    // 3: Measurement index
                    // 4: Measurement window min
                    // 5: Measurement window max
                    // 6: videocamera index
                    portController.writeSettings(commandParams);

                    // save measurement index
                    int measureIndex = Convert.ToInt32(commandParams[3]);
                    laserCameraSettingsModule.setMeasurementIndex(measureIndex);

                    // Save selected videocamera
                    int cameraIndex = Convert.ToInt32(commandParams[6]);
                    videoCameraSettingsModule.saveVideocameraIndex(cameraIndex);

                    _app.CloseSettingsWindow();
                } 
                else
                {
                    MessageBox.Show("Not all values are valid, please check them", "Invalid Values", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }
        private void CancelAction(object commandParam)
        {
            _app.CloseSettingsWindow();
        }

        private void calibrateMinimumDistance(object commandParam)
        {
            MeasurementWindowMin = String.Format("{0:0000.000}", portController.calibrateMeasurementWindow());
        }

        private void calibrateMaximumDistance(object commandParam)
        {
            MeasurementWindowMax = String.Format("{0:0000.000}", portController.calibrateMeasurementWindow());
        }

        private void ShowVaultNumberEditer(object commandParam)
        {
            _app.ShowVaultNumberEditorView();
        }
        private void ShowLocationEditer(object commandParam)
        {
            _app.ShowLocationEditorView();
        }

        #endregion

        protected override void initRelayCommands()
        {
            SaveSettingsCommand         = new RelayCommand(saveSettings);
            CancelCommand               = new RelayCommand(CancelAction);
            CalibrateMinimumDistance    = new RelayCommand(calibrateMinimumDistance);
            CalibrateMaximumDistance    = new RelayCommand(calibrateMaximumDistance);

            ShowLocationEditerCommand = new RelayCommand(ShowLocationEditer);
            ShowVaultNumberEditerCommand = new RelayCommand(ShowVaultNumberEditer);
        }
    }
}
