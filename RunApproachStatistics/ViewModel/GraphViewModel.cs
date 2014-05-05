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
        public GraphViewModel(IApplicationController app,AbstractViewModel chooseVM) : base()
        {
            _app = app;
            //MessageBox.Show(chooseVM.ToString());
            DisplayWidth = 3000;
            WidthChart = 3000;
            GridWidth = 3000;
            SizeAxisTime = 30;
            SizeAxisDistance = 30;
            SizeAxisSpeed = 30;
            //TODO check which viewmodel is active and set properties to specific VM
        }

        protected override void initRelayCommands()
        {

        }
    }
}
