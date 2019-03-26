using Autofac.Features.Indexed;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Mvvm_and_EF.Events;
using WPF_Mvvm_and_EF.Helper;
using WPF_Mvvm_and_EF.Services;

namespace WPF_Mvvm_and_EF.viewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IIndex<string, IDetailViewModel> detailViewModelCreator;
        private readonly IEventAggregator eventAggregator;
        private readonly IMessageDialogService messageDialogService;

        public INavigationViewModel navigationViewModel { get; }
        public ICommand CreateNewDetailCommand { get; }

        public ObservableCollection<IDetailViewModel> DetailViewModels { get; }

        private IDetailViewModel _selecteddetailViewModel;
        public IDetailViewModel SelectedDetailViewModel
        {
            get { return _selecteddetailViewModel; }
            set {
                _selecteddetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(INavigationViewModel navigationViewModel,
            IIndex<string, IDetailViewModel> detailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService
            )
        {

            this.eventAggregator = eventAggregator;
            this.messageDialogService = messageDialogService;
            this.eventAggregator.GetEvent<OpenDetailViewEvent>().Subscribe(OnOpenDetailView);
            eventAggregator.GetEvent<AfterDetailDeleteEvent>().Subscribe(AfterDetailDeleted);

            DetailViewModels = new ObservableCollection<IDetailViewModel>();
            this.CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetail);
            this.navigationViewModel = navigationViewModel;
            this.detailViewModelCreator = detailViewModelCreator;
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
            var detailViewModel = DetailViewModels.SingleOrDefault(
                viewModel => viewModel.Id == args.Id && viewModel.GetType().Name == args.ViewModelName);

            if (detailViewModel == null)
            {
                detailViewModel = detailViewModelCreator[args.ViewModelName];
                await detailViewModel.LoadAsync(args.Id);
                DetailViewModels.Add(detailViewModel);
            }
            SelectedDetailViewModel = detailViewModel;

        }

        private void AfterDetailDeleted(AfterDetailDeleteEventArgs args)
        {
            var detailViewModel = DetailViewModels.SingleOrDefault(
             viewModel => viewModel.Id == args.Id && viewModel.GetType().Name == args.ViewModelName);

            if ( detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }
        }
    }
}
