// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bracing.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Bracing type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AppModels
{
    using Prism.Mvvm;

    /// <summary>
    /// The bracing.
    /// </summary>
    public class Bracing : BindableBase
    {
        /// <summary>
        /// The bracing info.
        /// </summary>
        private BracingBase bracingInfo;

        /// <summary>
        /// Gets or sets the bracing info.
        /// </summary>
        public BracingBase BracingInfo
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
