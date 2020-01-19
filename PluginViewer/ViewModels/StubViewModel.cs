using System.ComponentModel.Composition;
using Caliburn.Micro;
using Common;

namespace PluginViewer.ViewModels
{
    //[Export(typeof(IPluginViewModel))]
    public class StubViewModel : PropertyChangedBase, IPluginViewModel
    {
    }
}
