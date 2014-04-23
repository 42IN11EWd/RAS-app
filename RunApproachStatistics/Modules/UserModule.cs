﻿using RunApproachStatistics.Model;
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
        private static Boolean _isLoggedIn;

        public void create(gymnast gymnast)
        {
            using (var db = new DataContext())
            {
                db.gymnast.Add(gymnast);

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

        public gymnast read(int id)
        {
            using (var db = new DataContext())
            {
                var query = from gym in db.gymnast
                            where gym.gymnast_id == id
                            select gym;
            }

            return null;
        }

        public void update(gymnast gymnast)
        {
            using (var db = new DataContext())
            {
                var query = from qGymnast in db.gymnast
                            where qGymnast.gymnast_id == gymnast.gymnast_id
                            select qGymnast;

                foreach(gymnast eGymnast in query)
                {
                    eGymnast.turnbondID = gymnast.turnbondID;
                    eGymnast.gender = gymnast.gender;
                    eGymnast.nationality = gymnast.nationality;
                    eGymnast.length = gymnast.length;
                    eGymnast.picture = gymnast.picture;
                    eGymnast.birthdate = gymnast.birthdate;
                    eGymnast.name = gymnast.name;
                    eGymnast.surname = gymnast.surname;
                    eGymnast.surname_prefix = gymnast.surname_prefix;
                    eGymnast.vault = gymnast.vault;
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
                var query = from qGymnast in db.gymnast
                            where qGymnast.gymnast_id == id
                            select qGymnast;

                foreach (gymnast eGymnast in query)
                {
                    eGymnast.deleted = true;
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

        public List<gymnast> getGymnastCollection()
        {
            using (var db = new DataContext())
            {
                return (from qGymnast in db.gymnast
                        select qGymnast
                ).ToList();
            }
        }

        public List<gymnast> getGymnastCollection(string filter)
        {
            throw new NotImplementedException();
        }

        #region Login Methods

        public Boolean login(string username, string password)
        {
            var query = from qUser in db.user
                        where user.username == username 
                        && user.password = password
                        select qUser;

            Boolean correctLoginVariables = false;
            foreach(user eUser in query)
            {
                if (eUser != null) {
                    _isLoggedIn = true;
                    return true;
                }
            }

            return false;
        }

        public void logout()
        {
            _isLoggedIn = false;
        }

        public static bool isLoggedIn()
        {
            return _isLoggedIn;
        }        

        #endregion
    }
}
