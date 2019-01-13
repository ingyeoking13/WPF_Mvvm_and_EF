using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPF_Mvvm_and_EF.Data;
using WPF_Mvvm_and_EF.Helper;
using WPF_Mvvm_and_EF.Model;

namespace WPF_Mvvm_and_EF.viewModel
{
    public class MainViewModel : ViewModelBase
    {
        public INavigationViewModel navigationViewModel { get; }
        public IFriendDetailViewModel friendDetailViewModel { get; }

        public MainViewModel(INavigationViewModel navigationViewModel,
            IFriendDetailViewModel friendDetailViewModel
            )
        {
            this.navigationViewModel = navigationViewModel;
            this.friendDetailViewModel = friendDetailViewModel;
        }

        public async Task LoadAsync()
        {
            await navigationViewModel.LoadAsync();
        }
    }
}
