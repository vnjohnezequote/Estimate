// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Beam.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The beam.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using AppModels.Enums;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Framings.Beams;
using devDept.Geometry;

namespace AppModels.ResponsiveData.Openings
{
    
/// <summary>
    /// The beam.
    /// </summary>
    public class Beam: BeamBase
{
        #region Field

        
        #endregion


        #region Property
        public override double QuoteLength
        {
            get
            {
                var fullLength = FullLength / Math.Cos(Utility.DegToRad(Pitch));
                var quoteLengthinMm = ((int)fullLength).RoundUpTo300();
                var quoteLength = (double)quoteLengthinMm / 1000 + ExtraLength;
                return quoteLength;
            }
        }

        

        protected override List<FramingTypes> FramingTypeAccepted { get; } = new List<FramingTypes>() {FramingTypes.TrussBeam,FramingTypes.LintelBeam,FramingTypes.FloorBeam,FramingTypes.PolePlate,FramingTypes.RafterBeam,FramingTypes.HipRafter,FramingTypes.SteelBeam,FramingTypes.RidgeBeam,FramingTypes.DeckBeam,FramingTypes.DeckRoofBeam};

        #endregion

        #region Constructor

        public Beam(FramingTypes beamType,LevelWall level): base(beamType,level)
        {
        }

        public Beam(FramingSheet framingSheet) : base(framingSheet)
        {

        }

        public Beam(BeamPoco beamPoco, List<TimberBase> timberList, List<EngineerMemberInfo> engineerMemberInfos) :
            base(beamPoco, timberList, engineerMemberInfos)
        {

        }

        public Beam(BeamPoco beamPoco, FramingSheet framingSheet, List<TimberBase> timberList,
            List<EngineerMemberInfo> engineerMemberInfos) : base(beamPoco, framingSheet, timberList,
            engineerMemberInfos)
        {

        }
        public Beam(BeamPoco beamPoco, LevelWall level, List<TimberBase> timberList, List<EngineerMemberInfo> engineerMemberInfos) : base(beamPoco, timberList, engineerMemberInfos)
        {
            this.Level = level;
            InitGlobalInfor(level.LevelInfo);
        }

        public Beam(Beam another) : base(another)
        {

        }

        #endregion

        #region protected Method


       


        public override object Clone()
        {
            return new Beam(this);
        }


        #endregion

    }
}
