using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

        private LookupItem _selectedFriend;
        public LookupItem selectedFriend
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

        public ObservableCollection<LookupItem> Friends { get; }

        public NavigationViewModel(IFriendLookupDataService svc
            , IEventAggregator eventAggregator
            )
        {
            _svc = svc;
            this.eventAggregator = eventAggregator;
            Friends = new ObservableCollection<LookupItem>();
        }
        public async Task LoadAsync()
        {
            var lookup = await _svc.GetFriendLookupAsync();
            Friends.Clear();
            foreach(var i in lookup)
            {
                Friends.Add(i);
            }
        }
    }
}
