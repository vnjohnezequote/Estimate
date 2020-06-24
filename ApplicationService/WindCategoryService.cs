// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindCategoryService.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the WindCategoryService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ApplicationService
{
    using Prism.Events;

    /// <summary>
    /// The wind category service.
    /// </summary>
    public class WindCategoryService : PubSubEvent<string>
    {
    }
}
