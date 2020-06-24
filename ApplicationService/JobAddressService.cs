// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobAddressService.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the JobAddressService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ApplicationService
{
    using Prism.Events;

    /// <summary>
    /// The job address service.
    /// </summary>
    public class JobAddressService : PubSubEvent<string>
    {
    }
}
