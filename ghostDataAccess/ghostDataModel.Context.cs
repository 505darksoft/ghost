﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ghostDataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ghostDBEntities : DbContext
    {
        public ghostDBEntities()
            : base("name=ghostDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Format> Formats { get; set; }
        public virtual DbSet<TextReplace> TextReplaces { get; set; }
        public virtual DbSet<TextReplaceWeb> TextReplaceWebs { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}