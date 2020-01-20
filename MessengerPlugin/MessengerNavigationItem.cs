using System.ComponentModel.Composition;
using Common;
using MessengerPlugin.ViewModels;

namespace MessengerPlugin
{
    [Export(typeof(PluginNavigationItem))]
    public class MessengerNavigationItem : PluginNavigationItem
    {
        [ImportingConstructor]
        public MessengerNavigationItem(MessengerViewModel messengerViewModel)
        {
            MainViewModel = messengerViewModel;
            Name = "Messenger";
        }
    }
}
