// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobService.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the JobService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ApplicationService
{
    using Prism.Events;

    /// <summary>
    /// The job service.
    /// </summary>
    public class JobNumberService : PubSubEvent<string>
    {
    }
}
