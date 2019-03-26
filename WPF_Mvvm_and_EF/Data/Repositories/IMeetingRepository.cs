using System.Collections.Generic;
using System.Threading.Tasks;
using WPF_Mvvm_and_EF.Model;

namespace WPF_Mvvm_and_EF.Data.Repositories
{
    public interface IMeetingRepository : IGenericRepository<Meeting>
    {
        Task<List<Friend>> GetAllFriendsAsync();
    }
}