// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegionService.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The region service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ApplicationService
{
    using Prism.Events;
    using Prism.Regions;

    /// <summary>
    /// The region service.
    /// </summary>
    public class RegionService : PubSubEvent<IRegionManager>
    {
    }
}
