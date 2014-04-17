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
        /// This module is responsible for the......
        /// ........................................
        /// ........................................
        /// ........................................
        /// </summary>
        public Gymnast(int id, long gymnasticFederationId, GenderEnum gender, String nationality, int length, DateTime birthday, String name, 
                       String surname, String surnamePrefix, List<Vault> vaultCollection)
        {

        }
    }
}
