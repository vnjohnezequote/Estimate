// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the JobModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppModels.Enums;
using AppModels.PocoDataModel;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The job model.
    /// </summary>
    public class JobInfo : BindableBase
    {
        #region Field
        private string _jobLocation;
        private string _clientName;
        private string _jobNumber;
        private string _unitNumber;
        private DesignInfor _beamDesignInfor;
        private DesignInfor _frameDesignInfor;
        private DesignInfor _bracingDesignInfor;
        private DateTime _planIssueDate;
        private bool _isEPlan;
        private bool _isEngineer;
        private bool _isBracingPlan;
        private Suppliers _supplier;
        private string _treatMent;
        private string _roofMaterial;
        private string _windrate;
        private string _tieDown;
        private bool _quoteCeilingBattent;
        private string _customer;
        private int _roofOverHang;
        private string _builderName;
        private int _trussSpacing;
        private int _rafterSpacing;
        private int _stepDown;
        private double _roofPitch;
        private double _ceilingPitch;
        private RoofFrameType _roofFrameType;
        private NoggingMethodType _noggingMethod;
        private int _raisedCeilingHeight;
        private CeilingBattensType _ceilingBattenType;
        private string _jobAddress;
        private string _subAddress;
        private ClientPoco _client;

        #endregion
        #region Property
        public JobModel JobModel { get; set; }
        public string JobLocation
        {
            get => this._jobLocation;
            set => this.SetProperty(ref this._jobLocation, value);
        }
        //public string ClientName
        //{
        //    get => this._clientName;
        //    set => this.SetProperty(ref this._clientName, value);
        //}

        public ClientPoco Client
        {
            get => this._client;
            set => SetProperty(ref _client,value);
        }
        public string Customer
        {
            get => this._customer;
            set
            {
                if (value == null)
                {
                    return;
                }
                 this.SetProperty(ref _customer, value);
            }
        }
        public string BuilderName
        {
            get => this._builderName;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                this.SetProperty(ref this._builderName, value);
            }
        }
        public Suppliers Supplier
        {
            get => _supplier;
            set => SetProperty(ref _supplier, value);
        }
        public string JobNumber
        {
            get => this._jobNumber;
            set => this.SetProperty(ref this._jobNumber, value);
        }
        public string JobAddress {
            get=>_jobAddress;
            set
            {
                SetProperty(ref _jobAddress, value);
                RaisePropertyChanged(nameof(FullAddress));
            }

        }

        public string SubAddress
        {
            get=>_subAddress;
            set
            {
                SetProperty(ref _subAddress, value);
                RaisePropertyChanged(nameof(FullAddress));
            }
        }
        public string FullAddress
        {
            get => JobAddress + ", " + SubAddress;
            set
            {
                if (string.IsNullOrEmpty(value)|| value == ", ")
                {
                    return;
                }
                var jobAddresses = value.Split(',');
                switch (jobAddresses.Length)
                {
                    case 0:
                        return;
                    case 1:
                        JobAddress = jobAddresses[0];
                        return;
                    case 2:
                        JobAddress = jobAddresses[0];
                        SubAddress = jobAddresses[1].Trim();
                        break;
                    case 3:
                        JobAddress = jobAddresses[0];
                        SubAddress = jobAddresses[1].Trim() + ", " + jobAddresses[2].Trim();
                        break;
                }
            } 
        }
        public string UnitNumber
        {
            get => this._unitNumber;
            set => this.SetProperty(ref this._unitNumber, value);
        }
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

                return WindRate == "N2" || WindRate == "N3" || WindRate == "C1";
            }
        }
        public int TotalLinearMeter { get; set; }
        public DateTime CompleteDate { get; set; }
        public DateTime PlanIsueDate
        {
            get => this._planIssueDate;
            set => this.SetProperty(ref this._planIssueDate, value);
        }
        public bool IsEPlan
        {
            get => this._isEPlan;
            set => this.SetProperty(ref this._isEPlan, value);
        }
        public bool IsEngineer
        {
            get
            {
                if ((BracingDesignInfor!=null && BracingDesignInfor.Content.Contains("Engineer"))||
                    (BeamDesignInfor!=null && BeamDesignInfor.Content.Contains("Engineer"))||
                    (FrameDesignInfor!=null && FrameDesignInfor.Content.Contains("Engineer")))
                {
                    return true;
                }

                return false;
            } 
            //set => this.SetProperty(ref this._isEngineer, value);
        }
        public bool IsBracingPlan
        {
            get
            {
                if (BracingDesignInfor!=null && BracingDesignInfor.Content.Contains("TBA"))
                {
                    return false;
                }

                if (BracingDesignInfor!=null && BracingDesignInfor.Content.Contains("Bracing Plan"))
                {
                    return true;
                }
                return false;
            }
            //set => this.SetProperty(ref this._isBracingPlan, value);
        }

        public string IssuesInfor { get; set; }
        public string Treatment
        {
            get => this._treatMent;
            set => this.SetProperty(ref this._treatMent, value);
        }
        public string TieDown
        {
            get => string.IsNullOrEmpty(_tieDown) ? this.GeneralTieDown() : _tieDown;
            set => SetTieDown(value);
        }
        public string RoofMaterial
        {
            get => this._roofMaterial;
            set => this.SetProperty(ref this._roofMaterial, value);
        }
        public double RoofPitch
        {
            get => this._roofPitch;
            set
            {
                this.SetProperty(ref this._roofPitch, value);
                this.RaisePropertyChanged(nameof(CeilingPitch));
            }
        }
        public double CeilingPitch
        {
            get
            {
                if (Math.Abs(_ceilingPitch) > 0.0001)
                {
                    return _ceilingPitch;
                }

                return RoofFrameType == RoofFrameType.Truss ? _ceilingPitch : _roofPitch;
            } 
            set => SetProperty(ref _ceilingPitch, value);
        }
        public int RoofOverHang
        {
            get => _roofOverHang;
            set => SetProperty(ref _roofOverHang, value);
        }
        public NoggingMethodType NoggingMethod
        {
            get => _noggingMethod;
            set => SetProperty(ref _noggingMethod, value);
        }
        public RoofFrameType RoofFrameType
        {
            get => _roofFrameType;
            set
            {
                SetProperty(ref _roofFrameType, value);
                RaisePropertyChanged(nameof(CeilingPitch));
            }
        }
        public int TrussSpacing
        {
            get => this._trussSpacing;
            set
            {
                this.SetProperty(ref this._trussSpacing, value);
                RaisePropertyChanged(nameof(RafterSpacing));
            }
        }
        public int RafterSpacing
        {
            get => this._rafterSpacing == 0 ? _trussSpacing : this._rafterSpacing;
            set => this.SetProperty(ref this._rafterSpacing, value);
        }
        public int RaisedCeilingHeight
        {
            get => _raisedCeilingHeight;
            set => SetProperty(ref _raisedCeilingHeight, value);
        }
        public bool NoggingsAndSillInLM { get; set; }
        public bool UpToLength { get; set; }
        public int QuoteTolengthSize { get; set; } = 5400;
        public bool JambBeamSupport { get; set; }
        public bool QuoteCeilingBattent
        {
            get => _quoteCeilingBattent;
            set => SetProperty(ref _quoteCeilingBattent, value);
        }
        public CeilingBattensType CeilingBattensType
        {
            get => _ceilingBattenType;
            set => SetProperty(ref _ceilingBattenType, value);
        }
        public DesignInfor FrameDesignInfor
        {
            get => this._frameDesignInfor;
            set
            {
                if (value == null)
                {
                    return;
                }
                this.SetProperty(ref this._frameDesignInfor, value);
            } 
        }
        public DesignInfor BeamDesignInfor
        {
            get => this._beamDesignInfor;
            set
            {
                if (value == null)
                {
                    return;
                }
                this.SetProperty(ref this._beamDesignInfor, value);
            } 
        }
        public DesignInfor BracingDesignInfor
        {
            get => this._bracingDesignInfor;
            set
            {
                if (value == null)
                {
                    return;
                }
                this.SetProperty(ref this._bracingDesignInfor, value);
            } 
        }
        public int StepDown
        {
            get => this._stepDown;
            set => this.SetProperty(ref this._stepDown, value);
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
        public JobInfo(JobModel job)
        {
            //this.GlobalWallInfo = new GlobalWallInfo();
            this.JobModel = job;
            PropertyChanged += JobInfo_PropertyChanged;
        }
        #endregion

        #region Private Function

        private void JobInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Client):
                    if (Client !=null && Client.Name == "Prenail")
                    {
                        this.Supplier = Suppliers.DINDAS;
                        Customer = "Noosa Truss";
                    }

                    else if(Client!=null && Client.Name!="Prenail")
                    {
                        Customer = "";
                    }
                    RaisePropertyChanged(nameof(TieDown));
                    break;
                case nameof(Customer):
                    {
                        if (this.Customer == "Bunnings Hallam Frame And Truss")
                        {
                            this.BuilderName = "Privium";
                        }

                        break;
                    }
                case nameof(BuilderName) when string.IsNullOrEmpty(BuilderName):
                    return;
                case nameof(BuilderName):
                    {
                        if (BuilderName.Contains("Privium"))
                        {
                            this.WindRate = "N2";
                            this.Treatment = "Untreated";
                        }

                        break;
                    }
                case nameof(WindRate):
                    this.RaisePropertyChanged(nameof(TieDown));
                    break;
                case nameof(FrameDesignInfor):
                case nameof(BeamDesignInfor):
                case nameof(BracingDesignInfor):
                    RaisePropertyChanged(nameof(IsEngineer));
                    RaisePropertyChanged(nameof(IsBracingPlan));
                    break;
            }
        }
        private string GeneralTieDown()
        {
            if (Client!=null && Client.Name == "Prenail")
            {
                return "1200";
            }
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
        private void SetTieDown(string value)
        {
            if (Client!=null && Client.Name == "Prenail" && value == "1200")
            {
                value = null;
            }
            switch (WindRate)
            {
                case "C2":
                case "C3":
                    if (value == "Direct")
                    {
                        value = null;
                    }
                    break;
                case "N1":
                case "N2":
                    if (string.IsNullOrEmpty(BuilderName) && value == "1800")
                    {
                        value = null;
                    }
                    else if (!string.IsNullOrEmpty(BuilderName) && BuilderName.Contains("Privium") && value == "1500")
                    {
                        value = null;
                    }
                    else if (value == "1800")
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

        public void LoadJobInfo(JobInfoPoco info,List<ClientPoco> clients)
        {
            JobLocation = info.JobLocation;
            Client = GetClient(info.ClientName,clients);
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
            //IsEngineer = info.IsEngineer;
            //IsBracingPlan = info.IsBracingPlan;
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

        private ClientPoco GetClient(string clientName,List<ClientPoco> clients)
        {
            //this.Clients = new ObservableCollection<ClientPoco>(this._dbBase.Clients);
            foreach (var clientPoco in clients)
            {
                if (clientPoco.Name == clientName)
                {
                    return clientPoco;
                }
            }

            return null;
        }
        #endregion

        #region public Member


        #endregion
    }
}
