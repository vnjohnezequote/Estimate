﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Beam.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The beam.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using AppModels;
namespace AppModels
{
    
/// <summary>
    /// The beam.
    /// </summary>
    public class Beam : WallMemberBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Beam"/> class.
        /// </summary>
        public Beam()
        {
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the beam grade.
        /// </summary>
        public string BeamGrade { get; set; }

        /// <summary>
        /// Gets or sets the beam name.
        /// </summary>
        public string BeamName { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        public int Quantity { get; set; }
}
}
