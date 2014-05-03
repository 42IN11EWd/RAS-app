using RunApproachStatistics.Controllers;
using RunApproachStatistics.Model.Entity;
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
        private PropertyChangedBase content;
        private ObservableCollection<location> locations = new ObservableCollection<location>();
        private location selectedItem;        

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
                this.OnPropertyChanged("Locations");
            }
        }
        public location SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
                OnPropertyChanged("Name");
                OnPropertyChanged("Description");
                
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
            //fill locations list
            locations.Add(new location{
                name = "Flikflak",
                description = "flikflak hal in den bosch"
            });
            locations.Add(new location
            {
                name = "Eindhoven",
                description = "gymnastiek hal in eindhoven"
            });

        }

        #region RelayCommands

        public void DeleteAction(object commandParam)
        {
            SelectedItem.deleted = true;
            locations.Remove(SelectedItem);
        }
        public void NewAction(object commandParam)
        {
            //new location
            location newlocation = new location();
            locations.Add(newlocation);
            SelectedItem = newlocation;
        }
        public void SaveAction(object commandParam)
        {
            //save changes
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
