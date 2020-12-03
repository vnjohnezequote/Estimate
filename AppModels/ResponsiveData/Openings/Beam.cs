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
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.Framings.FloorAndRafter;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Framings.Beams;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
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
        
        #endregion

        #region Constructor

        public Beam(FramingTypes beamType,LevelWall level): base(beamType,level)
        {
        }

        public Beam(FramingSheet framingSheet) : base(framingSheet)
        {

        }
        public Beam(BeamPoco beamPoco, LevelWall level, List<TimberBase> timberList,List<EngineerMemberInfo> engineerMemberInfos) : base(beamPoco, level, timberList,engineerMemberInfos)
        {
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
