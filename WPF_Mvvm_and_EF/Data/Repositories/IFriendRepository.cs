using System.Threading.Tasks;
using WPF_Mvvm_and_EF.Model;

namespace WPF_Mvvm_and_EF.Data.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<T> GetByIdAsync(int? dataId);
        Task SaveAsync();
        void Add(T data);
        void Delete(T data);
        bool HasChanges();
    }

    public interface IFriendRepository: IGenericRepository<Friend>
    {
       void RemovePhoneNumber(FriendPhoneNumber model);
    }
}