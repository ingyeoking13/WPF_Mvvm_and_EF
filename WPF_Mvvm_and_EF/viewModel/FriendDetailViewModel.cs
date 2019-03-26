using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Mvvm_and_EF.Data.LookUps;
using WPF_Mvvm_and_EF.Data.Repositories;
using WPF_Mvvm_and_EF.Events;
using WPF_Mvvm_and_EF.Helper;
using WPF_Mvvm_and_EF.Model;
using WPF_Mvvm_and_EF.Services;
using WPF_Mvvm_and_EF.Wrapper;

namespace WPF_Mvvm_and_EF.viewModel
{

    public class FriendDetailViewModel : DetailViewModelBase, IFriendDetailViewModel
    {
        private IFriendRepository friendRepository;
        private readonly IMessageDialogService messageDialogService;
        private readonly IProgramminLanguageLookupDataService programminLanguageLookupDataService;
        private FriendWrapper _friend;
        private FriendPhoneNumberWrapper _SelectedPhoneNumber;

        public FriendPhoneNumberWrapper SelectedPhoneNumber
        {
            get { return _SelectedPhoneNumber; }
            set {
                _SelectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }
        public ICommand AddPhoneNumberCommand { get; }
        public ICommand RemovePhoneNumberCommand { get; }
        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }
        public ObservableCollection<FriendPhoneNumberWrapper> PhoneNumbers { get; }

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
            IFriendRepository dataService, IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService ,
            IProgramminLanguageLookupDataService programminLanguageLookupDataService
            ):base(eventAggregator)
        {
            this.friendRepository = dataService;
            this.messageDialogService = messageDialogService;
            this.programminLanguageLookupDataService = programminLanguageLookupDataService;

            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExectue);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute); 
            ProgrammingLanguages = new ObservableCollection<LookupItem>();
            PhoneNumbers = new ObservableCollection<FriendPhoneNumberWrapper>();
        }

        private bool OnRemovePhoneNumberCanExecute()
        {
            return SelectedPhoneNumber != null;
        }

        private void OnRemovePhoneNumberExecute()
        {
            SelectedPhoneNumber.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            friendRepository.RemovePhoneNumber(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            hasChanges = friendRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddPhoneNumberExectue()
        {
            var newNumber = new FriendPhoneNumberWrapper(new FriendPhoneNumber());
            newNumber.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            _friend.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = "";
        }

        protected override async void OnDeleteExecute()
        {
            if ( await friendRepository.HasMeetingsAsync(friend.Id))
            {
                messageDialogService.ShowInfoDialog($"{friend.FirstName} {friend.LastName} can't be deleted. This friend has a meeting.");
                return;
            }

            var result = messageDialogService.ShowOkCancelDialog($"Really Want to Delete? {friend.FirstName} {friend.LastName}?", "Question");
            if (result == MessageDialogResult.Cancel) return;
            friendRepository.Delete(friend.Model);
            await friendRepository.SaveAsync();
            RaiseDetailDeleteEvent(friend.Id);
        }

        public override async Task LoadAsync(int? friendId)
        {
            var friend_ = friendId.HasValue ?
                await friendRepository.GetByIdAsync(friendId) : CreateNewFriend();

            InitializeFriend(friend_);
            Id = friend_.Id;

            InitializeFriendPhoneNumber(friend_.PhoneNumbers);

            await LoadProgrammingLanguageLoadAsync();
        }

        private void InitializeFriendPhoneNumber(ICollection<FriendPhoneNumber> phoneNumbers)
        {
            foreach (var wrapper in PhoneNumbers)
            {
                wrapper.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            }
            PhoneNumbers.Clear();

            foreach (var friendPhoneNumber in phoneNumbers)
            {
                var wrapper = new FriendPhoneNumberWrapper(friendPhoneNumber);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            }
        }

        private void FriendPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!hasChanges)
            {
                hasChanges = friendRepository.HasChanges();
            }
            if ( e.PropertyName == nameof(FriendPhoneNumberWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void InitializeFriend(Friend friend_)
        {
            friend = new FriendWrapper(friend_);

            friend.PropertyChanged += (s, e) =>
            {

                if (!hasChanges) hasChanges = friendRepository.HasChanges();
                if (e.PropertyName == nameof(friend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
                if (e.PropertyName == nameof(friend.FirstName) ||
                e.PropertyName == nameof(friend.LastName)) SetTitle();
            };

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (friend.Id == 0) friend.FirstName = "";
            SetTitle();
        }

        private void SetTitle()
        {
            Title = $"{friend.FirstName} {friend.LastName}";
        }

        private async Task LoadProgrammingLanguageLoadAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem { DisplayMember = "NONE" });
            var lookup = await programminLanguageLookupDataService.GetProgrammingLanguageLookupAsync();
            foreach (var lookupItem in lookup)
            {
                ProgrammingLanguages.Add(lookupItem);
            }
        }

        protected override async void OnSaveExecute()
        {
            await friendRepository.SaveAsync();
            hasChanges = friendRepository.HasChanges();

            Id = friend.Id;
            RaiseDetailSaveEvent(friend.Id, $"{friend.FirstName} {friend.LastName}");
        }

        protected override bool OnSaveCanExecute()
        {
            return friend != null && PhoneNumbers.All(pn=>!pn.HasErrors) && !friend.HasErrors && hasChanges;
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            friendRepository.Add(friend);
            return friend;
        }
    }
}
