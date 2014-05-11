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
    public class VaultSelectorViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase menu;
        private PropertyChangedBase ratingControl;
        private RatingViewModel ratingVM;

        private int starRating;
        private String[] vaultKind;
        private String filterText;
        private String filterType;
        private bool buttonEnabled;
        private ObservableCollection<ThumbnailViewModel> thumbnailCollection;
        private ThumbnailViewModel selectedThumbnail;
        private String selectedVaultKind;
        private ObservableCollection<String> filterItems;
        private String selectedFilterItem;
        private ObservableCollection<String> filterList; // ?
        private String dynamicToVaultText;

        #region Modules

        private IVaultModule vaultModule = new VaultModule();

        #endregion

        #region DataBinding

        public RelayCommand SaveChangesCommand      { get; private set; }
        public RelayCommand CancelChangesCommand    { get; private set; }
        public RelayCommand DeleteVaultCommand      { get; private set; }
        public RelayCommand DynamicToVaultCommand   { get; private set; }
        public RelayCommand RemoveAllFiltersCommand { get; private set; }

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
                    ButtonEnabled = false;
                else
                    ButtonEnabled = true;
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
        public bool ButtonEnabled
        {
            get { return buttonEnabled; }
            set
            {
                buttonEnabled = value;
                OnPropertyChanged("ButtonEnabled");
            }
        }
        public int StarRating
        {
            get
            {
                if (SelectedThumbnail != null)
                {
                    ratingVM.RatingValue = (int) SelectedThumbnail.Vault.rating_star;
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
            get {
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

        /*public String TotalScore
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.Vault.totalscore;
                return "";
            }
            set
            {
                SelectedThumbnail.Vault.totalscore = value;
                OnPropertyChanged("TotalScore");
            }
        }*/

        public String FilterText
        {
            get { return filterText; }
            set
            {
                filterText = value;
                OnPropertyChanged("FilterText");
            }
        }

        public String FilterType
        {
            get { return filterType; }
            set
            {
                filterType = value;
                OnPropertyChanged("FilterType");
            }
        }

        public ObservableCollection<String> FilterItems
        {
            get
            {
                // something fancy?
                return filterItems; // TBD
            }
        }

        public String SelectedFilterItem
        {
            get { return selectedFilterItem; }
            set
            {
                selectedFilterItem = value;
                OnPropertyChanged("SelectedFilterItem");
            }
        }

        public ObservableCollection<String> FilterList
        {
            get
            {
                // something fancy?
                return filterList; // TBD
            }
        }

        public String DynamicToVaultText
        {
            get { return dynamicToVaultText; }
            set
            {
                dynamicToVaultText = value;
                OnPropertyChanged("DynamicToVaultText");
            }
        }

        #endregion

        public VaultSelectorViewModel(IApplicationController app)
            : base()
        {
            _app = app;

            // Set menu
            MenuViewModel menuViewModel = new MenuViewModel(_app);
            menuViewModel.VisibilityLaser = false;
            Menu = menuViewModel;

            ratingVM = new RatingViewModel(_app);
            RatingControl = ratingVM;

            thumbnailCollection = new ObservableCollection<ThumbnailViewModel>();
            List<vault> vaults = vaultModule.getVaults();

            for (int i = 0; i < vaults.Count; i++ )
            {
                thumbnailCollection.Add(new ThumbnailViewModel(_app)
                {
                    Vault = vaults[i]                    
                });
                
            }

        }

        #region Command Methodes

        public void SaveChanges(object commandParam)
        {
            vaultModule.update(SelectedThumbnail.Vault);
            SelectedThumbnail = null;
        }

        public void CancelChanges(object commandParam)
        {
            SelectedThumbnail = null;
        }

        public void DeleteVault(object commandParam)
        {
            if (!_app.IsLoggedIn)
            {
                _app.ShowLoginView();

                while (_app.IsLoginWindowOpen)
                {
                    //wait for closing of the login window
                }
            }
            if(_app.IsLoggedIn)
            {
                vaultModule.delete(SelectedThumbnail.Vault.vault_id);
                thumbnailCollection.Remove(SelectedThumbnail);
            }
           
        }

        public void DynamicToVault(object commandParam)
        {
            // Do something
        }

        public void RemoveAllFilters(object commandParam)
        {
            // Do something
        }

        #endregion Command Methodes

        protected override void initRelayCommands()
        {
            SaveChangesCommand      = new RelayCommand(SaveChanges);
            CancelChangesCommand    = new RelayCommand(CancelChanges);
            DeleteVaultCommand      = new RelayCommand(DeleteVault);
            DynamicToVaultCommand   = new RelayCommand(DynamicToVault);
            RemoveAllFiltersCommand = new RelayCommand(RemoveAllFilters);
        }
    }
}