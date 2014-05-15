﻿using MvvmValidation;
using RunApproachStatistics.Controllers;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.ViewModel
{
    public class PostMeasurementViewModel : ValidationViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase menu;
        private PropertyChangedBase ratingControl;
        private RatingViewModel ratingVM;

        private ObservableCollection<ThumbnailViewModel> thumbnailCollection;
        private ThumbnailViewModel selectedThumbnail;
        private String selectedVaultKind;

        private vault vault = new vault();
        private bool changeState;

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


        #region Modules

        private IVaultModule vaultModule = new VaultModule();

        #endregion

        #region DataBinding

        public RelayCommand FinishCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }


        public PropertyChangedBase Menu
        {
            get { return menu; }
            set
            {
                menu = value;
                OnPropertyChanged("Menu");
            }
        }

        public PropertyChangedBase RatingControl
        {
            get { return ratingControl; }
            set
            {
                ratingControl = value;
                OnPropertyChanged("RatingControl");
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
            get
            {
                if (selectedThumbnail == null)
                {
                    ChangeState = false;
                }
                    
                else
                {
                    ChangeState = true;
                }
                return selectedThumbnail;
            }
            set
            {
                selectedThumbnail = value;
                setScores();
                ratingVM.RatingValue = (int)SelectedThumbnail.Vault.rating_star;
                OnPropertyChanged("SelectedThumbnail");
                OnPropertyChanged("Gymnast");
                OnPropertyChanged("Datetime");
                OnPropertyChanged("TimeSpan");
                OnPropertyChanged("VaultNumber");
                OnPropertyChanged("Location");
                OnPropertyChanged("SelectedVaultKind");
                OnPropertyChanged("TotalScore");
            }
        }

        public String Gymnast
        {
            get
            {
                if (SelectedThumbnail != null)
                {
                    if (SelectedThumbnail.Vault.gymnast != null)
                    {
                        return SelectedThumbnail.Vault.gymnast.name + " " + (SelectedThumbnail.Vault.gymnast.surname_prefix != null ? SelectedThumbnail.Vault.gymnast.surname_prefix + " " : "") + SelectedThumbnail.Vault.gymnast.surname;
                    }
                }
                return "";
            }
            set
            {
                SelectedThumbnail.Vault.gymnast.name = value;
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

        public DateTime Datetime
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.Vault.timestamp;
                return new DateTime();
            }
            set
            {
                SelectedThumbnail.Vault.timestamp = value;
                OnPropertyChanged("Datetime");
            }
        }

        //public decimal Timespan
        //{
        //    get
        //    {
        //        if (SelectedThumbnail != null)
        //            return SelectedThumbnail.Vault.duration;
        //        return 0;
        //    }
        //    set
        //    {
        //        SelectedThumbnail.Vault.duration = value;
        //        OnPropertyChanged("TimeSpan");
        //    }
        //}

        public String VaultNumber
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.Vault.vaultnumber != null ? SelectedThumbnail.Vault.vaultnumber.code : "";
                return "";
            }
            set
            {
                SelectedThumbnail.Vault.vaultnumber.code = value;
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

        public String Location
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.Vault.location != null ? SelectedThumbnail.Vault.location.name : "";
                return "";
            }
            set
            {
                SelectedThumbnail.Vault.location.name = value;
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

        public String VaultKind
        {
            get { return vaultKind; }
            set
            {
                vaultKind = value;
                OnPropertyChanged("VaultKind");
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

        public String DScore
        {
            get
            {
                return dscore;
            }
            set
            {
                dscore = value;
                Validator.Validate(() => DScore);
                calculateTotalScore();
                OnPropertyChanged("DScore");
            }
        }

        public String EScore
        {
            get
            {
                return escore;
            }
            set
            {
                escore = value;
                Validator.Validate(() => EScore);
                calculateTotalScore();
                OnPropertyChanged("EScore");
            }
        }

        public String Penalty
        {
            get
            {
                return penalty;
            }
            set
            {
                penalty = value;
                Validator.Validate(() => Penalty);
                calculateTotalScore();
                OnPropertyChanged("Penalty");
            }
        }

        public String TotalScore
        {
            get
            {
                if (SelectedThumbnail != null)
                    return totalscore;
                return "";
            }
            set
            {
                totalscore = value;
                OnPropertyChanged("TotalScore");
            }
        }

        public bool ChangeState
        {
            get { return changeState; }
            set
            {
                changeState = value;
                OnPropertyChanged("ChangeState");
            }
        }

        #endregion

        public PostMeasurementViewModel(IApplicationController app) : base()
        {
            _app = app;

            // Set menu
            MenuViewModel menuViewModel = new MenuViewModel(_app);
            menuViewModel.VisibilityLaser = false;
            Menu = menuViewModel;

            ratingVM = new RatingViewModel(_app);
            RatingControl = ratingVM;

            // Useless test data.
            thumbnailCollection = new ObservableCollection<ThumbnailViewModel>();
            List<vault> vaults = vaultModule.getVaults();

            for (int i = 0; i < vaults.Count; i++)
            {
                thumbnailCollection.Add(new ThumbnailViewModel(_app)
                {
                    Vault = vaults[i]
                });
            }

            //load autocompletion data
            VaultKinds = vaultModule.getVaultKindNames();
            vaultKindIds = vaultModule.getVaultKindIds();

            Locations = vaultModule.getLocationNames();
            locationIds = vaultModule.getLocationIds();

            Gymnasts = vaultModule.getGymnastNames();
            gymnastIds = vaultModule.getGymnastIds();

            VaultNumbers = vaultModule.getVaultNumberNames();
            vaultNumberIds = vaultModule.getVaultNumberIds();

            // Set validation
            SetValidationRules();        
        }

        private void setScores()
        {
            EScore = SelectedThumbnail.Vault.rating_official_E.ToString();
            DScore = SelectedThumbnail.Vault.rating_official_D.ToString();
            Penalty = selectedThumbnail.Vault.penalty.ToString();
        }

        private void calculateTotalScore()
        {
            if (DScore != null && EScore != null && !DScore.Equals("") && !EScore.Equals("")
                && GetErrorArr("DScore") == null && GetErrorArr("EScore") == null)
            {
                try
                {
                    float dscore = float.Parse(DScore.ToString(), CultureInfo.InvariantCulture);
                    float escore = float.Parse(EScore.ToString(), CultureInfo.InvariantCulture);
                    float penalty = 0;

                    if (Penalty != null && !Penalty.Equals(""))
                    {
                        try
                        {
                            penalty = float.Parse(Penalty.ToString(), CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            //No problem
                        }
                    }

                    float totalscore = (dscore + escore) - penalty;
                    TotalScore = totalscore.ToString("0.000");
                }
                catch (Exception e)
                {
                    TotalScore = "";
                }
            }
            else
            {
                TotalScore = "0";
            }
        }

        #region RelayCommands

        public void FinishAction(object commandParam)
        {
            _app.ShowHomeView();
        }
        public void DeleteAction(object commandParam)
        {
            vaultModule.delete(SelectedThumbnail.Vault.vault_id);
            thumbnailCollection.Remove(SelectedThumbnail);
        }
        public void CancelAction(object commandParam)
        {
            SelectedThumbnail = null;
        }
        public void SaveAction(object commandParam)
        {
            SelectedThumbnail.Vault.rating_star = ratingVM.RatingValue;
            vaultModule.update(SelectedThumbnail.Vault);
        }

        #endregion

        #region Validation rules

        private void SetValidationRules()
        {
            Validator.AddRule(() => EScore,
                              () =>
                              {
                                  return checkScore(EScore.ToString(),"Escore");
                              });

            Validator.AddRule(() => DScore,
                              () =>
                              {
                                  return checkScore(DScore.ToString(),"Dscore");
                              });

            Validator.AddRule(() => Penalty,
                              () =>
                              {
                                  return checkScore(Penalty.ToString(),"Penalty");
                              });
        }

        private RuleResult checkScore(String score, String type)
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
                    if (CountDecimalPlaces((decimal)fScore) <= 3)
                    {
                        switch (type)
                        {
                            case "Escore":
                                SelectedThumbnail.Vault.rating_official_E = (decimal)fScore;
                                break;
                            case "Dscore":
                                SelectedThumbnail.Vault.rating_official_D = (decimal)fScore;
                                break;
                            case "Penalty":
                                SelectedThumbnail.Vault.penalty = (decimal)fScore;
                                break;
                        }
                        return RuleResult.Valid();
                    }
                    else
                    {
                        return RuleResult.Invalid("Score can contain a maximum of 3 decimals");
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

        protected override void initRelayCommands()
        {
            FinishCommand = new RelayCommand(FinishAction);
            DeleteCommand = new RelayCommand(DeleteAction);
            CancelCommand = new RelayCommand(CancelAction);
            SaveCommand = new RelayCommand(SaveAction);
        }
    }
}
