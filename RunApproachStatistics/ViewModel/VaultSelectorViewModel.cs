using RunApproachStatistics.Controllers;
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

        private String selectionGymnast;
        private String selectionDatetime;
        private String selectionTimespan;
        private String selectionVaultNumber;
        private String selectionLocation;
        private String[] vaultKindList;
        private String vaultKindSelected;
        private String selectionDScore;
        private String selectionEScore;
        private String selectionPenalty;
        private String selectionTotalScore;
        private String filterText;
        private String filterType;
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

        public String SelectionGymnast
        {
            get { return selectionGymnast; }
            set
            {
                selectionGymnast = value;
                OnPropertyChanged("SelectionGymnast");
            }
        }

        public String SelectionDatetime
        {
            get { return selectionDatetime; }
            set
            {
                selectionDatetime = value;
                OnPropertyChanged("SelectionDatetime");
            }
        }

        public String SelectionTimespan
        {
            get { return selectionTimespan; }
            set
            {
                selectionTimespan = value;
                OnPropertyChanged("SelectionTimeSpan");
            }
        }

        public String SelectionVaultNumber
        {
            get { return selectionVaultNumber; }
            set
            {
                selectionVaultNumber = value;
                OnPropertyChanged("SelectionVaultNumber");
            }
        }

        public String SelectionLocation
        {
            get { return selectionLocation; }
            set
            {
                selectionLocation = value;
                OnPropertyChanged("SelectionLocation");
            }
        }

        public String[] VaultKindList
        {
            get { return vaultKindList; }
            set
            {
                vaultKindList = value;
                OnPropertyChanged("VaultKindList");
            }
        }

        public String VaultKindSelected
        {
            get { return vaultKindSelected; }
            set
            {
                vaultKindSelected = value;
                OnPropertyChanged("VaultKindSelected");
            }
        }

        public String SelectionDScore
        {
            get { return selectionDScore; }
            set
            {
                selectionDScore = value;
                OnPropertyChanged("SelectionDScore");
            }
        }

        public String SelectionEScore
        {
            get { return selectionEScore; }
            set
            {
                selectionEScore = value;
                OnPropertyChanged("SelectionEScore");
            }
        }

        public String SelectionPenalty
        {
            get { return selectionPenalty; }
            set
            {
                selectionPenalty = value;
                OnPropertyChanged("SelectionPenalty");
            }
        }

        public String SelectionTotalScore
        {
            get { return selectionTotalScore; }
            set
            {
                selectionTotalScore = value;
                OnPropertyChanged("SelectionTotalScore");
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

            RatingViewModel ratingVM = new RatingViewModel(_app);
            RatingControl = ratingVM;
        }

        #region Command Methodes

        public void SaveChanges(object commandParam)
        {
            // Do something
        }

        public void CancelChanges(object commandParam)
        {
            // Do something
        }

        public void DeleteVault(object commandParam)
        {
            // Do something
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