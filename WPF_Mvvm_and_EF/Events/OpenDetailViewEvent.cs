using Prism.Events;

namespace WPF_Mvvm_and_EF.Events
{
    public class OpenDetailViewEvent:PubSubEvent<OpenDetailViewEventArgs>
    {
    }

    public class OpenDetailViewEventArgs
    {
        public int? Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
