using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WPF_Mvvm_and_EF.Data;
using WPF_Mvvm_and_EF.Events;
using WPF_Mvvm_and_EF.Helper;
using WPF_Mvvm_and_EF.Model;

namespace WPF_Mvvm_and_EF.viewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private IFriendLookupDataService _svc;
        private IEventAggregator eventAggregator;

        private NavigationItemViewModel _selectedFriend;
        public NavigationItemViewModel selectedFriend
        {
            get { return _selectedFriend; }
            set {
                _selectedFriend = value;
                OnPropertyChanged();

                if ( _selectedFriend != null)
                {
                    eventAggregator.GetEvent<OpenFriendDetailViewEvent>().
                        Publish(_selectedFriend.Id);
                }
            }
        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; }

        public NavigationViewModel(IFriendLookupDataService svc, 
            IEventAggregator eventAggregator
            )
        {
            _svc = svc;
            this.eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            eventAggregator.GetEvent<AfterFriendSaveEvent>().Subscribe(AfterFriendSaved); ;
        }

        private void AfterFriendSaved(AfterFriendSaveEventArgs obj)
        {
            var item = Friends.Single(l => l.Id == obj.Id);
            item.DisplayMember = obj.DisplayMember;
            OnPropertyChanged();
        }

        public async Task LoadAsync()
        {
            var lookup = await _svc.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var i in lookup)
            {
                Friends.Add(new NavigationItemViewModel(i.Id, i.DisplayMember));
            }
        }
    }
}
