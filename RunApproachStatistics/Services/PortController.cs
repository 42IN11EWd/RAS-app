using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Services
{
    class PortController
    {
        private ReadPort  readport;
        private WritePort writePort;

        private float measurementFrequency;
        private float meanValue;
        private float measurementWindowMax;
        private float measurementWindowMin;
        private int   pilotLaser;

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
        }


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
    }
}
