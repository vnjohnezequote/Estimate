// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Beam.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The beam.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Security.RightsManagement;
using AppModels.Enums;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    
/// <summary>
    /// The beam.
    /// </summary>
    public class Beam: BindableBase
{
        #region Field
            private Opening _openingInfo;
            private BeamType _beamType;
            

        #endregion


        #region Property

        public BeamType Type
        {
            get=>_beamType;
            private set=>SetProperty(ref _beamType,value);
        }
        private Suppliers suplier { get; }
        public int NoItem { get; set; }
        public int Depth { get; set; }
        public int Thickness { get; set; }
        public string TimberGrade { get; set; }
        public string Size { get;  }
        public string SizeGrade { get;set; }

        public Opening OpeningInfo { get=>_openingInfo; set=>SetProperty(ref _openingInfo,value); }
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

        #endregion

        #region Constructor

        public Beam(Opening openingInfo, BeamType beamType)
        {
            OpeningInfo = openingInfo;
            Type = beamType;
        }

        #endregion


        
}
}
