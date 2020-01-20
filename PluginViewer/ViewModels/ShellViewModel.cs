using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Common;

namespace PluginViewer.ViewModels
{
    [Export]
    public class ShellViewModel : Conductor<IPluginViewModel>.Collection.OneActive
    {
        private readonly StubViewModel stub;

        public ObservableCollection<PluginNavigationItem> Plugins { get; }

        [ImportingConstructor]
        public ShellViewModel(
            [Import] StubViewModel stub,
            [ImportMany] IEnumerable<PluginNavigationItem> navigationItems)
        {
            this.stub = stub;

            Plugins = new ObservableCollection<PluginNavigationItem>(navigationItems);
            foreach (var plugin in Plugins)
                plugin.ActivateAction = ActivateItem;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            var activeItem = Plugins.Any() ? Plugins.First().MainViewModel : stub;
            ActivateItem(activeItem);
        }
    }
}
