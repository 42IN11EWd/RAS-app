﻿using AForge.Video.FFMPEG;
using RunApproachStatistics.Model;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

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
                if (vault.gymnast != null)
                {
                    db.gymnast.Attach(vault.gymnast);
                }

                if (vault.location != null)
                {
                    db.location.Attach(vault.location);
                }

                if (vault.vaultkind != null)
                {
                    db.vaultkind.Attach(vault.vaultkind);
                }

                if (vault.vaultnumber != null)
                {
                    db.vaultnumber.Attach(vault.vaultnumber);
                }

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
                var query = (from qVault in db.vault
                             where qVault.vault_id == vault.vault_id
                             select qVault).First();

                query.gymnast_id = vault.gymnast_id;
                query.timestamp = vault.timestamp;
                query.vault_id = vault.vault_id;
                query.location_id = vault.location_id;
                query.vaultkind_id = vault.vaultkind_id;
                query.vaultnumber_id = vault.vaultnumber_id;
                query.rating_star = vault.rating_star;
                query.rating_official_D = vault.rating_official_D;
                query.rating_official_E = vault.rating_official_E;
                query.penalty = vault.penalty;

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

        public List<gymnast> getGymnasts()
        {
            using (var db = new DataContext())
            {
                return (from qGymnast in db.gymnast
                        where qGymnast.deleted == false
                        select qGymnast
                ).ToList();
            }
        }

        public List<location> getLocations()
        {
            using (var db = new DataContext())
            {
                return (from qLocation in db.location
                        where qLocation.deleted == false
                        select qLocation
                    ).ToList();
            }
        }

        public List<vaultkind> getVaultKinds()
        {
            using (var db = new DataContext())
            {
                return (from qVaultKinds in db.vaultkind
                        where qVaultKinds.deleted == false
                        select qVaultKinds
                    ).ToList();
            }
        }

        public List<vaultnumber> getVaultNumbers()
        {
            using (var db = new DataContext())
            {
                return (from qVaultNumbers in db.vaultnumber
                        where qVaultNumbers.deleted == false
                        select qVaultNumbers
                    ).ToList();
            }
        }

        public List<vault> getVaults()
        {
            using (var db = new DataContext())
            {
                return (from qVault in db.vault.Include("gymnast").Include("vaultkind").Include("vaultnumber").Include("location")
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

        public void createVault(List<Bitmap> frames, List<String> writeBuffer, vault vault)
        {
            Thread createThread = new Thread(() =>
            {
                vault vaultThread = vault;
                // Create the filepath, add date stamp to filename
                String fileName = "LC_Video_" + vault.timestamp.ToString("yyyy_MM_dd_HH-mm-ss") + ".avi";
                String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RunApproachStatistics");

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                filePath = Path.Combine(filePath, fileName);

                //create the lasercamera string
                String graphdata = "";
                foreach (String s in writeBuffer)
                {
                    graphdata += s;
                }
                vault.graphdata = graphdata.Remove(graphdata.Length - 1); //Laatste komma weghalen

                //generate thumbnail
                try
                {
                    BitmapImage bImage = bmpToBitmapImage(frames[20]);
                    byte[] data = null;
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bImage));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        data = ms.ToArray();
                        vault.thumbnail = data;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                // Save the new vault and include the video path.            
                vault.videopath = fileName;
                create(vault);

                // Send vault back to view, for thumbnail list
                Application.Current.Dispatcher.Invoke(() =>
                    {
                        OnVaultCreated(vault);
                    });

                // Create a new thread to save the video
                Worker workerObject = new Worker(filePath, fileName, frames);
                Thread workerThread = new Thread(workerObject.DoWork);

                // Start the thread.
                workerThread.Start();
            });
            createThread.Start();
        }

        public event EventHandler<vault> VaultCreated;
        protected virtual void OnVaultCreated(vault createdVault)
        {
            EventHandler<vault> handler = VaultCreated;
            if (handler != null)
            {
                handler(this, createdVault);
            }
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
            private String fileName;
            private VideoFileWriter writer;
            private List<Bitmap> frames;

            public Worker(String filePath, String fileName, List<Bitmap> frames)
            {
                this.filePath = filePath;
                this.fileName = fileName;
                this.frames = frames;
            }

            public void DoWork()
            {
                try
                {
                    if (CaptureBuffer.fps > 0)
                    {
                        writer = new VideoFileWriter();
                        writer.Open(filePath, CaptureBuffer.width, CaptureBuffer.height, CaptureBuffer.fps, VideoCodec.MPEG4, 2000000);

                        foreach (Bitmap bmp in frames)
                        {
                            writer.WriteVideoFrame(bmp);
                        }

                        // Close the writer
                        writer.Close();
                        writer = null;
                        frames = null;

                        // Upload the file to the server.
                        WebClient myWebClient = new WebClient();
                        NetworkCredential myCredentials = new NetworkCredential("snijhof", "MKD7529s09");
                        myWebClient.Credentials = myCredentials;
                        byte[] responseArray = myWebClient.UploadFile("ftp://student.aii.avans.nl/GRP/42IN11EWd/Videos/" + fileName, filePath);

                        String temp = System.Text.Encoding.ASCII.GetString(responseArray);

                        // Decode and display the response.
                        Console.WriteLine("\nResponse Received.The contents of the file uploaded are:\n{0}",
                            System.Text.Encoding.ASCII.GetString(responseArray));
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.StackTrace);
                }
            }
        }

        private BitmapImage bmpToBitmapImage(Bitmap bmp)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bmp.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        #endregion

        /*#region Filter methods
        
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
        public List<vault> filter(decimal[] dRatings, decimal[] eRatings, int[] gymnasts, int[] locations, DateTime[] timestamps)
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
        public List<vault> filter(decimal[] dRatings, decimal[] eRatings, String[] gymnasts, String[] locations, DateTime[] timestamps)
        {
            return dRatingFilter(eRatingFilter(gymnastNameFilter(locationNameFilter(timestampFilter(getVaults(), timestamps), locations), gymnasts), eRatings), dRatings);
        }

        #region Filters

        private List<vault> dRatingFilter(List<vault> list, decimal[] dRatings)
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

        private List<vault> eRatingFilter(List<vault> list, decimal[] eRatings)
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
        */
        #region new Filter methods with List

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
        public List<vault> filter(List<decimal> dRatings, List<decimal> eRatings, List<int> gymnasts, List<int> locations)
        {
            return dRatingFilter(eRatingFilter(gymnastIdFilter(locationIdFilter(getVaults(), locations), gymnasts), eRatings), dRatings);
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
        public List<vault> filter(List<decimal> dRatings, List<decimal> eRatings, List<string> gymnasts, List<string> locations)
        {
            return dRatingFilter(eRatingFilter(gymnastNameFilter(locationNameFilter(getVaults(), locations), gymnasts), eRatings), dRatings);
        }

        #region Filters

        private List<vault> dRatingFilter(List<vault> list, List<decimal> dRatings)
        {
            if (dRatings.Count == 0)
            {
                return list;
            }

            List<vault> result = new List<vault>();

            for (int i = 0; i < dRatings.Count; i++)
            {
                result.AddRange(list.Where(x => x.rating_official_D == dRatings[i]).ToList());
            }

            return result;
        }

        private List<vault> eRatingFilter(List<vault> list, List<decimal> eRatings)
        {
            if (eRatings.Count == 0)
            {
                return list;
            }

            List<vault> result = new List<vault>();

            for (int i = 0; i < eRatings.Count; i++)
            {
                result.AddRange(list.Where(x => x.rating_official_E == eRatings[i]).ToList());
            }

            return result;
        }

        private List<vault> gymnastNameFilter(List<vault> list, List<string> gymnasts)
        {
            if (gymnasts.Count == 0)
            {
                return list;
            }

            List<vault> result = new List<vault>();

            for (int i = 0; i < gymnasts.Count; i++)
            {
                foreach(vault newVault in list)
                {
                    if (newVault.gymnast != null || newVault.gymnast.name != "")
                    {
                        // TODO check for null in gymnast
                        result.AddRange(list.Where(x => (x.gymnast.name == gymnasts[i]) && (x.gymnast != null) ).ToList());
                    }
                }
            }

            return result;
        }

        private List<vault> gymnastIdFilter(List<vault> list, List<int> gymnasts)
        {
            if (gymnasts.Count == 0)
            {
                return list;
            }

            List<vault> result = new List<vault>();

            for (int i = 0; i < gymnasts.Count; i++)
            {
                result.AddRange(list.Where(x => x.gymnast_id == gymnasts[i]).ToList());
            }

            return result;
        }

        private List<vault> locationNameFilter(List<vault> list, List<string> locations)
        {
            if (locations.Count == 0)
            {
                return list;
            }

            List<vault> result = new List<vault>();

            for (int i = 0; i < locations.Count; i++)
            {
                result.AddRange(list.Where(x => x.location.name == locations[i]).ToList());
            }

            return result;
        }

        private List<vault> locationIdFilter(List<vault> list, List<int> locations)
        {
            if (locations.Count == 0)
            {
                return list;
            }

            List<vault> result = new List<vault>();

            for (int i = 0; i < locations.Count; i++)
            {
                result.AddRange(list.Where(x => x.location_id == locations[i]).ToList());
            }

            return result;
        }

        /* private List<vault> timestampFilter(List<vault> list, DateTime[] timestamps)
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
         }*/
        #endregion
        #endregion
    }
}
