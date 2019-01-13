using System.Data.Entity.ModelConfiguration;
using WPF_Mvvm_and_EF.Model;

namespace WPF_Mvvm_and_EF.DataAccess
{
    public class FriendConfiguration : EntityTypeConfiguration<Friend>
    {
        public FriendConfiguration()
        {
        }
    }
}
