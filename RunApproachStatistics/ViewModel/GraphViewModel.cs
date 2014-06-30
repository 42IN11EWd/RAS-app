using RunApproachStatistics.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
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
        public double AxisMaxLeft { get; set; }
        public double AxisMaxRight { get; set; }
        public double AxisInterval { get; set; }

        private ObservableCollection<KeyValuePair<float, float>> distanceArray;
        private ObservableCollection<KeyValuePair<float, float>> speedArray;
        private Thickness lineMargin;
        private Thickness lineMargin2;
        private Brush lineOneColor;
        private Brush lineTwoColor;
        private Visibility lineVisibilty;
        private Visibility lineVisibilty2;
        private float graphSeconds;
        private String graphData;
        private Double axisTimeMaximum;

        private String type;
        private String[] vault1Data;
        private String[] vault2Data;

        // Enable customization (show/hide second axis (default distance) and set name of first axis)
        private Visibility hasSecondAxis;
        private String axisTitle;
        private Boolean IsSpecializedGraph { get; set; }

        public ObservableCollection<KeyValuePair<float, float>> DistanceArray
        {
            get { return distanceArray; }
            set
            {
                distanceArray = value;
                OnPropertyChanged("DistanceArray");
            }
        }

        public ObservableCollection<KeyValuePair<float, float>> SpeedArray
        {
            get { return speedArray; }
            set
            {
                speedArray = value;
                OnPropertyChanged("SpeedArray");
            }
        }

        public Thickness LineMargin
        {
            get { return lineMargin; }
            set
            {
                lineMargin = value;
                OnPropertyChanged("LineMargin");
            }
        }

        public Thickness LineMargin2
        {
            get { return lineMargin2; }
            set
            {
                lineMargin2 = value;
                OnPropertyChanged("LineMargin2");
            }
        }

        public Brush LineOneColor
        {
            get { return lineOneColor; }
            set
            {
                lineOneColor = value;
                OnPropertyChanged("LineOneColor");
            }
        }

        public Brush LineTwoColor
        {
            get { return lineTwoColor; }
            set
            {
                lineTwoColor = value;
                OnPropertyChanged("LineOneColor");
            }
        }

        public Visibility LineVisibilty
        {
            get { return lineVisibilty; }
            set
            {
                lineVisibilty = value;
                OnPropertyChanged("LineVisibilty");
            }
        }

        public Visibility LineVisibilty2
        {
            get { return lineVisibilty2; }
            set
            {
                lineVisibilty2 = value;
                OnPropertyChanged("LineVisibilty2");
            }
        }

        public Visibility HasSecondAxis
        {
            get { return hasSecondAxis; }
            set
            {
                hasSecondAxis = value;
                OnPropertyChanged("HasSecondAxis");
            }
        }

        public double AxisTimeMaximum
        {
            get { return axisTimeMaximum; }
            set
            {
                axisTimeMaximum = value;
                OnPropertyChanged("AxisTimeMaximum");
            }
        }

        public String AxisTitle
        {
            get { return axisTitle; }
            set
            {
                axisTitle = value;
                OnPropertyChanged("AxisTitle");
            }
        }

        public Brush AxisTitleColor
        {
            get { return HasSecondAxis == Visibility.Visible ? Brushes.Blue : Brushes.Black; }
        }

        public float GraphSeconds
        {
            get { return graphSeconds; }
            set
            {
                graphSeconds = value;
                OnPropertyChanged("GraphSeconds");
            }
        }

        public GraphViewModel(IApplicationController app, AbstractViewModel chooseVM, Boolean isLive, int width)
            : base()
        {
            _app = app;

            DistanceArray = new ObservableCollection<KeyValuePair<float, float>>();
            SpeedArray = new ObservableCollection<KeyValuePair<float, float>>();

            if (isLive)
            {
                GraphSeconds = 30;
                LineVisibilty = Visibility.Hidden;
                LineVisibilty2 = Visibility.Hidden;
            }
            else
            {
                GraphSeconds = 10;
                LineVisibilty = Visibility.Visible;
                LineVisibilty2 = Visibility.Hidden;
                LineMargin = new Thickness(87, 25, 0, 0);
                LineMargin2 = new Thickness(87, 25, 0, 0);
            }

            LineOneColor = Brushes.Black;
            LineTwoColor = new SolidColorBrush(Color.FromRgb(250, 42, 42));
            DisplayWidth = width;
            WidthChart = width;
            GridWidth = width;
            SizeAxisTime = 30;
            SizeAxisDistance = 30;
            SizeAxisSpeed = 30;

            if (isLive)
            {
                seconds = -1;
                timer = new System.Timers.Timer();
                timer.Interval = 500D;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
                timer.Start();
            }

            // Setup default customization
            HasSecondAxis = Visibility.Visible;
            AxisTitle = "Distance (m)";
            OnPropertyChanged("AxisTitleColor");
            IsSpecializedGraph = false;
            AxisTimeMaximum = GraphSeconds;
            AxisMaxLeft = 30;
            AxisMaxRight = 10;
            AxisInterval = 5;
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
                if (splitString.Length == 2)
                {
                    DistanceArray.Add(new KeyValuePair<float, float>(seconds, float.Parse(splitString[1], CultureInfo.InvariantCulture)));
                    SpeedArray.Add(new KeyValuePair<float, float>(seconds, float.Parse(splitString[0], CultureInfo.InvariantCulture)));
                }
                else
                {
                    DistanceArray.Add(new KeyValuePair<float, float>(seconds, float.Parse(splitString[0], CultureInfo.InvariantCulture)));
                }

                OnPropertyChanged("DistanceArray");
                OnPropertyChanged("SpeedArray");
            }));
        }

        public void insertGraphData(String graphData)
        {
            this.graphData = graphData;

            if (this.graphData != null && this.graphData.Length > 0)
            {
                DistanceArray = new ObservableCollection<KeyValuePair<float, float>>();
                SpeedArray = new ObservableCollection<KeyValuePair<float, float>>();

                String[] measurements = graphData.Split(',');
                Boolean hasSpeed = measurements[0].Split(' ')[1] != null;
                double timePerMeasurement = GraphSeconds / (measurements.Length - 1);
                float time = 0;

                if (hasSpeed)
                {
                    foreach (String measurement in measurements)
                    {
                        if (measurement.Length > 0)
                        {
                            String[] splitString = measurement.Split(' ');
                            if (splitString.Length <= 1)
                            {
                                break;
                            }

                            SpeedArray.Add(new KeyValuePair<float, float>(time, float.Parse(splitString[0], CultureInfo.InvariantCulture)));
                            DistanceArray.Add(new KeyValuePair<float, float>(time, float.Parse(splitString[1], CultureInfo.InvariantCulture)));
                            time += (float)timePerMeasurement;
                        }
                    }
                }
                else
                {
                    foreach (String distance in measurements)
                    {
                        if (distance.Length > 0)
                        {
                            DistanceArray.Add(new KeyValuePair<float, float>(time, float.Parse(distance, CultureInfo.InvariantCulture)));
                            time += (float)timePerMeasurement;
                        }
                    }
                }

                OnPropertyChanged("DistanceArray");
                OnPropertyChanged("SpeedArray");
            }
        }

        public void updateGraphLength(float seconds, String graphData = null)
        {
            if (IsSpecializedGraph)
            {
                return;
            }

            GraphSeconds = seconds;
            AxisTimeMaximum = seconds;

            if (graphData == null)
            {
                insertGraphData(this.graphData);
            }
            else
            {
                insertGraphData(graphData);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Type of graph</param>
        /// <param name="vault1Data">Data for player 1 (Red) of this particular type of graph</param>
        /// <param name="vault2Data">Data for player 2 (Blue) of this particular type of graph</param>
        public void setupSpecializedGraph(String type, String[] vault1Data, String[] vault2Data)
        {
            if (String.IsNullOrWhiteSpace(type) || !(type.Equals("Speed") || type.Equals("Distance")))
            {
                return;
            }

            HasSecondAxis = Visibility.Hidden;

            if (type.Equals("Speed"))
            {
                AxisTitle = type + " (m/s)";
            }
            else
            {
                AxisTitle = type + " (m)";
            }
            OnPropertyChanged("AxisTitleColor");

            this.type = type;
            this.vault1Data = vault1Data;
            this.vault2Data = vault2Data;

            SpeedArray = new ObservableCollection<KeyValuePair<float, float>>();
            DistanceArray = new ObservableCollection<KeyValuePair<float, float>>();

            LineOneColor = new SolidColorBrush(Color.FromRgb(250, 42, 42));
            LineTwoColor = new SolidColorBrush(Color.FromRgb(42, 84, 250));
            LineVisibilty2 = Visibility.Visible;

            IsSpecializedGraph = true;
            AxisInterval = type.Equals("Distance") ? 5 : 2;
            AxisMaxLeft = type.Equals("Distance") ? 30 : 10;
            AxisMaxRight = type.Equals("Distance") ? 30 : 10;
            OnPropertyChanged("DistanceArray");
            OnPropertyChanged("SpeedArray");
        }

        public void updateSpecializedGraphLength(float seconds1, float seconds2)
        {
            if (seconds1.Equals(0) || seconds2.Equals(0))
            {
                return;
            }

            AxisTimeMaximum = Math.Max(seconds1, seconds2);

            double timePerMeasurementVault1 = seconds1 / (vault1Data.Length - 1);
            double timePerMeasurementVault2 = seconds2 / (vault2Data.Length - 1);
            float time1 = 0;
            float time2 = 0;

            foreach (String vault1Measurement in vault1Data)
            {
                if (!String.IsNullOrWhiteSpace(vault1Measurement))
                {
                    SpeedArray.Add(new KeyValuePair<float, float>(time1, float.Parse(vault1Measurement, CultureInfo.InvariantCulture)));
                    time1 += (float)timePerMeasurementVault1;
                }
            }

            foreach (String vault2Measurement in vault2Data)
            {
                if (!String.IsNullOrWhiteSpace(vault2Measurement))
                {
                    DistanceArray.Add(new KeyValuePair<float, float>(time2, float.Parse(vault2Measurement, CultureInfo.InvariantCulture)));
                    time2 += (float)timePerMeasurementVault2;
                }
            }
        }

        public void updateSlider(double milliseconds)
        {
            double percentage = milliseconds / GraphSeconds;
            double perc = ((WidthChart - 100 - 100) * percentage) + 100;

            if (perc > (WidthChart - 88))
                perc = (WidthChart - 88);

            if (LineMargin.Left != perc)
            {
                LineMargin = new Thickness(perc, LineMargin.Top, 0, 0);
            }
        }

        public void updateLeftVideoSlider(double milliseconds)
        {
            updateSlider(milliseconds);
        }

        public void updateRightVideoSlider(double milliseconds)
        {
            double percentage = milliseconds / GraphSeconds;
            double perc = ((WidthChart - 100 - 100) * percentage) + 100;

            if (perc > (WidthChart - 88))
                perc = (WidthChart - 88);

            if (LineMargin2.Left != perc)
            {
                LineMargin2 = new Thickness(perc, LineMargin2.Top, 0, 0);
            }
        }

        protected override void initRelayCommands()
        {

        }
    }
}
