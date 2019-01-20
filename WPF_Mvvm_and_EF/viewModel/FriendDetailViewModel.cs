using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Mvvm_and_EF.Data;
using WPF_Mvvm_and_EF.Events;
using WPF_Mvvm_and_EF.Helper;
using WPF_Mvvm_and_EF.Wrapper;

namespace WPF_Mvvm_and_EF.viewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IFriendDataService dataService;
        private IEventAggregator eventAggregator;
        private FriendWrapper _friend;

        public FriendDetailViewModel(
            IFriendDataService dataService,
            IEventAggregator eventAggregator
            )
        {
            this.dataService = dataService;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.
                GetEvent<OpenFriendDetailViewEvent>().
                Subscribe(OnOpenFriendDetailView);

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public async Task LoadAsync(int friendId)
        {
            var friend_ = await dataService.GetByIdAsync(friendId);
            friend = new FriendWrapper(friend_);

            friend.PropertyChanged += (s, e) =>
            {
                if(e.PropertyName == nameof(friend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public FriendWrapper friend
        {
            get { return _friend; }
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }


        private async void OnSaveExecute()
        {
            await dataService.SaveAsync(friend.Model);
            eventAggregator.GetEvent<AfterFriendSaveEvent>().Publish(
                new AfterFriendSaveEventArgs
                {
                    Id = friend.Id,
                    DisplayMember=$"{friend.FirstName} {friend.LastName}"
                } );
        }

        private bool OnSaveCanExecute()
        {
            return friend != null && !friend.HasErrors;
        }

        private async void OnOpenFriendDetailView(int friendId)
        {
            await LoadAsync(friendId);
        }
    }
}
