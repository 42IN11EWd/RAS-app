using RunApproachStatistics.Controllers;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.ViewModel
{
    public class PostMeasurementViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase menu;
        private PropertyChangedBase ratingControl;
        private RatingViewModel ratingVM;

        private String[] vaultKind = new String[3];
        private ObservableCollection<ThumbnailViewModel> thumbnailCollection;
        private ThumbnailViewModel selectedThumbnail;
        private String selectedVaultKind;

        private vault vault = new vault();
        private bool changeState;

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
                OnPropertyChanged("StarRating");
                OnPropertyChanged("SelectedThumbnail");
                OnPropertyChanged("Gymnast");
                OnPropertyChanged("Datetime");
                OnPropertyChanged("TimeSpan");
                OnPropertyChanged("VaultNumber");
                OnPropertyChanged("Location");
                OnPropertyChanged("SelectedVaultKind");
                OnPropertyChanged("DScore");
                OnPropertyChanged("EScore");
                OnPropertyChanged("Penalty");
                OnPropertyChanged("TotalScore");
            }
        }

        public int StarRating
        {
            get
            {
                if (SelectedThumbnail != null)
                {
                    ratingVM.RatingValue = (int)SelectedThumbnail.Vault.rating_star;
                    return (int)SelectedThumbnail.Vault.rating_star;
                }
                return 0;
            }
            set
            {
                SelectedThumbnail.Vault.rating_star = ratingVM.RatingValue;
                OnPropertyChanged("StarRating");
            }
        }

        public String Gymnast
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.Vault.gymnast.name;
                return "";
            }
            set
            {
                SelectedThumbnail.Vault.gymnast.name = value;
                OnPropertyChanged("Gymnast");
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

        public decimal Timespan
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.Vault.duration;
                return 0;
            }
            set
            {
                SelectedThumbnail.Vault.duration = value;
                OnPropertyChanged("TimeSpan");
            }
        }

        public String VaultNumber
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.Vault.vaultnumber.code;
                return "";
            }
            set
            {
                SelectedThumbnail.Vault.vaultnumber.code = value;
                OnPropertyChanged("VaultNumber");
            }
        }

        public String Location
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.Vault.location.name;
                return "";
            }
            set
            {
                SelectedThumbnail.Vault.location.name = value;
                OnPropertyChanged("Location");
            }
        }

        public String[] VaultKind
        {
            get { return vaultKind; }
            set
            {
                vaultKind = value;
                OnPropertyChanged("VaultKind");
            }
        }

        public String SelectedVaultKind
        {
            get { return selectedVaultKind; }
            set
            {
                selectedVaultKind = value;
                OnPropertyChanged("SelectedVaultKind");
            }
        }

        public Nullable<int> DScore
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.Vault.rating_official_D;
                return 0;
            }
            set
            {
                SelectedThumbnail.Vault.rating_official_D = value;
                OnPropertyChanged("DScore");
            }
        }

        public Nullable<int> EScore
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.Vault.rating_official_E;
                return 0;
            }
            set
            {
                SelectedThumbnail.Vault.rating_official_E = value;
                OnPropertyChanged("EScore");
            }
        }

        public Nullable<decimal> Penalty
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.Vault.penalty;
                return 0;
            }
            set
            {
                SelectedThumbnail.Vault.penalty = value;
                OnPropertyChanged("Penalty");
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

            RatingViewModel ratingVM = new RatingViewModel(_app);
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

            vaultKind[0] = "Competition";
            vaultKind[1] = "Training";
            vaultKind[2] = "European championship";

            
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
            vaultModule.update(SelectedThumbnail.Vault);
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
