// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BracingBase.cs" company="John Nguyen">
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
    public class BracingBase : BindableBase
    {
        /// <summary>
        /// The width.
        /// </summary>
        private int width;

        /// <summary>
        /// The height.
        /// </summary>
        private int height;

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Width
        {
            get => this.width;
            set
            {
                this.SetProperty(ref this.width, value);
                this.RaisePropertyChanged(nameof(this.Size));
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public int Height
        {
            get => this.height;
            set => this.SetProperty(ref this.height, value);
        }

        /// <summary>
        /// The size.
        /// </summary>
        public string Size => this.Height + "x" + this.Width;
        
        /// <summary>
        /// Gets or sets the thickness.
        /// </summary>
        public int Thickness {get;set;}

        /// <summary>
        /// Gets or sets the unit price.
        /// </summary>
        public double UnitPrice {get;set;}

        /// <summary>
        /// Gets or sets the full size.
        /// </summary>
        public string FullSize {get;set;}
    }
}
