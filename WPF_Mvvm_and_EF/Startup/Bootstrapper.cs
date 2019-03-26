using Autofac;
using Prism.Events;
using WPF_Mvvm_and_EF.Data;
using WPF_Mvvm_and_EF.Data.LookUps;
using WPF_Mvvm_and_EF.Data.Repositories;
using WPF_Mvvm_and_EF.DataAccess;
using WPF_Mvvm_and_EF.Services;
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

            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<MeetingDetailViewModel>().
                Keyed<IDetailViewModel>(nameof(MeetingDetailViewModel));
            builder.RegisterType<FriendDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(FriendDetailViewModel));

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<FriendRepository>().As<IFriendRepository>();
            builder.RegisterType<MeetingRepository>().As<IMeetingRepository>();

            return builder.Build();
        }
    }
}
