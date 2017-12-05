namespace FriendOrganizer.DataAccess.Migrations
{
    using FriendOrganizer.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FriendOrganizer.DataAccess.FriendOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendOrganizer.DataAccess.FriendOrganizerDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Friends.AddOrUpdate(f => f.FirstName,
                new Friend { FirstName = "Josh", LastName = "Smith" },
                new Friend { FirstName = "Ziva", LastName = "David" },
                new Friend { FirstName = "Jethro", LastName = "Gibbs" },
                new Friend { FirstName = "Anthony", LastName = "DiNozzo" }
                );

            context.Languages.AddOrUpdate(l => l.Name,
                new Language { Name = "English" },
                new Language { Name = "French" },
                new Language { Name = "German" },
                new Language { Name = "Italian" },
                new Language { Name = "Spanish" },
                new Language { Name = "Farsi" } );
        }
    }
}
