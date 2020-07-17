// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bracing.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Bracing type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The bracing.
    /// </summary>
    public class GenericBracing : BindableBase
    {
        /// <summary>
        /// The bracing info.
        /// </summary>
        private GenericBracingBase bracingInfo;

        /// <summary>
        /// Gets or sets the bracing info.
        /// </summary>
        public GenericBracingBase BracingInfo
        {
            get => this.bracingInfo;
            set => this.SetProperty(ref this.bracingInfo, value);
        }

        /// <summary>
        /// Gets or sets the quantities.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        public double Price { get; set; }
    }
}
