using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules
{
    class EditorModule : IEditorModule
    {
        //Location methods
        public void deleteLocation(int id)
        {
            using (var db = new DataContext())
            {
                var query = from qlocation in db.location
                            where qlocation.location_id == id
                            select qlocation;

                foreach (location elocation in query)
                {
                    elocation.deleted = true;
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
        public ObservableCollection<location> readLocation()
        {
            ObservableCollection<location> locations = new ObservableCollection<location>();

            using (var db = new DataContext())
            {
                var query = from qlocation in db.location
                            where qlocation.deleted == false
                            select qlocation;

                foreach (location elocation in query)
                {
                    locations.Add(elocation);
                }
            }

            return locations;
        }
        public void createLocation(location location)
        {
            using (var db = new DataContext())
            {
                db.location.Add(location);

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
        public void updateLocation(location location)
        {
            if (location.location_id != 0)
            {
                using (var db = new DataContext())
                {
                    var query = from qlocation in db.location
                                where qlocation.location_id == location.location_id
                                select qlocation;

                    foreach (location elocation in query)
                    {
                        elocation.name = location.name;
                        elocation.description = location.description;
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
            else
            {
                createLocation(location);
            }
        }

        //Vaultnumber methods
        public void deleteVaultNumber(int id)
        {
            using (var db = new DataContext())
            {
                var query = from qvaultnumber in db.vaultnumber
                            where qvaultnumber.vaultnumber_id == id
                            select qvaultnumber;

                foreach (vaultnumber evaultnumber in query)
                {
                    evaultnumber.deleted = true;
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
        public ObservableCollection<vaultnumber> readVaultnumber()
        {
            ObservableCollection<vaultnumber> vaultnumbers = new ObservableCollection<vaultnumber>();

            using (var db = new DataContext())
            {
                var query = from qvaultnumber in db.vaultnumber
                            where qvaultnumber.deleted == false
                            select qvaultnumber;

                foreach (vaultnumber evaultnumber in query)
                {
                    vaultnumbers.Add(evaultnumber);
                }
            }

            return vaultnumbers;
        }
        public void createVaultnumber(vaultnumber vaultnumber)
        {
            using (var db = new DataContext())
            {
                db.vaultnumber.Add(vaultnumber);

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
        public void updateVaultnumber(vaultnumber vaultnumber)
        {
            if (vaultnumber.vaultnumber_id != 0)
            {
                using (var db = new DataContext())
                {
                    var query = from qvaultnumber in db.vaultnumber
                                where qvaultnumber.vaultnumber_id == vaultnumber.vaultnumber_id
                                select qvaultnumber;

                    foreach (vaultnumber evaultnumber in query)
                    {
                        evaultnumber.code = vaultnumber.code;
                        evaultnumber.description = vaultnumber.description;
                        evaultnumber.difficulty = vaultnumber.difficulty;
                        evaultnumber.male_female = vaultnumber.male_female;
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
            else
            {
                createVaultnumber(vaultnumber);
            }
        }
    }
}
