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
                        Thread.Sleep(150);
                        writePort.togglePilotLaser((value == 1) ? true : false);
                    }
                }

                if (value == 0)
                {
                    initializeMeasurement();
                }
                else
                {
                    reallyStopMeasurement();
                }
            }
        }

        #endregion

        public PortController()
        {
            if (isLive)
            {
                SerialPort port = null;
                foreach (String name in SerialPort.GetPortNames())
                {
                    port = new SerialPort(name, 115200, Parity.None, 8, StopBits.One);
                    try
                    {
                        port.Open();
                    }
                    catch (Exception e)
                    {
                        //no problem
                    }

                    if (port.IsOpen)
                    {
                        break;
                    }
                }

                Thread readThread   = new Thread(() => { readPort  = new ReadPort(this, port); });
                Thread writeThread  = new Thread(() => { writePort = new WritePort(port); });
                readThread.Start();
                writeThread.Start();

                readThread.Join();
                writeThread.Join();
                
                writePort.stopMeasurement();

                while (!readPort.lastCommandReceived.Equals("me"))
                {
                    // wait for settings
                    Thread.Sleep(50);
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
            measurementIndex = laserCameraSettingsModule.getMeasurementIndex();

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
                if (measurementIndex == 0)
                {
                    portEmulator.startEmulationMeasurement(true, true);
                }
                else if (measurementIndex == 1)
                {
                    portEmulator.startEmulationMeasurement(true, false);
                }
            }
        }

        public void startMeasurement()
        {
            readPort.startMeasurement(false);
        }

        public List<String> stopMeasurement()
        {
            return readPort.stopMeasurement();
        }

        public void reallyStopMeasurement()
        {
            if (isLive)
            {
                writePort.stopMeasurement();
            }
            else
            {
                portEmulator.stopEmulationMeasurement();
            }
        }

        public float calibrateMeasurementWindow()
        {
            return readPort.getLatestBufferDistance();
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
            // 7: comport index

            float measurementFrequency = (float)Convert.ToDouble(settings[0]);
            float meanValue            = (float)Convert.ToDouble(settings[1]);
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
                        float mean = float.Parse(settings[1].ToString());
                        writePort.setMeanValue(mean);
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

            MeanValue            = meanValue;
            MeasurementFrequency = measurementFrequency;
            MeasurementWindowMax = measurementWindowMax;
            MeasurementWindowMin = measurementWindowMin;
        }
        public String getLatestMeasurement()
        {
            return readPort.getLatestMeasurement();
        }
    }
}
