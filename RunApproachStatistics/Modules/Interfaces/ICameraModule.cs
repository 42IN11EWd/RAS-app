using RunApproachStatistics.Model;
using RunApproachStatistics.Model.Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules.Interfaces
{
    interface ICameraModule
    {
        Object getVideoData(int id);

        void createVault(List<Bitmap> frames);

        String getLaserData(int id);
    }
}
