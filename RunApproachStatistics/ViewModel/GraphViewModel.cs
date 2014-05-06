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
        public int DisplayWidth { get; set; }
        public int WidthChart { get; set; }
        public int GridWidth { get; set; }

        public int SizeAxisTime { get; set; }
        public int SizeAxisDistance { get; set; }
        public int SizeAxisSpeed { get; set; }

        public List<KeyValuePair<float,float>> DistanceArray { get; set; }

        public GraphViewModel(IApplicationController app, AbstractViewModel chooseVM) : base()
        {
            _app = app;
            //TODO check which viewmodel is active and set properties to specific VM
            if (chooseVM.ToString().Equals("RunApproachStatistics.ViewModel.HomeViewModel"))
            {
                DisplayWidth = 3000;
                WidthChart = 3000;
                GridWidth = 3000;
                SizeAxisTime = 30;
                SizeAxisDistance = 30;
                SizeAxisSpeed = 30;
            }
            else
            {
                DisplayWidth = 2000;
                WidthChart = 2000;
                GridWidth = 2000;
                SizeAxisTime = 30;
                SizeAxisDistance = 30;
                SizeAxisSpeed = 30;
            }
            // Example data
            setDistances();
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

        protected override void initRelayCommands()
        {

        }
    }
}
