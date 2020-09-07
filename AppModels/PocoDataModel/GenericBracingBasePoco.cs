// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericBracingBasePoco.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Generic Bracing type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AppModels.PocoDataModel
{
    /// <summary>
    /// The generic bracing base.
    /// </summary>
    public class GenericBracingBasePoco
    {
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name {get;set;}

        /// <summary>
        /// Gets or sets the unit price.
        /// </summary>
        public double UnitPrice { get; set; }
    }
}
