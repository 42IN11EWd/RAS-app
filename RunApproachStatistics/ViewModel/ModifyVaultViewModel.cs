using RunApproachStatistics.Controllers;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using System;
using MvvmValidation;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Linq;
using System.Windows.Input;

namespace RunApproachStatistics.ViewModel
{
    public class ModifyVaultViewModel : ValidationViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase ratingControl;
        private RatingViewModel ratingVM;

        private ObservableCollection<ThumbnailViewModel> thumbnailCollection;
        private ObservableCollection<ThumbnailViewModel> selectedThumbnails = new ObservableCollection<ThumbnailViewModel>();
        private String finishButtonText;

        private Visibility thumbnailVisibility;

        private vault vault = new vault();
        private bool changeState, finishButtonState;
        private bool hasGymnast;
        private string kind;

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
        private int listRowCount;


        //Splitted names
        private String name;
        private String surnamePrefix;
        private String surname;

        private List<gymnast>       gymnastList;
        private List<vaultnumber>   vaultNumberList;
        private List<vaultkind>     vaultKindList;
        private List<location>      locationsList;

        #region Modules

        private IVaultModule vaultModule = new VaultModule();

        #endregion

        #region DataBinding

        public RelayCommand FinishCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand SelectedItemsChangedCommand { get; private set; }

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

        public Visibility ThumbnailVisibility
        {
            get { return thumbnailVisibility; }
            set
            {
                thumbnailVisibility = value;
                OnPropertyChanged("ThumbnailVisibility");
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
            get
            {
                return gymnastList
                    .Select(g => g.name + (g.surname_prefix != null ? " " + g.surname_prefix : "") + " " + g.surname)
                    .ToList();
            }
            set
            {
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

        public int ListRowCount
        {
            get { return listRowCount; }
            set
            {
                listRowCount = value;
                OnPropertyChanged("ListRowCount");
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

        public bool FinishButtonState
        {
            get { return finishButtonState; }
            set
            {
                finishButtonState = value;
                OnPropertyChanged("FinishButtonState");
            }
        }

        public String FinishButtonText
        {
            get
            {
                if(kind == "POST")
                {
                    FinishButtonState = true;
                    return FinishButtonText = "Finish";
                }

                if (kind == "SELECT")
                {
                    if (SelectedThumbnails.Count == 0)
                    {
                        FinishButtonState = false;
                    }
                    else if (SelectedThumbnails.Count == 1)
                    {
                        FinishButtonState = true;
                        return FinishButtonText = "View";
                    }
                    else if (SelectedThumbnails.Count > 1)
                    {
                        FinishButtonState = true;
                        return FinishButtonText = "Compare";
                    }

                    return "";
                }
                return "";
            }
            set
            {
                finishButtonText = value;
            }
        }

        #endregion

        public ModifyVaultViewModel(IApplicationController app, string kind) : base()
        {
            _app = app;
            this.kind = kind;
            ratingVM = new RatingViewModel(_app);
            RatingControl = ratingVM;

            //load autocompletion data
            gymnastList     = vaultModule.getGymnasts();
            vaultNumberList = vaultModule.getVaultNumbers();
            vaultKindList   = vaultModule.getVaultKinds();
            locationsList   = vaultModule.getLocations();

            // Set validation
            SetValidationRules();
        }

        public ModifyVaultViewModel(IApplicationController app, string kind, List<gymnast> gymnastList, List<vaultnumber> vaultNumberList, 
                List<vaultkind> vaultKindList, List<location> locationsList) : base()
        {
            _app = app;
            this.kind = kind;
            ratingVM = new RatingViewModel(_app);
            RatingControl = ratingVM;

            this.gymnastList     = gymnastList;
            this.vaultNumberList = vaultNumberList;
            this.vaultKindList   = vaultKindList;
            this.locationsList   = locationsList;

            // Set validation
            SetValidationRules();
        }

        public void setData(List<vault> vaults)
        {
            thumbnailCollection = new ObservableCollection<ThumbnailViewModel>();
            if (vaults == null)
            {
                vaults = new List<vault>();
                vaults = vaultModule.getVaults();
            }

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
            ListRowCount = thumbnailCollection.Count / 4;

            OnPropertyChanged("ThumbnailCollection");
            OnPropertyChanged("FilterList");

        }

        public void setMeasuredVaults(ObservableCollection<ThumbnailViewModel> newThumbnailCollection)
        {
            ThumbnailCollection = newThumbnailCollection;
            for (int i = 0; i < thumbnailCollection.Count; i++)
            {
                thumbnailCollection[i].Vault = vaultModule.read(thumbnailCollection[i].Vault.vault_id);
                if (thumbnailCollection[i].Vault.gymnast_id == null)
                {
                    thumbnailCollection[i].noGymnast(hasGymnast);

                }
                else
                {
                    thumbnailCollection[i].Gymnast = thumbnailCollection[i].Vault.gymnast.name + " " + (thumbnailCollection[i].Vault.gymnast.surname_prefix != null ? thumbnailCollection[i].Vault.gymnast.surname_prefix + " " : "") + thumbnailCollection[i].Vault.gymnast.surname;
                }
            }
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

            //Check for scores
            for (int i = 0; i < SelectedThumbnails.Count; i++)
            {
                if (SelectedThumbnails[0].Vault.rating_official_D == selectedThumbnails[i].Vault.rating_official_D)
                {
                    DScore = SelectedThumbnails[0].Vault.rating_official_D.ToString();
                }
                else
                {
                    DScore = "";
                    break;
                }
            }
            for (int i = 0; i < SelectedThumbnails.Count; i++)
            {
                if (SelectedThumbnails[0].Vault.rating_official_E == selectedThumbnails[i].Vault.rating_official_E)
                {
                    EScore = SelectedThumbnails[0].Vault.rating_official_E.ToString();
                }
                else
                {
                    EScore = "";
                    break;
                }
            }
            for (int i = 0; i < SelectedThumbnails.Count; i++)
            {
                if (SelectedThumbnails[0].Vault.penalty == selectedThumbnails[i].Vault.penalty)
                {
                    Penalty = SelectedThumbnails[0].Vault.penalty.ToString();
                }
                else
                {
                    Penalty = "";
                    break;
                }
            }
            for (int i = 0; i < SelectedThumbnails.Count; i++)
            {
                if (SelectedThumbnails[0].Vault.rating_star == selectedThumbnails[i].Vault.rating_star)
                {
                    ratingVM.RatingValue = (int)SelectedThumbnails[0].Vault.rating_star;
                }
                else
                {
                    ratingVM.RatingValue = 0;
                    break;
                }
            }
            for (int i = 0; i < SelectedThumbnails.Count; i++)
            {
                if (SelectedThumbnails[0].Vault.vaultkind != null)
                {
                    if (SelectedThumbnails[0].Vault.vaultkind == selectedThumbnails[i].Vault.vaultkind)
                    {
                        VaultKind = SelectedThumbnails[0].Vault.vaultkind.name.ToString();
                    }
                    else
                    {
                        VaultKind = "";
                        break;
                    }
                }
                else
                {
                    VaultKind = "";
                }
            }
            for (int i = 0; i < SelectedThumbnails.Count; i++)
            {
                if (SelectedThumbnails[0].Vault.location != null)
                {
                    if (SelectedThumbnails[0].Vault.location == selectedThumbnails[i].Vault.location)
                    {
                        Location = SelectedThumbnails[0].Vault.location.name.ToString();
                    }
                    else
                    {
                        Location = "";
                        break;
                    }
                }
                else
                {
                    Location = "";
                }
            }
            for (int i = 0; i < SelectedThumbnails.Count; i++)
            {
                if (SelectedThumbnails[0].Vault.vaultnumber != null)
                {
                    if (SelectedThumbnails[0].Vault.vaultnumber == selectedThumbnails[i].Vault.vaultnumber)
                    {
                        vaultNumber = SelectedThumbnails[0].Vault.vaultnumber.code.ToString();
                    }
                    else
                    {
                        VaultNumber = "";
                        break;
                    }
                }
                else
                {
                    VaultNumber = "";
                }
            }
            for (int i = 0; i < SelectedThumbnails.Count; i++)
            {
                if (SelectedThumbnails[0].Vault.gymnast != null)
                {
                    if (SelectedThumbnails[0].Vault.gymnast == selectedThumbnails[i].Vault.gymnast)
                    {
                        name = SelectedThumbnails[0].Vault.gymnast.name;
                        surnamePrefix = SelectedThumbnails[0].Vault.gymnast.surname_prefix;
                        surname = SelectedThumbnails[0].Vault.gymnast.surname;
                        Gymnast = (String.IsNullOrEmpty(name) ? "" : name + " ") + (surnamePrefix != null ? surnamePrefix + " " : "") + surname;
                    }
                    else
                    {
                        Gymnast = "";
                        break;
                    }
                }
                else
                {
                    name = "";
                    surnamePrefix = "";
                    surname = "";
                    Gymnast = "";

                }
            }

            if (SelectedThumbnails.Count == 0)
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
            for (int i = 0; i < SelectedThumbnails.Count; i++)
            {
                if (VaultKind != "")
                {
                    SelectedThumbnails[i].Vault.vaultkind_id = vaultKindList.Where(v => v.name == VaultKind).First().vaultkind_id;
                }
                if (Location != "")
                {
                    SelectedThumbnails[i].Vault.location_id = locationsList.Where(l => l.name == Location).First().location_id;
                }
                if (Gymnast != "")
                {
                    SelectedThumbnails[i].Vault.gymnast_id = gymnastList.Where(g => 
                        (g.name + (!String.IsNullOrWhiteSpace(g.surname_prefix) ? " " + g.surname_prefix + " " : " ") + g.surname) == Gymnast).First().gymnast_id;
                }
                if (VaultNumber != "")
                {
                    SelectedThumbnails[i].Vault.vaultnumber_id = vaultNumberList.Where(v => v.code == VaultNumber).First().vaultnumber_id;
                }
                if (DScore != "")
                {
                    SelectedThumbnails[i].Vault.rating_official_D = decimal.Parse(DScore);
                }
                if (EScore != "")
                {
                    SelectedThumbnails[i].Vault.rating_official_E = decimal.Parse(EScore);
                }
                if (Penalty != "")
                {
                    SelectedThumbnails[i].Vault.penalty = decimal.Parse(Penalty);
                }
                if (ratingVM.RatingValue != 0)
                {
                    SelectedThumbnails[i].Vault.rating_star = ratingVM.RatingValue;
                }
            }
        }

        private void calculateTotalScore()
        {
            if (!String.IsNullOrEmpty(DScore) && String.IsNullOrWhiteSpace(EScore) && GetErrorArr("DScore") == null && GetErrorArr("EScore") == null)
            {
                try
                {
                    float dscore = float.Parse(DScore.ToString(), CultureInfo.InvariantCulture);
                    float escore = float.Parse(EScore.ToString(), CultureInfo.InvariantCulture);
                    float penalty = 0;

                    if (String.IsNullOrWhiteSpace(Penalty))
                    {
                        float.TryParse(Penalty, out penalty);
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

        private void ListViewDoubleClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Doe het eens...");
        }

        #region RelayCommands
        public void FinishAction(object commandParam)
        {
            if(kind == "POST")
            {
                _app.ShowHomeView();
            }
            if (kind == "SELECT")
            {
                List<vault> vaults = new List<vault>();
                for (int i = 0; i < SelectedThumbnails.Count; i++)
                {
                    vaults.Add(SelectedThumbnails[i].Vault);
                }
                if (SelectedThumbnails.Count == 1)
                {
                    _app.ShowVideoPlaybackView(vaults[0]);
                }
                else if (SelectedThumbnails.Count == 2)
                {
                    _app.ShowCompareVaultsView(vaults);
                }
            }
        }
        public void CancelAction(object commandParam)
        {
            SelectedThumbnails.Clear();
            OnPropertyChanged("SelectedThumbnails");
            if (kind == "POST")
            {
                // TODO : test
                setMeasuredVaults(ThumbnailCollection);
            }
            else if (kind == "SELECT")
            {
                List<vault> tempVaults = new List<vault>();
                foreach(ThumbnailViewModel thumb in ThumbnailCollection)
                {
                    tempVaults.Add(thumb.Vault);
                }
                setData(tempVaults);
            }
        }

        public void DeleteAction(object commandParam)
        {
            if(kind == "POST")
            {
                delete();
            }
            else if (kind == "SELECT")
            {
                if (!_app.IsLoggedIn)
                {
                    _app.ShowLoginView();
                }
                if (_app.IsLoggedIn)
                {
                    delete();
                }
            }
        }
        public void delete()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                for (int i = (SelectedThumbnails.Count -1); i >= 0; i--)
                {
                    vaultModule.delete(SelectedThumbnails[i].Vault.vault_id);
                    thumbnailCollection.Remove(SelectedThumbnails[i]);
                }
            }
        }

        public void SaveAction(object commandParam)
        {
            saveInfo();
            for (int i = 0; i < SelectedThumbnails.Count; i++)
            {
                vaultModule.update(SelectedThumbnails[i].Vault);
            }

            SelectedThumbnails.Clear();
            setProperties();
            if(kind == "POST")
            {
                setMeasuredVaults(ThumbnailCollection);
            }
            else if(kind == "SELECT")
            {
                _app.RefreshThumbnailCollection();
            }
            OnPropertyChanged("SelectedThumbnails");
        }

        #endregion

        #region Validation rules

        private void SetValidationRules()
        {
            Validator.AddRule(() => EScore,
                              () =>
                              {
                                  return checkScore(EScore.ToString(), "Escore");
                              });

            Validator.AddRule(() => DScore,
                              () =>
                              {
                                  return checkScore(DScore.ToString(), "Dscore");
                              });

            Validator.AddRule(() => Penalty,
                              () =>
                              {
                                  return checkScore(Penalty.ToString(), "Penalty");
                              });

            Validator.AddRule(() => Gymnast,
                              () => Gymnasts,
                              () =>
                              {
                                  if (Gymnast == null || Gymnast == "" || Gymnasts.Contains(Gymnast))
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
                                  if (VaultNumber == null || VaultNumber == "" || VaultNumbers.Contains(VaultNumber))
                                  {
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

        public RelayCommand doubleClickCommand { get; private set; }

        protected override void initRelayCommands()
        {
            doubleClickCommand = new RelayCommand((thumbnail) =>
            {
                ThumbnailViewModel thumbnailvm = (ThumbnailViewModel)thumbnail;
                Console.WriteLine(thumbnailvm.Vault.gymnast.name);
            });
            FinishCommand = new RelayCommand(FinishAction);
            DeleteCommand = new RelayCommand(DeleteAction);
            SaveCommand = new RelayCommand(SaveAction);
            CancelCommand = new RelayCommand(CancelAction);
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
                    OnPropertyChanged("FinishButtonText");
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
