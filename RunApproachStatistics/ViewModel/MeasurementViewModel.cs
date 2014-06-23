using MvvmValidation;
using RunApproachStatistics.Controllers;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using RunApproachStatistics.Services;
using RunApproachStatistics.View;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<ThumbnailViewModel> thumbnailCollection;

        private String vaultKind;
        private String location;
        private String gymnast;
        private String vaultNumber;
        private String dscore;
        private String escore;
        private String penalty;
        private String totalscore;
        private int listRowCount;

        private List<gymnast> gymnastList;
        private List<vaultnumber> vaultNumberList;
        private List<vaultkind> vaultKindList;
        private List<location> locationsList;

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

        private ThumbnailViewModel selectedThumbnail;
        private vault selectedVault;

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

        public Boolean OfflineMode
        {
            get { return !App.IsOfflineMode; }
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
            get { return locationsList.Select(l => l.name).ToList(); }
            set
            {
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
            get { return gymnastList
                .Select(g => g.name + (g.surname_prefix != null ? " " + g.surname_prefix : "") + " " + g.surname)
                .ToList(); }
            set
            {
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
            get { return vaultNumberList.Select(v => v.code).ToList(); }
            set
            {
                OnPropertyChanged("VaultNumbers");
            }
        }
        public List<String> VaultKinds
        {
            get { return vaultKindList.Select(v => v.name).ToList(); }
            set
            {
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
        public int ListRowCount
        {
            get 
            {
                if (listRowCount < 4)
                {
                    return 4;
                }
                else
                {
                    return listRowCount;
                }
            }
            set
            {
                listRowCount = value;
                OnPropertyChanged("ListRowCount");
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

        public ObservableCollection<ThumbnailViewModel> ThumbnailCollection
        {
            get { return thumbnailCollection; }
            set
            {
                thumbnailCollection = value;
                ListRowCount = thumbnailCollection.Count;
                OnPropertyChanged("ThumbnailCollection");
            }
        }

        public ThumbnailViewModel SelectedThumbnail
        {
            get { return selectedThumbnail; }
            set
            {
                selectedThumbnail = value;
                if (SelectedThumbnail != null)
                {
                    selectedVault = selectedThumbnail.Vault;
                    setVaultFields();
                    OnPropertyChanged("SelectedThumbnail");
                }
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

            gymnastList     = vaultModule.getGymnasts();
            vaultNumberList = vaultModule.getVaultNumbers();
            vaultKindList   = vaultModule.getVaultKinds();
            locationsList   = vaultModule.getLocations();


            // Set PortController
            this.portController = portController;

            // Set VideoCamera
            CameraView = new CameraViewModel(_app);
            VideoCameraController = videoCameraController;

            ManualModeChecked = true;

            //Turn pilotlaser off if it is on
            if (portController.PilotLaser != 0)
            {
                portController.PilotLaser = 0;
            }

            // Set Graph
            GraphViewModel graphVM = new GraphViewModel(_app, this, true, 1500);
            GraphViewMeasurement = graphVM;

            // Set validation
            SetValidationRules();

            // Set vault handler
            cameraModule.VaultCreated += vaultCreated;

            // Set thumbnail collection
            thumbnailCollection = new ObservableCollection<ThumbnailViewModel>();

            // Create empty vault
            vault newVault = new vault();
            newVault.timestamp = DateTime.Now;
            selectedVault = newVault;

            // Add empty thumbnail for live and set selected
            ThumbnailViewModel liveThumbnail = new ThumbnailViewModel(_app);
            liveThumbnail.setLive(true);
            liveThumbnail.Vault = newVault;
            thumbnailCollection.Add(liveThumbnail);
            SelectedThumbnail = liveThumbnail;
        }

        private void stopMeasuring()
        {
            ThumbnailViewModel liveThumbnail = ThumbnailCollection[0];
            vault newVault = liveThumbnail.Vault;

            RatingViewModel ratingVM = (RatingViewModel)RatingControl;
            newVault.rating_star = ratingVM.getScore();

            newVault.timestamp = DateTime.Now;

            List<String> writeBuffer = portController.stopMeasurement();
            videoCameraController.StopCapture();
            cameraModule.createVault(videoCameraController.RecordedVideo, writeBuffer, newVault);
            videoCameraController.Close();

            // Create new vault for the live thumbnail
            vault newLiveVault = new vault();
            liveThumbnail.Vault = newLiveVault;
            SelectedThumbnail = liveThumbnail;

            // Clear fields, keep data if checked
            // Send the just created vault with it so the checked fields can be kept
            clearFields(newVault);
        }

        private void clearFields(vault savedVault)
        {
            if (!VaultKindChecked)
            {
                VaultKind = "";
            }
            else
            {
                VaultKind = savedVault.vaultkind.name;
            }

            if (!LocationChecked)
            {
                Location = "";
            }
            else
            {
                Location = savedVault.location.name;
            }

            if (!GymnastChecked)
            {
                Gymnast = "";
            }
            else
            {
                Gymnast = savedVault.gymnast.name + savedVault.gymnast.surname_prefix != null ? " " + 
                    savedVault.gymnast.surname_prefix : "" + savedVault.gymnast.surname;
            }

            if (!VaultNumberChecked)
            {
                VaultNumber = ""; 
            }
            else
            {
                VaultNumber = savedVault.vaultnumber.code;
            }

            RatingViewModel ratingView = new RatingViewModel(_app);
            ratingView.OnRatingChanged += setVaultRating;
            RatingControl = ratingView;
            if (RatingChecked)
            {
                ratingView.RatingValue = (int)savedVault.rating_star;
            }

            if (!DscoreChecked)
            {
                Dscore = "";
            }
            else
            {
                Dscore = savedVault.rating_official_D.ToString();
            }

            if (!EscoreChecked)
            {
                Escore = "";
            }
            else
            {
                Escore = savedVault.rating_official_E.ToString();
            }

            if (!PenaltyChecked)
            {
                Penalty = "";
            }
            else
            {
                Penalty = savedVault.penalty.ToString();
            }
        }

        private void vaultCreated(object sender, vault receivedVault)
        {
            // Add vault to thumbnail list.
            ThumbnailViewModel vaultThumb = new ThumbnailViewModel(_app);
            vaultThumb.Vault = receivedVault;
            
            // Add to collection
            thumbnailCollection.Insert(1, vaultThumb);
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
                catch
                {
                    // No problem
                    Totalscore = "";
                }
            } 
            else
            {
                Totalscore = "";
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
            ThumbnailCollection.RemoveAt(0);
            if(ThumbnailCollection.Count == 0 || App.IsOfflineMode)
            {
                _app.ShowHomeView();
            }
            else
            {
               _app.ShowPostMeasurementView(ThumbnailCollection);
            }
           
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
                                  if (VaultKind == null || VaultKind == "" || VaultKinds.Contains(VaultKind))
                                  {
                                      if (!String.IsNullOrEmpty(VaultKind))
                                      {
                                          selectedVault.vaultkind = vaultKindList.Select(v => v)
                                              .Where(x => x.name == VaultKind).First();
                                      }
                                      else
                                      {
                                          selectedVault.vaultkind = null;
                                      }

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
                                  if (Location == null || Location == "" || Locations.Contains(Location))
                                  {
                                      if (!String.IsNullOrEmpty(Location))
                                      {
                                          selectedVault.location = locationsList.Select(l => l)
                                              .Where(x => x.name == Location).First();
                                      }
                                      else
                                      {
                                          selectedVault.location = null;
                                      }

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
                                  if (Gymnast == null || Gymnast == "" || Gymnasts.Contains(Gymnast))
                                  {
                                      if (!String.IsNullOrEmpty(Gymnast))
                                      {
                                          gymnast sGymnast = gymnastList.Select(g => g)
                                              .Where(x => (x.name + (x.surname_prefix != null ? " " + x.surname_prefix : "") 
                                                  + " " + x.surname) == Gymnast)
                                              .First();
                                          selectedVault.gymnast_id = sGymnast.gymnast_id;
                                          selectedVault.gymnast = sGymnast;
                                      }
                                      else
                                      {
                                          selectedVault.gymnast = null;
                                      }

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
                                  if (VaultNumber == null || VaultNumber == "" || VaultNumbers.Contains(VaultNumber))
                                  {
                                      if (!String.IsNullOrEmpty(VaultNumber))
                                      {
                                          selectedVault.vaultnumber = vaultNumberList.Select(v => v)
                                              .Where(x => x.code == VaultNumber).First();
                                      }
                                      else
                                      {
                                          selectedVault.vaultnumber = null;
                                      }

                                      return RuleResult.Valid();
                                  }
                                  else
                                  {
                                      return RuleResult.Invalid("Vault number is not in list");
                                  }
                              });

            Validator.AddRule(() => Escore,
                              () =>
                              {
                                  return checkScore(Escore, "escore");
                              });

            Validator.AddRule(() => Dscore,
                              () =>
                              {
                                  return checkScore(Dscore, "dscore");
                              });

            Validator.AddRule(() => Penalty,
                              () =>
                              {
                                    return checkScore(Penalty, "penalty");
                              });
        }

        private RuleResult checkScore(String score, String scoreType)
        {
            if (score.Equals(""))
            {
                switch(scoreType)
                {
                    case "dscore": selectedVault.rating_official_D = null; break;
                    case "escore": selectedVault.rating_official_E = null; break;
                    case "penalty": selectedVault.penalty = null; break;
                }
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
                    if (CountDecimalPlaces((decimal)fScore) <= 3)
                    {
                        switch(scoreType)
                        {
                            case "dscore": selectedVault.rating_official_D = (decimal)fScore; break;
                            case "escore": selectedVault.rating_official_E = (decimal)fScore; break;
                            case "penalty": selectedVault.penalty = (decimal)fScore; break;
                        }
                        return RuleResult.Valid();
                    }
                    else
                    {
                        return RuleResult.Invalid("Score can contain maximal 3 decimals");
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

        #region Thumbnail Methods

        private void setVaultRating(object sender, int rating)
        {
            selectedVault.rating_star = rating;
        }

        private void setVaultFields()
        {
            if (selectedVault.vaultkind_id != null)
            {

                VaultKind = selectedVault.vaultkind.name;
            }
            else
            {
                VaultKind = "";
            }

            if (selectedVault.location_id != null)
            {
                Location = selectedVault.location.name;
            }
            else
            {
                Location = "";
            }

            if (selectedVault.gymnast_id != null)
            {
                Gymnast = selectedVault.gymnast.name + (selectedVault.gymnast.surname_prefix != null ? " " +
                    selectedVault.gymnast.surname_prefix : "") + " " + selectedVault.gymnast.surname;
            }
            else
            {
                Gymnast = "";
            }

            if (selectedVault.vaultnumber_id != null)
            {
                VaultNumber = selectedVault.vaultnumber.code;
            }
            else
            {
                VaultNumber = "";
            }

            RatingViewModel rating = new RatingViewModel(_app);
            rating.OnRatingChanged += setVaultRating;
            RatingControl = rating;
            if (selectedVault.rating_star != null)
            {
                rating.RatingValue = (int)selectedVault.rating_star;
            }


            if (selectedVault.rating_official_D != null)
            {
                Dscore = selectedVault.rating_official_D.ToString();
            }
            else
            {
                Dscore = "";
            }

            if (selectedVault.rating_official_E != null)
            {
                Escore = selectedVault.rating_official_E.ToString();
            }
            else
            {
                Escore = "";
            }

            if (selectedVault.penalty != null)
            {
                Penalty = selectedVault.penalty.ToString();
            }
            else
            {
                Penalty = "";
            }
        }

        #endregion
    }
}
