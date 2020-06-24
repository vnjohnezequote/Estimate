﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoubleDimension.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the DoubleDimension type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AppModels
{
    using Prism.Mvvm;

    /// <summary>
    /// The double dimension.
    /// </summary>
    public class DoubleDimension: BindableBase
    {

        #region Private field

        /// <summary>
        /// The is default value.
        /// </summary>
        private bool isDefaultValue;

        /// <summary>
        /// The size.
        /// </summary>
        private double size;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleDimension"/> class. 
        /// </summary>
        public DoubleDimension()
        {
            this.IsDefaultValue = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleDimension"/> class. 
        /// </summary>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <param name="isDefault">
        /// The is default value.
        /// </param>
        public DoubleDimension(double? size, bool isDefault = true)
        {
            if (size != null)
            {
                this.Size = (double)size;
            }

            this.IsDefaultValue = isDefault;
        }


        #endregion
        #region public Property

        /// <summary>
        /// Gets or sets a value indicating whether is default value.
        /// </summary>
        public bool IsDefaultValue
        {
            get => this.isDefaultValue;
            set => this.SetProperty(ref this.isDefaultValue, value);
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public double Size
        {
            get => this.size;
            set => this.SetProperty(ref this.size, value);
        }
        #endregion

    }
}
