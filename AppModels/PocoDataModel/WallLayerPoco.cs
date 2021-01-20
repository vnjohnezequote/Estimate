using AppModels.AppData;
using AppModels.PocoDataModel.WallMemberData;
using AppModels.ResponsiveData;

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
        //public double WallLength { get; set; }
        public bool IsStepdown { get; set; }
        public bool IsRaisedCeiling { get; set; }
        public int TypeId { get; set; }
        public bool IsDesigned { get; set; }
        public int RunLength { get; set; }
        public double CeilingPitch { get; set; }
        public int StepDown { get; set; }
        public int RaisedCeiling { get; set; }
        public string LevelName { get; set; }
        public bool IsExportToUpper { get; set; }
        public WallMemberBasePoco RibbonPlate { get; set; }
        public WallMemberBasePoco TopPlate { get; set; }
        public WallMemberBasePoco BottomPlate { get; set; }
        public WallMemberBasePoco Stud { get; set; }
        public WallMemberBasePoco Nogging { get; set; }
        public int BeamPockets { get; set; }
        public int Corners { get; set; }
        public int TCorners { get; set; }
        public int InWallSupports { get; set; }
        public int BathCheckout { get; set; }
        public int NumberOfSameWall { get; set; }
        public double WetAreaLength { get; set; }
        #endregion

        public WallLayerPoco()
        {

        }
        public WallLayerPoco(WallBase wall)
        {
            Id = wall.Id;
            TypeId = wall.TypeId;
            WallType = wall.WallType;
            LevelName = wall.LevelName;
            BeamPockets = wall.BeamPockets;
            Corners = wall.Corners;
            TCorners = wall.TCorners;
            InWallSupports = wall.InWallSupports;
            NumberOfSameWall = wall.NumberOfSameWall;
            BathCheckout = wall.BathCheckout;
            WallColorLayer = wall.WallColorLayer;
            WallThickness = wall.WallThickness;
            WallSpacing = wall.WallSpacing;
            WallPitchingHeight = wall.WallPitchingHeight;
            ForcedWallUnderRakedArea = wall.ForcedWallUnderRakedArea;
            WetAreaLength = wall.WetAreaLength;
            //WallLength = wall.WallLength;
            IsStepdown = wall.IsStepDown;
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
            IsExportToUpper = wall.IsExportToUpper;

        }
    }
}
