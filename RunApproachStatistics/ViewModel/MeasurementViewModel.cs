using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using RunApproachStatistics.View;
using System;


namespace RunApproachStatistics.ViewModel
{
    public class MeasurementViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase ratingControl;

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

        public PropertyChangedBase RatingControl
        {
            get { return ratingControl; }
            set
            {
                ratingControl = value;
                OnPropertyChanged("RatingControl");
            }
        }

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

        

        #endregion

        public MeasurementViewModel(IApplicationController app) : base()
        {
            _app = app;
            Measuring = false;
            RatingViewModel ratingVM = new RatingViewModel(_app);
            RatingControl = ratingVM;
            
            

            //put data in array for testing
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
