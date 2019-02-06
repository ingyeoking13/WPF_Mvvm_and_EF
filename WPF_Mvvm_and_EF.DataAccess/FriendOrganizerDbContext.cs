using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using WPF_Mvvm_and_EF.Model;

namespace WPF_Mvvm_and_EF.DataAccess
{
    public class FriendOrganizerDbContext : DbContext
    {
        public FriendOrganizerDbContext() : base("FriendOrganizerDb")
        {
                
        }

        public DbSet<Friend>  Friends { get; set; }
        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public DbSet<FriendPhoneNumber> FriendPhoneNumbers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new FriendConfiguration());
        }
    }
}
