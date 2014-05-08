using RunApproachStatistics.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RunApproachStatistics.ViewModel
{
    public class GraphViewModel : AbstractViewModel
    {
        private IApplicationController _app;

        //live variables
        private System.Timers.Timer timer;
        private int seconds;

        public int DisplayWidth { get; set; }
        public int WidthChart { get; set; }
        public int GridWidth { get; set; }

        public int SizeAxisTime { get; set; }
        public int SizeAxisDistance { get; set; }
        public int SizeAxisSpeed { get; set; }

        private ObservableCollection<KeyValuePair<float, float>> distanceArray;
        private ObservableCollection<KeyValuePair<float, float>> speedArray;

        public ObservableCollection<KeyValuePair<float, float>> DistanceArray
        {
            get { return distanceArray;  }
            set
            {
                distanceArray = value;
                OnPropertyChanged("DistanceArray");
            }
        }

        public ObservableCollection<KeyValuePair<float, float>> SpeedArray
        {
            get { return speedArray;  }
            set
            {
                speedArray = value;
                OnPropertyChanged("SpeedArray");
            }
        }

        public GraphViewModel(IApplicationController app, AbstractViewModel chooseVM, Boolean isLive) : base()
        {
            _app = app;

            DistanceArray = new ObservableCollection<KeyValuePair<float, float>>();
            SpeedArray = new ObservableCollection<KeyValuePair<float, float>>();

            //TODO check which viewmodel is active and set properties to specific VM
            DisplayWidth = 3000;
            WidthChart = 3000;
            GridWidth = 3000;
            SizeAxisTime = 30;
            SizeAxisDistance = 30;
            SizeAxisSpeed = 30;
            // Example data
            //setDistances();
            //setSpeeds();

            if (isLive)
            {
                seconds = -1;
                timer = new System.Timers.Timer();
                timer.Interval = 1000D;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
                timer.Start();
            }
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (seconds > 29)
            {
                DistanceArray = new ObservableCollection<KeyValuePair<float, float>>();
                SpeedArray = new ObservableCollection<KeyValuePair<float, float>>();
                seconds = -1;
            }

            String measurement = App.portController.getLatestMeasurement().Replace(",", "");
            String[] splitString = measurement.Split(' ');

            seconds++;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                DistanceArray.Add(new KeyValuePair<float, float>(seconds, float.Parse(splitString[0], CultureInfo.InvariantCulture)));

                try
                {
                    SpeedArray.Add(new KeyValuePair<float, float>(seconds, float.Parse(splitString[1], CultureInfo.InvariantCulture)));
                }
                catch (Exception ex)
                {
                    //No problem
                }

                OnPropertyChanged("DistanceArray");
                OnPropertyChanged("SpeedArray");
            }));
        }

        private void setDistances()
        {
            DistanceArray = new ObservableCollection<KeyValuePair<float, float>>();
            float j = 0;
            for (float i = 0; i < 9; i++)
            {
                DistanceArray.Add(new KeyValuePair<float, float>(i, j));
                j += 3;
            }
        }

        private void setSpeeds()
        {
            SpeedArray = new ObservableCollection<KeyValuePair<float, float>>();
            SpeedArray.Add(new KeyValuePair<float, float>(0, 2));
            SpeedArray.Add(new KeyValuePair<float, float>(1, 4));
        }

        protected override void initRelayCommands()
        {

        }
    }
}
