// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobUnitService.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the JobUnitService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ApplicationService
{
    using Prism.Events;

    /// <summary>
    /// The job unit service.
    /// </summary>
    public class JobUnitService : PubSubEvent<string>
    {
    }
}
