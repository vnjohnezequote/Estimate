// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Opening.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Opening type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels.Enums;
using AppModels.Interaface;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The opening.
    /// </summary>
    public class Opening : BindableBase
    {
        private WallBase _wallParent;
        public WallBase WallParent
        {
            get=>_wallParent;
            set=>SetProperty(ref _wallParent,value);
        }
        public IGlobalWallInfo GlobalWallInfo { get; set; }
        public Suppliers Suppliers => GlobalWallInfo.Supplier;

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the opening type.
        /// </summary>
        public OpeningType OpeningType { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the short name.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the wall thickness.
        /// </summary>
        public int WallThickness => WallParent.WallThickness;

        /// <summary>
        /// Gets or sets the lintel.
        /// </summary>
        public Beam Lintel { get; set; }

        public Opening(IGlobalWallInfo globalWallInfo)
        {
            this.GlobalWallInfo = globalWallInfo;
            Lintel = new Beam(this,BeamType.Lintel);
        }
    }
}
