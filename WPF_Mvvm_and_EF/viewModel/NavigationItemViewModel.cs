using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Mvvm_and_EF.Events;
using WPF_Mvvm_and_EF.Helper;

namespace WPF_Mvvm_and_EF.viewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember =  displayMember;
            this.eventAggregator = eventAggregator;
            OpenFriendDetailViewCommand = new DelegateCommand(OnOpenFriendDetailView);

        }

        public int Id { get; set; }
        public ICommand OpenFriendDetailViewCommand { get; }
        private string _displaymember;
        private readonly IEventAggregator eventAggregator;

        public string DisplayMember
        {
            get { return _displaymember; }
            set {
                _displaymember = value;
                OnPropertyChanged();
            }
        }

        private void OnOpenFriendDetailView()
        {
            eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Publish(Id);
        }
    }
}
