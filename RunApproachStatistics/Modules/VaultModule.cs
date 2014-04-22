using RunApproachStatistics.Model;
using RunApproachStatistics.Modules.Interfaces;
using System;
using System.Collections.Generic;
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
        public void create()
        {
            throw new NotImplementedException();
        }

        public Vault read(int id)
        {
            throw new NotImplementedException();
        }

        public void update(Vault vault)
        {
            throw new NotImplementedException();
        }

        public void delete()
        {
            throw new NotImplementedException();
        }

        public List<Vault> getVaults(string filter)
        {
            throw new NotImplementedException();
        }

        #region Laser/Video Camera Methods

        public object getVideoData()
        {
            throw new NotImplementedException();
        }

        public void createVideoData()
        {
            throw new NotImplementedException();
        }

        public object getLaserData()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
