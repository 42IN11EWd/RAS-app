using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using RunApproachStatistics.MVVM.Validation;
using RunApproachStatistics.Services;
using RunApproachStatistics.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms.Integration;


namespace RunApproachStatistics.ViewModel
{
    [EnsureInList(ErrorMessage = "Invalid location")]
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

        private Boolean vaultKindChecked;
        private Boolean locationChecked;
        private Boolean gymnastChecked;
        private Boolean vaultNumberChecked;
        private Boolean dscoreChecked;
        private Boolean escoreChecked;
        private Boolean ratingChecked;
        private Boolean penaltyChecked;
        
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

        public Boolean VaultKindChecked
        {
            get { return vaultKindChecked; }
            set
            {
                vaultKindChecked = value;
                OnPropertyChanged("VaultKindChecked");
            }
        }

        public Boolean LocationChecked
        {
            get { return locationChecked; }
            set
            {
                vaultKindChecked = value;
                OnPropertyChanged("LocationChecked");
            }
        }

        public Boolean GymnastChecked
        {
            get { return gymnastChecked; }
            set
            {
                vaultKindChecked = value;
                OnPropertyChanged("GymnastChecked");
            }
        }

        public Boolean VaultNumberChecked
        {
            get { return vaultNumberChecked; }
            set
            {
                vaultKindChecked = value;
                OnPropertyChanged("VaultNumberChecked");
            }
        }

        public Boolean RatingChecked
        {
            get { return ratingChecked; }
            set
            {
                vaultKindChecked = value;
                OnPropertyChanged("RatingChecked");
            }
        }

        public Boolean DscoreChecked
        {
            get { return dscoreChecked; }
            set
            {
                vaultKindChecked = value;
                OnPropertyChanged("DscoreChecked");
            }
        }

        public Boolean EscoreChecked
        {
            get { return escoreChecked; }
            set
            {
                vaultKindChecked = value;
                OnPropertyChanged("EscoreChecked");
            }
        }

        public Boolean PenaltyChecked
        {
            get { return penaltyChecked; }
            set
            {
                vaultKindChecked = value;
                OnPropertyChanged("PenaltyChecked");
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
                Validate();
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


        [RegularExpression(@"^[0-9]{1,2}\.?[0-9]{0,3}$", ErrorMessage = "Invalid score, must contain max three decimals")]
        [Range(0.001, 10, ErrorMessage = "Invalid score, must be between 0.01 and 10")]
        public String Dscore
        {
            get { return dscore; }
            set
            {
                dscore = value;
                ValidateProperty(value);
                calculateTotalScore();
                OnPropertyChanged("Dscore");
            }
        }

        [RegularExpression(@"^[0-9]{1,2}\.?[0-9]{0,3}$", ErrorMessage = "Invalid score, must contain max three decimals")]
        [Range(0.001, 10, ErrorMessage="Invalid score, must be between 0.01 and 10")]
        public String Escore
        {
            get { return escore; }
            set
            {
                escore = value;
                ValidateProperty(value);
                calculateTotalScore();
                OnPropertyChanged("Escore");
            }
        }

        [RegularExpression(@"^[0-9]{1,2}\.?[0-9]{0,3}$", ErrorMessage = "Invalid penalty score, must contain max three decimals")]
        [Range(0.001, 10, ErrorMessage = "Invalid penalty score, must be between 0.01 and 10")]
        public String Penalty
        {
            get { return penalty; }
            set
            {
                penalty = value;
                ValidateProperty(value);
                calculateTotalScore();
                OnPropertyChanged("Penalty");
            }
        }

        public String Totalscore
        {
            get { return totalscore; }
            set
            {
                totalscore = value;
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
            //List<String> writeBuffer = portController.stopMeasurement();
            //videoCameraController.StopCapture();

            String vaultKind = SelectedVaultKind; //TODO: check if valid

            String location = Location;
            if (location == null || location.Equals("") || GetErrors("Location") != null)
            {
                location = null;
            }

            String gymnast = Gymnast;
            if (gymnast == null || gymnast.Equals("") || GetErrors("Gymnast") != null)
            {
                gymnast = null;
            }

            String vaultNumber = VaultNumber;
            if (vaultNumber == null || vaultNumber.Equals("") || GetErrors("VaultNumber") != null)
            {
                vaultNumber = null;
            }

            RatingViewModel ratingVM = (RatingViewModel)RatingControl;
            int rating = ratingVM.getScore();

            //cameraModule.createVault(videoCameraController.RecordedVideo, writeBuffer);
            //videoCameraController.Close();
            clearFields();
        }

        private void clearFields()
        {
            if (!VaultKindChecked)
            {
                SelectedVaultKind = null;
            }

            if (!LocationChecked)
            {
                Location = null;
            }

            if (!GymnastChecked)
            {
                Gymnast = null;
            }

            if (!VaultNumberChecked)
            {
                VaultNumber = null; ;
            }

            if (!RatingChecked)
            {
                RatingControl = new RatingViewModel(_app);
            }

            if (DscoreChecked)
            {
                Dscore = null;
            }

            if (EscoreChecked)
            {
                Escore = null;
            }

            if (PenaltyChecked)
            {
                Penalty = null;
            }
        }

        private void calculateTotalScore()
        {
            if (Dscore != null && Escore != null && !Dscore.Equals("") && !Escore.Equals(""))
            {
                try
                {
                    float dscore = float.Parse(Dscore, CultureInfo.InvariantCulture);
                    float escore = float.Parse(Escore, CultureInfo.InvariantCulture);
                    float penalty = 0;

                    if (Penalty != null && !Penalty.Equals(""))
                    {
                        try
                        {
                            penalty = float.Parse(Penalty, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            //No problem
                        }
                    }

                    float totalscore = (dscore + escore) - penalty;
                    Totalscore = totalscore.ToString("0.000");
                }
                catch (Exception e)
                {
                    Totalscore = "";
                }
            }
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
