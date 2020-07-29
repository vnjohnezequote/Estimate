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

        /// <summary>
        /// Gets or sets the job default.
        /// </summary>
        public GlobalWallInfoPoco GlobalWallInfo
        {
            get;
            set;
        }
        public int RoofOverHang
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets the job location.
        /// </summary>
        public string JobLocation
        {
            get; set;
        }
        public string TieDown
        {
            get; set;
        }
        public Suppliers Supplier { get; set; }

        /// <summary>
        /// Gets or sets the clientPoco name.
        /// </summary>
        public string ClientName
        {
            get; set;
        }

        public string Customer
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the job number.
        /// </summary>
        public string JobNumber
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the job address.
        /// </summary>
        public string JobAddress { get; set; }

        /// <summary>
        /// Gets or sets the sub address.
        /// </summary>
        public string SubAddress { get; set; }

        /// <summary>
        /// Gets or sets the full address.
        /// </summary>
        public string FullAddress
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the builder name.
        /// </summary>
        public string BuilderName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the unit number.
        /// </summary>
        public string UnitNumber
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the wind category.
        /// </summary>
        public string WindRate
        {
            get; set;
        }

        public bool QuoteCeilingBattent { get; set; }
        public CeilingBattensType CeilingBattensType { get; set; }

        public int QuoteTolengthSize { get; set; }
        public bool JambBeamSupport { get; set; }

        public bool NoggingsAndSillInLM { get; set; }
        public bool UpToLength { get; set; }
        /// <summary>
        /// Gets or sets the treatment.
        /// </summary>
        public string Treatment
        {
            get; set;
        }


        /// <summary>
        /// Gets or sets the roof type.
        /// </summary>
        public string RoofMaterial
        {
            get; set;
        }



        /// <summary>
        /// Gets or sets the total linear meter.
        /// </summary>
        public int TotalLinearMeter { get; set; }

        /// <summary>
        /// Gets or sets the date complete.
        /// </summary>
        public DateTime CompleteDate { get; set; }

        /// <summary>
        /// Gets or sets the plan date.
        /// </summary>
        public DateTime PlanIsueDate
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets a value indicating whether is e plan.
        /// </summary>
        public bool IsEPlan
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether is engineer.
        /// </summary>
        public bool IsEngineer
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether is bracing plan.
        /// </summary>
        public bool IsBracingPlan
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        public DesignInfor FrameDesignInfor
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the beam design infor.
        /// </summary>
        public DesignInfor BeamDesignInfor
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the bracing design infor.
        /// </summary>
        public DesignInfor BracingDesignInfor
        {
            get; set;
        }

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
            GlobalWallInfo = new GlobalWallInfoPoco(info.GlobalWallInfo);
            RoofOverHang = info.RoofOverHang;
            TieDown = info.TieDown;
            Supplier = info.Supplier;
            ClientName = info.ClientName;
            Customer = info.Customer;
            JobNumber = info.JobNumber;
            JobAddress = info.JobAddress;
            JobLocation = info.JobLocation;
            SubAddress = info.SubAddress;
            FullAddress = info.FullAddress;
            BuilderName = info.BuilderName;
            UnitNumber = info.UnitNumber;
            WindRate = info.WindRate;
            QuoteCeilingBattent = info.QuoteCeilingBattent;
            CeilingBattensType = info.CeilingBattensType;
            QuoteTolengthSize = info.QuoteTolengthSize;
            JambBeamSupport = info.JambBeamSupport;
            NoggingsAndSillInLM = info.NoggingsAndSillInLM;
            UpToLength = info.UpToLength;
            Treatment = info.Treatment;
            RoofMaterial = info.RoofMaterial;
            TotalLinearMeter = info.TotalLinearMeter;
            CompleteDate = info.CompleteDate;
            PlanIsueDate = info.PlanIsueDate;
            IsEPlan = info.IsEPlan;
            IsBracingPlan = info.IsBracingPlan;
            IsEngineer = info.IsEngineer;
            FrameDesignInfor = info.FrameDesignInfor;
            BeamDesignInfor = info.BeamDesignInfor;
            BracingDesignInfor = info.BracingDesignInfor;
        }
        #endregion
    }
}