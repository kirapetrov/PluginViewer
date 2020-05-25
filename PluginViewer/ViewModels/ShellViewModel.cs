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

        public ButtonWithDropDownViewModel ButtonWithDropDown { get; }

        public ObservableCollection<PluginNavigationItem> Plugins { get; }

        [ImportingConstructor]
        public ShellViewModel(
            [Import] StubViewModel stub,
            [Import] ButtonWithDropDownViewModel buttonWithDropDown,
            [ImportMany] IEnumerable<PluginNavigationItem> navigationItems)
        {
            this.stub = stub;

            ButtonWithDropDown = buttonWithDropDown;

            Plugins = new ObservableCollection<PluginNavigationItem>(navigationItems);
            foreach (var plugin in Plugins)
                plugin.ActivateAction = ActivatePlugin;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            if (Plugins.Any())
                Plugins.First().Activate();
            else
                ActivateItem(stub);
        }

        private void ActivatePlugin(IPluginViewModel pluginViewModel)
        {
            DeactivatePlugin();
            ActivateItem(pluginViewModel);
        }

        private void DeactivatePlugin()
        {
            var activated = Plugins.FirstOrDefault(x => x.MainViewModel == ActiveItem);
            if (activated != null)
                activated.IsActivated = false;
        }
    }
}
