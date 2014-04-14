using RunApproachStatistics.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.ViewModel
{
    public class CameraViewModel : AbstractViewModel
    {
        private IApplicationController _app;

        public CameraViewModel(IApplicationController app) : base()
        {
            _app = app;
        }

        protected override void initRelayCommands()
        {
        }
    }
}
