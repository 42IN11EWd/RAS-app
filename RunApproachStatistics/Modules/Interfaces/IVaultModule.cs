using RunApproachStatistics.Model;
using RunApproachStatistics.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules.Interfaces
{
    interface IVaultModule
    {
        void create(vault vault);

        vault read(int id);

        void update(vault vault);

        void delete(int id);

        List<vault> getVaults();

        List<vault> filter(double[] dRatings, double[] eRatings, int[] gymnasts, int[] locations, DateTime[] timestamps);

        List<vault> filter(double[] dRatings, double[] eRatings, String[] gymnasts, String[] locations, DateTime[] timestamps);
    }
}
