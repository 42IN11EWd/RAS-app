﻿using RunApproachStatistics.Controllers;
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

        private ObservableCollection<KeyValuePair<float, float>> distanceArray;
        private ObservableCollection<KeyValuePair<float, float>> speedArray;
        private Thickness lineMargin;
        private Visibility lineVisibilty;
        private float graphSeconds;
        private String graphData;

        // Enable customization (show/hide second axis (default distance) and set name of first axis)
        private Visibility hasSecondAxis;
        private String axisTitle;

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

        public Visibility LineVisibilty
        {
            get { return lineVisibilty; }
            set
            {
                lineVisibilty = value;
                OnPropertyChanged("LineVisibilty");
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
            }
            else
            {
                GraphSeconds = 10;
                LineVisibilty = Visibility.Visible;
                LineMargin = new Thickness(87, 25, 0, 0);
            }

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
            AxisTitle = "Distance";
            OnPropertyChanged("AxisTitleColor");
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
            AxisTitle = type;
            OnPropertyChanged("AxisTitleColor");

            SpeedArray = new ObservableCollection<KeyValuePair<float, float>>();
            DistanceArray = new ObservableCollection<KeyValuePair<float, float>>();
            double timePerMeasurementVault1 = GraphSeconds / vault1Data.Length;
            double timePerMeasurementVault2 = GraphSeconds / vault2Data.Length;
            float time = 0;

            foreach (String vault1Measurement in vault1Data)
            {
                if (!String.IsNullOrWhiteSpace(vault1Measurement))
                {
                    SpeedArray.Add(new KeyValuePair<float, float>(time, float.Parse(vault1Measurement, CultureInfo.InvariantCulture)));
                    time += (float)timePerMeasurementVault1;
                }
            }

            time = 0;
            foreach (String vault2Measurement in vault2Data)
            {
                if (!String.IsNullOrWhiteSpace(vault2Measurement))
                {
                    DistanceArray.Add(new KeyValuePair<float, float>(time, float.Parse(vault2Measurement, CultureInfo.InvariantCulture)));
                    time += (float)timePerMeasurementVault2;
                }
            }

            OnPropertyChanged("DistanceArray");
            OnPropertyChanged("SpeedArray");
        }

        public void insertGraphData(String graphData)
        {
            this.graphData = graphData;
            DistanceArray = new ObservableCollection<KeyValuePair<float, float>>();
            SpeedArray = new ObservableCollection<KeyValuePair<float, float>>();

            String[] measurements = graphData.Split(',');
            Boolean hasSpeed = measurements[0].Split(' ')[1] != null;
            double timePerMeasurement = GraphSeconds / measurements.Length;
            float time = 0;

            if (hasSpeed)
            {
                foreach (String measurement in measurements)
                {
                    if (measurement.Length > 0)
                    {
                        String[] splitString = measurement.Split(' ');

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

        public void updateGraphLength(float seconds, String graphData = null)
        {
            GraphSeconds = seconds;

            if (graphData == null)
            {
                insertGraphData(this.graphData);
            }
            else
            {
                insertGraphData(graphData);
            }
        }

        public void updateSlider(double milliseconds)
        {
            double percentage = milliseconds / GraphSeconds;
            lineMargin.Left = ((WidthChart - 87 - 87) * percentage) + 87; //Een left margin van 87 pixels is precies 0 op de grafiek

            if (!(lineMargin.Left > (WidthChart - 87)))
            {
                LineMargin = lineMargin;
            }
        }

        protected override void initRelayCommands()
        {

        }
    }
}
