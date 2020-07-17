// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StudMember.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the StudMember type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The stud member.
    /// </summary>
    public class WallStud : BindableBase
    {
        #region Private Member

        /// <summary>
        /// The timber info.
        /// </summary>
        private TimberBase _timberInfo;
        
        /// <summary>
        /// The height.
        /// </summary>
        private int _height;

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
            get => this._timberInfo;
            set
            {
                if (this._timberInfo == value)
                {
                    return;
                }

                this._timberInfo = new TimberBase(value);
                this.RaisePropertyChanged();
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
		
		public bool IsDefault {get;set;}

        #endregion


    }
}