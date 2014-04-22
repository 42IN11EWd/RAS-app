using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Model
{
    public class Vault
    {
        public int vault_id { get; set; }
        public int gymnast_id { get; set; }
        public decimal duration { get; set; }
        public string graphdata { get; set; }
        public string videopath { get; set; }
        public Nullable<int> rating_star { get; set; }
        public Nullable<int> rating_official_D { get; set; }
        public Nullable<int> rating_official_E { get; set; }
        public Nullable<decimal> penalty { get; set; }
        public System.DateTime timestamp { get; set; }
        public string context { get; set; }
        public string note { get; set; }
        public Nullable<int> vaultnumber_id { get; set; }
        public Nullable<int> location_id { get; set; }
        public bool deleted { get; set; }
        public byte[] thumbnail { get; set; }
    }
}
