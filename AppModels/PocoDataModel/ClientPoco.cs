// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientPoco.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the ClientPoco type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System.Collections.Generic;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Openings;
using LiteDB;

namespace AppModels.PocoDataModel
{
    // using MaterialDesignThemes.Wpf;

    /// <summary>
    /// The clientPoco.
    /// </summary>
    public class ClientPoco
    {
        /// <summary>
        /// Gets or sets the id of clientPoco.
        /// </summary>
        [BsonId]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the clientPoco's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the builders list.
        /// </summary>
        public List<string> Builders { get; set; }
        /// <summary>
        /// Gets or sets the treatments list.
        /// </summary>
        public List<string> Treatments { get; set; }
        /// <summary>
        /// Gets or sets the windrates list.
        /// </summary>
        public List<string> WinRates { get; set; }

        public List<string> RoofMaterials { get; set; }
        /// <summary>
        /// Gets or sets the builders list.
        /// </summary>
        public List<WallTypePoco> WallTypes { get; set; }

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
        public List<Hanger> Hangers { get; set; }

        //public List<TimberBase> FloorRafterMaterialList { get; set; }

        /// <summary>
        /// Gets or sets the timber bracing bases.
        /// </summary>
        public List<BracingBase> TimberBracingBases { get; set; }

        /// <summary>
        /// Gets or sets the generic bracing bases.
        /// </summary>
        public List<GenericBracingBasePoco> GenericBracingBases { get; set; }

        public List<TimberBase> TimberMaterialList { get; set; }
        /// <summary>
        /// Gets or sets the clientPoco icon.
        /// </summary>
        public string ClientIcon { get; set; }
        public List<Customer> Customers { get; set; }
    }
}
