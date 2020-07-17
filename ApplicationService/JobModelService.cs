// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobModelService.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the JobModelService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels.ResponsiveData;

namespace ApplicationService
{
    using AppModels;

    using Prism.Events;

    /// <summary>
    /// The job model service.
    /// </summary>
    public class JobModelService : PubSubEvent<JobModel>
    {
    }
}
