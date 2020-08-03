// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Beam.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The beam.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppModels.Enums;
using AppModels.Interaface;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Openings
{
    
/// <summary>
    /// The beam.
    /// </summary>
    public class Beam: BindableBase, IBeam
{
        #region Field
        private int _numberOfSupport;
        private BeamType _beamType;
        private SupportType _pointSupportType;
        //private IWallMemberInfo _globalSupportInfo;
        #endregion


        #region Property

        public IWallMemberInfo GLobalSupportInfo { get; private set; }

        public ObservableCollection<LoadPointSupport> LoadPointSupports { get; set; } 
            = new ObservableCollection<LoadPointSupport>();

        public SupportType PointSupportType
        {
            get=>_pointSupportType; 
            set=>SetProperty(ref _pointSupportType,value);
        }
        public int NumberOfSupport
        {
            get => _numberOfSupport;
            set => SetProperty(ref _numberOfSupport, value);
        }

        public BeamType Type
        {
            get=>_beamType;
            private set=>SetProperty(ref _beamType,value);
        }

        public Suppliers Suplier { get; }
        public int NoItem { get; set; }
        public int Depth { get; set; }
        public int Thickness { get; set; }
        public string TimberGrade { get; set; }
        public string Size { get;  }
        public string SizeGrade { get;set; }

        
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

        public Beam(BeamType beamType, IWallMemberInfo gLobalSupportInfo)
        {
            Type = beamType;
            GLobalSupportInfo = gLobalSupportInfo;
        }

        public void CreateSupport()
        {

        }

        #endregion


        
}
}
