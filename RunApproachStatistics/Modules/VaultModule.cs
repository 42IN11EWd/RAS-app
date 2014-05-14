using AForge.Video.FFMPEG;
using RunApproachStatistics.Model;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules
{
    /// <summary>
    /// This module is responsible for managing the vaults. In this module 
    /// you can create, read, update and delete vaults.
    /// 
    /// Another thing this module does is managing the laser and video data.
    /// You can read and create video data. And you can get the laser data, when
    /// the user needs the laser data for graphs.
    /// 
    /// This will be done by a combination of Linq and the Entity framework.
    /// </summary>
    public class VaultModule : IVaultModule, ICameraModule
    {
        public void create(vault vault)
        {
            using (var db = new DataContext())
            {
                db.vault.Add(vault);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public vault read(int id)
        {
            using (var db = new DataContext())
            {
                var query = (from qVault in db.vault.Include("gymnast").Include("vaultnumber").Include("location")
                            where qVault.vault_id == id
                            select qVault).First();

                return query;
            }
        }

        public void update(vault vault)
        {
            using (var db = new DataContext())
            {
                var query = from qVault in db.vault
                            where qVault.vault_id == vault.vault_id
                            select qVault;

                foreach (vault eVault in query)
                {
                    //eVault.context = vault.context;
                    eVault.duration = vault.duration;
                    eVault.gymnast_id = vault.gymnast_id;
                   // eVault.location = vault.location;
                    //eVault.penalty = vault.penalty;
                   // eVault.rating_official_D = vault.rating_official_D;
                    //eVault.rating_official_E = vault.rating_official_E;
                    //eVault.rating_star = vault.rating_star;
                    eVault.timestamp = vault.timestamp;
                   // eVault.vaultnumber = vault.vaultnumber;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void delete(int id)
        {
            using (var db = new DataContext())
            {
                var query = (from qVault in db.vault
                            where qVault.vault_id == id
                            select qVault).First();

                query.deleted = true;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public List<vault> getVaults()
        {
            using (var db = new DataContext())
            {
                return (from qVault in db.vault.Include("gymnast").Include("vaultnumber").Include("location")
                        where qVault.deleted == false
                        select qVault
                ).ToList();
            }
        }

        public List<String> getVaultKindNames()
        {
            using (var db = new DataContext())
            {
                return (from qVaultKind in db.vaultkind
                        where qVaultKind.deleted == false
                        select qVaultKind.name
                ).ToList();
            }
        }

        public List<int> getVaultKindIds()
        {
            using (var db = new DataContext())
            {
                return (from qVaultKind in db.vaultkind
                        where qVaultKind.deleted == false
                        select qVaultKind.vaultkind_id
                ).ToList();
            }
        }

        public List<String> getLocationNames()
        {
            using (var db = new DataContext())
            {
                return (from qLocation in db.location
                        where qLocation.deleted == false
                        select qLocation.name
                ).ToList();
            }
        }
        public List<int> getLocationIds()
        {
            using (var db = new DataContext())
            {
                return (from qLocation in db.location
                        where qLocation.deleted == false
                        select qLocation.location_id
                ).ToList();
            }
        }

        public List<String> getGymnastNames()
        {
            using (var db = new DataContext())
            {
                return (from qGymnast in db.gymnast
                        where qGymnast.deleted == false
                        select qGymnast.name + (qGymnast.surname_prefix.Length > 0 ? " " + qGymnast.surname_prefix : "") + " " + qGymnast.surname
                ).ToList();
            }
        }
        public List<int> getGymnastIds()
        {
            using (var db = new DataContext())
            {
                return (from qGymnast in db.gymnast
                        where qGymnast.deleted == false
                        select qGymnast.gymnast_id
                ).ToList();
            }
        }

        public List<String> getVaultNumberNames()
        {
            using (var db = new DataContext())
            {
                return (from qVaultnumber in db.vaultnumber
                        where qVaultnumber.deleted == false
                        select qVaultnumber.code
                ).ToList();
            }
        }

        public List<int> getVaultNumberIds()
        {
            using (var db = new DataContext())
            {
                return (from qVaultnumber in db.vaultnumber
                        where qVaultnumber.deleted == false
                        select qVaultnumber.vaultnumber_id
                ).ToList();
            }
        }

        #region Laser/Video Camera Methods

        public object getVideoData(int id)
        {
            throw new NotImplementedException();
        }

        public void createVault(List<Bitmap> frames, List<String> writeBuffer, String vaultKind, String location, String gymnast, String vaultNumber, int rating, float dscore, float escore, float penalty)
        {
            // Get the path for Desktop, to easily find the CSV
            String path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            String dateStamp = DateTime.Now.ToString("yyyy_MM_dd_HH-mm-ss");

            // Create the filepath, add date stamp to filename
            String filePath = Path.Combine(path, "LC_Video_" + dateStamp + ".avi");
            
            // Save the new vault and include the video path.            
            vault vault = new vault();

            vault.videopath = filePath;
            create(vault);

            // Create a new thread to save the video
            Worker workerObject = new Worker(filePath, frames);
            Thread workerThread = new Thread(workerObject.DoWork);

            // Start the thread.
            workerThread.Start();
        }

        public String getLaserData(int id)
        {
            using (var db = new DataContext())
            {
                var query = from qVault in db.vault
                            where qVault.vaultnumber_id == id
                            select qVault;

                String data = String.Empty;

                foreach (vault eVault in query)
                {
                    data = eVault.graphdata;
                }

                return data;
            }
        }

        public class Worker
        {
            private String filePath;
            private VideoFileWriter writer;
            private List<Bitmap> frames;

            public Worker(String filePath, List<Bitmap> frames)
            {
                this.filePath = filePath;
                this.frames = frames;
            }

            public void DoWork()
            {
                try
                {
                    writer = new VideoFileWriter();
                    writer.Open(filePath, CaptureBuffer.width, CaptureBuffer.height, CaptureBuffer.fps, VideoCodec.MPEG4, 2000000);

                    foreach(Bitmap bmp in frames)
                    {
                        writer.WriteVideoFrame(bmp);
                    }

                    // Close the writer
                    writer.Close();
                    writer = null;
                    frames = null;

                    // Upload the file to the server.
                    WebClient myWebClient = new WebClient();
                    NetworkCredential myCredentials = new NetworkCredential("username", "password");
                    myWebClient.Credentials = myCredentials;
                    byte[] responseArray = myWebClient.UploadFile("ftp://student.aii.avans.nl/GRP/42IN11EWd/Videos", filePath);

                    String temp = System.Text.Encoding.ASCII.GetString(responseArray);

                    // Decode and display the response.
                    Console.WriteLine("\nResponse Received.The contents of the file uploaded are:\n{0}",
                        System.Text.Encoding.ASCII.GetString(responseArray));
                }
                catch (Exception e)
                {
                    Console.Write(e.StackTrace);
                }
            }
        }

        #endregion

        #region Filter methods

        /// <summary>
        /// Filter that will filter vaults based on data and id's of gymnasts and locations.
        /// The input is always the full list (automatic), and it will return the filtered list.
        /// If a filter array is empty, it will pass the unfiltered list on to the next filter.
        /// </summary>
        /// <param name="dRatings">Array of d-ratings which should be in the final filtered list.</param>
        /// <param name="eRatings">Array of e-ratings which should be in the final filtered list.</param>
        /// <param name="gymnasts">Array of the id's of the gymnasts that should appear in the filtered list.</param>
        /// <param name="locations">Array of the id's of the locations that should appear in the filtered list.</param>
        /// <param name="timestamps">Array of datetimes the list should be filtered on.</param>
        /// <returns>A filtered list of vaults.</returns>
        public List<vault> filter(double[] dRatings, double[] eRatings, int[] gymnasts, int[] locations, DateTime[] timestamps)
        {
            return dRatingFilter(eRatingFilter(gymnastIdFilter(locationIdFilter(timestampFilter(getVaults(), timestamps), locations), gymnasts), eRatings), dRatings);
        }

        /// <summary>
        /// Filter that will filter vaults based on data and names of gymnasts and locations.
        /// The input is always the full list (automatic), and it will return the filtered list.
        /// If a filter array is empty, it will pass the unfiltered list on to the next filter.
        /// </summary>
        /// <param name="dRatings">Array of d-ratings which should be in the final filtered list.</param>
        /// <param name="eRatings">Array of e-ratings which should be in the final filtered list.</param>
        /// <param name="gymnasts">Array of the names of gymnasts that should appear in the filtered list.</param>
        /// <param name="locations">Array of the names of locations that should appear in the filtered list.</param>
        /// <param name="timestamps">Array of datetimes the list should be filtered on.</param>
        /// <returns>A filtered list of vaults.</returns>
        public List<vault> filter(double[] dRatings, double[] eRatings, String[] gymnasts, String[] locations, DateTime[] timestamps)
        {
            return dRatingFilter(eRatingFilter(gymnastNameFilter(locationNameFilter(timestampFilter(getVaults(), timestamps), locations), gymnasts), eRatings), dRatings);
        }

        #region Filters

        private List<vault> dRatingFilter(List<vault> list, double[] dRatings)
        {
            if (dRatings.Length == 0)
            {
                return list;
            }

            List<vault> result = new List<vault>();

            for (int i = 0; i < dRatings.Length; i++)
            {
                result.AddRange(list.Where(x => x.rating_official_D == dRatings[i]).ToList());
            }

            return result;
        }

        private List<vault> eRatingFilter(List<vault> list, double[] eRatings)
        {
            if (eRatings.Length == 0)
            {
                return list;
            }

            List<vault> result = new List<vault>();

            for (int i = 0; i < eRatings.Length; i++)
            {
                result.AddRange(list.Where(x => x.rating_official_E == eRatings[i]).ToList());
            }

            return result;
        }

        private List<vault> gymnastNameFilter(List<vault> list, String[] gymnasts)
        {
            if (gymnasts.Length == 0)
            {
                return list;
            }

            List<vault> result = new List<vault>();

            for (int i = 0; i < gymnasts.Length; i++)
            {
                result.AddRange(list.Where(x => x.gymnast.name == gymnasts[i]).ToList());
            }

            return result;
        }

        private List<vault> gymnastIdFilter(List<vault> list, int[] gymnasts)
        {
            if (gymnasts.Length == 0)
            {
                return list;
            }

            List<vault> result = new List<vault>();

            for (int i = 0; i < gymnasts.Length; i++)
            {
                result.AddRange(list.Where(x => x.gymnast_id == gymnasts[i]).ToList());
            }

            return result;
        }

        private List<vault> locationNameFilter(List<vault> list, String[] locations)
        {
            if (locations.Length == 0)
            {
                return list;
            }

            List<vault> result = new List<vault>();

            for (int i = 0; i < locations.Length; i++)
            {
                result.AddRange(list.Where(x => x.location.name == locations[i]).ToList());
            }

            return result;
        }

        private List<vault> locationIdFilter(List<vault> list, int[] locations)
        {
            if (locations.Length == 0)
            {
                return list;
            }

            List<vault> result = new List<vault>();

            for (int i = 0; i < locations.Length; i++)
            {
                result.AddRange(list.Where(x => x.location_id == locations[i]).ToList());
            }

            return result;
        }

        private List<vault> timestampFilter(List<vault> list, DateTime[] timestamps)
        {
            if (timestamps.Length == 0)
            {
                return list;
            }

            List<vault> result = new List<vault>();

            for (int i = 0; i < timestamps.Length; i++)
            {
                result.AddRange(list.Where(x => x.timestamp == timestamps[i]).ToList());
            }

            return result;
        }

        #endregion

        #endregion

    }
}
