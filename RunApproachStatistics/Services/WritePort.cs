using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Services
{
    class WritePort
    {
        private SerialPort port;
        private Byte[] escapeCharacter;

        public WritePort()
        {
            escapeCharacter = stringToByteArray("1B");
        }

        public WritePort(SerialPort port)
        {
            this.port = port;
            escapeCharacter = stringToByteArray("1B");
        }

        /// <summary>
        /// Start a continous measurement
        /// </summary>
        /// <param name="useSpeed">Use the speed data yes, true, or no, false</param>
        public void startContinousMeasurement(Boolean useSpeed = true)
        {
            Byte[] command;
            if (useSpeed)
                command = stringToByteArray("5654");
            else
                command = stringToByteArray("4454");

            writeBytes(command);
        }

        /// <summary>
        /// Starts a single distance measurement
        /// </summary>
        public void startSingleMeasurement()
        {
            writeBytes(stringToByteArray("444D"));
        }

        /// <summary>
        /// Stops the running measurement, 
        /// doesn't make use of writeBytes method
        /// </summary>
        public void stopMeasurement()
        {
            // Send stop command
            port.Write(escapeCharacter, 0, escapeCharacter.Length);
        }

        public void sendSettingsCommand()
        {
            writeBytes(stringToByteArray("5041"));
        }

        /// <summary>
        /// Toggle the pilot laser on and of
        /// </summary>
        /// <param name="setLaserOn">True if laser should be on, off otherwise</param>
        public void togglePilotLaser(Boolean setLaserOn)
        {
            Byte[] command;
            if (setLaserOn)
                command = stringToByteArray("504C31");
            else
                command = stringToByteArray("504C30");

            writeBytes(command);
        }

        /// <summary>
        /// Set the measurement window
        /// </summary>
        /// <param name="min">The minimum value for the window</param>
        /// <param name="max">The maximum value for the window</param>
        /// <returns>True if values are acceptable. Lower than 5000 and higher than 1</returns>
        public Boolean setMeasurementWindow(float min, float max)
        {
            if ((min < 5001 && min > -5001) && (max > 0 && max < 5001))
            {
                // Add padding on the left with 0's and then convert to hex 
                String minWindow = String.Format("{0:0000.000}", min);
                String maxWindow = String.Format("{0:0000.000}", max);
                minWindow = stringToHex(minWindow);
                maxWindow = stringToHex(maxWindow);

                Byte[] command = stringToByteArray("4D57" + minWindow + "20" + maxWindow);
                writeBytes(command);

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Method to set the measurement frequency with, Hz.
        /// The value of freq must be between 1 and 2000
        /// </summary>
        /// <param name="freq">The desired frequency</param>
        /// <returns>True if the freq parameter is acceptable, otherwise false</returns>
        public Boolean setMeasurementFrequency(float freq)
        {
            if (freq < 2001 && freq > 0)
            {
                // Add padding on the left with 0's and then convert to hex
                String freqFl = freq.ToString().PadLeft(4, '0');
                freqFl = stringToHex(freqFl);

                Byte[] command = stringToByteArray("4D46" + freqFl);
                writeBytes(command);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Method for setting the Mean Value. 
        /// The entered parameter is the amount of measurements used to create a mean value from.
        /// </summary>
        /// <param name="mean">
        /// Amount of measurements. Has to be between 1 and 2000</param>
        /// <returns>
        /// If the mean parameter is acceptable will return true, otherwise false</returns>
        public Boolean setMeanValue(float mean)
        {
            if (mean < 2001 && mean > 0)
            {
                // Add padding on the left with 0's and then convert to hex
                String meanFl = mean.ToString().PadLeft(4, '0');
                meanFl = stringToHex(meanFl);

                Byte[] command = stringToByteArray("5341" + meanFl);
                writeBytes(command);

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Method that writes the bytes to the Serialport
        /// </summary>
        /// <param name="command">The byte array containing the command</param>
        private void writeBytes(Byte[] command)
        {
            // Send stop command before writing new command
            port.Write(escapeCharacter, 0, escapeCharacter.Length);
            // Write new command
            port.Write(command, 0, command.Length);
        }

        /// <summary>
        /// Creates a byte array from a hex string
        /// </summary>
        /// <param name="hex">The string containing hex input</param>
        /// <returns>The byte array containing converted hex</returns>
        public static byte[] stringToByteArray(string hex)
        {
            if (hex != "1B")
            {
                // Add the "return" statement to command
                hex += "0D";
            }

            // Using Linq to convert the hex string into a byte array, what a serialport needs
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        /// <summary>
        /// Create a hex string from a ASCII string
        /// </summary>
        /// <param name="text">The ASCII string</param>
        /// <returns>The hex string</returns>
        public String stringToHex(String text)
        {
            char[] chars = text.ToCharArray();
            StringBuilder builder = new StringBuilder();
            foreach (char c in chars)
            {
                builder.Append(((Int16)c).ToString("X"));
            }
            return builder.ToString();
        }
    }
}
