// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoofInfoViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the RoofInfoViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using ApplicationInterfaceCore;

namespace JobInfoModule.ViewModels
{
    using System.Windows.Input;

    using ApplicationCore.BaseModule;

    using ApplicationService;

    using JetBrains.Annotations;

    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The general wind category view model.
    /// </summary>
    public class RoofInfoViewModel : BaseViewModelAware
    {
        #region Private Member
		private bool _isSheetRoof;
		private bool _isTileRoof;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralWindCategoryViewModel"/> class.
        /// </summary>
        public RoofInfoViewModel()
        {

        }

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
            ILayerManager layerManager)
            : base(unityContainer, regionManager, eventAggregator,layerManager)
        {
            this.RoofTypeCommand = new DelegateCommand<string>(OnSetRoofType);
        }

        #endregion
		
		#region Command
		public ICommand RoofTypeCommand {get;private set;}


        #endregion
		
		#region Public Member
		public ObservableCollection<int> TrussSpacings { get; } = new ObservableCollection<int>(){600,900};
		public ObservableCollection<int> RafterSpacings { get; } = new ObservableCollection<int>(){450,600,900};
		
		public bool IsSheetRoof{
			get => this.CheckRoofType("SHEET");
            set
            {
                if (this.Job.RoofType == "SHEET")
                {
                    this.SetProperty(ref this._isSheetRoof, true);
                }
                else
                {
                    this.SetProperty(ref this._isSheetRoof, false);
                }
            }
			}
		public bool IsTileRoof{
			get => this.CheckRoofType("TILES");
            set
            {
                if (this.Job.RoofType == "TILES")
                {
                    this.SetProperty(ref this._isTileRoof, true);
                }
                else
                {
                    this.SetProperty(ref this._isTileRoof, false);
                }
            }}
        #endregion

        #region Private Method
		private bool CheckRoofType(string roofType)	
		{
			if (this.Job == null)
            {
                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(this.Job.RoofType))
                {
                    return false;
                }
                else
                {
                    return this.Job.RoofType == roofType;
                }
            }
		}
	
		
		 private void OnSetRoofType(string roofType)
        {
            this.Job.RoofType = roofType;
			this.RaisePropertyChanged(nameof(this.IsSheetRoof));
            this.RaisePropertyChanged(nameof(this.IsTileRoof));
            
        }
        #endregion
		
		#region public Method
			
        #endregion
    }
}
