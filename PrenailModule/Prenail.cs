// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Prenail.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Prenail type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PrenailModule
{
    using System.Diagnostics.CodeAnalysis;

    using ApplicationCore.BaseModule;


    using Prism.Events;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The Prenail.
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "Reviewed. Suppression is OK here.")]
    public class Prenail : BaseModule
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Prenail"/> class.
        /// </summary>
        /// <param name="unityContainer">
        /// The unity container.
        /// </param>
        /// <param name="regionManager">
        /// The region manager.
        /// </param>
        public Prenail(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(unityContainer, regionManager, eventAggregator)
        {
            //regionManager.RegisterViewWithRegion("JobInforRegion", typeof(JobInforView));
        }


        #endregion

    }
}
