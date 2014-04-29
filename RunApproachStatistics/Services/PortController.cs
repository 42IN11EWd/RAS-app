using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RunApproachStatistics.Services
{
    public class PortController
    {
        private Boolean isLive = false;

        public static volatile ReadPort  readPort;
        public static volatile WritePort writePort;
        private PortEmulator portEmulator;

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
            set 
            { 
                pilotLaser = value;

                if (isLive)
                {
                    while (!readPort.lastCommandReceived.Equals("PL"))
                    {
                        Thread.Sleep(10);
                        writePort.togglePilotLaser((value == 1) ? true : false);
                    }
                }

                if(value == 0)
                {
                    initializeMeasurement();
                }
            }
        }

        #endregion

        public PortController()
        {
            measurementIndex = laserCameraSettingsModule.getMeasurementIndex();

            if (isLive)
            {
                SerialPort port = new SerialPort("COM4", 115200, Parity.None, 8, StopBits.One);
                port.Open();

                Thread readThread = new Thread(() => { readPort = new ReadPort(this, port); });
                Thread writeThread = new Thread(() => { writePort = new WritePort(port); });
                readThread.Start();
                writeThread.Start();

                readPort.PortDataReceived += readport_PortDataReceived;

                writePort.stopMeasurement();

                while(!readPort.settingsReceived)
                {
                    // wait for settings
                    getSettings();
                }

                initializeMeasurement();
            }
            else
            {
                Thread readThread = new Thread(() => { readPort = new ReadPort(this); });
                Thread writeThread = new Thread(() => { writePort = new WritePort(); });
                readThread.Start();
                writeThread.Start();

                readThread.Join();
                writeThread.Join();
                portEmulator = new PortEmulator(readPort, this);
                
                getSettings();
                initializeMeasurement();
            }
        }

        public void initializeMeasurement()
        {
            if (isLive)
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
            else
            {
                portEmulator.startEmulationMeasurement(true);
            }
        }

        public void startMeasurement()
        {
            if (!isLive)
            {
                portEmulator.startEmulationMeasurement(false);
            }
            readPort.startMeasurement(false);
        }

        public float calibrateMeasurementWindow()
        {
            return readPort.getLatestBufferDistance();
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
            if(isLive)
            {
                writePort.sendSettingsCommand();
            }
            else
            {
                portEmulator.sendSettingsCommand();
            }
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

            float measurementFrequency = (float)Convert.ToDouble(settings[0]);
            float meanValue = (float)Convert.ToDouble(settings[1]);
            float measurementWindowMin = (float)Convert.ToDouble(settings[4]);
            float measurementWindowMax = (float)Convert.ToDouble(settings[5]);

            if (isLive)
            {
                if (measurementFrequency != this.measurementFrequency)
                {
                    while (!readPort.lastCommandReceived.Equals("MF"))
                    {
                        Thread.Sleep(10);
                        writePort.setMeasurementFrequency(measurementFrequency);
                    }
                }

                if (meanValue != this.meanValue)
                {
                    while (!readPort.lastCommandReceived.Equals("SA"))
                    {
                        Thread.Sleep(10);
                        writePort.setMeanValue((float)settings[1]);
                    }
                }

                if (measurementWindowMin != this.measurementWindowMin || measurementWindowMax != this.measurementWindowMax)
                {
                    while (!readPort.lastCommandReceived.Equals("MW"))
                    {
                        Thread.Sleep(10);
                        writePort.setMeasurementWindow(measurementWindowMin, measurementWindowMax);
                    }
                }
            }

            MeanValue = meanValue;
            MeasurementFrequency = measurementFrequency;
            MeasurementWindowMax = measurementWindowMax;
            MeasurementWindowMin = measurementWindowMin;
        }
    }
}
