using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Mvvm_and_EF.Events;
using WPF_Mvvm_and_EF.Helper;

namespace WPF_Mvvm_and_EF.viewModel
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        private bool _hasChanges;
        private IEventAggreagator eventAggreagator;
        private int? id;
        private string _title;

        public bool hasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private readonly IEventAggregator eventAggregator;
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public int? Id
        {
            get { return id; }
            protected set { id = value; }
        }

        public DetailViewModelBase(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

        protected DetailViewModelBase(IEventAggreagator eventAggreagator)
        {
            this.eventAggreagator = eventAggreagator;
        }

        protected abstract void OnDeleteExecute();
        protected abstract bool OnSaveCanExecute();
        protected abstract void OnSaveExecute();

        public abstract Task LoadAsync(int? friendId);

        protected virtual void RaiseDetailDeleteEvent(int modelId)
        {
            eventAggregator.GetEvent<AfterDetailDeleteEvent>().Publish(new AfterDetailDeleteEventArgs
            {
                Id = modelId,
                ViewModelName = this.GetType().Name
            });

        }
        protected virtual void RaiseDetailSaveEvent(int modelId, string displayMember)
        {
            eventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(new AfterDetailSavedEventArgs
            {
                Id = modelId,
                DisplayMember = displayMember,
                ViewModelName = this.GetType().Name
            });
        }
    }
}
