using System;
using AppModels.Enums;
using AppModels.ResponsiveData;
using ProtoBuf;

namespace AppModels.PocoDataModel
{
    [ProtoContract]
    public class JobInfoPoco
    {
        #region Property
        [ProtoMember(1)]
        public JobWallDefaultInfoPoco JobWallDefault { get; set; }
        [ProtoMember(2)]
        public string JobLocation { get; set; }
        [ProtoMember(3)]
        public string ClientName { get; set; }
        [ProtoMember(4)]
        public string JobNumber { get; set; }
        [ProtoMember(5)]
        public string JobAddress { get; set; }
        [ProtoMember(6)]
        public string SubAddress { get; set; }
        [ProtoMember(7)]
        public string FullAddress { get; set; }
        [ProtoMember(8)]
        public string BuilderName { get; set; }
        [ProtoMember(9)]
        public string UnitNumber { get; set; }
        [ProtoMember(10)]
        public string WindRate { get; set; }
        [ProtoMember(11)]
        public string Treatment { get; set; }
        [ProtoMember(12)]
        public string RoofType { get; set; }
        [ProtoMember(13)]
        public RoofFrameType RoofFrameType { get; set; }
        [ProtoMember(14)]
        public int TrussSpacing { get; set; }
        [ProtoMember(15)]
        public int RafterSpacing { get; set; }
        [ProtoMember(16)]
        public int TotalLinearMeter { get; set; }
        [ProtoMember(17)]
        public DateTime CompleteDate { get; set; }
        [ProtoMember(18)]
        public DateTime PlanIsueDate { get; set; }
        [ProtoMember(19)]
        public bool IsElectricalPlan { get; set; }
        [ProtoMember(20)]
        public bool IsEngineer { get; set; }
        [ProtoMember(21)]
        public bool IsBracingPlan { get; set; }
        [ProtoMember(22)]
        public DesignInfor FrameDesignInfor { get; set; }
        [ProtoMember(23)]
        public DesignInfor BeamDesignInfor { get; set; }
        [ProtoMember(24)]
        public DesignInfor BracingDesignInfor { get; set; }
        #endregion

        #region Constructor
        #endregion
    }
}