//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RunApproachStatistics.Model.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class gymnast
    {
        public gymnast()
        {
            this.vault = new HashSet<vault>();
            this.user = new HashSet<user>();
        }
    
        public int gymnast_id { get; set; }
        public long turnbondID { get; set; }
        public string gender { get; set; }
        public string nationality { get; set; }
        public Nullable<decimal> length { get; set; }
        public byte[] picture { get; set; }
        public Nullable<System.DateTime> birthdate { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string surname_prefix { get; set; }
        public bool deleted { get; set; }
        public Nullable<decimal> weight { get; set; }
        public string note { get; set; }
    
        public virtual ICollection<vault> vault { get; set; }
        public virtual ICollection<user> user { get; set; }
    }
}
