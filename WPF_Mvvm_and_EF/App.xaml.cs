using Autofac;
using System.Windows;
using WPF_Mvvm_and_EF.Startup;

namespace WPF_Mvvm_and_EF
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootStrapper = new Bootstrapper();
            var container = bootStrapper.BootStrap();
            var mainwindow = container.Resolve<MainWindow>();
            mainwindow.Show();
        }
    }
}
