// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StudMember.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the StudMember type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AppModels
{
    using System;

    using Prism.Mvvm;

    /// <summary>
    /// The stud member.
    /// </summary>
    public class WallStud : BindableBase
    {
        #region Private Member

        /// <summary>
        /// The timber info.
        /// </summary>
        private TimberBase timberInfo;
        
        /// <summary>
        /// The height.
        /// </summary>
        private int height;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="WallStud"/> class.
        /// </summary>
        public WallStud()
        {
			this.IsDefault = true;
        }

        #region Public Member

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the timber info.
        /// </summary>
        public TimberBase TimberInfo
        {
            get => this.timberInfo;
            set
            {
                if (this.timberInfo == value)
                {
                    return;
                }

                this.timberInfo = new TimberBase(value);
                this.RaisePropertyChanged();
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
		
		public bool IsDefault {get;set;} = true;

        #endregion


    }
}