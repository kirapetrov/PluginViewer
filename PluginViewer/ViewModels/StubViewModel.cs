using System.ComponentModel.Composition;
using Caliburn.Micro;
using Common;

namespace PluginViewer.ViewModels
{
    [Export]
    public class StubViewModel : PropertyChangedBase, IPluginViewModel
    {
    }
}
