using System.Threading.Tasks;
using WPF_Mvvm_and_EF.Model;

namespace WPF_Mvvm_and_EF.Data.Repositories
{
    public interface IFriendRepository: IGenericRepository<Friend>
    {
       void RemovePhoneNumber(FriendPhoneNumber model);
       Task<bool> HasMeetingsAsync(int friendId);
    }
}