// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bracing.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Bracing type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels.PocoDataModel;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The bracing.
    /// </summary>
    public class GenericBracing : BindableBase
    {
        private int _quantity;
        /// <summary>
        /// The bracing info.
        /// </summary>
        private GenericBracingBasePoco bracingInfo;

        /// <summary>
        /// Gets or sets the bracing info.
        /// </summary>
        public GenericBracingBasePoco BracingInfo
        {
            get => this.bracingInfo;
            set => this.SetProperty(ref this.bracingInfo, value);
        }

        /// <summary>
        /// Gets or sets the quantities.
        /// </summary>
        public int Quantity { get=>_quantity; set=>SetProperty(ref _quantity,value); }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        public double Price { get; set; }

        public GenericBracing()
        {
            
        }
    }
}
