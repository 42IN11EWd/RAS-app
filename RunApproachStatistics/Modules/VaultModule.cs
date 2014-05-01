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
    class VaultModule : IVaultModule, ICameraModule
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
                var query = from qVault in db.vault
                            where qVault.vaultnumber_id == id
                            select qVault;

                foreach (vault vault in query)
                {
                    return vault;
                }

                return null;
            }
        }

        public void update(vault vault, int id)
        {
            using (var db = new DataContext())
            {
                var query = from qVault in db.vault
                            where qVault.vaultnumber_id == id
                            select qVault;

                foreach (vault eVault in query)
                {
                    eVault.gymnast_id = vault.gymnast_id;
                    eVault.duration = vault.duration;
                    eVault.graphdata = vault.graphdata;
                    eVault.videopath = vault.videopath;
                    eVault.rating_star = vault.rating_star;
                    eVault.rating_official_D = vault.rating_official_D;
                    eVault.rating_official_E = vault.rating_official_E;
                    eVault.penalty = vault.penalty;
                    eVault.timestamp = vault.timestamp;
                    eVault.context = vault.context;
                    eVault.note = vault.note;
                    eVault.vaultnumber_id = vault.vaultnumber_id;
                    eVault.location_id = vault.location_id;
                    eVault.deleted = vault.deleted;
                    eVault.thumbnail = vault.thumbnail;

                    eVault.gymnast = vault.gymnast;
                    eVault.location = vault.location;
                    eVault.vaultnumber = vault.vaultnumber;
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
                var query = from qVault in db.vault
                            where qVault.vaultnumber_id == id
                            select qVault;

                foreach (vault eVault in query)
                {
                    eVault.deleted = true;
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

        public List<vault> getVaults()
        {
            using (var db = new DataContext())
            {
                return (from qVault in db.vault
                        select qVault
                ).ToList();
            }
        }

        #region Laser/Video Camera Methods

        public object getVideoData(int id)
        {
            throw new NotImplementedException();
        }

        public void createVideoData(vault vault, Bitmap[] frames)
        {
            // Get the path for Desktop, to easily find the CSV
            String path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            String dateStamp = DateTime.Now.ToString("yyyy_MM_dd_HH-mm-ss");

            // Create the filepath, add date stamp to filename
            String filePath = Path.Combine(path, "LC_Video_" + dateStamp + ".avi");
            
            // Save the new vault and include the video path.
            vault.videopath = filePath;
            create(vault);

            // Create a new thread to save the video
            Worker workerObject = new Worker(filePath, frames);
            Thread workerThread = new Thread(workerObject.DoWork);

            // Start the thread.
            workerThread.Start();

            
            throw new NotImplementedException();
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
            private int framesCounter;
            private VideoFileWriter writer;
            private Bitmap[] frames;

            public Worker(String filePath, Bitmap[] frames)
            {
                this.filePath = filePath;
                this.frames = frames;
            }

            public void DoWork()
            {
                try
                {
                    writer.Open(filePath, CaptureBuffer.width, CaptureBuffer.height, CaptureBuffer.fps, VideoCodec.MPEG4, 2000000);

                    int count = 0;
                    while (frames[count] != null)
                    {
                        writer.WriteVideoFrame(frames[count]);
                        count++;
                    }

                    writer.Close();
                    writer = null;
                    frames = null;
                    framesCounter = 0;
                }
                catch (Exception e)
                {
                    Console.Write(e.StackTrace);
                }
            }
        }

        #endregion

        #region Filter methods

        public List<vault> filter(double[] dRatings, double[] eRatings, int[] gymnasts, int[] locations, DateTime[] timestamps)
        {
            return dRatingFilter(eRatingFilter(gymnastFilter(locationFilter(timestampFilter(getVaults(), timestamps), locations), gymnasts), eRatings), dRatings);
        }

        #region Filters

        private List<vault> dRatingFilter(List<vault> list, double[] dRatings)
        {
            // Check for 0 length filter input.
            if (dRatings.Length == 0)
            {
                return list;
            }

            // Add each dRating value to the result set.
            List<vault> result = new List<vault>();

            for (int i = 0; i < dRatings.Length; i++)
            {
                result.AddRange(list.Where(x => x.rating_official_D == dRatings[i]).ToList());
            }

            return result;
        }

        private List<vault> eRatingFilter(List<vault> list, double[] eRatings)
        {
            // Check for 0 length filter input.
            if (eRatings.Length == 0)
            {
                return list;
            }

            // Add each dRating value to the result set.
            List<vault> result = new List<vault>();

            for (int i = 0; i < eRatings.Length; i++)
            {
                result.AddRange(list.Where(x => x.rating_official_E == eRatings[i]).ToList());
            }

            return result;
        }

        private List<vault> gymnastFilter(List<vault> list, int[] gymnasts)
        {
            // Check for 0 length filter input.
            if (gymnasts.Length == 0)
            {
                return list;
            }

            // Add each dRating value to the result set.
            List<vault> result = new List<vault>();

            for (int i = 0; i < gymnasts.Length; i++)
            {
                result.AddRange(list.Where(x => x.gymnast_id == gymnasts[i]).ToList());
            }

            return result;
        }

        private List<vault> locationFilter(List<vault> list, int[] locations)
        {
            // Check for 0 length filter input.
            if (locations.Length == 0)
            {
                return list;
            }

            // Add each dRating value to the result set.
            List<vault> result = new List<vault>();

            for (int i = 0; i < locations.Length; i++)
            {
                result.AddRange(list.Where(x => x.location_id == locations[i]).ToList());
            }

            return result;
        }

        private List<vault> timestampFilter(List<vault> list, DateTime[] timestamps)
        {
            // Check for 0 length filter input.
            if (timestamps.Length == 0)
            {
                return list;
            }

            // Add each dRating value to the result set.
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
