﻿using FriendOrganizer.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FriendOrganizer.DataAccess
{
    public class FriendOrganizerDbContext : DbContext
    {
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

        public FriendOrganizerDbContext() : base("FriendOrganizerDb")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<Friend>()
            //    .Property(f => f.FirstName)
            //    .IsRequired()
            //    .HasMaxLength(50);
        }
    }
}
