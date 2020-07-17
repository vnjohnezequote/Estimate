// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientService.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the ClientService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ApplicationService
{
    using Prism.Events;

    /// <summary>
    /// The clientPoco service.
    /// </summary>
    public class ClientService : PubSubEvent<string>
    {
    }
}
