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
                if(selectedThumbnail != null)
                {
                    ratingVM.RatingValue = selectedThumbnail.StarRating;
                }
                else
                {
                    ratingVM.RatingValue = 0;
                }
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

        public String Gymnast
        {
            get {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.Gymnast;
                return "";
            }
            set
            {
                SelectedThumbnail.Gymnast = value;
                OnPropertyChanged("Gymnast");
            }
        }

        public String Datetime
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.Datetime;
                return "";
            }
            set
            {
                SelectedThumbnail.Datetime = value;
                OnPropertyChanged("Datetime");
            }
        }

        public String Timespan
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.Timespan;
                return "";
            }
            set
            {
                SelectedThumbnail.Timespan = value;
                OnPropertyChanged("TimeSpan");
            }
        }

        public String VaultNumber
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.VaultNumber;
                return "";
            }
            set
            {
                SelectedThumbnail.VaultNumber = value;
                OnPropertyChanged("VaultNumber");
            }
        }

        public String Location
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.Location;
                return "";
            }
            set
            {
                SelectedThumbnail.Location = value;
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

        public String DScore
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.DScore;
                return "";
            }
            set
            {
                SelectedThumbnail.DScore = value;
                OnPropertyChanged("DScore");
            }
        }

        public String EScore
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.EScore;
                return "";
            }
            set
            {
                SelectedThumbnail.EScore = value;
                OnPropertyChanged("EScore");
            }
        }

        public String Penalty
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.Penalty;
                return "";
            }
            set
            {
                SelectedThumbnail.Penalty = value;
                OnPropertyChanged("Penalty");
            }
        }

        public String TotalScore
        {
            get
            {
                if (SelectedThumbnail != null)
                    return SelectedThumbnail.TotalScore;
                return "";
            }
            set
            {
                SelectedThumbnail.TotalScore = value;
                OnPropertyChanged("TotalScore");
            }
        }

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
                    Datetime = vaults[i].timestamp.ToString(),
                    //Gymnast = vaults[i].gymnast.name,
                    DScore = vaults[i].rating_official_D.ToString(),
                    EScore = vaults[i].rating_official_E.ToString(),
                    StarRating = (int)vaults[i].rating_star,
                    //Location = vaults[i].location.name,
                    Penalty = vaults[i].penalty.ToString(),
                    //TotalScore = vaults[i]    bestaat niet
                    //VaultNumber = vaults[i].vaultnumber.code,
                    Vault_id = vaults[i].vault_id
                    
                });
                
            }

        }

        #region Command Methodes

        public void SaveChanges(object commandParam)
        {
            // Do something
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
                vaultModule.delete(SelectedThumbnail.Vault_id);
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