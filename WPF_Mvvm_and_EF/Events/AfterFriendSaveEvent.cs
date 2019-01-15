using Prism.Events;

namespace WPF_Mvvm_and_EF.Events
{
    public class AfterFriendSaveEvent : PubSubEvent<AfterFriendSaveEventArgs>
    {
    }

    public class AfterFriendSaveEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }

}
