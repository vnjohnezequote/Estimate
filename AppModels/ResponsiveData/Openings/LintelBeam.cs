using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Support;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Openings
{
    public class LintelBeam : BindableBase,IBeam
    {
        #region Field

        private Opening _openingInfo;
        private EngineerMemberInfo _engineerTimberInfo;
        private EngineerMemberInfo _supportReference;
        private WallBase _wallReference;
        private string _noOfJambSupport;
        private string _name;
        private int _supportHeight;
        private int _noItem;
        private int _thickness;
        private int _depth;
        private MaterialTypes? _materialType;
        private string _timberGrade;
        private double _extraLength;
        private int _id;
        private Suppliers? _supplier;
        private SupportType? _pointSupportType=null;

        #endregion

        #region Properties

        public Dictionary<int[], int> DoorJambStandardSupportTable { get; }
        public Opening OpeningInfo
        {
            get => _openingInfo; 
            set => SetProperty(ref _openingInfo, value);
        }
        public WallBase WallReference
        {
            get => _wallReference;
            set
            {
                value.PropertyChanged += Wall_PropertyChanged;
                SetProperty(ref _wallReference, value);
            }
        }
        public Suppliers? Supplier {
            get
            {
                if (_supplier!=null)
                {
                    return (Suppliers)_supplier;
                }

                if (EngineerMemberInfo?.Supplier != null)
                {
                    return (Suppliers)EngineerMemberInfo.Supplier;
                }

                return Suppliers.TILLINGS;

            }
            set
            {
                if (EngineerMemberInfo!=null && EngineerMemberInfo.Supplier!=null && EngineerMemberInfo.Supplier == value)
                {
                    value = null;
                }

                SetProperty(ref _supplier, value);
            }
        }
        public SupportType? PointSupportType
        {
            get
            {
                if (_pointSupportType!=null)
                {
                    return _pointSupportType;
                }

                if (SupportReference!=null && SupportReference.MemberType == WallMemberType.Post)
                {
                    return SupportReference.MaterialType == MaterialTypes.Steel ? SupportType.SteelPost : SupportType.Post;
                }

                return SupportType.Jamb;

            }
            set
            {
                if (SupportReference != null && SupportReference.MemberType == WallMemberType.Post)
                {
                    switch (SupportReference.MaterialType)
                    {
                        case MaterialTypes.Steel when value == SupportType.SteelPost:
                        case MaterialTypes.Timber when value == SupportType.Post:
                            value = null;
                            break;
                    }
                }

                if (SupportReference != null && SupportReference.MemberType == WallMemberType.DoorJamb && value == SupportType.Jamb)
                {
                    value = null;
                }

            }
        }
        public EngineerMemberInfo SupportReference
        {
            get => _supportReference;
            set
            {
                SetProperty(ref _supportReference, value);
                SupportReference.PropertyChanged += SupportReference_PropertyChanged;
            } 
        }
        public EngineerMemberInfo EngineerMemberInfo
        {
            get => _engineerTimberInfo;
            set => SetProperty(ref _engineerTimberInfo, value);
        }
        public ObservableCollection<SupportPoint> LoadPointSupports { get; }
            = new ObservableCollection<SupportPoint>();
        public int SupportHeight
        {
            get
            {
                if (_supportHeight != 0)
                {
                    return _supportHeight;
                }

                if (WallReference != null)
                {
                    return WallReference.FinalWallHeight;
                }

                if (OpeningInfo.GlobalWallInfo != null)
                {
                    return OpeningInfo.GlobalWallInfo.WallHeight;
                }
                return 0;
            }
            set
            {
                if (WallReference != null && value == WallReference.FinalWallHeight)
                {
                    value = 0;
                }

                SetProperty(ref _supportHeight, value);
            }
        }
        public int RealSupportHeight
        {
            get
            {
                if (PointSupportType == SupportType.Jamb)
                {
                    if (WallReference != null)
                    {
                        return WallReference.StudHeight;
                    }

                    return SupportHeight - ThicknessTBT;
                }

                if (WallReference != null)
                {
                    return WallReference.FinalWallHeight;
                }
                return SupportHeight;
                //GlobalInfo.GlobalExtWallDetailInfo.RibbonPlate.Depth
            }
        }
        public int ThicknessTBT
        {
            get
            {
                if (WallReference != null)
                {
                    return WallReference.ThicknessTBT;
                }
                if (OpeningInfo.GlobalWallInfo != null)
                {
                    return OpeningInfo.GlobalWallInfo.GlobalExtWallDetailInfo.RibbonPlate.Depth *
                           OpeningInfo.GlobalWallInfo.GlobalExtWallDetailInfo.RibbonPlate.NoItem +
                           OpeningInfo.GlobalWallInfo.GlobalExtWallDetailInfo.TopPlate.Depth *
                           OpeningInfo.GlobalWallInfo.GlobalExtWallDetailInfo.TopPlate.NoItem +
                           OpeningInfo.GlobalWallInfo.GlobalExtWallDetailInfo.BottomPlate.Depth *
                           OpeningInfo.GlobalWallInfo.GlobalExtWallDetailInfo.BottomPlate.NoItem;
                }

                return 105;
            }
        }
        public BeamType Type { get; set; }
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(_name))
                {
                    return _name;
                }

                return "L" + Id;
                
            }
            set => SetProperty(ref _name, value);
        }
        public int TotalSupportWidth
        {
            get
            {
                var supportWidth = 0;
                if (LoadPointSupports != null && LoadPointSupports.Count >= 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        supportWidth += LoadPointSupports[i].SupportWidth;
                    }
                }

                return supportWidth*Convert.ToInt32(StandardDoorJambSupport);
            }
        }
        public string StandardDoorJambSupport
        {
            get
            {
                if (!string.IsNullOrEmpty(_noOfJambSupport)) return _noOfJambSupport;
                if (OpeningInfo == null) return _noOfJambSupport;
                var doorWidth = OpeningInfo.Width;
                if (OpeningInfo.DoorType == DoorTypes.CavitySlidingDoor)
                {
                    doorWidth = OpeningInfo.Width * 2 * OpeningInfo.NoDoor;
                }

                if (MaterialType == MaterialTypes.Steel || SupportReference==null)
                    return 1.ToString();
                if (OpeningInfo.IsGarageDoor)
                {
                    return 5.ToString();
                }
                if (doorWidth<2100)
                {
                    return 2.ToString();
                }

                else if (doorWidth < 3000)
                {
                    return 3.ToString();
                }

                else if (doorWidth < 4000)
                {
                    return 4.ToString();
                }
                else if(doorWidth < 5100)
                {
                    return 5.ToString();
                }
                else
                {
                    return 6.ToString();
                }

            }
            set
            {
                var doorWidth = OpeningInfo.Width;
                if (OpeningInfo.DoorType == DoorTypes.CavitySlidingDoor)
                {
                    doorWidth = OpeningInfo.Width * 2 * OpeningInfo.NoDoor;
                }
                if (OpeningInfo.IsGarageDoor && value == "5")
                {
                    value = string.Empty;
                }
                if (doorWidth < 2100 && value=="2")
                {
                    value = string.Empty;
                }

                else if (doorWidth < 3000 && value == "3")
                {
                    value = string.Empty;
                }

                else if (doorWidth < 4000 && value == "4")
                {
                    value = string.Empty;
                }
                else if (doorWidth < 5100 && value=="5")
                {
                    value = string.Empty;
                }
                else if(doorWidth>=5100 && value=="6")
                {
                    value = string.Empty;
                }

                SetProperty(ref _noOfJambSupport, value);
            }
        }
        public int NoItem
        {
            get
            {
                if (_noItem != 0)
                {
                    return _noItem;
                }

                return EngineerMemberInfo?.NoItem ?? 0;
            }
            set => SetProperty(ref _noItem, value);
        }
        public int Thickness
        {
            get
            {
                if (_thickness != 0)
                {
                    return _thickness;
                }

                return EngineerMemberInfo?.Thickness ?? 0;
            }
            set => SetProperty(ref _thickness, value);
        }
        public int Depth
        {
            get
            {
                if (_depth != 0)
                {
                    return _depth;
                }

                return EngineerMemberInfo?.RealDepth ?? 0;
            }
            set => SetProperty(ref _depth, value);

        }
        public string Size
        {
            get
            {
                if (NoItem == 0)
                {
                    return "";
                }
                var result = Thickness + "x" + Depth;
                if (NoItem == 1)
                {
                    return result;
                }

                return NoItem + "/" + result;
            }
        }
        public MaterialTypes? MaterialType
        {
            get => _materialType ?? EngineerMemberInfo?.MaterialType;
            set => SetProperty(ref _materialType, value);
        }
        public virtual string SizeGrade
        {
            get
            {
                if (TimberGrade == "Steel" || MaterialType == MaterialTypes.Steel)
                    return "Steel";
                return this.Size + " " + this.TimberGrade;

            }
            set
            {
                var item = 0;
                var depth = 0;
                var grade = "";
                var thickness = 0;
                switch (value)
                {
                    case null:
                        return;
                    case "Steel":
                        NoItem = item;
                        Depth = depth;
                        TimberGrade = grade;
                        Thickness = thickness;
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
                Thickness = thickness;
                Depth = depth;
                TimberGrade = grade;

            }
        }
        public string TimberGrade
        {
            get
            {
                if (MaterialType == MaterialTypes.Steel)
                    return "Steel";
                return EngineerMemberInfo != null ? EngineerMemberInfo.TimberGrade : _timberGrade;
            } 
            set => SetProperty(ref _timberGrade, value);
        }
        public int SpanLength => OpeningInfo.Width*OpeningInfo.DoorQty;

        public double ExtraLength
        {
            get => _extraLength;
            set
            {
                SetProperty(ref _extraLength, value);
                RaisePropertyChanged(nameof(QuoteLength));
            }
        }
        public double QuoteLength
        {
            get
            {
                var cavityFactor = 1;
                if (OpeningInfo.DoorType == DoorTypes.CavitySlidingDoor)
                {
                    cavityFactor = 2;
                }
                var supportWidth = 0;
                if (LoadPointSupports == null || LoadPointSupports.Count < 2)
                    return (double) ((SpanLength*cavityFactor + supportWidth).RoundUpTo300()) / 1000 + ExtraLength;
                for (var i = 0; i < 2; i++)
                {
                    supportWidth += LoadPointSupports[i].SupportWidth;
                }

                return (double)((SpanLength*cavityFactor + supportWidth).RoundUpTo300()) / 1000 + ExtraLength;
            }
        }

        #endregion

        #region Contructor

        public LintelBeam(Opening openingInfo)
        {
            DoorJambStandardSupportTable = new Dictionary<int[], int>()
            {
                {new int[] {0,2100},2},
                {new int[] {2100,3000},3},
                {new int[] {3000,4000},4},
                {new int[] {4000,5100},5},
                {new int[] {5100,9000},6}

            };
            OpeningInfo = openingInfo;
            OpeningInfo.PropertyChanged += OpeningInfo_PropertyChanged;
            OpeningInfo.GlobalWallInfo.PropertyChanged += GlobalWallInfo_PropertyChanged;
            PropertyChanged += LintelBeam_PropertyChanged;
            OpeningInfo.GlobalWallInfo.GlobalExtWallDetailInfo.BottomPlate.PropertyChanged += PlatePropertiesChanged;
            OpeningInfo.GlobalWallInfo.GlobalExtWallDetailInfo.TopPlate.PropertyChanged += PlatePropertiesChanged;
            OpeningInfo.GlobalWallInfo.GlobalExtWallDetailInfo.RibbonPlate.PropertyChanged += PlatePropertiesChanged;
            InitializedBeamSupportPoint();
        }

        private void SupportReference_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SupportReference.RealDepth) ||
                e.PropertyName == nameof(SupportReference.NoItem))
            {
                RaisePropertyChanged(nameof(StandardDoorJambSupport));
                RaisePropertyChanged(nameof(TotalSupportWidth));
                
            }
        }

        private void LintelBeam_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(EngineerMemberInfo):
                    RaisePropertyChanged(nameof(MaterialType));
                    RaisePropertyChanged(nameof(TimberGrade));
                    RaisePropertyChanged(nameof(NoItem));
                    RaisePropertyChanged(nameof(Depth));
                    RaisePropertyChanged(nameof(Thickness));
                    RaisePropertyChanged(nameof(Size));
                    RaisePropertyChanged(nameof(SizeGrade));
                    break;
                case nameof(SupportReference):
                    RaisePropertyChanged(nameof(TotalSupportWidth));
                    break;
                case nameof(StandardDoorJambSupport):
                    RaisePropertyChanged(nameof(TotalSupportWidth));
                    break;
                case nameof(MaterialType):
                    RaisePropertyChanged(nameof(StandardDoorJambSupport));
                    RaisePropertyChanged(nameof(TimberGrade));
                    RaisePropertyChanged(nameof(SizeGrade));
                    break;
            }
        }

        private void GlobalWallInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(OpeningInfo.GlobalWallInfo.WallHeight):
                    RaisePropertyChanged(nameof(SupportHeight));
                    RaisePropertyChanged(nameof(RealSupportHeight));
                    break;
            }
        }
        private void PlatePropertiesChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Depth" || e.PropertyName == "NoItem")
            {
                RaisePropertyChanged(nameof(ThicknessTBT));
                RaisePropertyChanged(nameof(RealSupportHeight));
            }
        }

        private void InitializedBeamSupportPoint()
        {
            for (int i = 0; i < 2; i++)
            {
                var supportPoint = new SupportPoint(this, LoadPointLocation.StartPoint);
                LoadPointSupports.Add(supportPoint);
            }


        }
        private void OpeningInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Id":
                    RaisePropertyChanged(nameof(Id));
                    RaisePropertyChanged(nameof(Name));
                    break;
                case "Width":
                    RaisePropertyChanged(nameof(StandardDoorJambSupport));
                    RaisePropertyChanged(nameof(SpanLength));
                    RaisePropertyChanged(nameof(QuoteLength));
                    break;
                case nameof(OpeningInfo.DoorQty):
                    RaisePropertyChanged(nameof(SpanLength));
                    RaisePropertyChanged(nameof(QuoteLength));
                    break;
                case nameof(OpeningInfo.DoorType):
                    RaisePropertyChanged(nameof(SpanLength));
                    RaisePropertyChanged(nameof(QuoteLength));
                    break;
                case nameof(OpeningInfo.NoDoor):
                    RaisePropertyChanged(nameof(SpanLength));
                    RaisePropertyChanged(nameof(QuoteLength));
                    break;
                case nameof(OpeningInfo.IsGarageDoor):
                    RaisePropertyChanged(nameof(StandardDoorJambSupport));
                    RaisePropertyChanged(nameof(QuoteLength));
                    break;
            }
        }

        #endregion

        #region private method
        private void Wall_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }

        public void LoadLintelInfo(LintelBeamPoco lintel,List<EngineerMemberInfo> engineerSchedules)
        {
            foreach (var engineerMemberInfo in engineerSchedules)
            {
                if (engineerMemberInfo.Id == lintel.EngineerMemberInfoId)
                {
                    EngineerMemberInfo = engineerMemberInfo;
                }

                if (engineerMemberInfo.Id ==lintel.SupportReferenceId)
                {
                    SupportReference = engineerMemberInfo;
                }
            }
            InitializerSupportPoints(lintel.LoadPointSupports,engineerSchedules);
            Supplier = lintel.Supplier;
            PointSupportType = lintel.PointSupportType;
            SupportHeight = lintel.SupportHeight;
            Id = lintel.Id;
            Name = lintel.Name;
            StandardDoorJambSupport = lintel.StandardDoorJambSupport;
            NoItem = lintel.NoItem;
            Thickness = lintel.Thickness;
            Depth = lintel.Depth;
            MaterialType = lintel.MaterialType;
            TimberGrade = lintel.TimberGrade;
            ExtraLength = lintel.ExtraLength;

        }

        private void InitializerSupportPoints(List<SupportPointPoco> supports,List<EngineerMemberInfo> engineerSchedules)
        {
            foreach (var supportPointPoco in supports)
            {
                var i = supports.IndexOf(supportPointPoco);
                if (i == 0 || i == 1)
                {
                    LoadPointSupports[i].LoadSupportPoint(supportPointPoco, engineerSchedules);
                }
                else
                {
                    var support = new SupportPoint(this, LoadPointLocation.MidPoint);
                    support.LoadSupportPoint(supportPointPoco, engineerSchedules);
                    LoadPointSupports.Add(support);
                }
            }
        }


        #endregion
    }
}
