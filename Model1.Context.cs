﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebPubApp
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ArticleType> ArticleTypes { get; set; }
        public virtual DbSet<Authorship> Authorships { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Journal> Journals { get; set; }
        public virtual DbSet<Keyword> Keywords { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<OrgBranch> OrgBranches { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Publication> Publications { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
