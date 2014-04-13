using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules.Interfaces
{
    public interface ICRUDModule
    {
        void add();
        Object get();
        void update();
        void delete();
    }
}
