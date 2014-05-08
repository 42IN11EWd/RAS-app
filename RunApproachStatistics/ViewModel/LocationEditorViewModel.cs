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
    class LocationEditorViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private ILocationModule locationModule;

        private PropertyChangedBase content;
        private ObservableCollection<location> locations;
        private location selectedItem;
        private bool buttonEnabled;
        

        #region Bindings
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand NewCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand BackCommand { get; private set; }
        public PropertyChangedBase Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged("Content");
            }
        }
        public ObservableCollection<location> Locations
        {
            get { return locations; }
            set
            {
                locations = value;
                OnPropertyChanged("Locations");
            }
        }
        public location SelectedItem
        {
            get 
            {
                if (selectedItem == null)
                    ButtonEnabled = false;
                else
                    ButtonEnabled = true;
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
                OnPropertyChanged("Name");
                OnPropertyChanged("Description");
                
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

        public String Name
        {
            get {
                if (SelectedItem != null)
                    return SelectedItem.name;
                return "";
            }
            set 
            { 
                SelectedItem.name = value;
                OnPropertyChanged("Name");
            }
        }
        public String Description
        {
            get
            {
                if (SelectedItem != null)
                    return SelectedItem.description;
                return "";
            }
            set 
            {
                SelectedItem.description = value;
                OnPropertyChanged("Description");
            }
        }

        #endregion

        public LocationEditorViewModel(IApplicationController app) : base()
        {
            _app = app;
            locationModule = new EditorModule();
            Locations = locationModule.readLocations();

        }

        #region RelayCommands

        public void DeleteAction(object commandParam)
        {
            locationModule.deleteLocation(SelectedItem.location_id);
            Locations.Remove(SelectedItem);
        }

        public void NewAction(object commandParam)
        {
            location newlocation = new location();
            Locations.Add(newlocation);
            SelectedItem = newlocation;
        }

        public void SaveAction(object commandParam)
        {
            locationModule.updateLocation(SelectedItem);
            Locations = locationModule.readLocations();
        }

        public void BackAction(object commandParam)
        {
            _app.ShowSettingsView();
        }

        #endregion

        protected override void initRelayCommands()
        {
            DeleteCommand = new RelayCommand(DeleteAction);
            NewCommand = new RelayCommand(NewAction);
            SaveCommand = new RelayCommand(SaveAction);
            BackCommand = new RelayCommand(BackAction);
        }
    }
}
