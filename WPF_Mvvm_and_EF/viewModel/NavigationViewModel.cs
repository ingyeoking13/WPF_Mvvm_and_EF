using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WPF_Mvvm_and_EF.Data;
using WPF_Mvvm_and_EF.Data.LookUps;
using WPF_Mvvm_and_EF.Events;
using WPF_Mvvm_and_EF.Helper;
using WPF_Mvvm_and_EF.Model;

namespace WPF_Mvvm_and_EF.viewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private IFriendLookupDataService _svc;
        private IEventAggregator eventAggregator;

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
            var item = Friends.SingleOrDefault(l => l.Id == obj.Id);
            if ( item == null)
            {
                Friends.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, eventAggregator));
            }
            else item.DisplayMember = obj.DisplayMember;
        }

        public async Task LoadAsync()
        {
            var lookup = await _svc.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var i in lookup)
            {
                Friends.Add(new NavigationItemViewModel(i.Id, i.DisplayMember, eventAggregator));
            }
        }
    }
}
