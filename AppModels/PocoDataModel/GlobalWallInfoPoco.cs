using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel.WallMemberData;
using AppModels.ResponsiveData;
using devDept.Geometry;
using ProtoBuf;

namespace AppModels.PocoDataModel
{
    [ProtoContract]
    public class GlobalWallInfoPoco
    {
        public NoggingMethodType NoggingMethod { get; set; }
        public int WallHeight
        {
            get; set;
        }

        public int ExternalDoorHeight
        {
            get; set;
        }
        public int InternalDoorHeight
        {
            get; set;
        }

        public int ExternalWallSpacing
        {
            get; set;
        }

        public int InternalWallSpacing
        {
            get; set;
        }

        public int ExternalWallThickness
        {
            get; set;
        }

        public int InternalWallThickness
        {
            get; set;
        }
        public int ExternalWallTimberDepth
        {
            get;
            set;
        }
        public int InternalWallTimberDepth
        {
            get;
            set;
        }
        public string ExternalWallTimberGrade
        {
            get;
            set;
        }
        public string InternalWallTimberGrade
        {
            get;
            set;
        }

        public RoofFrameType RoofFrameType
        {
            get;
            set;
        }
        public int TrussSpacing
        {
            get;
            set;

        }
        public int RafterSpacing
        {
            get;set;
        }
        public int StepDown
        {
            get;
            set;
        }
        public int RaisedCeilingHeight
        {
            get;
            set;
        }
        public double RoofPitch
        {
            get;
            set;
        }
        public double CeilingPitch
        {
            get;
            set;
        }

        public GlobalWallDetailPoco GlobalExtWallDetailInfo { get; set; }
        public GlobalWallDetailPoco GlobalIntWallDetailInfo { get; set; }
        public WallMemberBasePoco GlobalNoggingInfo { get; set; }
        public WallMemberBasePoco GlobalDoorJambInfo { get; set; }

        public GlobalWallInfoPoco()
        {

        }
        public GlobalWallInfoPoco(GlobalWallInfo globalInfo)
        {
            NoggingMethod = globalInfo.NoggingMethod;
            WallHeight = globalInfo.WallHeight;
            ExternalDoorHeight = globalInfo.ExternalDoorHeight;
            InternalDoorHeight = globalInfo.InternalDoorHeight;
            ExternalWallSpacing = globalInfo.ExternalWallSpacing;
            InternalWallSpacing = globalInfo.InternalWallSpacing;
            ExternalWallThickness = globalInfo.ExternalWallThickness;
            InternalWallThickness = globalInfo.InternalWallThickness;
            ExternalWallTimberDepth = globalInfo.ExternalWallTimberDepth;
            InternalWallTimberDepth = globalInfo.InternalWallTimberDepth;
            ExternalWallTimberGrade = globalInfo.ExternalWallTimberGrade;
            InternalWallTimberGrade = globalInfo.InternalWallTimberGrade;
            RoofFrameType = globalInfo.RoofFrameType;
            TrussSpacing = globalInfo.TrussSpacing;
            RafterSpacing = globalInfo.RafterSpacing;
            StepDown = globalInfo.StepDown;
            RaisedCeilingHeight = globalInfo.RaisedCeilingHeight;
            RoofPitch = globalInfo.RoofPitch;
            CeilingPitch = globalInfo.CeilingPitch;
            GlobalExtWallDetailInfo = new GlobalWallDetailPoco(globalInfo.GlobalExtWallDetailInfo);
            GlobalIntWallDetailInfo = new GlobalWallDetailPoco(globalInfo.GlobalIntWallDetailInfo);
            GlobalNoggingInfo = new WallMemberBasePoco(globalInfo.GlobalNoggingInfo);
            GlobalDoorJambInfo = new WallMemberBasePoco(globalInfo.GlobalDoorJambInfo);
        }

        public GlobalWallInfoPoco(LevelGlobalWallInfo levelInfo)
        {
            NoggingMethod = levelInfo.NoggingMethod;
            WallHeight = levelInfo.WallHeight;
            ExternalDoorHeight = levelInfo.ExternalDoorHeight;
            InternalDoorHeight = levelInfo.InternalDoorHeight;
            ExternalWallSpacing = levelInfo.ExternalWallSpacing;
            InternalWallSpacing = levelInfo.InternalWallSpacing;
            ExternalWallThickness = levelInfo.ExternalWallThickness;
            InternalWallThickness = levelInfo.InternalWallThickness;
            GlobalExtWallDetailInfo = new GlobalWallDetailPoco(levelInfo.GlobalExtWallDetailInfo);
            GlobalIntWallDetailInfo = new GlobalWallDetailPoco(levelInfo.GlobalIntWallDetailInfo);
            GlobalDoorJambInfo = new WallMemberBasePoco(levelInfo.GlobalDoorJambInfo);
        }

    }
}
