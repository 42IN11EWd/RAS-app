using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Model
{
    public class Gymnast
    {
        // Properties
        public int Id { get; set; }
        public long GymnasticsFederationId { get; set; }
        public GenderEnum Gender { get; set; }
        public String Nationality { get; set; }
        public int Length { get; set; }
        public DateTime Birthday { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String SurnamePrefix { get; set; }
        public List<Vault> VaultCollection { get; set; }

        /// <summary>
        /// This constructor will be used to set a gymnast loaded from the database.
        /// </summary>
        public Gymnast(int id, long gymnasticsFederationId, GenderEnum gender, String nationality, int length, DateTime birthday, String name, 
                       String surname, String surnamePrefix, List<Vault> vaultCollection)
        {
            Id = id;
            GymnasticsFederationId = gymnasticsFederationId;
            Gender = gender;
            Nationality = nationality;
            Length = length;
            Birthday = birthday;
            Name = name;
            Surname = surname;
            SurnamePrefix = surnamePrefix;
            VaultCollection = vaultCollection;
        }

        /// <summary>
        /// This constructor will be used to create a new gymnast that doesnt exist in the database.
        /// </summary>
        public Gymnast(long gymnasticsFederationId, GenderEnum gender, String nationality, int length, DateTime birthday, String name,
                       String surname, String surnamePrefix, List<Vault> vaultCollection)
        {
            GymnasticsFederationId = gymnasticsFederationId;
            Gender = gender;
            Nationality = nationality;
            Length = length;
            Birthday = birthday;
            Name = name;
            Surname = surname;
            SurnamePrefix = surnamePrefix;
            VaultCollection = vaultCollection;
        }

        public Gymnast()
        {
            // TODO: Complete member initialization
        }
    }
}
