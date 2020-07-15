// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the JobModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using ProtoBuf;

namespace AppModels
{
    using System;
    using System.Collections.ObjectModel;

    using Prism.Mvvm;

    /// <summary>
    /// The job model.
    /// </summary>
    [ProtoContract]
    public class JobInfo : BindableBase
    {

        /// <summary>
        /// The job location.
        /// </summary>
        private string jobLocation;

        /// <summary>
        /// The client name.
        /// </summary>
        private string clientName;

        /// <summary>
        /// The job number.
        /// </summary>
        private string jobNumber;

        /// <summary>
        /// The builder name.
        /// </summary>
        private string builderName;

        /// <summary>
        /// The unit number.
        /// </summary>
        private string unitNumber;

        /// <summary>
        /// The full address.
        /// </summary>
        private string fullAddress;

        /// <summary>
        /// The treatment.
        /// </summary>
        private string treatMent;

        /// <summary>
        /// The roof type.
        /// </summary>
        private string roofType;

        /// <summary>
        /// The is truss.
        /// </summary>
        private bool isTruss;

        /// <summary>
        /// The is rafter.
        /// </summary>
        private bool isRafter;

        /// <summary>
        /// The truss spacing.
        /// </summary>
        private int? trussSpacing;

        /// <summary>
        /// The rafter spacing.
        /// </summary>
        private int? rafterSpacing;
        

        /// <summary>
        /// The beam design infor.
        /// </summary>
        private DesignInfor beamDesignInfor;

        /// <summary>
        /// The frame design infor.
        /// </summary>
        private DesignInfor frameDesignInfor;

        /// <summary>
        /// The bracing design infor.
        /// </summary>
        private DesignInfor bracingDesignInfor;

        /// <summary>
        /// The plan issue date.
        /// </summary>
        private DateTime? planIssueDate;

        /// <summary>
        /// The is engineer plan.
        /// </summary>
        private bool isEPlan;

        /// <summary>
        /// The is engineer.
        /// </summary>
        private bool isEngineer;

        /// <summary>
        /// The is bracing plan.
        /// </summary>
        private bool isBracingPlan;
       

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="JobInfo"/> class.
        /// </summary>
        public JobInfo()
        {
            this.JobDefault = new JobWallDefaultInfo();
			this.WindRate = new WindCategory();
        }
        #endregion

        #region public Member

        /// <summary>
        /// Gets or sets the job default.
        /// </summary>
        [ProtoMember(1)]
        public JobWallDefaultInfo JobDefault
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the job location.
        /// </summary>
        [ProtoMember(2)]
        public string JobLocation
        {
            get => this.jobLocation;
            set => this.SetProperty(ref this.jobLocation, value);
        }

        /// <summary>
        /// Gets or sets the client name.
        /// </summary>
        public string ClientName
        {
            get => this.clientName;
            set => this.SetProperty(ref this.clientName, value);
        }

        /// <summary>
        /// Gets or sets the job number.
        /// </summary>
        public string JobNumber
        {
            get => this.jobNumber;
            set => this.SetProperty(ref this.jobNumber, value);
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
            get => this.fullAddress;
            set => this.SetProperty(ref this.fullAddress, value);
        }

        /// <summary>
        /// Gets or sets the builder name.
        /// </summary>
        public string BuilderName
        {
            get => this.builderName;
            set => this.SetProperty(ref this.builderName, value);
        }

        /// <summary>
        /// Gets or sets the unit number.
        /// </summary>
        public string UnitNumber
        {
            get => this.unitNumber;
            set => this.SetProperty(ref this.unitNumber, value);
        }

        /// <summary>
        /// Gets or sets the wind category.
        /// </summary>
        public WindCategory WindRate { get ;set ;}

        /// <summary>
        /// Gets or sets the treatment.
        /// </summary>
        public string Treatment
        {
            get => this.treatMent;
            set => this.SetProperty(ref this.treatMent, value);
        }
        

        /// <summary>
        /// Gets or sets the roof type.
        /// </summary>
        public string RoofType
        {
            get => this.roofType;
            set => this.SetProperty(ref this.roofType, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether is truss.
        /// </summary>
        public bool IsTruss
        {
            get => this.isTruss;
            set => this.SetProperty(ref this.isTruss, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether is rafter.
        /// </summary>
        public bool IsRafter
        {
            get => this.isRafter;
            set => this.SetProperty(ref this.isRafter, value);
        }

        /// <summary>
        /// Gets or sets the truss spacing.
        /// </summary>
        public int? TrussSpacing
        {
            get => this.trussSpacing;
            set => this.SetProperty(ref this.trussSpacing, value);
        }

        /// <summary>
        /// Gets or sets the rafter spacing.
        /// </summary>
        public int? RafterSpacing
        {
            get => this.rafterSpacing;
            set => this.SetProperty(ref this.rafterSpacing, value);
        }


        /// <summary>
        /// Gets or sets the total linear meter.
        /// </summary>
        public int TotalLinearMeter { get; set; }

        /// <summary>
        /// Gets or sets the date complete.
        /// </summary>
        public DateTime? CompleteDate { get; set; }

        /// <summary>
        /// Gets or sets the plan date.
        /// </summary>
        public DateTime? PlanIsueDate
        {
            get => this.planIssueDate;
            set => this.SetProperty(ref this.planIssueDate, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether is e plan.
        /// </summary>
        public bool IsEPlan
        {
            get => this.isEPlan;
            set => this.SetProperty(ref this.isEPlan, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether is engineer.
        /// </summary>
        public bool IsEngineer
        {
            get => this.isEngineer;
            set => this.SetProperty(ref this.isEngineer, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether is bracing plan.
        /// </summary>
        public bool IsBracingPlan
        {
            get => this.isBracingPlan;
            set => this.SetProperty(ref this.isBracingPlan, value);
        }

        /// <summary>
        /// Gets or sets the issues info.
        /// </summary>
        public string IssuesInfor { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        public DesignInfor FrameDesignInfor
        {
            get => this.frameDesignInfor;
            set => this.SetProperty(ref this.frameDesignInfor, value);
        }

        /// <summary>
        /// Gets or sets the beam design infor.
        /// </summary>
        public DesignInfor BeamDesignInfor
        {
            get => this.beamDesignInfor;
            set => this.SetProperty(ref this.beamDesignInfor, value);
        }

        /// <summary>
        /// Gets or sets the bracing design infor.
        /// </summary>
        public DesignInfor BracingDesignInfor
        {
            get => this.bracingDesignInfor;
            set => this.SetProperty(ref this.bracingDesignInfor, value);
        }

        /// <summary>
        /// Gets or sets the excel notes.
        /// </summary>
        public ExcelNote ExcelNotes {get;set;}

        /// <summary>
        /// Gets or sets the word notes.
        /// </summary>
        public ObservableCollection<string> WordNotes {get;set;}

        #endregion
    }
}
