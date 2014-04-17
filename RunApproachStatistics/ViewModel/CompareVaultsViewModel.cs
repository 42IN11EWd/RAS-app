using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.ViewModel
{
    public class CompareVaultsViewModel : AbstractViewModel
    {
        private IApplicationController _app;

        #region Modules

        private IVaultModule vaultModule = new VaultModule();

        #endregion

        public CompareVaultsViewModel(IApplicationController app) : base()
        {
            _app = app;
        }

        protected override void initRelayCommands()
        {
        }
    }
}
