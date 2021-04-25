// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Sharp.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Bracing type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Prism.Mvvm;
using System;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The bracing.
    /// </summary>
    public class Shape : BindableBase
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
        /// Initializes a new instance of the <see cref="Shape"/> class.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        public Shape(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
        
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

    }
}
