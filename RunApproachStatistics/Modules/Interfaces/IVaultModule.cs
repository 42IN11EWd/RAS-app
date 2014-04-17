using RunApproachStatistics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules.Interfaces
{
    interface IVaultModule
    {
        void create();

        Vault read(int id);

        void update(Vault vault);

        void delete();

        List<Vault> getVaults(string filter);
    }
}
