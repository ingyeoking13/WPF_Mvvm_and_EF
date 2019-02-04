using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Mvvm_and_EF.Events;
using WPF_Mvvm_and_EF.Helper;
using WPF_Mvvm_and_EF.Services;

namespace WPF_Mvvm_and_EF.viewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IMessageDialogService messageDialogService;
        private readonly Func<IFriendDetailViewModel> _friendDetailViewModelCreator;

        public INavigationViewModel navigationViewModel { get; }
        public ICommand CreateNewFreindCommand { get; }
        private IFriendDetailViewModel _friendDetailViewModel;

        public IFriendDetailViewModel friendDetailViewModel
        {
            get { return _friendDetailViewModel; }
            private set {
                _friendDetailViewModel = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel(INavigationViewModel navigationViewModel,
            Func<IFriendDetailViewModel> friendDetailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService
            )
        {

            this.eventAggregator = eventAggregator;
            this.messageDialogService = messageDialogService;
            _friendDetailViewModelCreator = friendDetailViewModelCreator;

            this.eventAggregator.
                GetEvent<OpenFriendDetailViewEvent>().
                Subscribe(OnOpenFriendDetailView);

            this.CreateNewFreindCommand = new DelegateCommand(OnCreateNewFriend);
            this.navigationViewModel = navigationViewModel;
        }

        private void OnCreateNewFriend()
        {
            OnOpenFriendDetailView(null);
//            throw new NotImplementedException();
        }

        public async Task LoadAsync()
        {
            await navigationViewModel.LoadAsync();
        }

        private async void OnOpenFriendDetailView(int? friendId)
        {
            if ( friendDetailViewModel!=null && friendDetailViewModel.hasChanges)
            {
                var ret = messageDialogService.ShowOkCancelDialog("You've made changes", "Are you OK?");
                if (ret == MessageDialogResult.Cancel) return;
            }
            friendDetailViewModel =  _friendDetailViewModelCreator();
            await friendDetailViewModel.LoadAsync(friendId);
        }
    }
}
