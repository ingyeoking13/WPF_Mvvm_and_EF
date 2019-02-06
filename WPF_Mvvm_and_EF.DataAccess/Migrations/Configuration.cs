namespace WPF_Mvvm_and_EF.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

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

            context.ProgrammingLanguages.AddOrUpdate(
                pl => pl.Name,
                new Model.ProgrammingLanguage { Name = "C#" },
                new Model.ProgrammingLanguage { Name = "TypeScript" },
                new Model.ProgrammingLanguage { Name = "F#" },
                new Model.ProgrammingLanguage { Name = "Swift" },
                new Model.ProgrammingLanguage { Name = "Java" }
                );

            context.SaveChanges();

            context.FriendPhoneNumbers.AddOrUpdate(pn => pn.Number,
                new Model.FriendPhoneNumber {
                    Number = "+82 1012345678",
                    FriendId = context.Friends.First().Id });
        }
    }
}
