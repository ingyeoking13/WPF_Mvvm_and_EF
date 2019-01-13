using System.Threading.Tasks;

namespace WPF_Mvvm_and_EF.viewModel
{
    public interface IFriendDetailViewModel
    {
        Task LoadAsync(int friendId);
    }
}