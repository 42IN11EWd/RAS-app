using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using System;


namespace RunApproachStatistics.ViewModel
{
    public class MeasurementViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private int ratingValue;
        private bool star1Checked;
        private bool star2Checked;
        private bool star3Checked;
        private bool star4Checked;
        private bool star5Checked;
        private bool[] starArray = new bool[6];

        private string vaultKind;
        public string[] vaultKindArray;
        private string location;
        private string gymnast;
        private string vaultNumber;
        private string dscore;
        private string escore;
        private string penalty;
        private string totalscore;

        private bool manualModeChecked;
        private bool measuring;
        private string measurementButtonContent;

        #region Modules

        private ICameraModule cameraModule = new VaultModule();

        #endregion

        #region DataBinding

        public RelayCommand PostMeasurementCommand { get; private set; }
        public RelayCommand StartMeasurementCommand { get; private set; }

        public bool Measuring
        {
            get { return measuring; }
            set
            {
                measuring = value;
                if(value == true && ManualModeChecked)
                {
                    MeasurementButtonContent = "Stop Measurement";
                }
                else if(value == false && ManualModeChecked) 
                {
                    MeasurementButtonContent = "Start Measurement";
                }
                else
                {
                    MeasurementButtonContent = "";
                }
                
                OnPropertyChanged("Measuring");
            }
        }

        public bool ManualModeChecked
        {
            get { return manualModeChecked; }
            set
            {
                manualModeChecked = value;
                //turn manual mode on
                Measuring = false;
                OnPropertyChanged("ManualModeChecked");
            }
        }

        public string MeasurementButtonContent
        {
            get { return measurementButtonContent; }
            set
            {
                measurementButtonContent = value;
                OnPropertyChanged("MeasurementButtonContent");
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

        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged("Location");
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

        public string VaultNumber
        {
            get { return vaultNumber; }
            set
            {
                vaultNumber = value;
                OnPropertyChanged("VaultNumber");
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
                for (int i = 1; i < 6; i++ )
                {
                    
                    if(i <= ratingValue)
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

        public MeasurementViewModel(IApplicationController app) : base()
        {
            _app = app;
            Measuring = false;
            vaultKindArray = new string[3];
            vaultKindArray[0] = "practice";
            vaultKindArray[1] = "nk";
            vaultKindArray[2] = "ek";
        }

        #region RelayCommands

        public void LoadPostMeasurementScreen(object commandParam)
        {
            _app.ShowPostMeasurementView();
        }
        public void StartMeasurement(object commandParam)
        {
            //start measurement
            if(Measuring)
            {
                Measuring = false;
            }
            else
            {
                Measuring = true;
            }
            


        }

        #endregion

        protected override void initRelayCommands()
        {
            PostMeasurementCommand = new RelayCommand(LoadPostMeasurementScreen);
            StartMeasurementCommand = new RelayCommand(StartMeasurement);
        }
       
    }
}
