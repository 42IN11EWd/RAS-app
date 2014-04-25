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
        private PortController          portController;

        private int         selectedCameraIndex;
        private String[]    devices;

        private float measurementFrequency;
        private float meanValue;
        private float measurementWindowMax;
        private float measurementWindowMin;
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

        public float MeasurementFrequency
        {
            get { return measurementFrequency; }
            set 
            {
                measurementFrequency = value;
                OnPropertyChanged("MeasurementFrequency");
            }
        }

        public float MeanValue
        {
            get { return meanValue; }
            set 
            { 
                meanValue = value;
                OnPropertyChanged("MeasurementFrequency");
            }
        }

        public float MeasurementWindowMax
        {
            get { return measurementWindowMax; }
            set 
            { 
                measurementWindowMax = value;
                OnPropertyChanged("MeasurementWindowMax");
            }
        }

        public float MeasurementWindowMin
        {
            get { return measurementWindowMin; }
            set 
            {
                measurementWindowMin = value;
                OnPropertyChanged("MeasurementWindowMin");
            }
        }

        public int PilotLaser
        {
            get { return pilotLaser; }
            set 
            { 
                pilotLaser = value;
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
        }

        private void setSettingsProperties()
        {
            MeasurementFrequency = portController.MeasurementFrequency;
            MeanValue            = portController.MeanValue;
            MeasurementWindowMax = portController.MeasurementWindowMax;
            MeasurementWindowMin = portController.MeasurementWindowMin;

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
            _app.ShowLoginView();

            if (commandParams[6] != null)
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
                int measureIndex = (int)commandParams[3];
                laserCameraSettingsModule.setMeasurementIndex(measureIndex);
                
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
