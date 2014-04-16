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
    /// This module is responsible for the......
    /// ........................................
    /// ........................................
    /// ........................................
    /// </summary>
    public class UserModule : IUserModule, ILoginModule
    {
        private static Boolean isLoggedIn;

        public void create(Gymnast gymnast)
        {
            throw new NotImplementedException();
        }

        Gymnast IUserModule.read(int id)
        {
            throw new NotImplementedException();
        }

        public void update(Gymnast gymnast)
        {
            throw new NotImplementedException();
        }

        public void delete()
        {
            throw new NotImplementedException();
        }

        #region Login Methods

        public Boolean login(string username, string password)
        {
            return isLoggedIn;
        }

        public void logout()
        {
            throw new NotImplementedException();
        }

        public bool ILoginModule.isLoggedIn()
        {
            return isLoggedIn;
        }        

        #endregion
    }
}
