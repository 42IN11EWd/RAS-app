using RunApproachStatistics.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules
{
    class CompareVaultsModule : ICompareVaultsModule
    {
        #region Modules

        private IUserModule userModule = new UserModule();
        private IVaultModule userModule = new VaultModule();

        #endregion
    }
}
