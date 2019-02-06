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
        public ICommand CreateNewDetailCommand { get; }

        private IDetailViewModel _detailViewModel;
        public IDetailViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            private set {
                _detailViewModel = value;
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
                GetEvent<OpenDetailViewEvent>().
                Subscribe(OnOpenDetailView);
            eventAggregator.GetEvent<AfterDetailDeleteEvent>().Subscribe(AfterDetailDeleted);

            this.CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetail);
            this.navigationViewModel = navigationViewModel;
        }

        private void OnCreateNewDetail(Type viewModelType)
        {
            OnOpenDetailView(new OpenDetailViewEventArgs
            {
                ViewModelName = viewModelType.Name
            });

        }

        public async Task LoadAsync()
        {
            await navigationViewModel.LoadAsync();
        }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if ( DetailViewModel!=null && DetailViewModel.hasChanges)
            {
                var ret = messageDialogService.ShowOkCancelDialog("You've made changes", "Are you OK?");
                if (ret == MessageDialogResult.Cancel) return;
            }

            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    DetailViewModel =  _friendDetailViewModelCreator();
                    break;
            }

            await DetailViewModel.LoadAsync(args.Id);
        }

        private void AfterDetailDeleted(AfterDetailDeleteEventArgs args)
        {
            DetailViewModel = null;
        }
    }
}
