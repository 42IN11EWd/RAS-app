﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<gymnast> gymnast { get; set; }
        public virtual DbSet<location> location { get; set; }
        public virtual DbSet<vaultnumber> vaultnumber { get; set; }
        public virtual DbSet<vault> vault { get; set; }
        public virtual DbSet<user> user { get; set; }
    }
}
