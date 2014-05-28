using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Services
{
    public static class CsvService
    {
        public static String StringToCsv(String data)
        {
            String result = String.Empty;
            result = data.Replace(' ', ',').Trim();

            return result;
        }

        public static String CsvToString(String csv)
        {
            String result = String.Empty;
            result = csv.Replace(',', ' ').Trim();

            return result;
        }
    }
}
