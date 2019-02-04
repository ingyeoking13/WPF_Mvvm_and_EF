using System.Collections.Generic;
using System.Threading.Tasks;
using WPF_Mvvm_and_EF.Model;

namespace WPF_Mvvm_and_EF.Data.Repositories
{
    public interface IFriendRepository
    {
        Task<Friend> GetByIdAsync(int? friendId);
        Task SaveAsync();
        void Add(Friend friend);
        void Delete(Friend friend);
        bool HasChanges();
    }
}