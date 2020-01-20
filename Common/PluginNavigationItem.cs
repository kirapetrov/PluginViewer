using System;
using Caliburn.Micro;

namespace Common
{
    public class PluginNavigationItem : PropertyChangedBase
    {
        private bool isActivated;

        public IPluginViewModel MainViewModel { get; protected set; }

        public string Name { get; protected set; }

        public Action<IPluginViewModel> ActivateAction { get; set; }

        public bool IsActivated
        {
            get => isActivated;
            set
            {
                isActivated = value;
                NotifyOfPropertyChange(nameof(IsActivated));
            }
        }

        public void Activate()
        {
            IsActivated = true;
            ActivateAction?.Invoke(MainViewModel);
        }
    }
}
