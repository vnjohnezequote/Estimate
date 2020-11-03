// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The base view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ApplicationInterfaceCore;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings;
using LiteDB;

namespace ApplicationCore.BaseModule
{
    using System.Diagnostics.CodeAnalysis;

    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The base view model.
    /// </summary>
    public abstract class BaseViewModel : BindableBase
    {
        #region Field
        private ILayerManager _layerManager;

        #endregion

        #region Constructor


        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        protected BaseViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        /// <param name="unityContainer">
        /// The unity Container.
        /// </param>
        /// <param name="regionManager">
        /// The region Manager.
        /// </param>
        /// <param name="eventAggregator">
        /// The event Aggregator.
        /// </param>
        protected BaseViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager,IJob jobModel)
        {
            using (var db = new LiteDatabase(@"filename=DesignInfo.db;upgrade=true"))
            {
                var designInfors = db.GetCollection<DesignInfor>("DesignInfors");

                var result = designInfors.Find(x => x.InfoType == "Beam").ToList();
                this.BeamDesignInfors = new ObservableCollection<DesignInfor>(result);
                result = designInfors.Find(x => x.InfoType == "Frame").ToList();
                this.FrameDesignInfors = new ObservableCollection<DesignInfor>(result);
                result = designInfors.Find(x => x.InfoType == "Bracing").ToList();
                this.BracingInfors = new ObservableCollection<DesignInfor>(result);

                // this.CreateDatabase(designInfors);
            }
            this.JobModel = jobModel;
            this.UnityContainer = unityContainer;
            this.RegionManager = regionManager;
            this.EventAggregator = eventAggregator;
            _layerManager = layerManager;
            this.RaisePropertyChanged(nameof(LayerManager));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unity container.
        /// </summary>
        public List<string> TimberGradeList { get; set; } = new List<string>(){ "MGP10", "MGP12", "F5" ,"F7","F17","P10","P12"};
        protected IUnityContainer UnityContainer { get; private set; }
        public ILayerManager LayerManager => _layerManager;
        public IRegionManager RegionManager { get; set; }
        protected IEventAggregator EventAggregator { get; private set; }
        public IJob JobModel { get; set; }
        /// <summary>
        /// Gets or sets the beam design infors.
        /// </summary>
        public ObservableCollection<DesignInfor> BeamDesignInfors { get; set; }

        /// <summary>
        /// Gets or sets the frame design infors.
        /// </summary>
        public ObservableCollection<DesignInfor> FrameDesignInfors { get; set; }

        /// <summary>
        /// Gets or sets the bracing infors.
        /// </summary>
        public ObservableCollection<DesignInfor> BracingInfors { get; set; }
        #endregion Properties

        /// <summary>
        /// The create database.
        /// </summary>
        /// <param name="db">
        /// The db.
        /// </param>
        private void CreateDatabase(LiteCollection<DesignInfor> db)
        {
            var designInfor = new DesignInfor() { Content = "Engineer", Header = "Frame follow as per", InfoType = "Frame" };
            db.Insert(designInfor);
            designInfor = new DesignInfor() { Content = "Australia Standard", Header = "Frame Design as per", InfoType = "Frame" };
            db.Insert(designInfor);
            designInfor = new DesignInfor() { Content = "Framing Schedule", Header = "Frame follow as per", InfoType = "Frame" };
            db.Insert(designInfor);
            designInfor = new DesignInfor() { Content = "Engineer and Australia Standard", Header = "Frame design as per", InfoType = "Frame" };
            db.Insert(designInfor);

            designInfor = new DesignInfor() { Content = "Engineer", Header = "Beam follow as per", InfoType = "Beam" };
            db.Insert(designInfor);
            designInfor = new DesignInfor() { Content = "Australia Standard", Header = "Beam Design as per", InfoType = "Beam" };
            db.Insert(designInfor);
            designInfor = new DesignInfor() { Content = "Framing Schedule", Header = "Beam follow as per", InfoType = "Beam" };
            db.Insert(designInfor);
            designInfor = new DesignInfor() { Content = "Engineer and Australia Standard", Header = "Frame design as per", InfoType = "Beam" };
            db.Insert(designInfor);

            designInfor = new DesignInfor() { Content = "Engineer", Header = "Bracing supply as per", InfoType = "Bracing" };
            db.Insert(designInfor);
            designInfor = new DesignInfor() { Content = "Bracing Plan", Header = "Bracing supply as per", InfoType = "Bracing" };
            db.Insert(designInfor);
            designInfor = new DesignInfor() { Content = "TBA", Header = "No Bracing supply", InfoType = "Bracing" };
            db.Insert(designInfor);

            db.EnsureIndex(x => x.Content);
            db.EnsureIndex(x => x.InfoType);

        }
    }
}
