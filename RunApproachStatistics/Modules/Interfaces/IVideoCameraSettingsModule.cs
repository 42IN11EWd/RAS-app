using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules.Interfaces
{
    interface IVideoCameraSettingsModule
    {
        void saveVideocameraIndex(int index);
        int getVideocameraIndex();
    }
}
