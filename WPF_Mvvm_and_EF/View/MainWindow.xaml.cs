using System.Windows;
using WPF_Mvvm_and_EF.Data;
using WPF_Mvvm_and_EF.viewModel;

namespace WPF_Mvvm_and_EF
{
    public partial class MainWindow : Window
    {
        private MainViewModel vm;
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();

            this.vm = vm;
            DataContext = this.vm;

            Loaded += MainWindow_Loaded;

        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await vm.LoadAsync();
        }
    }
}
