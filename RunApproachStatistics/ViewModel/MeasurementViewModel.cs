﻿using MvvmValidation;
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

        public ObservableCollection<ThumbnailViewModel> ThumbnailCollection
        {
            get { return thumbnailCollection; }
            set
            {
                thumbnailCollection = value;
                OnPropertyChanged("ThumbnailCollection");
            }
        }

        public ThumbnailViewModel SelectedThumbnail
        {
            get { return selectedThumbnail; }
            set
            {
                selectedThumbnail = value;
                selectedVault = selectedThumbnail.Vault;
                setVaultFields();
                OnPropertyChanged("SelectedThumbnail");
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

            // Set vault handler
            cameraModule.VaultCreated += vaultCreated;

            // Set thumbnail collection
            thumbnailCollection = new ObservableCollection<ThumbnailViewModel>();
            // Add empty thumbnail for live 
            ThumbnailViewModel liveThumbnail = new ThumbnailViewModel(_app);
            liveThumbnail.setLive(true);
            thumbnailCollection.Add(liveThumbnail);
        }

        private void stopMeasuring()
        {
            ThumbnailViewModel liveThumbnail = ThumbnailCollection[0];
            vault newVault = liveThumbnail.Vault;
            /*
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
            }*/

            RatingViewModel ratingVM = (RatingViewModel)RatingControl;
            int rating = ratingVM.getScore();
            newVault.rating_star = rating;

            newVault.timestamp = DateTime.Now;

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

        private void vaultCreated(object sender, vault receivedVault)
        {
            // Add vault to thumbnail list.
            ThumbnailViewModel vaultThumb = new ThumbnailViewModel(_app);
            vaultThumb.Vault = receivedVault;
            
            // Add to collection
            thumbnailCollection.Add(vaultThumb);
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
                vault newVault = new vault();
                newVault.timestamp = DateTime.Now;

                ThumbnailViewModel liveThumbnail = ThumbnailCollection[0];
                liveThumbnail.Vault = newVault;
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
                                      if (VaultKind != null)
                                      {
                                          selectedVault.vaultkind_id = vaultKindIds[VaultKinds.IndexOf(VaultKind)];
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
                                      if (Location != null)
                                      {
                                          selectedVault.location_id = locationIds[Locations.IndexOf(Location)];
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
                                      if (Gymnast != null)
                                      {
                                          selectedVault.gymnast_id = gymnastIds[Gymnasts.IndexOf(Gymnast)];
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
                                      if (VaultNumber != null)
                                      {
                                          selectedVault.vaultnumber_id = vaultNumberIds[VaultNumbers.IndexOf(VaultNumber)];
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

        private void setVaultFields()
        {
            if (selectedVault.vaultkind != null)
            {
                VaultKind = VaultKinds[vaultKindIds.IndexOf((int)selectedVault.vaultkind_id)];
            }

            if (selectedVault.location != null)
            {
                Location = selectedVault.location.name;
            }

            if (selectedVault.gymnast != null)
            {
                Gymnast = Gymnasts[gymnastIds.IndexOf((int)selectedVault.gymnast_id)];
            }

            if (selectedVault.vaultnumber != null)
            {
                VaultNumber = VaultNumbers[vaultNumberIds.IndexOf((int)selectedVault.vaultnumber_id)];
            }

            if (selectedVault.rating_star != null)
            {
                RatingViewModel rating = new RatingViewModel(_app);
                rating.RatingValue = (int)selectedVault.rating_star;
                RatingControl = rating;
            }

            if (selectedVault.rating_official_D != null)
            {
                Dscore = selectedVault.rating_official_D.ToString();
            }

            if (selectedVault.rating_official_E != null)
            {
                Escore = selectedVault.rating_official_E.ToString();
            }

            if (selectedVault.penalty != null)
            {
                Penalty = selectedVault.penalty.ToString();
            }
        }

        #endregion
    }
}
