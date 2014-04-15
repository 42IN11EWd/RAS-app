using RunApproachStatistics.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.ViewModel
{
    public class PostMeasurementViewModel : AbstractViewModel
    {
        private IApplicationController _app;

        #region Modules

        private IVaultModule vaultModule = new VaultModule();

        #endregion

        public PostMeasurementViewModel(IApplicationController app) : base()
        {
            _app = app;
        }

        protected override void initRelayCommands()
        {
        }
    }
}
