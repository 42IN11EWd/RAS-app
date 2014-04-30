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
    class VaultNumberEditorViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase content;
        private ObservableCollection<vaultnumber> vaults = new ObservableCollection<vaultnumber>();
        private vaultnumber selectedItem;

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
        public ObservableCollection<vaultnumber> Vaults
        {
            get { return vaults; }
            set
            {
                vaults = value;
                this.OnPropertyChanged("Locations");
            }
        }
        public vaultnumber SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
                OnPropertyChanged("Code");
                OnPropertyChanged("Gender");
                OnPropertyChanged("Difficulty");
                OnPropertyChanged("Description");

            }
        }
        public String Code
        {
            get
            {
                if (SelectedItem != null)
                    return SelectedItem.code;
                return "";

            }
            set
            {
                SelectedItem.code = value;
                OnPropertyChanged("Code");
            }
        }
        public String Gender
        {
            get
            {
                if (SelectedItem != null)
                    return SelectedItem.male_female;
                return "";

            }
            set
            {
                SelectedItem.male_female = value;
                OnPropertyChanged("Gender");
            }
        }
        public Nullable<decimal> Difficulty
        {
            get
            {
                if (SelectedItem != null)
                    return SelectedItem.difficulty;
                return 0;

            }
            set
            {
                SelectedItem.difficulty = value;
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

        public VaultNumberEditorViewModel(IApplicationController app) : base()
        {
            _app = app;
            //fill vaults list
            vaults.Add(new vaultnumber
            {
                code = "30.1",
                description = "flikflak hal in den bosch"
            });
            vaults.Add(new vaultnumber
            {
                code = "5.12",
                description = "gymnastiek hal in eindhoven"
            });
        }

        #region RelayCommands

        public void DeleteAction(object commandParam)
        {
            SelectedItem.deleted = true;
            vaults.Remove(SelectedItem);
        }
        public void NewAction(object commandParam)
        {
            vaultnumber newvault = new vaultnumber();
            vaults.Add(newvault);
            SelectedItem = newvault;
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