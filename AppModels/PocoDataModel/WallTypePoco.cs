// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Layer.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Layer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace AppModels.PocoDataModel
{
    /// <summary>
    /// The layer.
    /// </summary>
    public class WallTypePoco : IComparable<WallTypePoco>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the wall type is LoadBearing Wall or Non LoadBearing Wall
        /// </summary>
        public bool IsLoadBearingWall { get; set; }

        /// <summary>
        /// Gets or sets the wall type is Raked Wall or Not
        /// </summary>
        public bool IsRaked { get; set; }

        /// <summary>
        /// Gets or sets the layer code.
        /// </summary>
        public string AliasName { get; set; }

        /// <summary>
        /// The compare to.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int CompareTo(WallTypePoco other)
        {
            return this.Id.CompareTo(other.Id);
        }
    }
}
