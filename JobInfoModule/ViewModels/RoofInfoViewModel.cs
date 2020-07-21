// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoofInfoViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the RoofInfoViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ApplicationInterfaceCore;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;

namespace JobInfoModule.ViewModels
{
    using ApplicationCore.BaseModule;
    using JetBrains.Annotations;
    using Prism.Events;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The general wind category view model.
    /// </summary>
    public class RoofInfoViewModel : BaseJobInForViewModel
    {
        #region Field
        private ObservableCollection<string> _roofMaterials;
        #endregion
        #region Property
        public ObservableCollection<int> TrussSpacings { get; } = new ObservableCollection<int>() { 600, 900 };
        public ObservableCollection<int> RafterSpacings { get; } = new ObservableCollection<int>() { 450, 600, 900 };

        public ObservableCollection<string> RoofMaterials
        {
            get=>_roofMaterials;
            set=>SetProperty(ref _roofMaterials,value);
        }

        public bool IsTrussSpacingEnable =>
            this.JobInfo.DefaultInfo.RoofFrameType == RoofFrameType.Truss ||
            this.JobInfo.DefaultInfo.RoofFrameType == RoofFrameType.TrussAndRafter;

        public bool IsRaterSpacingEnable =>
        this.JobInfo.DefaultInfo.RoofFrameType == RoofFrameType.Rafter ||
        this.JobInfo.DefaultInfo.RoofFrameType == RoofFrameType.TrussAndRafter;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralWindCategoryViewModel"/> class.
        /// </summary>
        /// <param name="unityContainer">
        /// The unity container.
        /// </param>
        /// <param name="regionManager">
        /// The region manager.
        /// </param>
        /// <param name="eventAggregator">
        /// The event aggregator.
        /// </param>
        public RoofInfoViewModel(
            [NotNull] IUnityContainer unityContainer,
            [NotNull] IRegionManager regionManager,
            [NotNull] IEventAggregator eventAggregator,
            ILayerManager layerManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator,layerManager,jobModel)
        {
            this.JobInfo.DefaultInfo.PropertyChanged += JobDefault_PropertyChanged;
        }

        private void JobDefault_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "RoofFrameType") return;
            RaisePropertyChanged(nameof(IsTrussSpacingEnable));
            RaisePropertyChanged(nameof(IsRaterSpacingEnable));
        }

        #endregion

        #region Command


        #endregion

        #region Private Method

        protected override void SelectedClientReceive(ClientPoco selectClient)
        {
            base.SelectedClientReceive(selectClient);
            RoofMaterialsReseive(selectClient.RoofMaterials);
        }

        private void RoofMaterialsReseive(List<string> roofMaterials)
        {
            this.RoofMaterials = new ObservableCollection<string>(roofMaterials);
        }
        protected override void Initilazied()
        {
            base.Initilazied();
            this.RoofMaterialsReseive(this.SelectedClient.RoofMaterials);
            JobInfo.RoofMaterial = RoofMaterials.FirstOrDefault();
        }
        #endregion

        #region public Method

        #endregion
    }
}
