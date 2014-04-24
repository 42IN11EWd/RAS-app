using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules.Interfaces
{
    interface ILaserCameraSettingsModule
    {
        void setMeasurementIndex(int measurementIndex);
        int getMeasurementIndex();
    }
}
