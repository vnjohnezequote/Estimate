// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Client.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Client type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace AppModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using LiteDB;

    // using MaterialDesignThemes.Wpf;

    /// <summary>
    /// The client.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Gets or sets the id of client.
        /// </summary>
        [BsonId]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the client's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the builders list.
        /// </summary>
        public List<string> Builders { get; set; }

        /// <summary>
        /// Gets or sets the builders list.
        /// </summary>
        public List<WallType> WallTypes { get; set; }

        /// <summary>
        /// Gets or sets the timber studs.
        /// </summary>
        public Dictionary<string, List<TimberBase>> Studs { get; set; }
		
		/// <summary>
        /// Gets or sets the Ribbon Plates .
        /// </summary>
        public Dictionary<string, List<TimberBase>> RibbonPlates { get; set; }
		
		/// <summary>
        /// Gets or sets the Top Plates.
        /// </summary>
        public Dictionary<string, List<TimberBase>> TopPlates { get; set; }

        /// <summary>
        /// Gets or sets the Top Plates.
        /// </summary>
        public Dictionary<string, List<TimberBase>> BottomPlates { get; set; }

        /// <summary>
        /// Gets or sets the beams.
        /// </summary>
        public Dictionary<string, List<TimberBase>> Beams { get; set; }

        /// <summary>
        /// Gets or sets the timber bracing bases.
        /// </summary>
        public List<BracingBase> TimberBracingBases { get; set; }

        /// <summary>
        /// Gets or sets the generic bracing bases.
        /// </summary>
        public List<GenericBracingBase> GenericBracingBases { get; set; }

        /// <summary>
        /// Gets or sets the client icon.
        /// </summary>
        public string ClientIcon { get; set; }
    }
}
