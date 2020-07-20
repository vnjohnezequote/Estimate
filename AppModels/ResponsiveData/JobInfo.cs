// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the JobModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using AppModels.Enums;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The job model.
    /// </summary>
    public class JobInfo : BindableBase
    {
        #region Field
        /// <summary>
        /// The job location.
        /// </summary>
        private string _jobLocation;

        /// <summary>
        /// The clientPoco name.
        /// </summary>
        private string _clientName;

        /// <summary>
        /// The job number.
        /// </summary>
        private string _jobNumber;

        /// <summary>
        /// The builder name.
        /// </summary>
        private string _builderName;

        /// <summary>
        /// The unit number.
        /// </summary>
        private string _unitNumber;

        /// <summary>
        /// The full address.
        /// </summary>
        private string _fullAddress;

        /// <summary>
        /// The treatment.
        /// </summary>
        private string _treatMent;

        /// <summary>
        /// The roof type.
        /// </summary>
        private string _roofMaterial;


        /// <summary>
        /// The beam design infor.
        /// </summary>
        private DesignInfor _beamDesignInfor;

        /// <summary>
        /// The frame design infor.
        /// </summary>
        private DesignInfor _frameDesignInfor;

        /// <summary>
        /// The bracing design infor.
        /// </summary>
        private DesignInfor _bracingDesignInfor;

        /// <summary>
        /// The plan issue date.
        /// </summary>
        private DateTime _planIssueDate;

        /// <summary>
        /// The is engineer plan.
        /// </summary>
        private bool _isEPlan;

        /// <summary>
        /// The is engineer.
        /// </summary>
        private bool _isEngineer;

        /// <summary>
        /// The is bracing plan.
        /// </summary>
        private bool _isBracingPlan;

        private string _windrate;
        private string _tieDown;
        private int _externalDoorHeight;
        private int _internalDoorHeight;
        private bool _quoteCeilingBattent;
        private string _customer;
        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the job default.
        /// </summary>
        public JobWallDefaultInfo JobDefault
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the job location.
        /// </summary>
        public string JobLocation
        {
            get => this._jobLocation;
            set => this.SetProperty(ref this._jobLocation, value);
        }
        public string TieDown {
            get => string.IsNullOrEmpty(_tieDown) ? this.GeneralTieDown() : _tieDown;
            set => SetTieDown(value);
        }
        public Suppliers Supplier { get; set; }

        /// <summary>
        /// Gets or sets the clientPoco name.
        /// </summary>
        public string ClientName
        {
            get => this._clientName;
            set => this.SetProperty(ref this._clientName, value);
        }

        public string Customer
        {
            get => this._customer;
            set => this.SetProperty(ref _customer, value);
        }

        /// <summary>
        /// Gets or sets the job number.
        /// </summary>
        public string JobNumber
        {
            get => this._jobNumber;
            set => this.SetProperty(ref this._jobNumber, value);
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
            get => this._fullAddress;
            set => this.SetProperty(ref this._fullAddress, value);
        }

        /// <summary>
        /// Gets or sets the builder name.
        /// </summary>
        public string BuilderName
        {
            get => this._builderName;
            set => this.SetProperty(ref this._builderName, value);
        }

        /// <summary>
        /// Gets or sets the unit number.
        /// </summary>
        public string UnitNumber
        {
            get => this._unitNumber;
            set => this.SetProperty(ref this._unitNumber, value);
        }

        /// <summary>
        /// Gets or sets the wind category.
        /// </summary>
        public string WindRate
        {
            get => _windrate;
            set
            {
                SetProperty(ref _windrate, value);
                RaisePropertyChanged(nameof(CheckWinRate));
            }
        }

        public bool CheckWinRate
        {
            get
            {
                if (string.IsNullOrEmpty(WindRate))
                {
                    return false;
                }

                return WindRate=="N2" || WindRate=="N3" || WindRate == "C1";
            }
        }

        public int ExternalDoorHeight { 
            get=>_externalDoorHeight;
            set
            {
                SetProperty(ref _externalDoorHeight, value);
                RaisePropertyChanged(nameof(InternalDoorHeight));
            }

        }

       
        public bool QuoteCeilingBattent { get=>_quoteCeilingBattent; set=>SetProperty(ref _quoteCeilingBattent,value); }
        public CeilingBattensType CeilingBattensType { get; set; } = CeilingBattensType.Timber;

        public int QuoteTolengthSize { get; set; } = 5400;
        public bool JambBeamSupport { get; set; }
        public int InternalDoorHeight
        {
            get => _internalDoorHeight == 0 ? _externalDoorHeight : _internalDoorHeight;
            set=>SetProperty(ref _internalDoorHeight,value);
        }

        public bool NoggingsAndSillInLM { get; set; }
        public bool UpToLength { get; set; }
        /// <summary>
        /// Gets or sets the treatment.
        /// </summary>
        public string Treatment
        {
            get => this._treatMent;
            set => this.SetProperty(ref this._treatMent, value);
        }


        /// <summary>
        /// Gets or sets the roof type.
        /// </summary>
        public string RoofMaterial
        {
            get => this._roofMaterial;
            set => this.SetProperty(ref this._roofMaterial, value);
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
            get => this._planIssueDate;
            set => this.SetProperty(ref this._planIssueDate, value);
        }
        /// <summary>
        /// Gets or sets a value indicating whether is e plan.
        /// </summary>
        public bool IsEPlan
        {
            get => this._isEPlan;
            set => this.SetProperty(ref this._isEPlan, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether is engineer.
        /// </summary>
        public bool IsEngineer
        {
            get => this._isEngineer;
            set => this.SetProperty(ref this._isEngineer, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether is bracing plan.
        /// </summary>
        public bool IsBracingPlan
        {
            get => this._isBracingPlan;
            set => this.SetProperty(ref this._isBracingPlan, value);
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
            get => this._frameDesignInfor;
            set => this.SetProperty(ref this._frameDesignInfor, value);
        }

        /// <summary>
        /// Gets or sets the beam design infor.
        /// </summary>
        public DesignInfor BeamDesignInfor
        {
            get => this._beamDesignInfor;
            set => this.SetProperty(ref this._beamDesignInfor, value);
        }

        /// <summary>
        /// Gets or sets the bracing design infor.
        /// </summary>
        public DesignInfor BracingDesignInfor
        {
            get => this._bracingDesignInfor;
            set => this.SetProperty(ref this._bracingDesignInfor, value);
        }

        /// <summary>
        /// Gets or sets the excel notes.
        /// </summary>
        //public ExcelNote ExcelNotes {get;set;}

        /// <summary>
        /// Gets or sets the word notes.
        /// </summary>
        //public ObservableCollection<string> WordNotes {get;set;}


        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="JobInfo"/> class.
        /// </summary>
        public JobInfo()
        {
            this.JobDefault = new JobWallDefaultInfo();
            this.PropertyChanged += JobInfo_PropertyChanged;
            WindRate = "N3";
            Treatment = "Untreated";
        }

        private void SetTieDown(string value)
        {
            switch (WindRate)
            {
                case "C2":
                case "C3":
                    if (value =="Direct")
                    {
                        value = null;
                    }
                    break;
                case "N1":
                case "N2":
                    if (string.IsNullOrEmpty(BuilderName) && value =="1800")
                    {
                        value = null;
                    }
                    else if (BuilderName.Contains("Privium") && value=="1500")
                    {
                        value = null;
                    }
                    else if(value =="1800")
                    {
                        value = null;
                    }

                    break;
                case "N3":
                    if (value == "1200")
                    {
                        value = null;
                    }
                    break;
            }

            this.SetProperty(ref _tieDown, value);

        }

        private void JobInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Customer))
            {
                if (this.Customer == "Bunnings Hallam Frame And Truss")
                {
                    this.BuilderName = "Privium";
                }
            }

            if (e.PropertyName==nameof(BuilderName))
            {
                if (BuilderName.Contains("Privium"))
                {
                    this.WindRate = "N2";
                    this.Treatment = "Untreated";
                }
            }

            if (e.PropertyName==nameof(WindRate))
            {
                this.RaisePropertyChanged(nameof(TieDown));
            }
        }

        private string GeneralTieDown()
        {
            switch (WindRate)
            {
                case "N1":
                case "N2":
                    if (string.IsNullOrEmpty(BuilderName))
                    {
                        return "1800";
                    }
                    return BuilderName.Contains("Privium") ? "1500" : "1800";

                case "N3":
                    return "1200";
                case "C2":
                case "C3":
                    return "Direct";
                default:
                    return string.Empty;
            }
        }
        #endregion

        #region public Member


        #endregion
    }
}
