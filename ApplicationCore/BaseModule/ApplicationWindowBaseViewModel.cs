// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationWindowBaseViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The new job wizard view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using ApplicationInterfaceCore;
using AppModels.Interaface;
using AppModels.ResponsiveData;

namespace ApplicationCore.BaseModule
{
    using ApplicationService;

    using AppModels;

    using Prism.Events;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The new job wizard view model.
    /// </summary>
    public class ApplicationWindowBaseViewModel : BaseViewModel
    {
        #region Private member

        /// <summary>
        /// The job.
        /// </summary>
        private JobModel job;


        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationWindowBaseViewModel"/> class.
        /// </summary>
        public ApplicationWindowBaseViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationWindowBaseViewModel"/> class. 
        /// </summary>
        /// <param name="unityContainer">
        /// The unity container.
        /// </param>
        /// <param name="regionManager">
        /// The region manager.
        /// </param>
        /// <param name="eventAggregator">
        /// The event Aggregator.
        /// </param>
        public ApplicationWindowBaseViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator,ILayerManager layerManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator,layerManager,jobModel)
        {
            this.RegionManager = this.RegionManager.CreateRegionManager();
            //this.EventAggregator.GetEvent<JobModelService>().Subscribe(this.JobReceive);
        }

        #endregion

        #region Command


        #endregion

        #region Public Property

        /// <summary>
        /// Gets or sets the job.
        /// </summary>
        //public JobModel Job
        //{
        //    get => this.job;
        //    set => this.SetProperty(ref this.job, value);
        //}

        #endregion

        #region Private Function

        /// <summary>
        /// The job recesive.
        /// </summary>
        /// <param name="job">
        /// The job.
        /// </param>
        //private void JobReceive(JobModel job)
        //{
        //    if (job != null)
        //    {
        //        this.Job = job;
        //    }
        //}

        #endregion
    }
}
