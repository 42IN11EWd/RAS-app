using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Services
{
    public class PortController
    {
        private ReadPort  readport;
        private WritePort writePort;

        private float measurementFrequency;
        private float meanValue;
        private float measurementWindowMax;
        private float measurementWindowMin;
        private int   pilotLaser;
        private int   measurementIndex;

        private ILaserCameraSettingsModule laserCameraSettingsModule = new SettingsModule();

        public event EventHandler<String> PortDataReceived;

        #region Databinding
        
        public float MeasurementFrequency
        {
            get { return measurementFrequency; }
            set { measurementFrequency = value; }
        }

        public float MeanValue
        {
            get { return meanValue; }
            set { meanValue = value; }
        }

        public float MeasurementWindowMax
        {
            get { return measurementWindowMax; }
            set { measurementWindowMax = value; }
        }

        public float MeasurementWindowMin
        {
            get { return measurementWindowMin; }
            set { measurementWindowMin = value; }
        }

        public int PilotLaser
        {
            get { return pilotLaser; }
            set { pilotLaser = value; }
        }

        #endregion

        public PortController()
        {
            readport    = new ReadPort(this);
            writePort   = new WritePort();

            readport.PortDataReceived += readport_PortDataReceived;

            measurementIndex = laserCameraSettingsModule.getMeasurementIndex();
        }

        public void startMeasurement()
        {
            if (measurementIndex == 0)
            {
                writePort.startContinousMeasurement(true);
            }
            else if (measurementIndex == 1)
            {
                writePort.startContinousMeasurement(false);
            }
        }

        /*public float calibrateMeasurementWindow()
        {
            return readPort.
        }*/

        void readport_PortDataReceived(object sender, string e)
        {
            EventHandler<String> handler = PortDataReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        
        public void getSettings()
        {
            writePort.sendSettingsCommand();
        }

        public void writeSettings(Object[] settings)
        {
            // 0: Frequency
            // 1: Meanvalue
            // 2: camera position
            // 3: Measurement index
            // 4: Measurement window min
            // 5: Measurement window max
            // 6: videocamera index

            float measurementFrequency = (float)settings[0];
            if (measurementFrequency != this.measurementFrequency)
            {
                writePort.setMeasurementFrequency(measurementFrequency);
            }

            float meanValue = (float)settings[1];
            if(meanValue != this.meanValue)
            {
                writePort.setMeanValue((float)settings[1]);
            }

            float measurementWindowMin = (float)settings[4];
            float measurementWindowMax = (float)settings[5];
            if(measurementWindowMin != this.measurementWindowMin || measurementWindowMax != this.measurementWindowMax)
            {
                writePort.setMeasurementWindow(measurementWindowMin, measurementWindowMax);
            }
        }
    }
}
