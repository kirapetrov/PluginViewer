using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        #region MyRegion


        public ConcurrentStack<int> IntegerValues = new ConcurrentStack<int>();

        public int Counter = 1;

        public Task Worker;

        public CancellationTokenSource CancellationTokenSource;

        public void StartProcess()
        {
            IntegerValues.Push(Counter);
            Counter++;

            CancellationTokenSource?.Cancel();
            if (Worker?.Status == TaskStatus.Running)
            {
                Console.WriteLine("Running");
                return;
            }

            Worker = Task.Run(SomeWork);
        }

        private async void SomeWork()
        {
            Console.WriteLine("Start");
            CancellationTokenSource = new CancellationTokenSource();
            IntegerValues.TryPop(out var result);
            IntegerValues.Clear();
            await IntegerHandler(result, CancellationTokenSource.Token);
            var isCanceled = CancellationTokenSource.IsCancellationRequested;
            CancellationTokenSource.Dispose();
            CancellationTokenSource = null;
            if (!isCanceled)
            {
                Console.WriteLine("Yeah");
                return;
            }

            SomeWork();
        }

        private Task IntegerHandler(int value, CancellationToken token)
        {
            Console.WriteLine($"Integer is {value}");
            Thread.Sleep(3000);
            Console.WriteLine("After sleep");
            Console.WriteLine(token.IsCancellationRequested);
            return Task.CompletedTask;
        }

        #endregion


    }
}
