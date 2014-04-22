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
    /// This module is responsible for managing the gymnasts by deleting, updating, creating
    /// and reading gymnasts. This will be done by a combination of Linq and the Entity framework.
    /// 
    /// The last thing that will be done in this module is the login feature.
    /// This module will manage if the user is logged in, if the user can login and if 
    /// it can logout.
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
                var query = from gym in db.gymnast
                            where gym.gymnast_id == id
                            select gym;
            }

            return null;
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
            using (var db = new Entities3())
            {
                return (from gym in db.gymnast
                        select new Gymnast(gym.gymnast_id, gym.turnbondID, new GenderEnum(), gym.nationality,
                                           0, new DateTime(), gym.name, gym.surname, gym.surname_prefix, null))
                                           .ToList();
            }
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
