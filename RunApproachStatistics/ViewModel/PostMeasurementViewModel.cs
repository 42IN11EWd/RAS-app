using MvvmValidation;
using RunApproachStatistics.Controllers;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RunApproachStatistics.ViewModel
{
    public class PostMeasurementViewModel : ValidationViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase menu;
        private PropertyChangedBase ratingControl;
        private RatingViewModel ratingVM;

        private ObservableCollection<ThumbnailViewModel> thumbnailCollection;
        private ObservableCollection<ThumbnailViewModel> selectedThumbnails = new ObservableCollection<ThumbnailViewModel>();
        private String selectedVaultKind;

        private vault vault = new vault();
        private bool changeState;
        private bool hasGymnast;

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

        private List<vault> vaults;

        //Splitted names
        private String name;
        private String surnamePrefix;
        private String surname;


        #region Modules

        private IVaultModule vaultModule = new VaultModule();

        #endregion

        #region DataBinding

        public RelayCommand FinishCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand SelectedItemsChangedCommand { get; private set; }


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
        public ObservableCollection<ThumbnailViewModel> SelectedThumbnails
        {
            get
            {
                if (selectedThumbnails.Count == 0)
                    ChangeState = false;
                if (selectedThumbnails.Count == 1)
                    ChangeState = true;

                return selectedThumbnails;
            }
            set
            {
                selectedThumbnails = value;

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

        public DateTime Datetime
        {
            get
            {
                if (SelectedThumbnails.Count == 1)
                    return SelectedThumbnails[0].Vault.timestamp;
                return new DateTime();
            }
            set
            {
                if (SelectedThumbnails.Count == 1)
                    SelectedThumbnails[0].Vault.timestamp = value;
                OnPropertyChanged("Datetime");
            }
        }

        public String VaultNumber
        {
            get { return vaultNumber; }
            set
            {
                vaultNumber = value;
                Validator.Validate(() => VaultNumber);
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
                return location;
            }
            set
            {
                location = value;
                Validator.Validate(() => Location);
                //SelectedThumbnail.Vault.location.name = value;
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
                Validator.Validate(() => VaultKind);
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
                if (SelectedThumbnails.Count == 1)
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

            //Useless test data.
            setData();

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

        private void setData()
        {
            thumbnailCollection = new ObservableCollection<ThumbnailViewModel>();
            vaults = new List<vault>();
            vaults = vaultModule.getVaults();

            for (int i = 0; i < vaults.Count; i++)
            {
                thumbnailCollection.Add(new ThumbnailViewModel(_app)
                {
                    Vault = vaults[i]
                });

                if (vaults[i].vaultnumber != null)
                {
                    thumbnailCollection[i].VaultNumber = vaults[i].vaultnumber.code;
                }

                if (vaults[i].gymnast_id == null)
                {
                    thumbnailCollection[i].noGymnast(hasGymnast);
                    
                }
                else
                {
                    thumbnailCollection[i].Gymnast = vaults[i].gymnast.name + " " + (vaults[i].gymnast.surname_prefix != null ? vaults[i].gymnast.surname_prefix + " " : "") + vaults[i].gymnast.surname;
                }
            }
            OnPropertyChanged("ThumbnailCollection");
        }

        private void setProperties()
        {
            foreach (ThumbnailViewModel newVM in ThumbnailCollection)
            {
                if (newVM.Vault.gymnast_id == null)
                {
                    hasGymnast = false;
                }
                else
                {
                    hasGymnast = true;
                }
                newVM.noGymnast(hasGymnast);
            }

            if (SelectedThumbnails.Count == 1)
            {
                //Check for scores
                EScore = SelectedThumbnails[0].Vault.rating_official_E.ToString();
                DScore = SelectedThumbnails[0].Vault.rating_official_D.ToString();
                Penalty = selectedThumbnails[0].Vault.penalty.ToString();

                //Check ratingstars
                ratingVM.RatingValue = (int)SelectedThumbnails[0].Vault.rating_star;

                //Check for vaultkind
                if (SelectedThumbnails[0].Vault.vaultkind != null)
                {
                    VaultKind = SelectedThumbnails[0].Vault.vaultkind.name.ToString();
                }
                else
                {
                    VaultKind = "";
                }
                
                // Check for location
                if (SelectedThumbnails[0].Vault.location != null)
                {
                    Location = SelectedThumbnails[0].Vault.location.name.ToString();
                }
                else
                {
                    Location = "";
                }

                // Check for vaultnumber
                if (SelectedThumbnails[0].Vault.vaultnumber != null)
                {
                    vaultNumber = SelectedThumbnails[0].Vault.vaultnumber.code.ToString();
                }
                else
                {
                    vaultNumber = "";
                    VaultNumber = "";
                }

                // Check for gymnast
                if (SelectedThumbnails[0].Vault.gymnast != null)
                {
                    name = SelectedThumbnails[0].Vault.gymnast.name;
                    surnamePrefix = SelectedThumbnails[0].Vault.gymnast.surname_prefix;
                    surname = SelectedThumbnails[0].Vault.gymnast.surname;
                    Gymnast = (String.IsNullOrEmpty(name) ? "" : name + " ") + (surnamePrefix != null ? surnamePrefix + " " : "") + surname;
                }
                else
                {
                    name = "";
                    surnamePrefix = "";
                    surname = "";
                    Gymnast = "";
                }
            }

            if(SelectedThumbnails.Count == 0)
            {
                VaultKind = "";
                Location = "";
                Gymnast = "";
                VaultNumber = "";
                Datetime = new DateTime();
                DScore = "";
                EScore = "";
                Penalty = "";
                TotalScore = "";
                ratingVM.RatingValue = 0;
            }
        }

        private void saveInfo()
        {
            // Save Gymnast
            if (Gymnast == null || Gymnast.Equals("") || GetErrorArr("Gymnast") != null)
            {
                SelectedThumbnails[0].Vault.gymnast = null;
            }
            else
            {
                SelectedThumbnails[0].Vault.gymnast_id = gymnastIds[Gymnasts.IndexOf(Gymnast)];
            }
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
                TotalScore = "";
            }
        }

        #region RelayCommands

        public void FinishAction(object commandParam)
        {
            _app.ShowHomeView();
        }
        public void DeleteAction(object commandParam)
        {
            if (!_app.IsLoggedIn)
            {
                _app.ShowLoginView();
            }
            if (_app.IsLoggedIn)
            {
                //TODO : Confirm if you are sure to delete this vault
                for (int i = 0; i < SelectedThumbnails.Count; i++)
                {
                    vaultModule.delete(SelectedThumbnails[i].Vault.vault_id);
                    thumbnailCollection.Remove(SelectedThumbnails[i]);
                }
                
            }
        }

        public void SaveAction(object commandParam)
        {
            //solution for multiple vaults
            //SelectedThumbnail.Vault.rating_star = ratingVM.RatingValue;
            saveInfo();
            for (int i = 0; i < SelectedThumbnails.Count; i++)
            {
                vaultModule.update(SelectedThumbnails[i].Vault);
            }
            
            SelectedThumbnails.Clear();
            setProperties();
            //setData();
            OnPropertyChanged("SelectedThumbnails");
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

            Validator.AddRule(() => Gymnast,
                              () => Gymnasts,
                              () =>
                              {
                                  if (Gymnast == null || Gymnast == "" || Gymnasts.Contains(Gymnast))
                                  {
                                      if (Gymnasts.Contains(Gymnast))
                                      {
                                          int j = ThumbnailCollection.IndexOf(SelectedThumbnails[0]);
                                          ThumbnailCollection[j].Gymnast = Gymnast;
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
                                          if (VaultNumbers.Contains(VaultNumber))
                                          {
                                              int j = ThumbnailCollection.IndexOf(SelectedThumbnails[0]);
                                              ThumbnailCollection[j].VaultNumber = VaultNumber;
                                          }
                                      }
                                      else
                                      {
                                            SelectedThumbnails[0].Vault.vaultnumber = null;
                                      }

                                      return RuleResult.Valid();
                                  }
                                  else
                                  {
                                      return RuleResult.Invalid("Vault number is not in list");
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
                                          if (Locations.Contains(Location))
                                          {
                                              SelectedThumbnails[0].Vault.location_id = locationIds[Locations.IndexOf(Location)];
                                          }
                                      }
                                      else
                                      {
                                          SelectedThumbnails[0].Vault.location = null;
                                      }

                                      return RuleResult.Valid();
                                  }
                                  else
                                  {
                                      return RuleResult.Invalid("Location is not in list");
                                  }
                              });

            Validator.AddRule(() => VaultKind,
                              () => VaultKinds,
                              () =>
                              {
                                  if (VaultKind == null || VaultKind == "" || VaultKinds.Contains(VaultKind))
                                  {
                                      if (VaultKind != null)
                                      {
                                          if (VaultKinds.Contains(VaultKind))
                                          {
                                              SelectedThumbnails[0].Vault.vaultkind_id = vaultKindIds[VaultKinds.IndexOf(VaultKind)];
                                          }
                                          
                                      }
                                      else
                                      {
                                          SelectedThumbnails[0].Vault.vaultkind = null;
                                      }

                                      return RuleResult.Valid();
                                  }
                                  else
                                  {
                                      return RuleResult.Invalid("Vaultkind is not in list");
                                  }
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
                                SelectedThumbnails[0].Vault.rating_official_E = (decimal)fScore;
                                break;
                            case "Dscore":
                                SelectedThumbnails[0].Vault.rating_official_D = (decimal)fScore;
                                break;
                            case "Penalty":
                                SelectedThumbnails[0].Vault.penalty = (decimal)fScore;
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
            SaveCommand = new RelayCommand(SaveAction);
            SelectedItemsChangedCommand = new RelayCommand((thumbnails) =>
            {
                if (thumbnails != null)
                {
                    IList selectedthumbnails = (IList)thumbnails;
                    SelectedThumbnails.Clear();
                    foreach (ThumbnailViewModel thumbnail in selectedthumbnails)
                    {
                        SelectedThumbnails.Add(thumbnail);
                    }
                    setProperties();
                    OnPropertyChanged("SelectedThumbnails");
                    OnPropertyChanged("Gymnast");
                    OnPropertyChanged("Datetime");
                    OnPropertyChanged("TimeSpan");
                    OnPropertyChanged("VaultNumber");
                    OnPropertyChanged("Location");
                    OnPropertyChanged("VaultKind");
                    OnPropertyChanged("TotalScore");
                }
            });
        }
    }
}
