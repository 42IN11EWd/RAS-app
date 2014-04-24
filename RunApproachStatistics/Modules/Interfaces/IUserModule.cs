using RunApproachStatistics.Model;
using RunApproachStatistics.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules.Interfaces
{
    interface IUserModule
    {
        void create(gymnast gymnast);

        gymnast read(int id);

        void update(gymnast gymnast);

        void delete(int id);

        List<gymnast> getGymnastCollection(String filter);

        List<gymnast> getGymnastCollection();
    }
}
