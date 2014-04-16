using RunApproachStatistics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules.Interfaces
{
    public interface IUserModule
    {
        void create(User user);

        User read(int id);

        void update(User user);

        void delete();
    }
}
