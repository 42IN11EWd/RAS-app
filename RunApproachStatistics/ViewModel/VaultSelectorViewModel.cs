using RunApproachStatistics.Controllers;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
        private ObservableCollection<ThumbnailViewModel> selectedThumbnails = new ObservableCollection<ThumbnailViewModel>();
        private ThumbnailViewModel selectedThumbnail;
        private String selectedVaultKind;
        private ObservableCollection<String> filterItems;
        private String selectedFilterItem;
        private ObservableCollection<String> filterList; // ?
        private String compareButtonText;

        #region Modules

        private IVaultModule vaultModule = new VaultModule();

        #endregion

        #region DataBinding

        public RelayCommand SaveChangesCommand { get; private set; }
        public RelayCommand CancelChangesCommand { get; private set; }
        public RelayCommand DeleteVaultCommand { get; private set; }
        public RelayCommand CompareCommand { get; private set; }
        public RelayCommand RemoveAllFiltersCommand { get; private set; }
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
                    ButtonEnabled = false;
                if (selectedThumbnails.Count == 1)
                    ButtonEnabled = true;
                
                return selectedThumbnails;
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
                if (SelectedThumbnails.Count == 1)
                {
                    ratingVM.RatingValue = (int)SelectedThumbnails[0].Vault.rating_star;
                    return (int)SelectedThumbnails[0].Vault.rating_star;
                }
                return 0;
            }
            set
            {
                if (SelectedThumbnails.Count == 1)
                    SelectedThumbnails[0].Vault.rating_star = ratingVM.RatingValue;
                OnPropertyChanged("StarRating");
            }
        }

        public String Gymnast
        {
            get
            {
                if (SelectedThumbnails.Count == 1)
                    return SelectedThumbnails[0].Vault.gymnast != null ? SelectedThumbnails[0].Vault.gymnast.name : "";
                return "";
            }
            set
            {
                if (SelectedThumbnails.Count == 1)
                    SelectedThumbnails[0].Vault.gymnast.name = value;
                OnPropertyChanged("Gymnast");
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
            get
            {
                if (SelectedThumbnails.Count == 1)
                    return SelectedThumbnails[0].Vault.vaultnumber != null ? SelectedThumbnails[0].Vault.vaultnumber.code : "";
                return "";
            }
            set
            {
                if (SelectedThumbnails.Count == 1)    
                    SelectedThumbnails[0].Vault.vaultnumber.code = value;
                OnPropertyChanged("VaultNumber");
            }
        }

        public String Location
        {
            get
            {
                if (SelectedThumbnails.Count == 1)
                    return SelectedThumbnails[0].Vault.location != null ? SelectedThumbnails[0].Vault.location.name : "";
                return "";
            }
            set
            {
                if (SelectedThumbnails.Count == 1)
                    SelectedThumbnails[0].Vault.location.name = value;
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

        public Nullable<decimal> DScore
        {
            get
            {
                if (SelectedThumbnails.Count == 1)
                    return SelectedThumbnails[0].Vault.rating_official_D;
                return 0;
            }
            set
            {
                if (SelectedThumbnails.Count == 1)
                    SelectedThumbnails[0].Vault.rating_official_D = value;
                OnPropertyChanged("DScore");
            }
        }

        public Nullable<decimal> EScore
        {
            get
            {
                if (SelectedThumbnails.Count == 1)
                    return SelectedThumbnails[0].Vault.rating_official_E;
                return 0;
            }
            set
            {
                if (SelectedThumbnails.Count == 1)
                    SelectedThumbnails[0].Vault.rating_official_E = value;
                OnPropertyChanged("EScore");
            }
        }

        public Nullable<decimal> Penalty
        {
            get
            {
                if (SelectedThumbnails.Count == 1)
                    return SelectedThumbnails[0].Vault.penalty;
                return 0;
            }
            set
            {
                if (SelectedThumbnails.Count == 1)
                    SelectedThumbnails[0].Vault.penalty = value;
                OnPropertyChanged("Penalty");
            }
        }
        private String totalscore = "0";
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

        public String CompareButtonText
        {
            get 
            { 
                if(SelectedThumbnails.Count == 1)
                {
                    return CompareButtonText = "View";
                }
                else if(SelectedThumbnails.Count > 1)
                {
                    return CompareButtonText = "Compare";
                }
                
                return ""; 
            }
            set
            {
                compareButtonText = value;
               // OnPropertyChanged("CompareButtonText");
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

            for (int i = 0; i < vaults.Count; i++)
            {
                thumbnailCollection.Add(new ThumbnailViewModel(_app)
                {
                    Vault = vaults[i]
                });

            }

        }
        public void updateFields()
        {
            OnPropertyChanged("SelectedThumbnails");
            OnPropertyChanged("CompareButtonText");
            OnPropertyChanged("StarRating");
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

        #region RelayCommands

        public void SaveChanges(object commandParam)
        {
            for (int i = 0; i < SelectedThumbnails.Count; i++ )
            {
                vaultModule.update(SelectedThumbnails[i].Vault);
            }
                
            SelectedThumbnails.Clear();
            updateFields();
        }

        public void CancelChanges(object commandParam)
        {
            SelectedThumbnails.Clear();
            updateFields();
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
            if (_app.IsLoggedIn)
            {
                for (int i = 0; i < SelectedThumbnails.Count; i++ )
                {
                    vaultModule.delete(SelectedThumbnails[i].Vault.vault_id);
                    thumbnailCollection.Remove(SelectedThumbnails[i]);
                }
                    
            }

        }

        public void CompareAction(object commandParam)
        {
            List<vault> vaults = new List<vault>();
            for(int i = 0; i < SelectedThumbnails.Count; i++)
            {
                vaults.Add(SelectedThumbnails[i].Vault);
            }
            if(SelectedThumbnails.Count == 1)
            {
                _app.ShowHomeView();
            }
            else if(SelectedThumbnails.Count == 2)
            {
                _app.ShowCompareVaultsView(vaults);
            }
        }

        public void RemoveAllFilters(object commandParam)
        {
            // Do something
        }
        

        #endregion RelayCommands

        protected override void initRelayCommands()
        {
            SaveChangesCommand = new RelayCommand(SaveChanges);
            CancelChangesCommand = new RelayCommand(CancelChanges);
            DeleteVaultCommand = new RelayCommand(DeleteVault);
            CompareCommand = new RelayCommand(CompareAction);
            RemoveAllFiltersCommand = new RelayCommand(RemoveAllFilters);
            SelectedItemsChangedCommand = new RelayCommand((thumbnails) =>
            {
                IList selectedthumbnails = (IList)thumbnails;
                SelectedThumbnails.Clear();
                foreach (ThumbnailViewModel thumbnail in selectedthumbnails)
                {
                    SelectedThumbnails.Add(thumbnail);
                }
                updateFields();
                
            });
        }
    }
}