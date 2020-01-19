using System.ComponentModel.Composition;
using Caliburn.Micro;
using Common;

namespace PluginViewer.ViewModels
{
    [Export]
    public class ShellViewModel : Conductor<IPluginViewModel>.Collection.OneActive
    {
        [Import] public IPluginViewModel TestProperty { get; set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            ActivateItem(TestProperty);
        }
    }
}
