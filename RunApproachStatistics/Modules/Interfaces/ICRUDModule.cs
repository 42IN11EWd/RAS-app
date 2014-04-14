using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules.Interfaces
{
    public interface ICRUDModule
    {
        void create();
        Object read();
        void update();
        void delete();
    }
}
