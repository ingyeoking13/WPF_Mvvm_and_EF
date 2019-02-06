using Prism.Events;

namespace WPF_Mvvm_and_EF.Events
{
    public class AfterDetailDeleteEvent:PubSubEvent<AfterDetailDeleteEventArgs>
    {
    }

    public class AfterDetailDeleteEventArgs
    {
        public int Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
