namespace WPF_Mvvm_and_EF.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WPF_Mvvm_and_EF.Model;

    internal sealed class Configuration : DbMigrationsConfiguration<WPF_Mvvm_and_EF.DataAccess.FriendOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WPF_Mvvm_and_EF.DataAccess.FriendOrganizerDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Friends.AddOrUpdate(
            f => f.FirstName,
                new Friend { FirstName = "정", LastName = "요한" },
                new Friend { FirstName = "박", LastName = "융" },
                new Friend { FirstName = "유", LastName = "비" },
                new Friend { FirstName = "장", LastName = "비" }
            );
        }
    }
}
