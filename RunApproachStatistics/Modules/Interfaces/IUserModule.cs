using RunApproachStatistics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules.Interfaces
{
    interface IUserModule
    {
        void create(Gymnast gymnast);

        Gymnast read(int id);

        void update(Gymnast gymnast);

        void delete();
    }
}
