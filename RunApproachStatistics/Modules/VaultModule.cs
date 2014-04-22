using RunApproachStatistics.Model;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules
{
    class VaultModule : IVaultModule, ICameraModule
    {
        public void create(Vault vault)
        {
            RunApproachStatistics.Model.Entity.vault eVault = new vault();

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

            using (var db = new Entities3())
            {
                db.vault.Add(eVault);

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

        public Vault read(int id)
        {
            using (var db = new Entities3())
            {
                var query = from qVault in db.vault
                            where qVault.vaultnumber_id == id
                            select qVault;

                foreach (vault eVault in query)
                {
                    Vault vault = new Vault();

                    vault.gymnast_id = eVault.gymnast_id;
                    vault.duration = eVault.duration;
                    vault.graphdata = eVault.graphdata;
                    vault.videopath = eVault.videopath;
                    vault.rating_star = eVault.rating_star;
                    vault.rating_official_D = eVault.rating_official_D;
                    vault.rating_official_E = eVault.rating_official_E;
                    vault.penalty = eVault.penalty;
                    vault.timestamp = eVault.timestamp;
                    vault.context = eVault.context;
                    vault.note = eVault.note;
                    vault.vaultnumber_id = eVault.vaultnumber_id;
                    vault.location_id = eVault.location_id;
                    vault.deleted = eVault.deleted;
                    vault.thumbnail = eVault.thumbnail;

                    return vault;
                }

                return null;
            }
        }

        public void update(Vault vault, int id)
        {
            using (var db = new Entities3())
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
            using (var db = new Entities3())
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

        public List<Vault> getVaults(string filter)
        {
            using (var db = new Entities3())
            {
                return (from qVault in db.vault
                        select new Vault { 
                            gymnast_id = qVault.gymnast_id,
                            duration = qVault.duration,
                            graphdata = qVault.graphdata,
                            videopath = qVault.videopath,
                            rating_star = qVault.rating_star,
                            rating_official_D = qVault.rating_official_D,
                            rating_official_E = qVault.rating_official_E,
                            penalty = qVault.penalty,
                            timestamp = qVault.timestamp,
                            context = qVault.context,
                            note = qVault.note,
                            vaultnumber_id = qVault.vaultnumber_id,
                            location_id = qVault.location_id,
                            deleted = qVault.deleted,
                            thumbnail = qVault.thumbnail
                        }
                ).ToList();
            }
        }

        #region Laser/Video Camera Methods

        public object getVideoData(int id)
        {
            throw new NotImplementedException();
        }

        public void createVideoData(int id)
        {
            throw new NotImplementedException();
        }

        public String getLaserData(int id)
        {
            using (var db = new Entities3())
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
