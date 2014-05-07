using RunApproachStatistics.Model.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules.Interfaces
{
    interface IVaultnumberModule
    {
        void deleteVaultNumber(int id);
        ObservableCollection<vaultnumber> readVaultnumbers();
        void createVaultnumber(vaultnumber vaultnumber);
        void updateVaultnumber(vaultnumber vaultnumber);
    }
}
