// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Interaction logic for App.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System.Text;
using AppAddons;
using AppDataBase.DataBase;
using ApplicationInterfaceCore;
using AppModels;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using Serilog;

namespace Estimate
{
    using System;
    using DrawingModule.Application;
    using WallFrameInputModule;
    using System.Windows;
    using CommonServiceLocator;
    using Views;
    using JobInfoModule;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;
    using Prism.Unity;

    //using Syncfusion.UI.Xaml.Grid;
    using WindowControlModule;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <summary>
        /// The register types.
        /// </summary>
        /// <param name="containerRegistry">
        /// The container registry.
        /// </param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSerilog();
            containerRegistry.RegisterSingleton<IJob, JobModel>();
            containerRegistry.RegisterSingleton<ILayerManager,LayerManager>();
            containerRegistry.RegisterSingleton<IEntitiesManager, EntitiesManager>();
        }

        /// <summary>
        /// The create shell.
        /// </summary>
        /// <returns>
        /// The <see cref="Window"/>.
        /// </returns>
        protected override Window CreateShell()
        {
            try
            {
                var shell = ServiceLocator.Current.GetInstance<MainWindowView>();
                RegionManager.SetRegionManager(shell, this.Container.Resolve<IRegionManager>());
                //RegionManager.UpdateRegions();
                return shell;
            }
            catch (Exception e)
            {
                MessageBox.Show("Fail To Create Window");
                return null;
            }
            
            //var shell = ServiceLocator.Current.GetInstance<MainWindowView>();
            //RegionManager.SetRegionManager(shell, this.Container.Resolve<IRegionManager>());
            ////RegionManager.UpdateRegions();
            //return shell;
        }

        /// <summary>
        /// The configure module catalog.
        /// </summary>
        /// <param name="moduleCatalog">
        /// The module catalog.
        /// </param>
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule(typeof(WindowControlModule));
            moduleCatalog.AddModule(typeof(JobInformationModule));
            moduleCatalog.AddModule(typeof(WallFrameModule));
            moduleCatalog.AddModule(typeof(DrawingModule.DrawingModuleControler));
            moduleCatalog.AddModule(typeof(AppAddonsModuleControl));

            //moduleCatalog.AddModule(typeof(DrawingToolsModuleControler));
            // moduleCatalog.AddModule(typeof(NewJobWizard));
            // moduleCatalog.AddModule(typeof(NewJobWizard));
            // moduleCatalog.AddModule(typeof(JobInformationModule));
            // moduleCatalog.AddModule(typeof(NewJobWizard));
            // moduleCatalog.AddModule(typeof(Prenail));
            // moduleCatalog.AddModule(typeof(Warnervale.WarnervaleModule), InitializationMode.OnDemand);
            // moduleCatalog.AddModule(typeof(StickFrame.StickFrameModule));
            // moduleCatalog.AddModule(typeof(Warnervale.WarnervaleModule));
            // moduleCatalog.AddModule(typeof(Warnervale.WarnervaleModule), InitializationMode.OnDemand)
            // .AddModule(typeof(PrenailModule.PrenailModule), InitializationMode.OnDemand)
            // .AddModule(typeof(RivoModule.RivoModule), InitializationMode.OnDemand);
        }

        private void CurrentDomainOnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            ApplicationHolder.CreateCommandList(args.LoadedAssembly);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel.Error().WriteTo.File("Estimate.log").CreateLogger();
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomainOnAssemblyLoad;
            base.OnStartup(e);
            if (e.Args.Length > 0)
            {
                MessageBox.Show(e.Args[0]);
            }
        }
        protected override void OnExit(ExitEventArgs e)
        {
            Log.CloseAndFlush();
            

            base.OnExit(e);
        }



    }
}
