using RunApproachStatistics.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.ViewModel
{
    class LockViewModel : AbstractViewModel
    {
        private IApplicationController _app;

        public LockViewModel(IApplicationController app) : base()
        {
            _app = app;
        }
        protected override void initRelayCommands()
        {
        }
    }
}
