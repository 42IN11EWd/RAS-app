using RunApproachStatistics.Model;
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
