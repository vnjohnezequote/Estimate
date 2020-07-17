// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BracingBase.cs" company="John Nguyen">
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
    public class BracingBase : BindableBase
    {
        /// <summary>
        /// The width.
        /// </summary>
        private int _width;

        /// <summary>
        /// The height.
        /// </summary>
        private int _height;

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Width
        {
            get => this._width;
            set
            {
                this.SetProperty(ref this._width, value);
                this.RaisePropertyChanged(nameof(this.Size));
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public int Height
        {
            get => this._height;
            set => this.SetProperty(ref this._height, value);
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
