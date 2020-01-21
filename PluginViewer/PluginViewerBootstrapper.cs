using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.ReflectionModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using PluginViewer.ViewModels;

namespace PluginViewer
{
    /// В данном классе, за основу была взята предлагаемая в документации реализация (https://caliburnmicro.com/documentation/bootstrapper)
    /// Я расширил её для загрузки плагинов
    public class PluginViewerBootstrapper : BootstrapperBase
    {
        private List<Assembly> priorityAssemblies;
        private CompositionContainer container;

        public const string PluginFolderName = "Plugins";

        public PluginViewerBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            var directoryCatalog = new DirectoryCatalog(@"./");
            AssemblySource.Instance.AddRange(
                directoryCatalog.Parts
                    .Select(part => ReflectionModelServices.GetPartType(part).Value.Assembly)
                    .Where(assembly => !AssemblySource.Instance.Contains(assembly)));

            LoadPlugins();

            priorityAssemblies = SelectAssemblies().ToList();
            var priorityCatalog = new AggregateCatalog(priorityAssemblies.Select(x => new AssemblyCatalog(x)));
            var priorityProvider = new CatalogExportProvider(priorityCatalog);

            var mainCatalog = new AggregateCatalog(
                AssemblySource.Instance
                    .Where(assembly => !priorityAssemblies.Contains(assembly))
                    .Select(x => new AssemblyCatalog(x)));

            var mainProvider = new CatalogExportProvider(mainCatalog);
            container = new CompositionContainer(priorityProvider, mainProvider);
            priorityProvider.SourceProvider = container;
            mainProvider.SourceProvider = container;
            var batch = new CompositionBatch();
            BindServices(batch);
            batch.AddExportedValue(mainCatalog);
            container.Compose(batch);
        }

        private void LoadPlugins()
        {
            var executionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if(string.IsNullOrWhiteSpace(executionPath))
                return;

            var pathToPlugins = Path.Combine(executionPath, PluginFolderName);
            if (!Directory.Exists(pathToPlugins)) 
                return;

            var directoryCatalog = new DirectoryCatalog($".//{PluginFolderName}//");
            AssemblySource.Instance.AddRange(
                directoryCatalog.Parts
                    .Select(part => ReflectionModelServices.GetPartType(part).Value.Assembly)
                    .Where(assembly => !AssemblySource.Instance.Contains(assembly)));
        }

        protected virtual void BindServices(CompositionBatch batch)
        {
            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(container);
            batch.AddExportedValue(this);
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] { Assembly.GetExecutingAssembly() };
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = container.GetExportedValues<object>(contract);

            if (exports.Any())
                return exports.First();

            throw new Exception($"Could not locate any instances of contract {contract}.");
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            container.SatisfyImportsOnce(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
