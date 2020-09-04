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
        public string JobLocation { get; set; }
        public string ClientName { get; set; }
        public string Customer { get; set; }
        public string BuilderName { get; set; }
        public Suppliers Supplier { get; set; }
        public string JobNumber { get; set; }
        public string JobAddress { get; set; }
        public string SubAddress { get; set; }
        public string FullAddress { get; set; }
        public string UnitNumber { get; set; }
        public string WindRate { get; set; }
        public int TotalLinearMeter { get; set; }
        public DateTime CompleteDate { get; set; }
        public DateTime PlanIsueDate { get; set; }
        public bool IsEPlan { get; set; }
        public bool IsEngineer { get; set; }
        public bool IsBracingPlan { get; set; }
        public string Treatment { get; set; }
        public string TieDown { get; set; }
        public string RoofMaterial { get; set; }
        public double RoofPitch { get; set; }
        public double CeilingPitch { get; set; }
        public int RoofOverHang { get; set; }
        public NoggingMethodType NoggingMethod { get; set; }
        public RoofFrameType RoofFrameType { get; set; }
        public int TrussSpacing { get; set; }
        public int RafterSpacing { get; set; }
        public int RaisedCeilingHeight { get; set; }
        public bool NoggingsAndSillInLM { get; set; }
        public bool UpToLength { get; set; }
        public int QuoteTolengthSize { get; set; }
        public bool JambBeamSupport { get; set; }
        public bool QuoteCeilingBattent { get; set; }
        public CeilingBattensType CeilingBattensType { get; set; }
        public DesignInfor FrameDesignInfor { get; set; }
        public DesignInfor BeamDesignInfor { get; set; }
        public DesignInfor BracingDesignInfor { get; set; }
        public int StepDown { get; set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="JobInfo"/> class.
        /// </summary>
        public JobInfoPoco()
        {

        }

        public JobInfoPoco(JobInfo info)
        {
            JobLocation = info.JobLocation;
            if (info.Client!=null)
            {
                ClientName = info.Client.Name;
            }
            //ClientName = info.ClientName;
            Customer = info.Customer;
            BuilderName = info.BuilderName;
            Supplier = info.Supplier;
            JobNumber = info.JobNumber;
            JobAddress = info.JobAddress;
            SubAddress = info.SubAddress;
            FullAddress = info.FullAddress;
            UnitNumber = info.UnitNumber;
            WindRate = info.WindRate;
            TotalLinearMeter = info.TotalLinearMeter;
            CompleteDate = info.CompleteDate;
            PlanIsueDate = info.PlanIsueDate;
            IsEPlan = info.IsEPlan;
            IsEngineer = info.IsEngineer;
            IsBracingPlan = info.IsBracingPlan;
            Treatment = info.Treatment;
            TieDown = info.TieDown;
            RoofMaterial = info.RoofMaterial;
            RoofPitch = info.RoofPitch;
            CeilingPitch = info.CeilingPitch;
            RoofOverHang = info.RoofOverHang;
            NoggingMethod = info.NoggingMethod;
            RoofFrameType = info.RoofFrameType;
            TrussSpacing = info.TrussSpacing;
            RafterSpacing = info.RafterSpacing;
            RaisedCeilingHeight = info.RaisedCeilingHeight;
            NoggingsAndSillInLM = info.NoggingsAndSillInLM;
            UpToLength = info.UpToLength;
            QuoteTolengthSize = info.QuoteTolengthSize;
            JambBeamSupport = info.JambBeamSupport;
            QuoteCeilingBattent = info.QuoteCeilingBattent;
            CeilingBattensType = info.CeilingBattensType;
            FrameDesignInfor = info.FrameDesignInfor;
            BeamDesignInfor = info.BeamDesignInfor;
            BracingDesignInfor = info.BracingDesignInfor;
            StepDown = info.StepDown;
        }
        #endregion
    }
}