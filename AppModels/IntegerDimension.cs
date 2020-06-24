// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Dimention.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Dimention type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AppModels
{

    using Prism.Mvvm;

    /// <summary>
    /// The integer dimension.
    /// </summary>
    public class IntegerDimension : BindableBase
    {
        #region Private field

        /// <summary>
        /// The is default value.
        /// </summary>
        private bool isDefaultValue;

        /// <summary>
        /// The size.
        /// </summary>
        private int size;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerDimension"/> class.
        /// </summary>
        public IntegerDimension()
        {
            this.IsDefaultValue = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerDimension"/> class.
        /// </summary>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <param name="isDefault">
        /// The is default value.
        /// </param>
        public IntegerDimension(int? size, bool isDefault = true)
        {
            if (size != null)
            {
                this.Size = (int)size;
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
        public int Size
        {
            get => this.size;
            set => this.SetProperty(ref this.size, value);
        }
        #endregion
    }
}
