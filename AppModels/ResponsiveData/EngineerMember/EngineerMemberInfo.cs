using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Navigation;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.EngineerMember;
using Prism.Mvvm;
using ProtoBuf;

namespace AppModels.ResponsiveData.EngineerMember
{
    public class EngineerMemberInfo: BindableBase, ITimberInfo
    {
        #region Field
        private string _levelType;
        private Suppliers? _suplier;
        private string _engineerName;
        private WallMemberType _memberType;
        private MaterialTypes _materialType;
        private int _minSpan;
        private int _maxSpan;
        private int _noItem;
        private int _depth;
        private int _thickness;
        private string _timberGrade;
        private int _id;
        private TimberBase _timberInfo;

        #endregion

        #region Properties
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public JobInfo GlobalInfor { get; }
        public string LevelType { get=>_levelType; set=>SetProperty(ref _levelType,value); }
        public Suppliers? Supplier {
            get
            {
                if (_suplier != null)
                    return _suplier;
                if (GlobalInfor!=null)
                {
                    return GlobalInfor.Supplier;
                }

                return null;
            }
            set
            {
                if (GlobalInfor!=null && value== GlobalInfor.Supplier)
                {
                    value = null;
                }

                SetProperty(ref _suplier, value);
            }
    }
        public string EngineerName { get=>_engineerName; set=>SetProperty(ref _engineerName,value); }
        public WallMemberType MemberType { get=>_memberType; set=>SetProperty(ref _memberType,value); }
        public MaterialTypes MaterialType { get=>_materialType; set=>SetProperty(ref _materialType,value); }
        public int MinSpan { get=>_minSpan; set=>SetProperty(ref _minSpan,value); }
        public int MaxSpan { get=>_maxSpan; set=>SetProperty(ref _maxSpan,value); }
        public int NoItem
        {
            get => _noItem == 0 ? 1 : _noItem;
            set=>SetProperty(ref _noItem,value);
        }
        public int Depth { get=>_depth; set=>SetProperty(ref _depth,value); }
        public int Thickness { get=>_thickness; set=>SetProperty(ref _thickness,value); }

        public string TimberGrade
        {
            get => MaterialType == MaterialTypes.Steel ? "Steel" : _timberGrade;
            set
            {
                if (value == "Steel")
                {
                    MaterialType = MaterialTypes.Steel;
                }
                else
                {
                    MaterialType = MaterialTypes.Timber;
                }
                SetProperty(ref _timberGrade,value);
            }
        }
        public string Size => this.NoItem == 1 ? this.Thickness + "x" + this.Depth : this.NoItem + "/" + this.Thickness + "x" + this.Depth;
        public string SizeGrade
        {
            get
            {
                if (MaterialType == MaterialTypes.Steel)
                {
                    return "Steel";
                }
                if (this.Thickness == 0 || this.Depth == 0)
                {
                    return "Nil";
                }
                else
                {
                    return this.Size + " " + this.TimberGrade;
                }
            }
            set
            {
                if (MaterialType == MaterialTypes.Steel)
                {
                    NoItem = 0;
                    Depth = 0;
                    Thickness = 0;
                    TimberGrade = "Steel";

                }
                var item = 0;
                var depth = 0;
                var grade = "";
                var thickness = 0;
                if (value == null || value == "Nil")
                {
                    return;
                }
                var words = value.Split(new char[] { '/', 'x', ' ' });
                if (value.Contains("/"))
                {
                    item = Convert.ToInt32(words[0]);
                    thickness = Convert.ToInt32(words[1]);
                    depth = Convert.ToInt32(words[2]);
                    grade = words[3];

                }
                else
                {
                    item = 0;
                    thickness = Convert.ToInt32(words[0]);
                    depth = Convert.ToInt32(words[1]);
                    grade = words[2];
                }
                NoItem = item;
                Depth = depth;
                Thickness = thickness;
                TimberGrade = grade;
            }

        }
        public string RealGrade
        {
            get
            {
                if (MaterialType == MaterialTypes.Steel)
                {
                    return "Steel";
                }

                if (string.IsNullOrEmpty(TimberGrade))
                {
                    return string.Empty;
                }
                if (!TimberGrade.Contains( "LVL")) return TimberGrade;
                switch (Supplier)
                {
                    case Suppliers.WESBEAM:
                        return "WLVL";
                    case Suppliers.TILLINGS:
                        return "LVL15";
                    default:
                        return TimberGrade;
                }
            }
        }

        public int RealDepth
        {
            get
            {
                if (string.IsNullOrEmpty(TimberGrade))
                {
                    return 0;
                }
                if (!TimberGrade.Contains( "LVL")) return Depth;
                switch (Supplier)
                {
                    case Suppliers.TILLINGS:
                        switch (Depth)
                        {
                            case 45:
                                return 42;
                            case 63:
                                return 58;
                            default:
                                return Depth;
                        }
                    default:
                        return Depth;

                }

            }
        }

        public string RealSizeGrade
        {
            get
            {
                if (MaterialType == MaterialTypes.Steel)
                {
                    return "Steel";
                }
                return this.NoItem == 1 ? this.Thickness + "x" + this.RealDepth + " " + RealGrade : this.NoItem + "/" + this.Thickness + "x" + RealDepth + " " + RealGrade;
            }


        }

        public TimberBase TimberInfo
        {
            get => _timberInfo;
            set
            {
                 SetProperty(ref _timberInfo, value);
                 Depth = TimberInfo.Depth;
                 Thickness = TimberInfo.Thickness;
                 TimberGrade = TimberInfo.TimberGrade;
            }
        }
        #endregion

        #region Constructor
        public EngineerMemberInfo(JobInfo globalInfo)
        {
            GlobalInfor = globalInfo;
            GlobalInfor.PropertyChanged += GlobalInfor_PropertyChanged;
            PropertyChanged += EngineerMemberInfo_PropertyChanged;
        }

        private void GlobalInfor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Supplier))
            {
                RaisePropertyChanged(nameof(Supplier));
            }
        }


        #endregion

        #region Private Method
        public void LoadMemberInfo(EngineerMemberInfoPoco memberInfo,Dictionary<string,List<TimberBase>> timberInfos)
        {
            TimberInfo = GetTimberInfoFromList(timberInfos,memberInfo.TimberInfoId,memberInfo.TimberGrade);
            Id = memberInfo.Id;
            Supplier = memberInfo.Supplier;
            LevelType = memberInfo.LevelType;
            EngineerName = memberInfo.EngineerName;
            MemberType = memberInfo.MemberType;
            MaterialType = memberInfo.MaterialType;
            MinSpan = memberInfo.MinSpan;
            MaxSpan = memberInfo.MaxSpan;
            NoItem = memberInfo.NoItem;
            Depth = memberInfo.Depth;
            Thickness = memberInfo.Thickness;
            TimberGrade = memberInfo.TimberGrade;
        }

        private TimberBase GetTimberInfoFromList(Dictionary<string,List<TimberBase>> timberInfos,int timberId,string timberGrade)
        {
            if (timberInfos==null)
            {
                return null;
            }
            
            timberInfos.TryGetValue(timberGrade, out var beams);
            return beams?.FirstOrDefault(timberBase => timberBase.Id == timberId);
        }
        private void EngineerMemberInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Supplier):
                    RaisePropertyChanged(nameof(RealDepth));
                    RaisePropertyChanged(nameof(RealGrade));
                    RaisePropertyChanged(nameof(RealSizeGrade));
                    break;
                case nameof(Depth):
                    RaisePropertyChanged(nameof(RealDepth));
                    RaiseSizeGradeChanged();
                    break;
                case nameof(Thickness):
                    RaiseSizeGradeChanged();
                    break;
                case nameof(MaterialType):
                    RaisePropertyChanged(nameof(TimberGrade));
                    RaiseSizeGradeChanged();
                    break;
                case nameof(NoItem):
                    RaiseSizeGradeChanged();
                    break;
                case nameof(TimberGrade):
                    RaiseSizeGradeChanged();
                    break;
            }
        }

        private void RaiseSizeGradeChanged()
        {
            RaisePropertyChanged(nameof(Size));
            RaisePropertyChanged(nameof(SizeGrade));
            RaisePropertyChanged(nameof(RealSizeGrade));
        }

        #endregion



    }
}
