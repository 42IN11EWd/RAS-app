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
        public int WidthChart { get; set; }

        public int SizeAxisTime { get; set; }
        public int SizeAxisDistance { get; set; }
        public int SizeAxisSpeed { get; set; }

        public List<KeyValuePair<float,float>> DistanceArray { get; set; }

        public List<KeyValuePair<float, float>> SpeedArray { get; set; }

        public GraphViewModel(IApplicationController app) : this(app, 2000)
        {
        }

        public GraphViewModel(IApplicationController app, int width) : base()
        {
            _app = app;
            //TODO check which viewmodel is active and set properties to specific VM

            WidthChart = width;
            SizeAxisTime = 30;
            SizeAxisDistance = 30;
            SizeAxisSpeed = 30;

            // Example data
            setDistances();
            setSpeeds();
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
