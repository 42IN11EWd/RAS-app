using RunApproachStatistics.Model.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules.Interfaces
{
    interface ILocationModule
    {
        void deleteLocation(int id);
        ObservableCollection<location> readLocations();
        void createLocation(location location);
        void updateLocation(location location);
    }
}
