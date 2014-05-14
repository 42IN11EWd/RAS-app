using MvvmValidation;
using RunApproachStatistics.Controllers;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using RunApproachStatistics.Services;
using RunApproachStatistics.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms.Integration;


namespace RunApproachStatistics.ViewModel
{
    public class MeasurementViewModel : ValidationViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase ratingControl;

        private String vaultKind;
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
        private List<String> vaultKinds;
        private List<int> locationIds;
        private List<int> gymnastIds;
        private List<int> vaultNumberIds;
        private List<int> vaultKindIds;

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
        private String rectangleColor;

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

        public String RectangleColor
        {
           get { return rectangleColor; }
           set
           {
               rectangleColor = value;
               OnPropertyChanged("RectangleColor");
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
                    RectangleColor = "Red";
                }
                else if (value == false && ManualModeChecked) 
                {
                    MeasurementButtonContent = "Start Measurement";
                    if (videoCameraController.IsCapturing)
                    {
                        stopMeasuring();
                        RectangleColor = "White";
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
                locationChecked = value;
                OnPropertyChanged("LocationChecked");
            }
        }

        public Boolean GymnastChecked
        {
            get { return gymnastChecked; }
            set
            {
                gymnastChecked = value;
                OnPropertyChanged("GymnastChecked");
            }
        }

        public Boolean VaultNumberChecked
        {
            get { return vaultNumberChecked; }
            set
            {
                vaultNumberChecked = value;
                OnPropertyChanged("VaultNumberChecked");
            }
        }

        public Boolean RatingChecked
        {
            get { return ratingChecked; }
            set
            {
                ratingChecked = value;
                OnPropertyChanged("RatingChecked");
            }
        }

        public Boolean DscoreChecked
        {
            get { return dscoreChecked; }
            set
            {
                dscoreChecked = value;
                OnPropertyChanged("DscoreChecked");
            }
        }

        public Boolean EscoreChecked
        {
            get { return escoreChecked; }
            set
            {
                escoreChecked = value;
                OnPropertyChanged("EscoreChecked");
            }
        }

        public Boolean PenaltyChecked
        {
            get { return penaltyChecked; }
            set
            {
                penaltyChecked = value;
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
        public String VaultKind
        {
            get { return vaultKind; }
            set
            {
                vaultKind = value;
                Validator.Validate(() => VaultKind);
                OnPropertyChanged("VaultKind");
            }
        }
        
        public String Location
        {
            get { return location; }
            set
            {
                location = value;
                Validator.Validate(() => Location);
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
                Validator.Validate(() => Gymnast);
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
                Validator.Validate(() =>  VaultNumber);
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
        public List<String> VaultKinds
        {
            get { return vaultKinds; }
            set
            {
                vaultKinds = value;
                OnPropertyChanged("VaultKinds");
            }
        }

        public String Dscore
        {
            get { return dscore; }
            set
            {
                dscore = value;
                Validator.Validate(() => Dscore);
                calculateTotalScore();
                OnPropertyChanged("Dscore");
            }
        }

        public String Escore
        {
            get { return escore; }
            set
            {
                escore = value;
                Validator.Validate(() => Escore);
                calculateTotalScore();
                OnPropertyChanged("Escore");
            }
        }

        public String Penalty
        {
            get { return penalty; }
            set
            {
                penalty = value;
                Validator.Validate(() => Penalty);
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

            RectangleColor = "White";

            //load autocompletion data
            VaultKinds      = vaultModule.getVaultKindNames();
            vaultKindIds    = vaultModule.getVaultKindIds();

            Locations       = vaultModule.getLocationNames();
            locationIds     = vaultModule.getLocationIds();

            Gymnasts        = vaultModule.getGymnastNames();
            gymnastIds      = vaultModule.getGymnastIds();

            VaultNumbers    = vaultModule.getVaultNumberNames();
            vaultNumberIds  = vaultModule.getVaultNumberIds();

            // Set PortController
            this.portController = portController;

            // Set VideoCamera
            CameraView = new CameraViewModel(_app);
            VideoCameraController = videoCameraController;

            ManualModeChecked = true;

            // Set Graph
            GraphViewModel graphVM = new GraphViewModel(_app, this, 0,1500);
            GraphViewMeasurement = graphVM;

            // Set validation
            SetValidationRules();
        }

        private void stopMeasuring()
        {
            //List<String> writeBuffer = portController.stopMeasurement();
            //videoCameraController.StopCapture();
            
            // Create new vault
            vault newVault = new vault();

            if (VaultKind == null || VaultKind.Equals("") || GetErrorArr("VaultKind") != null)
            {
                newVault.vaultkind = null;
            }
            else
            {
                newVault.vaultkind_id = vaultKindIds[VaultKinds.IndexOf(VaultKind)];
            }

            if (Location == null || Location.Equals("") || GetErrorArr("Location") != null)
            {
                newVault.location = null;
            }
            else
            {
                newVault.location_id = locationIds[Location.IndexOf(Location)];
            }
            

            if (Gymnast == null || Gymnast.Equals("") || GetErrorArr("Gymnast") != null)
            {
                newVault.gymnast = null;
            }
            else
            {
                newVault.gymnast_id = gymnastIds[Gymnasts.IndexOf(Gymnast)];
            }

            if (VaultNumber == null || VaultNumber.Equals("") || GetErrorArr("VaultNumber") != null)
            {
                newVault.vaultnumber = null;
            }
            else
            {
                newVault.vaultnumber_id = vaultNumberIds[VaultNumbers.IndexOf(VaultNumber)];
            }

            if (Dscore == null || Dscore.Equals("") || GetErrorArr("Dscore") != null)
            {
                newVault.rating_official_D = null;
            }
            else
            {
                decimal dDecimal = 0;
                try
                {
                    dDecimal = decimal.Parse(Dscore, CultureInfo.InvariantCulture);
                }
                catch
                {
                    //No problem
                }
                newVault.rating_official_E = dDecimal;
            }

            if (Escore == null || Escore.Equals("") || GetErrorArr("Escore") != null)
            {
                newVault.rating_official_E = null;
            }
            else
            {
                decimal eDecimal = 0;
                try
                {
                    eDecimal = decimal.Parse(Escore, CultureInfo.InvariantCulture);
                }
                catch
                {
                    //No problem
                }
                newVault.rating_official_E = eDecimal;
            }

            if (Penalty == null || Penalty.Equals("") || GetErrorArr("Penalty") != null)
            {
                newVault.penalty = null;
            }
            else
            {
                decimal pDecimal = 0;
                try
                {
                    pDecimal = decimal.Parse(Penalty, CultureInfo.InvariantCulture);
                }
                catch
                {
                    //No problem
                }
                newVault.penalty = pDecimal;
            }


            RatingViewModel ratingVM = (RatingViewModel)RatingControl;
            int rating = ratingVM.getScore();
            newVault.rating_star = rating;

            List<String> writeBuffer = portController.stopMeasurement();
            videoCameraController.StopCapture();
            cameraModule.createVault(videoCameraController.RecordedVideo, writeBuffer, newVault);
            videoCameraController.Close();

            clearFields();
        }

        private void clearFields()
        {
            if (!VaultKindChecked)
            {
                VaultKind = "";
            }

            if (!LocationChecked)
            {
                Location = "";
            }

            if (!GymnastChecked)
            {
                Gymnast = "";
            }

            if (!VaultNumberChecked)
            {
                VaultNumber = ""; 
            }

            if (!RatingChecked)
            {
                RatingControl = new RatingViewModel(_app);
            }

            if (!DscoreChecked)
            {
                Dscore = "";
            }

            if (!EscoreChecked)
            {
                Escore = "";
            }

            if (!PenaltyChecked)
            {
                Penalty = "";
            }
        }

        private void calculateTotalScore()
        {
            if (Dscore != null && Escore != null && !Dscore.Equals("") && !Escore.Equals("")
                && GetErrorArr("Dscore") == null && GetErrorArr("Escore") == null)
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

        #region Validation Rules

        private void SetValidationRules()
        {
            Validator.AddRule(() => VaultKind,
                              () => VaultKinds,
                              () =>
                              {
                                  if (VaultKinds.Contains(VaultKind))
                                  {
                                      return RuleResult.Valid();
                                  }
                                  else
                                  {
                                      return RuleResult.Invalid("Vaultkind is not in list");
                                  }
                              });

            Validator.AddRule(() => Location,
                              () => Locations,
                              () =>
                              {
                                  if (Locations.Contains(Location))
                                  {
                                      return RuleResult.Valid();
                                  }
                                  else
                                  {
                                      return RuleResult.Invalid("Location is not in list");
                                  }
                              });

            Validator.AddRule(() => Gymnast,
                              () => Gymnasts,
                              () =>
                              {
                                  if (Gymnasts.Contains(Gymnast))
                                  {
                                      return RuleResult.Valid();
                                  }
                                  else
                                  {
                                      return RuleResult.Invalid("Gymnast is not in list");
                                  }  
                              });

            Validator.AddRule(() => VaultNumber,
                              () => VaultNumbers,
                              () =>
                              {
                                  if (VaultNumbers.Contains(VaultNumber))
                                  {
                                      return RuleResult.Valid();
                                  }
                                  else
                                  {
                                      return RuleResult.Invalid("Gymnast is not in list");
                                  }
                              });

            Validator.AddRule(() => Escore,
                              () =>
                              {
                                  return checkScore(Escore);
                              });

            Validator.AddRule(() => Dscore,
                              () =>
                              {
                                  return checkScore(Dscore);
                              });

            Validator.AddRule(() => Penalty,
                              () =>
                              {
                                    return checkScore(Penalty);
                              });
        }

        private RuleResult checkScore(String score)
        {
            if (score.Equals(""))
            {
                return RuleResult.Valid();
            }
            else
            {
                float fScore = 0;
                if (!float.TryParse(score, out fScore))
                {
                    return RuleResult.Invalid("Score is not a number");
                }
                else
                {
                    if (fScore <= 10 && fScore > 0)
                    {
                        return RuleResult.Assert(CountDecimalPlaces((decimal)fScore) <= 3, "Score can contain maximal 3 decimals");
                    }
                    else
                    {
                        return RuleResult.Invalid("Score must be between 0.001 and 10");
                    }
                }
            }
        }

        private static decimal CountDecimalPlaces(decimal dec)
        {
            int[] bits = Decimal.GetBits(dec);
            int exponent = bits[3] >> 16;
            int result = exponent;
            long lowDecimal = bits[0] | (bits[1] >> 8);
            while ((lowDecimal % 10) == 0)
            {
                result--;
                lowDecimal /= 10;
            }

            return result;
        }

        #endregion
    }
}
