using System.Threading.Tasks;

namespace WPF_Mvvm_and_EF.viewModel
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int? friendId);
        bool hasChanges { get; }
        int? Id { get; }
    }
}