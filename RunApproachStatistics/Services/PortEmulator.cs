using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RunApproachStatistics.Services
{
    class PortEmulator
    {
        private ReadPort readPort;
        private PortController portController;
        private Timer measurementTimer;

        private float speed;
        private float distance;

        private Boolean isContinuousMeasurement;
        private Boolean useSpeed;

        public PortEmulator(ReadPort readPort, PortController portController)
        {
            this.readPort = readPort;
            this.portController = portController;
        }

        public void startEmulationMeasurement(Boolean isContinuousMeasurement, Boolean useSpeed)
        {
            this.isContinuousMeasurement = isContinuousMeasurement;
            this.useSpeed                = useSpeed;
            measurementTimer             = new Timer(this.measurementTimer_Tick);

            speed    = 0;
            distance = portController.MeasurementWindowMin;

            int timeSpan = (int)(portController.MeasurementFrequency / portController.MeanValue);
            timeSpan     = 1000 / timeSpan;
            measurementTimer.Change(new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, timeSpan));
        }

        public void stopEmulationMeasurement()
        {
            measurementTimer.Dispose();
        }

        void measurementTimer_Tick(object sender)
        {
            String line = "";
            if (distance < portController.MeasurementWindowMax)
            {
                speed += (float)0.0008;
                distance += (float)0.006;
            }
            else
            {
                speed = 0;
                distance = portController.MeasurementWindowMin;
            }
            
            if (useSpeed)
            {
                line = "D " + String.Format(CultureInfo.InvariantCulture, "{0:0000.000}", speed) + "  " + String.Format(CultureInfo.InvariantCulture, "{0:0000.000}", distance);
            }
            else
            {
                line = "D " + String.Format(CultureInfo.InvariantCulture, "{0:0000.000}", distance);
            }
           
            readPort.checkReceivedData(line);
        }

        public void sendSettingsCommand()
        {
            String settings = "measure frequency[MF]............2000(max 2000)hz" +
                               "trigger delay/level/mode[TD].....0.00msec 0 0" +
                               "average value[SA]................20" +
                               "scale factor[SF].................1.000000" +
                               "measure window[MW]...............2.000 30.000" +
                               "distance offset[OF]..............0.000" +
                               "error mode[SE]...................1" +
                               "digital out[Q1]..................0.000 0.000 0.000 1" +
                               "digital out[Q2]..................0.000 0.000 0.000 1" +
                               "analog out[QA]...................1.000 300.000" +
                               "RS232/422 baud rate[BR]..........115200" +
                               "RS232/422 output format[SD]......dec (0), value (0)" +
                               "RS232/422 output terminator[TE]..0Dh 0Ah (0)" +
                               "SSI format[SC]...................bin (0)" +
                               "pilot laser [PL].................0" +
                               "autostart command[AS]............ID heater threshold levels[HE]......4 10";
                               
            readPort.checkReceivedData(settings);
        }
    }
}
