// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Timber.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Timber type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace AppModels
{
    using Prism.Mvvm;

    /// <summary>
    /// The timber.
    /// </summary>
    public class WallMemberBase : BindableBase
    {
        #region Private Member

        /// <summary>
        /// The timber in fo.
        /// </summary>
        private TimberBase timberInfo;
		
		

        #endregion
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WallMemberBase"/> class.
        /// </summary>
        public WallMemberBase()
        {
            this.IsDefault = true;
        }

        #region Public Region

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is default.
        /// </summary>
        public bool IsDefault {get;set;}
  
        /// <summary>
        /// Gets or sets the timber info.
        /// </summary>
        public TimberBase TimberInfo
        {
            get => this.timberInfo;
            set => this.SetProperty(ref this.timberInfo, value);
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public double Length { get; set; }

        #endregion



    }
}
