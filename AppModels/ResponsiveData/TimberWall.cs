// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimberWall.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the TimberWall type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The timber wall.
    /// </summary>
    public class TimberWall : WallBase
    {
        #region Constructor
        #endregion

        #region Property
        

        /// <summary>
        /// Gets or sets the wall length.
        /// </summary>
        public double WallLength { get; set; }


        /// <summary>
        /// Gets or sets the wall layer.
        /// </summary>
        public WallLayer WallLayer
        {
            get;
            set;
        }

        #endregion

    }

}
