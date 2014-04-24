using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using System;
using System.Collections.Generic;
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

        private string vaultKind;
        public string[] vaultKindArray;
        private string gymnast;
        private string datetime;
        private string timespan;
        private string vaultNumber;
        private string location;
        private string dscore;
        private string escore;
        private string penalty;
        private string totalscore;

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

        public string SelectedVaultKind
        {
            get
            {
                if (vaultKind == null) return "";
                return vaultKind;
            }
            set
            {
                vaultKind = value;
                OnPropertyChanged("VaultKind");
            }

        }


        public string[] VaultKind
        {
            get { return vaultKindArray; }
            set
            {
                vaultKindArray = value;
                OnPropertyChanged("VaultKind");
            }
        }

        public string Gymnast
        {
            get { return gymnast; }
            set
            {
                gymnast = value;
                OnPropertyChanged("Gymnast");
            }
        }

        public string Datetime
        {
            get { return datetime; }
            set
            {
                datetime = value;
                OnPropertyChanged("Datetime");
            }
        }

        public string Timespan
        {
            get { return timespan; }
            set
            {
                timespan = value;
                OnPropertyChanged("Timespan");
            }
        } 
        public string VaultNumber
        {
            get { return vaultNumber; }
            set
            {
                vaultNumber = value;
                OnPropertyChanged("VaultNumber");
            }
        }

        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        }

        public string Dscore
        {
            get { return dscore; }
            set
            {
                dscore = value;
                OnPropertyChanged("Dscore");
            }
        }

        public string Escore
        {
            get { return escore; }
            set
            {
                escore = value;
                OnPropertyChanged("Escore");
            }
        }

        public string Penalty
        {
            get { return penalty; }
            set
            {
                penalty = value;
                OnPropertyChanged("Penalty");
            }
        }

        public string Totalscore
        {
            get { return totalscore; }
            set
            {
                totalscore = value;
                OnPropertyChanged("Totalscore");
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
        }

        #region RelayCommands

        public void FinishAction(object commandParam)
        {
            _app.ShowHomeView();
        }
        public void DeleteAction(object commandParam)
        {
            //delete selected thumbnails
        }
        public void CancelAction(object commandParam)
        {
            //cancel current changes to thumbnail(s)
        }
        public void SaveAction(object commandParam)
        {
            //save changes to thumbnail(s)
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
