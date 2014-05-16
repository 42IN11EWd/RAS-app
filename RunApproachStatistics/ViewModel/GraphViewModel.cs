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
        private float seconds;

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

        public GraphViewModel(IApplicationController app, AbstractViewModel chooseVM, float duration, int width) : base()
        {
            _app = app;

            DistanceArray = new ObservableCollection<KeyValuePair<float, float>>();
            SpeedArray = new ObservableCollection<KeyValuePair<float, float>>();

            //TODO check which viewmodel is active and set properties to specific VM
            DisplayWidth = width;
            WidthChart = width;
            GridWidth = width;
            SizeAxisTime = 30;
            SizeAxisDistance = 30;
            SizeAxisSpeed = 30;

            if (duration <= 0)
            {
                seconds = -1;
                timer = new System.Timers.Timer();
                timer.Interval = 500D;
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

            seconds += (float)0.5;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                SpeedArray.Add(new KeyValuePair<float, float>(seconds, float.Parse(splitString[0], CultureInfo.InvariantCulture)));
               
                try
                {
                    DistanceArray.Add(new KeyValuePair<float, float>(seconds, float.Parse(splitString[1], CultureInfo.InvariantCulture)));
                 }
                catch (Exception ex)
                {
                    //No problem
                }

                OnPropertyChanged("DistanceArray");
                OnPropertyChanged("SpeedArray");
            }));
        }

        protected override void initRelayCommands()
        {

        }
    }
}
