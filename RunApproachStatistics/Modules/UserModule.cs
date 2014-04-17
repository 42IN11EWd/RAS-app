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
    /// <summary>
    /// This module is responsible for...
    /// </summary>
    public class UserModule : IUserModule, ILoginModule
    {
        private static Boolean IsLoggedIn;

        public void create(Gymnast gymnast)
        {
            throw new NotImplementedException();
        }

        Gymnast IUserModule.read(int id)
        {
            using (var db = new Entities3())
            {
                var query = from loc in db.location
                            orderby loc.name
                            select loc;

                foreach (var row in query)
                {
                    Console.WriteLine("{0}, {1}", row.name, row.description);
                }
            }

            return new Gymnast();
        }

        public void update(Gymnast gymnast)
        {
            throw new NotImplementedException();
        }

        public void delete()
        {
            throw new NotImplementedException();
        }

        public List<Gymnast> getGymnastCollection(string filter)
        {
            throw new NotImplementedException();
        }

        public List<Gymnast> getGymnastCollection()
        {
            throw new NotImplementedException();
        }

        #region Login Methods

        public Boolean login(string username, string password)
        {
            return IsLoggedIn;
        }

        public void logout()
        {
            throw new NotImplementedException();
        }

        public bool isLoggedIn()
        {
            return IsLoggedIn;
        }        

        #endregion
    }
}
