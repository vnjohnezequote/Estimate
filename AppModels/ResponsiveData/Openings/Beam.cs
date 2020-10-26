// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Beam.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The beam.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Windows.Documents;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.Openings;
using AppModels.PocoDataModel.WallMemberData;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Support;
using devDept.Geometry;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Openings
{
    
/// <summary>
    /// The beam.
    /// </summary>
    public class Beam: BindableBase, IBeam,ITimberInfo
{
        #region Field

        private int _id;
        private string _name;
        private BeamType _type;
        private SupportType? _pointSupportType;
        private int _spanLength;
        private double _extraLength;
        private EngineerMemberInfo _supportReference;
        private EngineerMemberInfo _engineerTimberInfo;
        private WallBase _wallReference;
        private string _location;
        private int _supportHeight;
        private int _noItem;
        private TimberBase _timberInfo;
        private string _timberGrade;
        private double _beamPitch;

        private bool _isBeamExportToExcel = true;
        //private string _size;

        //private string _sizeGrade;

        private int _thickness;

        private int _depth;
        private Suppliers? _supplier;
        private int _quantity;
        private MaterialTypes? _materialType = null;
        //private IWallMemberInfo _globalSupportInfo;
        #endregion


        #region Property
       public bool IsBeamExportToExcel
       {
           get => _isBeamExportToExcel;
           set => SetProperty(ref _isBeamExportToExcel,value);
       }
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }
        //public IWallMemberInfo GLobalSupportInfo { get; private set; }

        public ObservableCollection<SupportPoint> LoadPointSupports { get;} 
            = new ObservableCollection<SupportPoint>();

        public List<Hanger> Hangers { get; private set; } = new List<Hanger>();
        public SupportType? PointSupportType
        {
            get
            {
                if (_pointSupportType!=null)
                {
                    return (SupportType)_pointSupportType;
                }

                if (SupportReference!=null && SupportReference.MemberType == WallMemberType.Post)
                {
                    if (SupportReference.MaterialType == MaterialTypes.Steel)
                    {
                        return SupportType.SteelPost;
                    }
                    else
                    {
                        return SupportType.Post;
                    }
                }

                return SupportType.Jamb;
            }
            set
            {
                if (SupportReference!=null && SupportReference.MemberType == WallMemberType.Post )
                {
                    if (SupportReference.MaterialType == MaterialTypes.Steel && value == SupportType.SteelPost)
                    {
                        value = null;
                    }

                    if (SupportReference.MaterialType == MaterialTypes.Timber && value==SupportType.Post)
                    {
                        value = null;
                    }
                }

                if (SupportReference!=null && SupportReference.MemberType == WallMemberType.DoorJamb && value == SupportType.Jamb)
                {
                    value = null;
                }
                SetProperty(ref _pointSupportType, value);
            } 
        }
        public BeamType Type
        {
            get=>_type;
            set=>SetProperty(ref _type,value);
        }
        public Suppliers? Supplier
        {
            get => _supplier ?? GlobalInfo.GlobalInfo.Supplier;
            set
            {
                if (GlobalInfo?.GlobalInfo != null && GlobalInfo.GlobalInfo.Supplier== value)
                {
                    value = null;
                }

                SetProperty(ref _supplier, value);
            }
        }
        public int Id { get=>_id; set=>SetProperty(ref _id,value); }
        /// <summary>
        /// Gets or sets the beam name.
        /// </summary>
        public string Name {
            get
            {
                if (!string.IsNullOrEmpty(_name))
                {
                    return _name;
                }

                if (Type == BeamType.TrussBeam || Type == BeamType.FloorBeam)
                {
                    return "B" + Id;
                }
                else
                {
                    return "L" + Id;
                }

            } 
            set
            {
                if (value == Name)
                {
                    return;
                }
                SetProperty(ref _name, value);
            }
        }
        public int SpanLength
        {
            get => _spanLength;
            set
            {
                SetProperty(ref _spanLength, value);
                RaisePropertyChanged(nameof(QuoteLength));
            }
        }
        public double ExtraLength
        {
            get=>_extraLength;
            set
            {
                SetProperty(ref _extraLength, value);
                RaisePropertyChanged(nameof(QuoteLength));
            }
        }

        /*** Moi them vao ***/
        public double BeamPitch
        {
            get=>_beamPitch;
            set
            {
                SetProperty(ref _beamPitch, value);
                RaisePropertyChanged(nameof(QuoteLength));
            } 
        }
        public double QuoteLength {
            get
            {
                var supportWidth = 0;
                if (LoadPointSupports!=null && LoadPointSupports.Count>=2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        supportWidth += LoadPointSupports[i].SupportWidth;
                    }
                }

                var quoteLengthInmm = (int)Math.Ceiling((SpanLength + supportWidth) / Math.Cos(Utility.DegToRad(BeamPitch)));
                var length2 = quoteLengthInmm.RoundUpTo300();
                var quoteLength = (double)length2/1000+ExtraLength;
                return quoteLength;
            }
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

                return supportWidth;
            }
        }
        public int Quantity { get=>_quantity; set=>SetProperty(ref _quantity,value); }
        public IGlobalWallInfo GlobalInfo { get; set; }
        public EngineerMemberInfo EngineerMemberInfo
        {
            get=> _engineerTimberInfo; 
            set
            {
                SetProperty(ref _engineerTimberInfo, value);
                if (EngineerMemberInfo!=null)
                {
                    TimberInfo = EngineerMemberInfo.TimberInfo;
                }
                
            }
        }
        public EngineerMemberInfo SupportReference
        {
            get => _supportReference;
            set => SetProperty(ref _supportReference, value);
        }
        public int SupportHeight
        {
            get
            {
                if (_supportHeight!=0)
                {
                    return _supportHeight;
                }

                if (WallReference!=null)
                {
                    return WallReference.FinalWallHeight;
                }

                if (GlobalInfo!=null)
                {
                    return GlobalInfo.WallHeight;
                }
                return 0;
            }
            set
            {
                if (WallReference!=null && value== WallReference.FinalWallHeight)
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

                if (WallReference !=null)
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
                if (GlobalInfo!=null)
                {
                    return GlobalInfo.GlobalExtWallDetailInfo.RibbonPlate.Depth *
                           GlobalInfo.GlobalExtWallDetailInfo.RibbonPlate.NoItem +
                           GlobalInfo.GlobalExtWallDetailInfo.TopPlate.Depth *
                           GlobalInfo.GlobalExtWallDetailInfo.TopPlate.NoItem +
                           GlobalInfo.GlobalExtWallDetailInfo.BottomPlate.Depth *
                           GlobalInfo.GlobalExtWallDetailInfo.BottomPlate.NoItem;
                }

                return 105;
            }
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
        public int NoItem
        {
            get
            {
                if (_noItem!=0)
                {
                    return _noItem;
                }

                return EngineerMemberInfo?.NoItem ?? 0;
            }
            set
            {
                if (EngineerMemberInfo!=null && value == EngineerMemberInfo.NoItem)
                {
                    value = 0;
                }
                SetProperty(ref _noItem, value);
            }
                
        }
        public int Thickness
        {
            get
            {
                if (_thickness!=0)
                {
                    return _thickness;
                }

                return EngineerMemberInfo?.Thickness ?? 0;
            }
            set
            {
                if (EngineerMemberInfo!=null && value == EngineerMemberInfo.Thickness)
                {
                    value = 0;
                }
                SetProperty(ref _thickness, value);
            } 
        }
        public int Depth
        {
            get
            {
                if (_depth!=0)
                {
                    return _depth;
                }

                return EngineerMemberInfo?.RealDepth ?? 0;
            }
            set
            {
                if (EngineerMemberInfo!=null && value==EngineerMemberInfo.Depth)
                {
                    value = 0;
                }
                SetProperty(ref _depth, value);
            } 

        }
        public string Size
        {
            get
            {
                if (NoItem ==0)
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
            set
            {
                if (EngineerMemberInfo!=null && EngineerMemberInfo.MaterialType == value)
                {
                    value = null;
                }
                SetProperty(ref _materialType, value);
            }
            
        }
        public virtual string SizeGrade
        {
            get
            {
                if (TimberGrade == "Steel"|| MaterialType == MaterialTypes.Steel)
                    return "Steel";
                return this.Size + " " + this.TimberGrade;

            }
            set
            {
                var item = 0;
                var depth = 0;
                var thickness = 0;
                var grade = "";
                switch (value)
                {
                    case null:
                        return;
                    case "Steel":
                        NoItem = item;
                        Thickness = thickness;
                        Depth = depth;
                        TimberGrade = grade;
                        return;
                }

                var words = value.Split(new char[] { '/', 'x', ' ' });
                if (value.Contains("/"))
                {
                    item = Convert.ToInt32(words[0]);
                    Thickness = Convert.ToInt32(words[1]);
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
        public string TimberGrade
        {
            get
            {
                if (MaterialType == MaterialTypes.Steel)
                    return "Steel";
                if (!string.IsNullOrEmpty(_timberGrade))
                {
                    return _timberGrade;
                }
                return TimberInfo != null ? TimberInfo.TimberGrade : _timberGrade;
            }
            set
            {
                if (TimberInfo!=null && TimberInfo.TimberGrade == value)
                {
                    value = string.Empty;
                }
                SetProperty(ref _timberGrade, value);
            } 
        }
        public TimberBase TimberInfo
        {
            get => _timberInfo;
            set
            {
              SetProperty(ref _timberInfo, value);  
            } 
        }
        public bool IsBeamToLongWithStockList
        {
            get
            {
                if (TimberInfo!=null && TimberInfo.MaximumLength< QuoteLength)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string SheetName { get; set; }

        #endregion

        #region Constructor

        public Beam(BeamType beamType,IGlobalWallInfo globalInfo)
        {
            Type = beamType;
            InitGlobalInfor(globalInfo);
            PropertyChanged += Beam_PropertyChanged;
            InitializedBeamSupportPoint();
            InitializerHanger();
        }

        public void InitGlobalInfor(IGlobalWallInfo globalWallInfo)
        {
            if (GlobalInfo!=null)
            {
                GlobalInfo.PropertyChanged -= GlobalInfo_PropertyChanged;
                GlobalInfo.GlobalExtWallDetailInfo.BottomPlate.PropertyChanged -= PlatePropertiesChanged;
                GlobalInfo.GlobalExtWallDetailInfo.TopPlate.PropertyChanged -= PlatePropertiesChanged;
                GlobalInfo.GlobalExtWallDetailInfo.RibbonPlate.PropertyChanged -= PlatePropertiesChanged;
            }
            GlobalInfo = globalWallInfo;
            GlobalInfo.PropertyChanged += GlobalInfo_PropertyChanged;
            GlobalInfo.GlobalExtWallDetailInfo.BottomPlate.PropertyChanged += PlatePropertiesChanged;
            GlobalInfo.GlobalExtWallDetailInfo.TopPlate.PropertyChanged += PlatePropertiesChanged;
            GlobalInfo.GlobalExtWallDetailInfo.RibbonPlate.PropertyChanged += PlatePropertiesChanged;
            RaisePropertyChanged(nameof(SupportHeight));
            RaisePropertyChanged(nameof(ThicknessTBT));
        }
        private void Wall_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FinalWallHeight")
            {
                RaisePropertyChanged(nameof(SupportHeight));
            }

            if (e.PropertyName == "ThicknessTBT")
            {
                RaisePropertyChanged(nameof(ThicknessTBT));
            }
        }

        private void GlobalInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WallHeight")
            {
                RaisePropertyChanged(nameof(SupportHeight));
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

        private void InitializerHanger()
        {
            for (int i = 0; i < 2; i++)
            {
                var hanger = new Hanger();
                Hangers.Add(hanger);
            }
        }
        public void NotifyPropertyChanged()
        {
            RaisePropertyChanged(nameof(TotalSupportWidth));
            RaisePropertyChanged(nameof(QuoteLength));
        }
        public void AddSupport()
        {

            var supportPoint = new SupportPoint(this, LoadPointLocation.MidPoint);
            LoadPointSupports.Add(supportPoint);
        }
        private void Beam_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Id):
                case nameof(Type):
                    RaisePropertyChanged(nameof(Name));
                    break;
                case nameof(SupportReference):
                    RaisePropertyChanged(nameof(QuoteLength));
                    RaisePropertyChanged(nameof(TotalSupportWidth));
                    break;
                case nameof(WallReference):
                    RaisePropertyChanged(nameof(ThicknessTBT));
                    RaisePropertyChanged(nameof(SupportHeight));
                    RaisePropertyChanged(nameof(RealSupportHeight));
                    break;
                case nameof(SupportHeight):
                    RaisePropertyChanged(nameof(RealSupportHeight));
                    break;
                case nameof(PointSupportType):
                    RaisePropertyChanged(nameof(RealSupportHeight));
                    break;
                case nameof(EngineerMemberInfo):
                    RaisePropertyChanged(nameof(NoItem));
                    RaisePropertyChanged(nameof(Depth));
                    RaisePropertyChanged(nameof(Thickness));
                    RaisePropertyChanged(nameof(TimberGrade));
                    RaisePropertyChanged(nameof(TimberInfo));
                    RaisePropertyChanged(nameof(Size));
                    RaisePropertyChanged(nameof(SizeGrade));
                    RaisePropertyChanged(nameof(MaterialType));
                    break;
                case nameof(MaterialType):
                    RaisePropertyChanged(nameof(TimberGrade));
                    RaisePropertyChanged(nameof(SizeGrade));
                    break;
                case nameof(TimberInfo):
                case nameof(QuoteLength):
                    RaisePropertyChanged(nameof(IsBeamToLongWithStockList));
                    break;
            }
        }

        #endregion

        public void LoadBeamInfo(BeamPoco beam, List<EngineerMemberInfo> engineerSchedules, List<WallBase> walls,Dictionary<string,List<TimberBase>> timberInfos)
        {
            LoadPointSupportInfo(beam.LoadPointSupports, engineerSchedules);
            LoadWallInfo(walls, beam);
            LoadEngineerInfo(engineerSchedules, beam);
            TimberInfo = LoadTimberInfor(timberInfos, beam.TimberInfoId,beam.TimberGrade);
            BeamPitch = beam.BeamPitch;
            Hangers = beam.Hangers;
            Location = beam.Location;
            PointSupportType = beam.PointSupportType;
            Type = beam.Type;
            Supplier = beam.Suplier;
            Id = beam.Id;
            //Name = beam.Name;
            SpanLength = beam.SpanLength;
            ExtraLength = beam.ExtraLength;
            Quantity = beam.Quantity;
            NoItem = beam.NoItem;
            Thickness = beam.Thickness;
            Depth = beam.Depth;
            MaterialType = beam.MaterialType;
            IsBeamExportToExcel = beam.IsBeamExportToExcel;
        }

        private TimberBase LoadTimberInfor(Dictionary<string,List<TimberBase>> timberInfos, int timberId,string timberGrade)
        {
            if (timberInfos == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(timberGrade))
            {
                return null;
            }

            timberInfos.TryGetValue(timberGrade, out var beams);
            return beams?.FirstOrDefault(timberBase => timberBase.Id == timberId);
        }
        private void LoadEngineerInfo(List<EngineerMemberInfo> engineerSchedules, BeamPoco beam)
        {
            foreach (var engineerMemberInfo in engineerSchedules)
            {
                if (engineerMemberInfo.Id == beam.EngineerMemberId)
                    this.EngineerMemberInfo = engineerMemberInfo;
                if (engineerMemberInfo.Id == beam.SupportReferenceId)
                {
                    SupportReference = engineerMemberInfo;
                }
            }
        }

        private void LoadWallInfo(List<WallBase> wallList, BeamPoco beam)
        {
            foreach (var wallBase in wallList)
            {
                if (wallBase.Id == beam.WallReferenceID)
                {
                    this.WallReference = wallBase;
                }
            }
        }

        private void LoadPointSupportInfo(List<SupportPointPoco> supports,List<EngineerMemberInfo> engineerMemberInfos)
        {
            foreach (var supportPointPoco in supports)
            {
                var i = supports.IndexOf(supportPointPoco);
                if (i ==0 || i==1)
                {
                    LoadPointSupports[i].LoadSupportPoint(supportPointPoco,engineerMemberInfos);
                }
                else
                {
                    var support = new SupportPoint(this,LoadPointLocation.MidPoint);
                    support.LoadSupportPoint(supportPointPoco,engineerMemberInfos);
                    LoadPointSupports.Add(support);
                }
            }
            
        }


}
}
