using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WPF_Mvvm_and_EF.Data.LookUps;
using WPF_Mvvm_and_EF.Events;
using WPF_Mvvm_and_EF.Helper;

namespace WPF_Mvvm_and_EF.viewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private IFriendLookupDataService _svc;
        private IMeetingLookupDataService meetingLookupService;
        private IEventAggregator eventAggregator;

        public ObservableCollection<NavigationItemViewModel> Friends { get; }
        public ObservableCollection<NavigationItemViewModel> Meetings { get; }

        public NavigationViewModel(IFriendLookupDataService svc, 
            IMeetingLookupDataService meetingLookupService,
            IEventAggregator eventAggregator
            )
        {
            _svc = svc;
            this.meetingLookupService = meetingLookupService;
            this.eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();
            eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved); ;
            eventAggregator.GetEvent<AfterDetailDeleteEvent>().Subscribe(AfterDetailDeleted); ;
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs obj)
        {
            switch (obj.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    AfterDetailSaved(Friends, obj);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailSaved(Meetings, obj);
                    break;
            }
        }

        private void AfterDetailDeleted(AfterDetailDeleteEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    AfterDetailDeleted(Friends, args);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailDeleted(Meetings, args);
                    break;
            }
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs args)
        {
            var item = items.SingleOrDefault(l => l.Id == args.Id);
            if (item == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember,
                    args.ViewModelName,
                    eventAggregator));
            }
            else item.DisplayMember = args.DisplayMember;
        }

         private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, AfterDetailDeleteEventArgs args)
        {
            var item = items.SingleOrDefault(n => n.Id == args.Id);
            if ( item != null) items.Remove(item);
        }

        public async Task LoadAsync()
        {
            var lookup = await _svc.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var i in lookup)
            {
                Friends.Add(new NavigationItemViewModel(i.Id, i.DisplayMember, 
                    nameof(FriendDetailViewModel), eventAggregator));
            }

            lookup = await meetingLookupService.GetMeetingLookupAsync();
            Meetings.Clear();
            foreach (var i in lookup)
            {
                Meetings.Add(new NavigationItemViewModel(
                    i.Id, i.DisplayMember, 
                    nameof(MeetingDetailViewModel), eventAggregator ));
            }
        }
    }
}
