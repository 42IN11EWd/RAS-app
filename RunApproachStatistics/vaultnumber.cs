//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RunApproachStatistics
{
    using System;
    using System.Collections.Generic;
    
    public partial class vaultnumber
    {
        public vaultnumber()
        {
            this.vault = new HashSet<vault>();
        }
    
        public int vaultnumber_id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string male_female { get; set; }
        public Nullable<decimal> difficulty { get; set; }
    
        public virtual ICollection<vault> vault { get; set; }
    }
}
