using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Mvvm_and_EF.Data.Repositories;
using WPF_Mvvm_and_EF.Events;
using WPF_Mvvm_and_EF.Helper;
using WPF_Mvvm_and_EF.Model;
using WPF_Mvvm_and_EF.Wrapper;

namespace WPF_Mvvm_and_EF.viewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IFriendRepository friendRepository;
        private IEventAggregator eventAggregator;
        private FriendWrapper _friend;
        private bool _hasChanges;

        public bool hasChanges
        {
            get { return _hasChanges; }
            set {
                if(_hasChanges != value)
                {

                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public FriendWrapper friend
        {
            get { return _friend; }
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public FriendDetailViewModel(
            IFriendRepository dataService, IEventAggregator eventAggregator
            )
        {
            this.friendRepository = dataService;
            this.eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

        private async void OnDeleteExecute()
        {
            friendRepository.Delete(friend.Model);
            await friendRepository.SaveAsync();
        }

        public async Task LoadAsync(int? friendId)
        {
            var friend_ = friendId.HasValue? 
                await friendRepository.GetByIdAsync(friendId):CreateNewFriend();
            friend = new FriendWrapper(friend_);

            friend.PropertyChanged += (s, e) =>
            {

                if (!hasChanges) hasChanges = friendRepository.HasChanges();
                if(e.PropertyName == nameof(friend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (friend.Id == 0) friend.FirstName = "";
        }

        private async void OnSaveExecute()
        {
            await friendRepository.SaveAsync();
            hasChanges = friendRepository.HasChanges();
            eventAggregator.GetEvent<AfterFriendSaveEvent>().Publish(
                new AfterFriendSaveEventArgs
                {
                    Id = friend.Id,
                    DisplayMember=$"{friend.FirstName} {friend.LastName}"
                } );
        }

        private bool OnSaveCanExecute()
        {
            return friend != null && !friend.HasErrors && hasChanges;
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            friendRepository.Add(friend);
            return friend;
        }
    }
}
