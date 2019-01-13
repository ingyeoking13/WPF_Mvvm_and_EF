using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Mvvm_and_EF.Data;
using WPF_Mvvm_and_EF.Events;
using WPF_Mvvm_and_EF.Helper;
using WPF_Mvvm_and_EF.Model;

namespace WPF_Mvvm_and_EF.viewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IFriendDataService dataService;
        private IEventAggregator eventAggregator;

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
        }

        private async void OnOpenFriendDetailView(int friendId)
        {
            await LoadAsync(friendId);
        }

        public async Task LoadAsync(int friendId)
        {
            friend = await dataService.GetByIdAsync(friendId);
        }
        private Friend _friend;

        public Friend friend
        {
            get { return _friend; }
            set {
                _friend = value;
                OnPropertyChanged();
            }
        }

    }
}
