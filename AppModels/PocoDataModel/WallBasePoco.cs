using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.AppData;
using AppModels.ResponsiveData;
using ProtoBuf;

namespace AppModels.PocoDataModel
{
    [ProtoContract]
    [ProtoInclude(1, typeof(TimberWallPoco))]
    [ProtoInclude(2, typeof(WallLayerPoco))]
    public class WallBasePoco
    {
        public LayerItemPoco WallColorLayer { get; set; }
        public int Id { get; set; }
        public IntegerDimension PitchingHeight { get; set; }
        public int WallHeight { get; set; }
        public string Treatment { get; set; }
        public bool IsRakedWall { get; set; }
        public int RunLength { get; set; }
        public DoubleDimension RoofPitch { get; set; }
        public int IsHPitching { get; set; }
        public int HPitching { get; set; }
        public bool IsStepdown { get; set; }
        public IntegerDimension StepDown { get; set; }
        public bool IsCeilingRaised { get; set; }
        public int CeilingRaised { get; set; }
        public int IsSubStud { get; set; }
        public bool IsParallelWithTruss { get; set; }
        public bool IsHightWall { get; set; }
        public int SudStud { get; set; }
        public int IsNonLbWall { get; set; }
        public WallMemberBasePoco RibbonPlate { get; set; }
        public WallMemberBasePoco TopPlate { get; set; }
        public WallMemberBasePoco BottomPlate { get; set; }
        public WallStudPoco Stud { get; set; }
    }
}
