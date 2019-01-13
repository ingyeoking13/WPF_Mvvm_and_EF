using Autofac;
using Prism.Events;
using WPF_Mvvm_and_EF.Data;
using WPF_Mvvm_and_EF.DataAccess;
using WPF_Mvvm_and_EF.viewModel;

namespace WPF_Mvvm_and_EF.Startup
{
    public class Bootstrapper
    {
        public IContainer BootStrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<FriendOrganizerDbContext>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();

            builder.RegisterType<FriendDataService>().As<IFriendDataService>();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<FriendDetailViewModel>().As<IFriendDetailViewModel>();

            return builder.Build();
        }
    }
}
