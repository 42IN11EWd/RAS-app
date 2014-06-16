using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Services
{
    public static class CsvService
    {
        public static String MeasurementDataToString(String data)
        {
            StringBuilder csv = new StringBuilder();
            String[] measurements = data.Split(',');
            csv.AppendLine("sep=,");

            if (data.Contains(" ")) //Contains distance and speed
            {
                csv.AppendLine("Distance=,Speed=");

                foreach (String s in measurements)
                {
                    csv.AppendLine(s.Replace(" ", ","));
                }
            }
            else
            { //Contains only distance
                csv.AppendLine("Distance=");

                foreach (String s in measurements)
                {
                    csv.AppendLine(s);
                }
            }

            return csv.ToString();
        }
    }
}
