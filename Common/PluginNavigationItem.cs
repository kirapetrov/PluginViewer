using System;

namespace Common
{
    public class PluginNavigationItem
    {
        public IPluginViewModel MainViewModel { get; protected set; }

        public string Name { get; protected set; }

        public Action<IPluginViewModel> ActivateAction { get; set; }

        public void Activate()
        {
            ActivateAction?.Invoke(MainViewModel);
        }
    }
}
