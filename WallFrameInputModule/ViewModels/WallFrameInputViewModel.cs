// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WallFrameInputViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the WallFrameInputViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using ApplicationInterfaceCore;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;

namespace WallFrameInputModule.ViewModels
{
    using System.Diagnostics.CodeAnalysis;

    using ApplicationCore.BaseModule;

    using AppModels;

    using Prism.Events;
    using Prism.Regions;

    using Unity;

    using WallFrameInputModule.Views;

    /// <summary>
    /// The WallFrame input view model.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class WallFrameInputViewModel : BaseViewModel, INavigationAware
    {
        #region private Field

        /// <summary>
        /// The selected clientPoco.
        /// </summary>
        private ClientPoco _selectedClient;

        /// <summary>
        /// The job.
        /// </summary>
        private JobModel job;
        #endregion



        /// <summary>
        /// Initializes a new instance of the <see cref="WallFrameInputViewModel"/> class.
        /// </summary>
        public WallFrameInputViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrenailFloorInputViewModel"/> class.
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
        public WallFrameInputViewModel(
            IUnityContainer unityContainer,
            IRegionManager regionManager,
            IEventAggregator eventAggregator,ILayerManager layerManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator,layerManager,jobModel)
        {
        }


        #region public property

        /// <summary>
        /// Gets or sets a value indicating whether is created.
        /// </summary>
        public bool IsCreated { get; set; }

        /// <summary>
        /// Gets or sets the selected clientPoco.
        /// </summary>
        public ClientPoco SelectedClient
        {
            get => this._selectedClient;
            set => this.SetProperty(ref this._selectedClient, value);
        }

        /// <summary>
        /// Gets or sets the job.
        /// </summary>
        public JobModel Job
        {
            get => this.job;
            set => this.SetProperty(ref this.job, value);
        }
        #endregion

        #region public Method
        /// <summary>
        /// The on navigated to.
        /// </summary>
        /// <param name="navigationContext">
        /// The navigation context.
        /// </param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters["JobInfo"] is JobModel jobParam)
            {
                this.Job = jobParam;
            }

            if (navigationContext.Parameters["SelectedClient"] is ClientPoco selectedCustomer)
            {
                this.SelectedClient = selectedCustomer;
            }

            if (this.Job == null)
            {
                return;
            }

            if (this.Job.Levels == null)
            {
                return;
            }
            else
            {
                if (this.IsCreated)
                {
                    return;
                }
                this.LoadInput();
                this.Job.Levels.CollectionChanged += this.LevelsCollectionChanged;
            }
        }

        /// <summary>
        /// The is navigation target.
        /// </summary>
        /// <param name="navigationContext">
        /// The navigation context.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// The on navigated from.
        /// </summary>
        /// <param name="navigationContext">
        /// The navigation context.
        /// </param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }


        #endregion

        #region private Method

        /// <summary>
        /// The levels collection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void LevelsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.LoadInput();
        }

        /// <summary>
        /// The load input.
        /// </summary>
        private void LoadInput()
        {
            this.RegionManager.Regions["FloorInputRegion"].RemoveAll();
            foreach (var level in this.Job.Levels)
            {
                var parameters =
                    new NavigationParameters
                        {
                            { "Level", level },
                            { "SelectedClient", this.SelectedClient }
                        };
                if (level != null)
                {
                    this.RegionManager.RequestNavigate("FloorInputRegion", nameof(PrenailFloorInputView), parameters);
                }
            }

            this.IsCreated = true;
        }

        #endregion



    }
}
