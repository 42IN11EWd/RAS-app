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

    [Serializable]
    public partial class user
    {
        public int user_id { get; set; }
        public string type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public Nullable<int> gymnast_id { get; set; }
        public bool deleted { get; set; }
    
        public virtual gymnast gymnast { get; set; }
    }
}
