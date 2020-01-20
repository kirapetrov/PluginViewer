//using System;
//using System.Collections.Generic;
//using System.ComponentModel.Composition;
//using System.ComponentModel.Composition.Hosting;
//using System.ComponentModel.Composition.Primitives;
//using System.Linq;
//using System.Reflection;
//using System.Windows;
//using Caliburn.Micro;
//using PluginViewer.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.ReflectionModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using PluginViewer.ViewModels;

namespace PluginViewer
{
    public class PluginViewerBootstrapper : BootstrapperBase
    {
        private List<Assembly> priorityAssemblies;
        private CompositionContainer container;

        public PluginViewerBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            // Add all assemblies to AssemblySource (using a temporary DirectoryCatalog).
            var directoryCatalog = new DirectoryCatalog(@"./");
            AssemblySource.Instance.AddRange(
                directoryCatalog.Parts
                    .Select(part => ReflectionModelServices.GetPartType(part).Value.Assembly)
                    .Where(assembly => !AssemblySource.Instance.Contains(assembly)));

            // Prioritise the executable assembly. This allows the client project to override exports, including IShell.
            // The client project can override SelectAssemblies to choose which assemblies are prioritised.
            priorityAssemblies = SelectAssemblies().ToList();
            var priorityCatalog = new AggregateCatalog(priorityAssemblies.Select(x => new AssemblyCatalog(x)));
            var priorityProvider = new CatalogExportProvider(priorityCatalog);

            // Now get all other assemblies (excluding the priority assemblies).
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

            //var executingAssembly = Assembly.GetExecutingAssembly();
            //var catalog = new AggregateCatalog();
            //catalog.Catalogs.Add(new AssemblyCatalog(executingAssembly));
            //var folder = System.IO.Path.GetDirectoryName(executingAssembly.Location);
            //catalog.Catalogs.Add(new DirectoryCatalog(folder));
            //container = new CompositionContainer(catalog);
            //container.ComposeParts(this);

            //var batch = new CompositionBatch();
            //batch.AddExportedValue<IWindowManager>(new WindowManager());
            //batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            //batch.AddExportedValue(container);
            //container.Compose(batch);
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
