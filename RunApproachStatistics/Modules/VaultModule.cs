using RunApproachStatistics.Model;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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

        public List<vault> getVaults(string filter)
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

        #endregion

    }
}
