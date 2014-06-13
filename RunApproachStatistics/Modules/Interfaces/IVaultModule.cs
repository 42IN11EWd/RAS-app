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

        List<String> getLocationNames();

        List<String> getGymnastNames();

        List<String> getVaultNumberNames();

        List<String> getVaultKindNames();

        List<int> getLocationIds();

        List<int> getGymnastIds();

        List<int> getVaultNumberIds();

        List<int> getVaultKindIds();

        List<gymnast> getGymnasts();

        List<vaultkind> getVaultKinds();

        List<vaultnumber> getVaultNumbers();

        List<location> getLocations();

        //List<vault> filter(decimal[] dRatings, decimal[] eRatings, int[] gymnasts, int[] locations, DateTime[] timestamps);

        //List<vault> filter(decimal[] dRatings, decimal[] eRatings, String[] gymnasts, String[] locations, DateTime[] timestamps);

        List<vault> filter(List<decimal> dRatings, List<decimal> eRatings, List<int> gymnasts, List<int> locations);

        List<vault> filter(List<decimal> dRatings, List<decimal> eRatings, List<string> gymnasts, List<string> locations, List<string> vaultNumbers, List<string> dateValues);
    }
}
