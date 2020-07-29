using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.AppData;
using AppModels.Interaface;
using AppModels.PocoDataModel.WallMemberData;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.WallMemberData;

namespace AppModels.PocoDataModel
{
    public class WallLayerPoco
    {
        #region Property
        public int Id { get; set; }
        public virtual WallTypePoco WallType { get; set; }
        public LayerItem WallColorLayer { get; set; }
        public int WallThickness { get; set; }
        public int WallSpacing { get; set; }
        public int WallPitchingHeight { get; set; }
        public bool ForcedWallUnderRakedArea { get; set; }
        public double WallLength { get; set; }
        public bool IsStepdown { get; set; }
        public bool IsRaisedCeiling { get; set; }
        public int TypeId { get; set; }
        public bool IsDesigned { get; set; }
        public int RunLength { get; set; }
        public double CeilingPitch { get; set; }
        public int StepDown { get; set; }
        public int RaisedCeiling { get; set; }
        public WallMemberBasePoco RibbonPlate { get; set; }
        public WallMemberBasePoco TopPlate { get; set; }
        public WallMemberBasePoco BottomPlate { get; set; }
        public WallMemberBasePoco Stud { get; set; }
        public WallMemberBasePoco Nogging { get; set; }

        #endregion

        public WallLayerPoco()
        {

        }
        public WallLayerPoco(WallBase wall)
        {
            Id = wall.Id;
            TypeId = wall.TypeId;
            WallType = wall.WallType;
            WallColorLayer = wall.WallColorLayer;
            WallThickness = wall.WallThickness;
            WallSpacing = wall.WallSpacing;
            WallPitchingHeight = wall.WallPitchingHeight;
            ForcedWallUnderRakedArea = wall.ForcedWallUnderRakedArea;
            WallLength = wall.WallLength;
            IsStepdown = wall.IsStepdown;
            IsRaisedCeiling = wall.IsRaisedCeiling;
            IsDesigned = wall.IsDesigned;
            RunLength = wall.RunLength;
            CeilingPitch = wall.CeilingPitch;
            StepDown = wall.StepDown;
            RaisedCeiling = wall.RaisedCeiling;
            RibbonPlate = new WallMemberBasePoco(wall.RibbonPlate);
            TopPlate = new WallMemberBasePoco(wall.TopPlate);
            Stud = new WallMemberBasePoco(wall.Stud);
            BottomPlate = new WallMemberBasePoco(wall.BottomPlate);
            Nogging = new WallMemberBasePoco(wall.Nogging);
        }
    }
}
