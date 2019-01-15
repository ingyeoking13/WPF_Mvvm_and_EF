using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Mvvm_and_EF.Helper;

namespace WPF_Mvvm_and_EF.viewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public NavigationItemViewModel(int id, string displayMember)
        {
            Id = id;
            DisplayMember =  displayMember;

        }
        private string _displaymember;

        public string DisplayMember
        {
            get { return _displaymember; }
            set {
                _displaymember = value;
                OnPropertyChanged();
            }
        }
    }
}
