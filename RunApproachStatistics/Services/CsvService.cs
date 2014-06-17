using RunApproachStatistics.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Services
{
    public static class CsvService
    {
        public static String CSVFolder = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RunApproachStatistics"), "TempCSV");
            
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

        //Returns the folder in which the CSV's are saved
        public static void MultipleMeasurementsToZip(ObservableCollection<ThumbnailViewModel> thumbnails)
        {
            if (!Directory.Exists(CSVFolder))
            {
                Directory.CreateDirectory(CSVFolder);
            }

            foreach (ThumbnailViewModel thumbnail in thumbnails)
            {
                String filepath = Path.Combine(CSVFolder, GenerateFilename(thumbnail) + ".csv");

                File.WriteAllText(filepath, MeasurementDataToString(thumbnail.Vault.graphdata), Encoding.UTF8);
            }
        }

        public static void ClearCSVFolderAfterSave()
        {
            Directory.Delete(CSVFolder, true);
        }

        public static String GenerateFilename(ThumbnailViewModel thumbnail)
        {
            try
            {
                String name = thumbnail.Vault.gymnast.name + (thumbnail.Vault.gymnast.surname_prefix != null && thumbnail.Vault.gymnast.surname_prefix.Length > 0 ? " " + thumbnail.Vault.gymnast.surname_prefix : "") + " " + thumbnail.Vault.gymnast.surname;
                return thumbnail.Vault.timestamp.ToString().Replace("/", "-").Replace(":", ".") + " " + name;
            }
            catch
            {
                return thumbnail.Vault.timestamp.ToString().Replace("/", "-").Replace(":", ".") + " Unknown gymnast";
            }
        }
    }
}
