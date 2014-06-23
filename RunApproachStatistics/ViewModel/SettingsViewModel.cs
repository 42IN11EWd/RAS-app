using MvvmValidation;
using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using RunApproachStatistics.Services;
using RunApproachStatistics.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
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

        private int         selectedComportIndex;
        private String[]    comports;

        private int         selectedCameraIndex;
        private String[]    devices;

        private String measurementFrequency;
        private String meanValue;
        private String measurementWindowMax;
        private String measurementWindowMin;
        private int   pilotLaser;
        private int   measurementIndex;
        private int   selectedMeasurementPositionIndex;

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

        public String[] ComPorts
        {
            get
            {
                return comports;
            }
            set 
            {
                comports = value;
                OnPropertyChanged("ComPorts");
            }
        }

        public String MeasurementFrequency
        {
            get { return measurementFrequency; }
            set 
            {
                measurementFrequency = value;
                Validator.Validate(() => MeasurementFrequency);
                OnPropertyChanged("MeasurementFrequency");
            }
        }

        public String MeanValue
        {
            get { return meanValue; }
            set 
            { 
                meanValue = value;
                Validator.Validate(() => MeanValue);
                OnPropertyChanged("MeanValue");
            }
        }

        public String MeasurementWindowMax
        {
            get { return measurementWindowMax; }
            set 
            { 
                measurementWindowMax = value;
                Validator.Validate(() => MeasurementWindowMax);
                OnPropertyChanged("MeasurementWindowMax");
            }
        }

        public String MeasurementWindowMin
        {
            get { return measurementWindowMin; }
            set 
            {
                measurementWindowMin = value;
                Validator.Validate(() => MeasurementWindowMin);
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
                measurementIndex = value;
                OnPropertyChanged("MeasurementIndex");
            }
        }

        public int SelectedMeasurementPositionIndex
        {
            get { return selectedMeasurementPositionIndex; }
            set
            {
                selectedMeasurementPositionIndex = value;
                OnPropertyChanged("SelectedMeasurementPositionIndex");
            }
        }

        public int SelectedComportIndex
        {
            get { return selectedComportIndex; }
            set
            {
                selectedComportIndex = value;
                OnPropertyChanged("SelectedComportIndex");
            }
        }

        public RelayCommand SaveSettingsCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand CalibrateMinimumDistance { get; private set; }
        public RelayCommand CalibrateMaximumDistance { get; private set; }
        public RelayCommand ShowLocationEditerCommand { get; private set; }
        public RelayCommand ShowVaultNumberEditerCommand { get; private set; }
        public RelayCommand ShowVaultKindEditorCommand { get; private set; }
        public RelayCommand ClearLocalDataCommand { get; private set; }

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

            getComPortDevices();
            setSettingsProperties();
            setValidationRules();
        }

        private void setSettingsProperties()
        {
            MeasurementFrequency = String.Format("{0:####.###}", portController.MeasurementFrequency);
            MeanValue            = String.Format("{0:####.###}", portController.MeanValue);
            MeasurementWindowMax = String.Format("{0:####.###}", portController.MeasurementWindowMax);
            MeasurementWindowMin = String.Format("{0:####.###}", portController.MeasurementWindowMin);

            if (MeasurementWindowMin.Length < 1)
            {
                MeasurementWindowMin = "0";
            }

            MeasurementIndex                 = laserCameraSettingsModule.getMeasurementIndex();
            SelectedMeasurementPositionIndex = laserCameraSettingsModule.getMeasurementPosition();

            // Get comport index from name
            String comportName   = laserCameraSettingsModule.getComPortName();
            int comportIndex     = Array.IndexOf(comports, comportName);
            comportIndex         = (comportIndex != -1) ? comportIndex : 0;
            SelectedComportIndex = comportIndex;
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

        public void getComPortDevices()
        {
            comports = SerialPort.GetPortNames();
            if (comports.Length == 0)
            {
                comports = new String[] { "No usable comports found" };
            }

            OnPropertyChanged("ComPorts");
        }

        #region Relay Commands

        private void saveSettings(object commandParam)
        {
            Object[] commandParams = (Object[]) commandParam;

            if (IsValid)
            {
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
                    // 0: Frequency
                    // 1: Meanvalue
                    // 2: camera position
                    // 3: Measurement index
                    // 4: Measurement window min
                    // 5: Measurement window max
                    // 6: videocamera index
                    // 7: comport index
                    portController.writeSettings(commandParams);

                    // save measurement position
                    int measurePosition = Convert.ToInt32(commandParams[2]);
                    laserCameraSettingsModule.setMeasurementPosition(measurePosition);

                    // save measurement index
                    int measureIndex = Convert.ToInt32(commandParams[3]);
                    laserCameraSettingsModule.setMeasurementIndex(measureIndex);

                    // Save selected videocamera
                    int cameraIndex = Convert.ToInt32(commandParams[6]);
                    videoCameraSettingsModule.saveVideocameraIndex(cameraIndex);

                    // save comport
                    int comportIndex = Convert.ToInt32(commandParams[7]);
                    laserCameraSettingsModule.setComPortName(comports[comportIndex]);

                    _app.CloseSettingsWindow();
                }
            }
            else
            {
                String errors = "";
                IEnumerable errorList = GetErrors("");
                foreach(String error in errorList)
                {
                    errors += error + "\n";
                }
                MessageBox.Show("Not all values are valid, please check the following: \r\n " + errors, "Invalid Values", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void CancelAction(object commandParam)
        {
            _app.CloseSettingsWindow();
        }

        private void calibrateMinimumDistance(object commandParam)
        {
            MeasurementWindowMin = String.Format("{0:####.###}", portController.calibrateMeasurementWindow());
        }

        private void calibrateMaximumDistance(object commandParam)
        {
            MeasurementWindowMax = String.Format("{0:####.###}", portController.calibrateMeasurementWindow());
        }

        private void ShowVaultNumberEditor(object commandParam)
        {
            _app.ShowVaultNumberEditorView();
        }

        private void ShowVaultKindEditor(object commandParam)
        {
            _app.ShowVaultKindEditorView();
        }

        private void ShowLocationEditor(object commandParam)
        {
            _app.ShowLocationEditorView();
        }

        private void ClearLocalData(object commandParam)
        {
            MessageBoxResult result = MessageBox.Show("Warning: this action will delete all videos that are saved on this computer.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RunApproachStatistics");
                    Array.ForEach(Directory.GetFiles(filePath), File.Delete);
                    MessageBoxResult messageBox = MessageBox.Show("Files succesfully deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception e)
                {
                    MessageBoxResult messageBox = MessageBox.Show("Error deleting files!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion

        protected override void initRelayCommands()
        {
            SaveSettingsCommand         = new RelayCommand(saveSettings);
            CancelCommand               = new RelayCommand(CancelAction);
            CalibrateMinimumDistance    = new RelayCommand(calibrateMinimumDistance);
            CalibrateMaximumDistance    = new RelayCommand(calibrateMaximumDistance);

            ShowLocationEditerCommand = new RelayCommand(ShowLocationEditor);
            ShowVaultNumberEditerCommand = new RelayCommand(ShowVaultNumberEditor);
            ShowVaultKindEditorCommand = new RelayCommand(ShowVaultKindEditor);
            ClearLocalDataCommand = new RelayCommand(ClearLocalData);
        }

        #region Validation Rules
        private void setValidationRules()
        {
            Validator.AddRule(() => MeasurementFrequency,
                              () =>
                              {
                                  float mFreq = 0;
                                  if (!float.TryParse(MeasurementFrequency, out mFreq))
                                  {
                                      return RuleResult.Invalid("Value is not a number");
                                  }
                                  else
                                  {
                                      return RuleResult.Assert((mFreq <= 2000 && mFreq > 0), "Invalid value, must be between 1 and 2000");
                                  }
                              });

            Validator.AddRule(() => MeanValue,
                              () =>
                              {
                                  float mValue = 0;
                                  if (!float.TryParse(MeanValue, out mValue))
                                  {
                                      return RuleResult.Invalid("Value is not a number");
                                  }
                                  else
                                  {
                                      return RuleResult.Assert((mValue <= 10000 && mValue > 0), "Invalid value, must be between 1 and 10.000");
                                  }
                              });

            Validator.AddRule(() => MeasurementWindowMax,
                              () =>
                              {
                                  float mWindowMax = 0;
                                  if (!float.TryParse(MeasurementWindowMax, out mWindowMax))
                                  {
                                      return RuleResult.Invalid("Value is not a number");
                                  }
                                  else
                                  {
                                      return RuleResult.Assert((mWindowMax <= 5000 && mWindowMax > 0), "Invalid value, must be between 1 and 10.000");
                                  }
                              });

            Validator.AddRule(() => MeasurementWindowMin,
                              () =>
                              {
                                  float mWindowMin = 0;
                                  if (!float.TryParse(MeasurementWindowMin, out mWindowMin))
                                  {
                                      return RuleResult.Invalid("Value is not a number");
                                  }
                                  else
                                  {
                                      return RuleResult.Assert((mWindowMin <= 5000 && mWindowMin > 0), "Invalid value, must be between 1 and 10.000");
                                  }
                              });

            Validator.AddRule(() => MeasurementWindowMax,
                              () => MeasurementWindowMin,
                              () =>
                              {
                                  return RuleResult.Assert(!MeasurementWindowMin.Equals(MeasurementWindowMax), "Values can't be equal");
                              });
        }
        #endregion
    }
}
