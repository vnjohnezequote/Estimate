﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Sharp.cs" company="John Nguyen">
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
    public class Shape : BindableBase
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

    }
}
