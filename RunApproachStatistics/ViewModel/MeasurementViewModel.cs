using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using RunApproachStatistics.Services;
using RunApproachStatistics.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Windows.Forms.Integration;


namespace RunApproachStatistics.ViewModel
{
    public class MeasurementViewModel : ValidationViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase ratingControl;

        private String vaultKind;
        public String[] vaultKindArray;
        private String location;
        private String gymnast;
        private String vaultNumber;
        private String dscore;
        private String escore;
        private String penalty;
        private String totalscore;
        private List<String> locations;
        private List<String> gymnasts;
        private List<String> vaultNumbers;
        
        private Boolean manualModeChecked;
        private Boolean measuring;
        private String measurementButtonContent;

        private CameraViewModel cameraView;
        private CameraWindow cameraWindow;
        private VideoCameraController videoCameraController;
        private PortController portController;

        private GraphViewModel graphView;

        public VideoCameraController VideoCameraController
        {
            get { return videoCameraController; }
            set
            {
                videoCameraController = value;
                openVideoSource();
            }
        }

        #region Modules

        private IVaultModule vaultModule = new VaultModule();
        private ICameraModule cameraModule = new VaultModule();

        #endregion

        #region DataBinding

        public RelayCommand PostMeasurementCommand { get; private set; }
        public RelayCommand StartMeasurementCommand { get; private set; }

        public PropertyChangedBase RatingControl
        {
            get { return ratingControl; }
            set
            {
                ratingControl = value;
                OnPropertyChanged("RatingControl");
            }
        }

        public Boolean Measuring
        {
            get { return measuring; }
            set
            {
                measuring = value;
                if (value == true && ManualModeChecked)
                {
                    MeasurementButtonContent = "Stop Measurement";
                    videoCameraController.Capture();
                    portController.startMeasurement();
                }
                else if (value == false && ManualModeChecked) 
                {
                    MeasurementButtonContent = "Start Measurement";
                    if (videoCameraController.IsCapturing)
                    {
                        stopMeasuring();
                    }
                }
                else
                {
                    MeasurementButtonContent = "";
                }
                
                OnPropertyChanged("Measuring");
            }
        }

        public GraphViewModel GraphViewMeasurement
        {
            get { return graphView; }
            set
            {
                graphView = value;
                OnPropertyChanged("GraphViewMeasurement");
            }
        }

        public Boolean ManualModeChecked
        {
            get { return manualModeChecked; }
            set
            {
                manualModeChecked = value;
                //turn manual mode on
                Measuring = false;
                OnPropertyChanged("ManualModeChecked");
            }
        }

        public String MeasurementButtonContent
        {
            get { return measurementButtonContent; }
            set
            {
                measurementButtonContent = value;
                OnPropertyChanged("MeasurementButtonContent");
            }
        }
        public String SelectedVaultKind
        {
            get
            {
                if (vaultKind == null) return "";
                return vaultKind;
            }
            set
            {
                vaultKind = value;
                OnPropertyChanged("VaultKind");
            }

        }

        
        public String[] VaultKind
        {
            get { return vaultKindArray; }
            set
            {
                vaultKindArray = value;
                OnPropertyChanged("VaultKind");
            }
        }

        public String Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        }

        public List<String> Locations
        {
            get { return locations; }
            set
            { 
                locations = value;
                OnPropertyChanged("Locations");
            }
        }

        public String Gymnast
        {
            get { return gymnast; }
            set
            {
                gymnast = value;
                OnPropertyChanged("Gymnast");
            }
        }

        public List<String> Gymnasts
        {
            get { return gymnasts; }
            set
            {
                gymnasts = value;
                OnPropertyChanged("Gymnasts");
            }
        }

        public String VaultNumber
        {
            get { return vaultNumber; }
            set
            {
                vaultNumber = value;
                OnPropertyChanged("VaultNumber");
            }
        }
        public List<String> VaultNumbers
        {
            get { return vaultNumbers; }
            set
            {
                vaultNumbers = value;
                OnPropertyChanged("VaultNumbers");
            }
        }


        [RegularExpression(@"^[0-9]{1,2}\.?[0-9]{0,2}$", ErrorMessage = "Invalid score, must contain max two decimals")]
        [Range(0.01, 10, ErrorMessage = "Invalid score, must be between 0.01 and 10")]
        public String Dscore
        {
            get { return dscore; }
            set
            {
                dscore = value;
                ValidateProperty(value);
                OnPropertyChanged("Dscore");
            }
        }

        [RegularExpression(@"^[0-9]{1,2}\.?[0-9]{0,2}$", ErrorMessage = "Invalid score, must contain max two decimals")]
        [Range(0.01, 10, ErrorMessage="Invalid score, must be between 0.01 and 10")]
        public String Escore
        {
            get { return escore; }
            set
            {
                escore = value;
                ValidateProperty(value);
                OnPropertyChanged("Escore");
            }
        }

        [RegularExpression(@"^[0-9]{1,2}\.?[0-9]{0,2}$", ErrorMessage = "Invalid penalty score, must contain max two decimals")]
        [Range(0.01, 10, ErrorMessage = "Invalid penalty score, must be between 0.01 and 10")]
        public String Penalty
        {
            get { return penalty; }
            set
            {
                penalty = value;
                ValidateProperty(value);
                OnPropertyChanged("Penalty");
            }
        }

        [RegularExpression(@"^[0-9]{1,2}\.?[0-9]{0,2}$", ErrorMessage = "Invalid score, must contain max two decimals")]
        [Range(0.01, 10, ErrorMessage = "Invalid score, must be between 0.01 and 10")]
        public String Totalscore
        {
            get { return totalscore; }
            set
            {
                totalscore = value;
                ValidateProperty(value);
                OnPropertyChanged("Totalscore");
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
        #endregion

        public MeasurementViewModel(IApplicationController app, PortController portController, VideoCameraController videoCameraController) : base()
        {
            _app = app;
            Measuring = false;
            RatingViewModel ratingVM = new RatingViewModel(_app);
            RatingControl = ratingVM;        

            //put data in array for testing
            vaultKindArray = new String[3];
            vaultKindArray[0] = "Practice";
            vaultKindArray[1] = "NK";
            vaultKindArray[2] = "EK";

            //load autocompletion data
            Locations = vaultModule.getLocationNames();
            Gymnasts = vaultModule.getGymnastNames();
            VaultNumbers = vaultModule.getVaultNumberNames();

            // Set PortController
            this.portController = portController;

            // Set VideoCamera
            CameraView = new CameraViewModel(_app);
            VideoCameraController = videoCameraController;

            ManualModeChecked = true;

            // Set Graph
            GraphViewModel graphVM = new GraphViewModel(_app, this, 0,1500);
            GraphViewMeasurement = graphVM;
        }

        private void stopMeasuring()
        {
            List<String> writeBuffer = portController.stopMeasurement();
            videoCameraController.StopCapture();
            //cameraModule.createVault(videoCameraController.RecordedVideo, writeBuffer);
            videoCameraController.Close();
        }

        private void openVideoSource()
        {
            cameraWindow = videoCameraController.CameraWindow;

            if (cameraWindow != null)
            {
                setVideoSource(cameraWindow);
            }
        }

        private void setVideoSource(CameraWindow cameraWindow)
        {
            WindowsFormsHost host = new WindowsFormsHost();
            host.Child = cameraWindow;
            cameraView.CameraHost = host;
        }

        #region RelayCommands

        public void LoadPostMeasurementScreen(object commandParam)
        {
            _app.ShowPostMeasurementView();
        }
        public void StartMeasurement(object commandParam)
        {
            //start measurement
            if(Measuring)
            {
                Measuring = false;
            }
            else
            {
                Measuring = true;
            }
        }

        #endregion

        protected override void initRelayCommands()
        {
            PostMeasurementCommand = new RelayCommand(LoadPostMeasurementScreen);
            StartMeasurementCommand = new RelayCommand(StartMeasurement);
        }
       
    }
}
