using RunApproachStatistics.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public List<KeyValuePair<float,float>> DistanceArray { get; set; }

        public List<KeyValuePair<float, float>> SpeedArray { get; set; }

        public GraphViewModel(IApplicationController app, AbstractViewModel chooseVM, Boolean isLive) : base()
        {
            _app = app;

            DistanceArray = new List<KeyValuePair<float, float>>();
            SpeedArray = new List<KeyValuePair<float, float>>();

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
                timer = new System.Timers.Timer();
                timer.Interval = 1000D;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
                timer.Start();
            }
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            String measurement = App.portController.getLatestMeasurement();
            //DistanceArray.Add(new KeyValuePair<float, float>(seconds, ));
        }

        private void setDistances()
        {
            DistanceArray = new List<KeyValuePair<float, float>>();
            float j = 0;
            for (float i = 0; i < 9; i++)
            {
                DistanceArray.Add(new KeyValuePair<float, float>(i, j));
                j += 3;
            }
        }

        private void setSpeeds()
        {
            SpeedArray = new List<KeyValuePair<float, float>>();
            SpeedArray.Add(new KeyValuePair<float, float>(0, 2));
            SpeedArray.Add(new KeyValuePair<float, float>(1, 4));
        }

        protected override void initRelayCommands()
        {

        }
    }
}
