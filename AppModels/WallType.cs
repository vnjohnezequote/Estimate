// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Layer.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Layer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AppModels
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// The layer.
    /// </summary>
    public class WallType : IComparable<WallType>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the wall type is LoadBearing Wall or Non LoadBearing Wall
        /// </summary>
        public bool IsLBW { get; set; }

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
        public int CompareTo(WallType other)
        {
            return this.Id.CompareTo(other.Id);
        }
    }
}
