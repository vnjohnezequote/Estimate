// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Opening.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Opening type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AppModels
{
    /// <summary>
    /// The opening.
    /// </summary>
    public class Opening
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the opening type code.
        /// </summary>
        public string OpeningTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the opening type.
        /// </summary>
        public string OpeningType { get; set; }

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
        public int WallThickness { get; set; }

        /// <summary>
        /// Gets or sets the lintel.
        /// </summary>
        public Beam Lintel { get; set; }

        /// <summary>
        /// Gets or sets the wall.
        /// </summary>
        public TimberWall Wall { get; set; }
    }
}
