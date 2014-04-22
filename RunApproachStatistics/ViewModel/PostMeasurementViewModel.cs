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

        private int ratingValue;
        private bool star1Checked;
        private bool star2Checked;
        private bool star3Checked;
        private bool star4Checked;
        private bool star5Checked;
        private bool[] starArray = new bool[6];

        private string gymnast;
        private string datetime;
        private string timespan;
        private string vaultNumber;
        private string location;
        private string vaultKind;
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

        public string VaultKind
        {
            get { return vaultKind; }
            set
            {
                vaultKind = value;
                OnPropertyChanged("VaultKind");
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

        public int RatingValue
        {
            get { return ratingValue; }
            set
            {
                ratingValue = value;
                Console.WriteLine(ratingValue);
                for (int i = 1; i < 6; i++)
                {

                    if (i <= ratingValue)
                    {
                        starArray[i] = true;
                    }
                    else
                    {
                        starArray[i] = false;
                    }
                }
                star1Checked = starArray[1];
                star2Checked = starArray[2];
                star3Checked = starArray[3];
                star4Checked = starArray[4];
                star5Checked = starArray[5];
                OnPropertyChanged("Star1Checked");
                OnPropertyChanged("Star2Checked");
                OnPropertyChanged("Star3Checked");
                OnPropertyChanged("Star4Checked");
                OnPropertyChanged("Star5Checked");
                OnPropertyChanged("RatingValue");
            }
        }
        public bool Star1Checked
        {
            get { return star1Checked; }
            set
            {
                star1Checked = value;
                RatingValue = 1;
                OnPropertyChanged("Star1Checked");
            }
        }
        public bool Star2Checked
        {
            get { return star2Checked; }
            set
            {
                star2Checked = value;
                RatingValue = 2;
                OnPropertyChanged("Star2Checked");

            }
        }
        public bool Star3Checked
        {
            get { return star3Checked; }
            set
            {
                star3Checked = value;
                RatingValue = 3;
                OnPropertyChanged("Star3Checked");
            }
        }
        public bool Star4Checked
        {
            get { return star4Checked; }
            set
            {
                star4Checked = value;
                RatingValue = 4;
                OnPropertyChanged("Star4Checked");

            }
        }
        public bool Star5Checked
        {
            get { return star5Checked; }
            set
            {
                star5Checked = value;
                RatingValue = 5;
                OnPropertyChanged("Star5Checked");
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
