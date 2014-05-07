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
    class VaultNumberEditorViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private IVaultnumberModule vaultnumberModule;

        private PropertyChangedBase content;
        private ObservableCollection<vaultnumber> vaults;
        private vaultnumber selectedItem;
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

        public ObservableCollection<vaultnumber> Vaults
        {
            get { return vaults; }
            set
            {
                vaults = value;
                OnPropertyChanged("Vaults");
            }
        }

        public vaultnumber SelectedItem
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
                OnPropertyChanged("Code");
                OnPropertyChanged("Gender");
                OnPropertyChanged("Difficulty");
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
            vaultnumberModule = new EditorModule();
            Vaults = vaultnumberModule.readVaultnumbers();
        }

        #region RelayCommands

        public void DeleteAction(object commandParam)
        {
            vaultnumberModule.deleteVaultNumber(SelectedItem.vaultnumber_id);
            Vaults.Remove(SelectedItem);
        }

        public void NewAction(object commandParam)
        {
            vaultnumber newvault = new vaultnumber();
            Vaults.Add(newvault);
            SelectedItem = newvault;
        }

        public void SaveAction(object commandParam)
        {
            vaultnumberModule.updateVaultnumber(SelectedItem);
            Vaults = vaultnumberModule.readVaultnumbers();
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