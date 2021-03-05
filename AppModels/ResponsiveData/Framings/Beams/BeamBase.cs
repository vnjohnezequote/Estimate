using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.Framings.Beams;
using AppModels.PocoDataModel.Framings.FloorAndRafter;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Support;

namespace AppModels.ResponsiveData.Framings.Beams
{
    public abstract class BeamBase: FramingBase, IBeam
    {
        #region Private
        private SupportType? _pointSupportType;
        private WallBase _wallReference;
        private Suppliers? _supplier;
        private EngineerMemberInfo _supportReference;
        private string _location;
        private int _noItem;
        private bool _isQuoteToExtraExcel;
        private int _thickness;
        private int _depth;
        private int _supportHeight;

        #endregion

        #region Properties
        public bool IsQuoteToExtraExcel
        {
            get => _isQuoteToExtraExcel;
            set => SetProperty(ref _isQuoteToExtraExcel, value);
        }
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }
        public ObservableCollection<SupportPoint> LoadPointSupports { get; }
            = new ObservableCollection<SupportPoint>();
        public SupportType? PointSupportType
        {
            get
            {
                if (_pointSupportType != null)
                {
                    return (SupportType)_pointSupportType;
                }

                if (SupportReference != null && SupportReference.MemberType == WallMemberType.Post)
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
                if (SupportReference != null && SupportReference.MemberType == WallMemberType.Post)
                {
                    if (SupportReference.MaterialType == MaterialTypes.Steel && value == SupportType.SteelPost)
                    {
                        value = null;
                    }

                    if (SupportReference.MaterialType == MaterialTypes.Timber && value == SupportType.Post)
                    {
                        value = null;
                    }
                }

                if (SupportReference != null && SupportReference.MemberType == WallMemberType.DoorJamb && value == SupportType.Jamb)
                {
                    value = null;
                }
                SetProperty(ref _pointSupportType, value);
            }
        }
        public Suppliers? Supplier
        {
            get => _supplier ?? GlobalInfo?.GlobalInfo?.Supplier;
            set
            {
                if (GlobalInfo?.GlobalInfo != null && GlobalInfo.GlobalInfo.Supplier == value)
                {
                    value = null;
                }

                SetProperty(ref _supplier, value);
            }
        }
        public virtual int TotalSupportWidth
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
        public IGlobalWallInfo GlobalInfo { get; set; }
        public WallBase WallReference
        {
            get => _wallReference;
            set
            {
                value.PropertyChanged += Wall_PropertyChanged;
                SetProperty(ref _wallReference, value);
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

                if (GlobalInfo != null)
                {
                    return GlobalInfo.WallHeight;
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

                    return SupportHeight - ThicknessTbt;
                }

                if (WallReference != null)
                {
                    return WallReference.FinalWallHeight;
                }
                return SupportHeight;
                //GlobalInfo.GlobalExtWallDetailInfo.RibbonPlate.Depth
            }
        }
        public int ThicknessTbt
        {
            get
            {
                if (WallReference != null)
                {
                    return WallReference.ThicknessTBT;
                }
                if (GlobalInfo != null)
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
        public int NoItem
        {
            get
            {
                if (_noItem != 0)
                {
                    return _noItem;
                }
                if (FramingInfo != null)
                {
                    return FramingInfo.NoItem;
                }
                return EngineerMember?.NoItem ?? 0;
            }
            set
            {
                if (FramingInfo != null)
                {
                    if (value == FramingInfo.NoItem)
                        value = 0;
                }
                else if (EngineerMember != null && value == EngineerMember.NoItem)
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
                if (_thickness != 0)
                {
                    return _thickness;
                }

                if (FramingInfo != null)
                {
                    return FramingInfo.Thickness;
                }

                return EngineerMember?.Thickness ?? 0;
            }
            set
            {
                if (EngineerMember != null && value == EngineerMember.Thickness)
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
                if (_depth != 0)
                {
                    return _depth;
                }
                if (FramingInfo != null)
                {
                    return FramingInfo.Depth;
                }

                return EngineerMember?.RealDepth ?? 0;
            }
            set
            {
                if (EngineerMember != null && value == EngineerMember.Depth)
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
        public virtual string SizeGrade
        {
            get
            {
                //if (TimberGrade == "Steel"|| MaterialType == MaterialTypes.Steel)
                //    return "Steel";
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

        #endregion

        #region Constructor

        protected BeamBase(FramingTypes beamType, LevelWall level)
        {
            Id = Guid.NewGuid();
            Quantity = 1;
            FramingType = beamType;
            InitGlobalInfor(level.LevelInfo);
            InitializedBeamSupportPoint();
            //IsExisting = false;
            PropertyChanged += FramingBasePropertyChanged;
        }

        protected BeamBase(FramingSheet framingSheet):base(framingSheet)
        {
            InitGlobalInfor(framingSheet.Level.LevelInfo);
            InitializedBeamSupportPoint();
        }

        protected BeamBase(BeamBasePoco beamPoco, FramingSheet framingSheet, List<TimberBase> timberList,List<EngineerMemberInfo> engineerMemberInfos) : base(beamPoco,framingSheet, timberList,engineerMemberInfos)
        {
            InitGlobalInfor(framingSheet.Level.LevelInfo);
            Location = beamPoco.Location;
            PointSupportType = beamPoco.PointSupportType;
            Supplier = beamPoco.Suplier;
            NoItem = beamPoco.NoItem;
            Thickness = beamPoco.Thickness;
            Depth = beamPoco.Depth;
            InitializedBeamSupportPoint();
            LoadPointSupportInfo(beamPoco.LoadPointSupports, engineerMemberInfos);
        }

        protected BeamBase(BeamPoco beamPoco, List<TimberBase> timberList, List<EngineerMemberInfo> engineerMemberInfos) :
            base(beamPoco, timberList, engineerMemberInfos)
        {

        }
        protected BeamBase(BeamBase another) : base(another)
        {
            InitGlobalInfor(another.GlobalInfo);
            Location = another.Location;
            PointSupportType = another.PointSupportType;
            Supplier = another.Supplier;
            NoItem = another.NoItem;
            Thickness = another.Thickness;
            Depth = another.Depth;
            InitializedBeamSupportPoint();
            ClonePointSupport(another);
        }



        #endregion

        #region Private method
        private void Wall_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FinalWallHeight")
            {
                RaisePropertyChanged(nameof(SupportHeight));
            }

            if (e.PropertyName == "ThicknessTBT")
            {
                RaisePropertyChanged(nameof(ThicknessTbt));
            }
        }

        public void InitGlobalInfor(IGlobalWallInfo globalWallInfo)
        {
            if (GlobalInfo != null)
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
            RaisePropertyChanged(nameof(ThicknessTbt));
            RaisePropertyChanged(nameof(SupportHeight));
        }

        private void SupportReference_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SupportReference.RealDepth) ||
                e.PropertyName == nameof(SupportReference.NoItem))
            {
                //RaisePropertyChanged(nameof(StandardDoorJambSupport));
                RaisePropertyChanged(nameof(TotalSupportWidth));

            }
        }
        protected  override void FramingBasePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.FramingBasePropertyChanged(sender, e);
            switch (e.PropertyName)
            {
                case nameof(SupportReference):
                    RaisePropertyChanged(nameof(QuoteLength));
                    RaisePropertyChanged(nameof(TotalSupportWidth));
                    break;
                case nameof(WallReference):
                    RaisePropertyChanged(nameof(ThicknessTbt));
                    RaisePropertyChanged(nameof(SupportHeight));
                    RaisePropertyChanged(nameof(RealSupportHeight));
                    break;
                case nameof(SupportHeight):
                    RaisePropertyChanged(nameof(RealSupportHeight));
                    break;
                case nameof(PointSupportType):
                    RaisePropertyChanged(nameof(RealSupportHeight));
                    break;
                case nameof(EngineerMember):
                    RaisePropertyChanged(nameof(NoItem));
                    RaisePropertyChanged(nameof(Depth));
                    RaisePropertyChanged(nameof(Thickness));
                    RaisePropertyChanged(nameof(TimberGrade));
                    RaisePropertyChanged(nameof(Size));
                    RaisePropertyChanged(nameof(SizeGrade));
                    break;
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
                RaisePropertyChanged(nameof(ThicknessTbt));
                RaisePropertyChanged(nameof(RealSupportHeight));
            }
        }
        private void ClonePointSupport(BeamBase another)
        {
            foreach (var supportPoint in another.LoadPointSupports)
            {
                var i = another.LoadPointSupports.IndexOf(supportPoint);
                if (i == 0 || i == 1)
                {
                    LoadPointSupports[i].EngineerMemberInfo = supportPoint.EngineerMemberInfo;
                    LoadPointSupports[i].PointSupportType = supportPoint.PointSupportType;

                }
                else
                {
                    var support = new SupportPoint(this, LoadPointLocation.MidPoint);
                    support.EngineerMemberInfo = supportPoint.EngineerMemberInfo;
                    support.PointSupportType = supportPoint.PointSupportType;
                    LoadPointSupports.Add(support);
                }
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
        

        public void LoadWallSupportBeam(List<WallBase> walls,BeamPoco beam)
        {
            LoadWallInfo(walls, beam);
        }
        //private TimberBase LoadTimberInfor(Dictionary<string, List<TimberBase>> timberInfos, int timberId, string timberGrade)
        //{
        //    if (timberInfos == null)
        //    {
        //        return null;
        //    }

        //    if (string.IsNullOrEmpty(timberGrade))
        //    {
        //        return null;
        //    }

        //    timberInfos.TryGetValue(timberGrade, out var beams);
        //    return beams?.FirstOrDefault(timberBase => timberBase.Id == timberId);
        //}
        protected override void LoadExtraEngineerInfo(FramingBasePoco framing, EngineerMemberInfo engineerMember )
        {
            var beam = (BeamPoco) framing;
            if (engineerMember != null)
            {
                if (engineerMember.Id == beam.SupportReferenceId)
                {
                    SupportReference = engineerMember;
                }
                
            }

        }
        private void LoadWallInfo(List<WallBase> wallList, BeamBasePoco beam)
        {
            if (wallList != null)
            {
                foreach (var wallBase in wallList)
                {
                    if (wallBase.Id == beam.WallReferenceId)
                    {
                        this.WallReference = wallBase;
                    }
                }
            }

        }
        private void LoadPointSupportInfo(List<SupportPointPoco> supports, List<EngineerMemberInfo> engineerMemberInfos)
        {
            foreach (var supportPointPoco in supports)
            {
                var i = supports.IndexOf(supportPointPoco);
                if (i == 0 || i == 1)
                {
                    LoadPointSupports[i].LoadSupportPoint(supportPointPoco, engineerMemberInfos);
                }
                else
                {
                    var support = new SupportPoint(this, LoadPointLocation.MidPoint);
                    support.LoadSupportPoint(supportPointPoco, engineerMemberInfos);
                    LoadPointSupports.Add(support);
                }
            }

        }
        #endregion

        #region Protected method
        protected void InitializedBeamSupportPoint()
        {
            for (int i = 0; i < 2; i++)
            {
                var supportPoint = new SupportPoint(this, LoadPointLocation.StartPoint);
                LoadPointSupports.Add(supportPoint);
            }
        }


        #endregion
    }
}
