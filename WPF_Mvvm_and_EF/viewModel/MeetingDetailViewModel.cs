using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using WPF_Mvvm_and_EF.Data.Repositories;
using WPF_Mvvm_and_EF.Model;
using WPF_Mvvm_and_EF.Services;
using WPF_Mvvm_and_EF.Wrapper;

namespace WPF_Mvvm_and_EF.viewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private IMeetingRepository _meetingRepository;
        private MeetingWrapper _meeting;
        private IMessageDialogService _messageDialogService;
        private Friend _selectedAvailableFriend;
        private Friend _selectedAddedFriend;
        private List<Friend> _allFriends;

        public MeetingDetailViewModel(IEventAggregator eventAggregator,
          IMessageDialogService messageDialogService,
          IMeetingRepository meetingRepository) : base(eventAggregator)
        {
            _meetingRepository = meetingRepository;
            _messageDialogService = messageDialogService;

            AddedFriends = new ObservableCollection<Friend>();
            AvailableFriends = new ObservableCollection<Friend>();

            AddFriendCommand = new DelegateCommand(OnAddFriendExecute, OnAddFriendCanExecute);
            RemoveFriendCommand = new DelegateCommand(OnRemoveFriendExecute, OnRemoveFriendCanExecute);
        }

        public ICommand AddFriendCommand { get; }
        public ICommand RemoveFriendCommand { get; }

        public MeetingWrapper Meeting
        {
            get { return _meeting; }
            private set
            {
                _meeting = value;
                OnPropertyChanged();
            }
        }

        public Friend SelectedAvailableFriend {
            get => _selectedAvailableFriend;
            set
            {
                _selectedAvailableFriend = value;
                OnPropertyChanged();
                ((DelegateCommand)AddFriendCommand).RaiseCanExecuteChanged();
            }
        }

        public Friend SelectedAddedFriend {
            get => _selectedAddedFriend;
            set
            {
                _selectedAddedFriend = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveFriendCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Friend> AddedFriends { get; }
        public ObservableCollection<Friend> AvailableFriends { get; }

        public override async Task LoadAsync(int? meetingId)
        {
            var meeting = meetingId.HasValue
              ? await _meetingRepository.GetByIdAsync(meetingId.Value)
              : CreateNewMeeting();

            InitializeMeeting(meeting);
            Id = meeting.Id;

            _allFriends = await _meetingRepository.GetAllFriendsAsync();
            SetupPicklist();
        }

        private void SetupPicklist()
        {
            var meetingFriendIds = Meeting.Model.Friends.Select(f => f.Id).ToList();
            var addedFriends = _allFriends.Where(f => meetingFriendIds.Contains(f.Id)).OrderBy(f=>f.FirstName);
            var availableFriends = _allFriends.Except(addedFriends).OrderBy(f => f.FirstName);

            AddedFriends.Clear();
            AvailableFriends.Clear();

            foreach(var addedFriend in addedFriends) AddedFriends.Add(addedFriend);
            foreach(var availableFriend in availableFriends) AvailableFriends.Add(availableFriend);
        }

        protected override void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog($"Do you really want to delete the meeting {Meeting.Title}?", "Question");
            if (result == MessageDialogResult.Ok)
            {
                _meetingRepository.Delete(Meeting.Model);
                _meetingRepository.SaveAsync();
                RaiseDetailDeleteEvent(Meeting.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && hasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await _meetingRepository.SaveAsync();
            hasChanges = _meetingRepository.HasChanges();
            Id = _meeting.Id;
            RaiseDetailSaveEvent(Meeting.Id, Meeting.Title);
        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date
            };
            _meetingRepository.Add(meeting);
            return meeting;
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (s, e) =>
            {
                if (!hasChanges)
                {
                    hasChanges = _meetingRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Meeting.Id == 0) Meeting.Title = "";
        }

        private bool OnRemoveFriendCanExecute()
        {
            return SelectedAddedFriend != null;
        }

        private bool OnAddFriendCanExecute()
        {
            return SelectedAvailableFriend != null;
        }

        private void OnRemoveFriendExecute() {
            var friendToAdd = SelectedAddedFriend;

            Meeting.Model.Friends.Remove(friendToAdd);

            AddedFriends.Remove(friendToAdd);
            AvailableFriends.Add(friendToAdd);
            hasChanges = _meetingRepository.HasChanges();

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }
        private void OnAddFriendExecute() {
            var friendToAdd = SelectedAvailableFriend;

            Meeting.Model.Friends.Add(friendToAdd);

            AddedFriends.Add(friendToAdd);
            AvailableFriends.Remove(friendToAdd);
            hasChanges = _meetingRepository.HasChanges();

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }
    }
}
