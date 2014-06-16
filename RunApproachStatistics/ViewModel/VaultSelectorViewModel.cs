﻿using RunApproachStatistics.Controllers;
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
using System.Windows;
using System.Windows.Controls;

namespace RunApproachStatistics.ViewModel
{
    public class VaultSelectorViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase content;

        private PropertyChangedBase ratingControl;
        private RatingViewModel ratingVM;

        private ModifyVaultViewModel modifyVaultVM;
        private PropertyChangedBase modifyControl;

        private Visibility dateVisibility;
        private String selectedDate;

        private int starRating;
        private String[] vaultKind;
        private String filterText;
        private String filterType;
        private bool buttonEnabled;
        private ObservableCollection<ThumbnailViewModel> thumbnailCollection;
        private ObservableCollection<ThumbnailViewModel> selectedThumbnails = new ObservableCollection<ThumbnailViewModel>();
        private String selectedVaultKind;
        private ObservableCollection<String> filterItems;
        private String selectedFilterItem;
        private ObservableCollection<String> filterList;
        private String selectedExistingFilterItem;

        private String compareButtonText;

        // List for all sort of info
        private List<gymnast> gymnastList;
        private ObservableCollection<location> locationList;
        private ObservableCollection<vaultnumber> vaultNumberList;
        private ObservableCollection<Rating> ratingList; // later on
        
        #region Modules

        private IVaultModule vaultModule = new VaultModule();
        private UserModule userModule = new UserModule(); // list of all gymnasts
        private ILocationModule locationModule = new EditorModule();
        private IVaultnumberModule vaultNumberModule = new EditorModule();

        #endregion

        #region DataBinding

        public RelayCommand SaveChangesCommand { get; private set; }
        public RelayCommand CancelChangesCommand { get; private set; }
        public RelayCommand DeleteVaultCommand { get; private set; }
        public RelayCommand CompareCommand { get; private set; }
        public RelayCommand RemoveAllFiltersCommand { get; private set; }
        public RelayCommand SelectedItemsChangedCommand { get; private set; }

        public RelayCommand AddToFilterCommand { get; private set; }
        //do not delete -----------------------------

        public RelayCommand AddDateToFilterCommand { get; private set; }
        
        public PropertyChangedBase Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged("Content");
            }
        }
        //------------------------------------------

        public PropertyChangedBase RatingControl
        {
            get { return ratingControl; }
            set
            {
                ratingControl = value;
                OnPropertyChanged("RatingControl");
            }
        }

        public PropertyChangedBase ModifyViewModelControl
        {
            get { return modifyControl; }
            set
            {
                modifyControl = value;
                OnPropertyChanged("ModifyViewModelControl");
            }
        }

        public Visibility DateVisibility
        {
            get {  return dateVisibility; }
            set
            {
                dateVisibility = value;
                OnPropertyChanged("DateVisibility");
            }
        }

        public String SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                OnPropertyChanged("SelectedDate");
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
                SetFilter();
            }
        }

        public String FilterType
        {
            get { return filterType; }
            set
            {
                filterType = value;
                OnPropertyChanged("FilterType");
                SetFilter();
            }
        }

        public ObservableCollection<String> FilterItems
        {
            get
            {
                return filterItems;
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

        public String SelectedExistingFilterItem
        {
            get { return selectedExistingFilterItem; }
            set 
            {
                selectedExistingFilterItem = value;
                OnPropertyChanged("SelectedExistingFilterItem");
            }
        }

        public ObservableCollection<String> FilterList
        {
            get
            {
                return filterList;
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

            // get all info on startup of this viewmodel
            List<location> locations = vaultModule.getLocations();
            List<vaultnumber> vaultnumbers = vaultModule.getVaultNumbers();
            locationList = new ObservableCollection<location>(locations);
            vaultNumberList = new ObservableCollection<vaultnumber>(vaultnumbers);

            gymnastList = userModule.getGymnastCollection();

            modifyVaultVM = new ModifyVaultViewModel(_app, "SELECT", gymnastList, vaultnumbers, vaultModule.getVaultKinds(), locations);
            ModifyViewModelControl = modifyVaultVM;
            this.Content = modifyVaultVM;
            modifyVaultVM.setData(null);

            dateVisibility = Visibility.Hidden;
            OnPropertyChanged("DateVisibility");

            FilterText = "";
            filterList = new ObservableCollection<string>();

        }

        public void RefreshList()
        {
            ShowFilteredThumbnails(filterList);
            OnPropertyChanged("ThumbnailCollection");
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

        private void SetFilter()
        {
            if (filterType != null)
            {
                string[] valueOfType = filterType.Split(' ');
                filterItems = new ObservableCollection<String>();
                if (valueOfType[1].Equals("Gymnast"))
                {
                    filterItems.Clear();
                    dateVisibility = Visibility.Hidden;
                    foreach (gymnast gymnast in gymnastList)
                    {
                        String tempFullname = gymnast.name + (!String.IsNullOrWhiteSpace(gymnast.surname_prefix) ? " " + gymnast.surname_prefix + " " : " ") + gymnast.surname;
                        if (tempFullname.ToLower().Contains(FilterText.ToLower()))
                        {
                            filterItems.Add(tempFullname);
                        }
                    }
                   
                }
                if (valueOfType[1].Equals("Location"))
                {
                    filterItems.Clear();
                    dateVisibility = Visibility.Hidden;
                    foreach (location location in locationList)
                    {
                        String tempLocationName = location.name;
                        if (tempLocationName.ToLower().Contains(FilterText.ToLower()))
                        {
                            filterItems.Add(tempLocationName);
                        }
                    }
                }
                if(valueOfType[1].Equals("Vault"))
                {
                    filterItems.Clear();
                    dateVisibility = Visibility.Hidden;
                    foreach(vaultnumber number in vaultNumberList)
                    {
                        String tempVaultnumber = number.code;
                        if(tempVaultnumber.ToLower().Contains(FilterText.ToLower()))
                        {
                            filterItems.Add(tempVaultnumber);
                        }
                    }
                }
                if(valueOfType[1].Equals("Date"))
                {
                    filterItems.Clear();
                    dateVisibility = Visibility.Visible;
                    OnPropertyChanged("DateVisibility");
                }
            }
            OnPropertyChanged("DateVisibility");
            OnPropertyChanged("FilterItems");
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
                    modifyVaultVM.ThumbnailCollection.Remove(SelectedThumbnails[i]);
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
                _app.ShowVideoPlaybackView(vaults[0]);
            }
            else if(SelectedThumbnails.Count == 2)
            {
                _app.ShowCompareVaultsView(vaults);
            }
        }

        public void RemoveAllFilters(object commandParam)
        {
            string itemFromFilter = selectedExistingFilterItem;
            filterList.Remove(itemFromFilter);
            ShowFilteredThumbnails(filterList);
            OnPropertyChanged("ModifyViewModelControl");
            OnPropertyChanged("FilterList");
        }

        public void AddToFilters(object commandParam)
        {
            string itemToFilter = selectedFilterItem;
            if (itemToFilter != null)
            {
                string[] valueToFilter = filterType.Split(' ');
                string checkDuplicates = valueToFilter[1] + ":" + itemToFilter;

                foreach (String newFilter in filterList)
                {
                    if (newFilter.Equals(checkDuplicates))
                    {
                        MessageBox.Show("Can't add the same filter, please choose another one.");
                        return;
                    }
                }
                // no duplicates, add to list
                filterList.Add(valueToFilter[1] + ":" + itemToFilter);
                ShowFilteredThumbnails(filterList);
            }
            OnPropertyChanged("FilterList");
        }

        public void AddDateToFilter(object commandParam)
        {
            filterItems.Add(selectedDate.Split(' ')[0]);
            OnPropertyChanged("FilterItems");
        }

        public void ShowFilteredThumbnails(ObservableCollection<string> itemsToFilter)
        {
            Dictionary<string, string> categoryAndName = new Dictionary<string, string>();
            List<string> gymnastValues = new List<string>();
            List<string> locationValues = new List<string>();
            List<string> vaultNumberValues = new List<string>();
            List<string> dateValues = new List<string>();

            List<decimal> eRatings = new List<decimal>();
            List<decimal> dRatings = new List<decimal>();

            for (int k = 0; k < itemsToFilter.Count; k++)
            {
                categoryAndName.Add(itemsToFilter[k].Split(':')[1], itemsToFilter[k].Split(':')[0]);
            }

            for (int j = 0; j < categoryAndName.Count; j++)
            {
                if (categoryAndName.ElementAt(j).Value.Equals("Gymnast"))
                {
                    gymnastValues.Add(categoryAndName.ElementAt(j).Key);
                }
                else if (categoryAndName.ElementAt(j).Value.Equals("Location"))
                {
                    locationValues.Add(categoryAndName.ElementAt(j).Key);
                }
                else if(categoryAndName.ElementAt(j).Value.Equals("Vault"))
                {
                    vaultNumberValues.Add(categoryAndName.ElementAt(j).Key);
                }
                else if(categoryAndName.ElementAt(j).Value.Equals("Date"))
                {
                    dateValues.Add(categoryAndName.ElementAt(j).Key);
                }
            }

            List<vault> newVaults = vaultModule.filter(dRatings, eRatings, gymnastValues, locationValues, vaultNumberValues,dateValues);
            modifyVaultVM.setData(newVaults);
            OnPropertyChanged("ModifyViewModelControl");
        }
        
        #endregion RelayCommands

        protected override void initRelayCommands()
        {
            SaveChangesCommand = new RelayCommand(SaveChanges);
            CancelChangesCommand = new RelayCommand(CancelChanges);
            DeleteVaultCommand = new RelayCommand(DeleteVault);
            CompareCommand = new RelayCommand(CompareAction);
            RemoveAllFiltersCommand = new RelayCommand(RemoveAllFilters);
            AddToFilterCommand = new RelayCommand(AddToFilters);
            AddDateToFilterCommand = new RelayCommand(AddDateToFilter);
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
                    updateFields();
                }
                
            });
        }
    }
}