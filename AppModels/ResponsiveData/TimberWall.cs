// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimberWall.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the TimberWall type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels.Interaface;
using AppModels.PocoDataModel;

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

        public override WallTypePoco WallType { get; set; }

        public override int FinalWallHeight { get; }

        /// <summary>
        /// Gets or sets the wall length.
        /// </summary>
        public double WallLength { get; set; }


        /// <summary>
        /// Gets or sets the wall layer.
        /// </summary>
        public PrenailWallLayer WallLayer
        {
            get;
            set;
        }

        #endregion


        public TimberWall(int id, IGlobalWallInfo globalWallInfo, WallTypePoco wallType) : base(id, globalWallInfo, wallType)
        {
        }
    }

}
