﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace course1Folder.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class instDBEntities : DbContext
    {
        public instDBEntities()
            : base("name=courseDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Likes> Likes { get; set; }
        public virtual DbSet<Photos> Photos { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<Subscribes> Subscribes { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
