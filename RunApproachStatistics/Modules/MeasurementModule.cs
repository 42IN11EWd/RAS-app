using RunApproachStatistics.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules
{
    class MeasurementModule : IMeasurementModule
    {
        #region Modules

        private IUserModule userModule = new UserModule();
        private IVaultModule vaultModule = new VaultModule();

        #endregion
    }
}
