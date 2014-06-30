using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RunApproachStatistics.Services
{
    public class ReadPort
    {
        private PortController portController;

        private SerialPort port;
        private List<String> writeBuffer;
        private List<String> dynamicBuffer;
        private Boolean save;
        private Boolean modifiyingBuffer;
        public Boolean settingsReceived = false;
        public String lastCommandReceived = "";
        private String errorLine = "D 0000.000 0000.000";
        private Timer timer;
        private Boolean measurementRecieved;

        public ReadPort(PortController portController, SerialPort port)
        {
            this.portController = portController;
            this.port = port;
            this.port.DataReceived += dataReceived;

            writeBuffer = new List<String>();
            dynamicBuffer = new List<String>();
            modifiyingBuffer = false;

            AutoResetEvent autoEvent = new AutoResetEvent(false);
            timer = new Timer(timer_Tick, autoEvent, 1000, 1000);
        }

        public ReadPort(PortController portController)
        {
            this.portController = portController;

            writeBuffer = new List<String>();
            dynamicBuffer = new List<String>();
            modifiyingBuffer = false;
        }

        public float getLatestBufferDistance()
        {
            while (modifiyingBuffer)
            {

            }

            int count = (dynamicBuffer.Count - 1);
            string result = null;

            result = dynamicBuffer[count];

            if (result == null)
            {
                return -1;
            }
            else
            {
                String[] spaceIndex = result.Split(' ');
                result = spaceIndex[1];
                result = result.Replace(",", "");
                
                float fResult = float.Parse(result, CultureInfo.InvariantCulture);
                
                return fResult;
            }
        }

        /// <summary>
        /// DataReceived event for reading the data from the SerialPort
        /// </summary>
        /// <param name="sender">The SerialPort sending the data</param>
        /// <param name="e"> - </param>
        private async void dataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (port.BytesToRead > 2)
                {
                    byte[] data = new byte[port.BytesToRead];
                    int bytesRead = await port.BaseStream.ReadAsync(data, 0, port.BytesToRead);

                    String line = Encoding.ASCII.GetString(data);
                    line = line.Trim(new char[] { '\r', '\n', '\x1B' });

                    checkReceivedData(line);
                }
            }
            catch(Exception ex)
            {

            }
        }

        /// <summary>
        /// Called from DataReceived, calls the correct method for each function
        /// </summary>
        /// <param name="line">The line read from the SerialPort</param>
        public void checkReceivedData(String line)
        {
            measurementRecieved = true;
            if (line.Length >= 2)
            {
                String subLine = line.Substring(0, 2);

                switch (subLine)
                {
                    case "D-":
                    case "D ": lastCommandReceived = "D";  writeMeasurement(line); break;
                    case "ER": lastCommandReceived = "ER"; writeMeasurement(errorLine); break;
                    case "PL": lastCommandReceived = "PL"; break;
                    case "DT": lastCommandReceived = "DT"; Console.WriteLine("Single distance measurement started"); break;
                    case "VT": lastCommandReceived = "VT"; Console.WriteLine("Continous distance + speed measurement started"); break;
                    case "MF": lastCommandReceived = "MF"; break;
                    case "MW": lastCommandReceived = "MW"; break;
                    case "SA": lastCommandReceived = "SA"; break;
                    case "me": lastCommandReceived = "me"; onSettingsRecieved(line); break;
                }
            }
        }

        private void onSettingsRecieved(String line)
        {
            try
            {
                //get measure frequency
                portController.MeasurementFrequency = (float)Convert.ToDouble(line.Substring(line.IndexOf("MF") + 15, line.IndexOf("(") - (line.IndexOf("MF") + 15)));

                //get meanvalue
                portController.MeanValue = (float)Convert.ToDouble(line.Substring(line.IndexOf("SA") + 19, line.IndexOf("scale factor") - (line.IndexOf("SA") + 19)));

                //get measurement window
                String[] splitString = line.Substring(line.IndexOf("MW") + 18, line.IndexOf("distance offset") - (line.IndexOf("MW") + 18)).Split(' ');

                portController.MeasurementWindowMin = (float)Convert.ToDouble(splitString[0], CultureInfo.InvariantCulture);
                portController.MeasurementWindowMax = (float)Convert.ToDouble(splitString[1], CultureInfo.InvariantCulture);

                //get pilot laser setting
                portController.PilotLaser = (int)Convert.ToDouble(line.Substring(line.IndexOf("PL") + 20, line.IndexOf("autostart command") - (line.IndexOf("PL") + 20)));
            }
            catch (Exception e)
            {
                portController.MeasurementFrequency = 2000;
                portController.MeanValue = 200;
                portController.MeasurementWindowMin = 10;
                portController.MeasurementWindowMax = 200;
                portController.PilotLaser = 0;
                Console.WriteLine(line);
                Console.WriteLine("Settings werken niet!");
            }
            settingsReceived = true;
        }

        /// <summary>
        /// Write the measurement data to ArrayList
        /// </summary>
        /// <param name="line">Measurement data line</param>
        public void writeMeasurement(String line)
        {
            String distance = "", speed = "";
            try
            {
                if (line.Length < 9)
                {
                    return;
                }
                else if (line.Length >= 11 && line.Substring(10, 1) == " ")
                {
                    if (line.Length < 20)
                        return;

                    // Check if distance data is negative
                    if (line.Substring(1, 1) == "-")
                        speed = line.Substring(1, 9);
                    else
                        speed = line.Substring(2, 8);

                    // Check if speed data is available            
                    if (line.Substring(11, 1) == "-")
                        distance = " " + line.Substring(11, 9);
                    else
                        distance = " " + line.Substring(12, 8);
                }
                else
                {
                    if (line.Substring(1, 1) == "-")
                        distance = line.Substring(1, 9);
                    else
                        distance = line.Substring(2, 8);
                }
            }
            catch (Exception e)
            {
                return;
            }

            if (save)
            {
                lock (writeBuffer)
                {
                    writeBuffer.Add(speed + distance + ",");
                }
            }

            lock (dynamicBuffer)
            {
                dynamicBuffer.Add(speed + distance + ",");
                resetBuffer();
            }
        }

        private void resetBuffer()
        {
            modifiyingBuffer = true;

            if (dynamicBuffer.Count > ((portController.MeasurementFrequency / portController.MeanValue) * 3))
            {
                dynamicBuffer.RemoveAt(0);
            }

            modifiyingBuffer = false;
        }

        public void startMeasurement(Boolean automaticDetection)
        {
            writeBuffer = new List<String>();

            if (automaticDetection)
            {
                writeBuffer = dynamicBuffer;
            }

            save = true;
        }

        //Checks once a second if there is still a lasercamera connected.
        private void timer_Tick(object state)
        {
            if (!measurementRecieved)
            {
                timer.Dispose();
                MessageBox.Show("Lost connection to the lasercamera. To reconnect the lasercamera, reconnect the lasercamera and restart the program.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            measurementRecieved = false;
        }

        public List<String> stopMeasurement()
        {
            save = false;
            return writeBuffer;
        }

        public String getLatestMeasurement()
        {
            try
            {
                while (modifiyingBuffer)
                {

                }
                if (portController.PilotLaser == 1)
                {
                    return "0 0,";
                }
                else
                {
                    return dynamicBuffer[dynamicBuffer.Count - 1];
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return "0 0,";
            }
        }
    }
}
