using RunApproachStatistics.Model.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules.Interfaces
{
    interface IEditorModule
    {
        //Location methods
        void deleteLocation(int id);
        ObservableCollection<location> readLocation();
        void createLocation(location location);
        void updateLocation(location location);

        //Vaultnumber methods
        void deleteVaultNumber(int id);
        ObservableCollection<vaultnumber> readVaultnumber();
        void createVaultnumber(vaultnumber vaultnumber);
        void updateVaultnumber(vaultnumber vaultnumber);
    }
}
