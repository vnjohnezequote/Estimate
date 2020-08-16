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
using System.Security.AccessControl;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.EngineerMember;
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
        private BeamType _beamType;
        private SupportType _pointSupportType;
        private int _spanLength;
        private double _extraLength;
        private EngineerMemberInfo _supportReference;
        private EngineerMemberInfo _engineerTimberInfo;
        private WallBase _wallReference;
        private string _location;
        private int _supportHeight;
        private int _noItem;

        private string _timberGrade;

        private string _size;

        private string _sizeGrade;

        private int _thickness;

        private int _depth;

        private MaterialTypes? _materialType = null;
        //private IWallMemberInfo _globalSupportInfo;
        #endregion


        #region Property
       
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }
        public IWallMemberInfo GLobalSupportInfo { get; private set; }

        public ObservableCollection<SupportPoint> LoadPointSupports { get;} 
            = new ObservableCollection<SupportPoint>();

        public SupportType PointSupportType
        {
            get => _pointSupportType;
            set => SetProperty(ref _pointSupportType, value);
        }

        public BeamType Type
        {
            get=>_beamType;
            private set=>SetProperty(ref _beamType,value);
        }

        public Suppliers Supplier => GlobalInfo.GlobalInfo.Supplier;
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

                if (Type == BeamType.RoofBeam || Type == BeamType.FloorBeam)
                {
                    return "B" + Id;
                }
                else
                {
                    return "L" + Id;
                }

            } 
            set => SetProperty(ref _name, value);
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
                
                return (double)((SpanLength+supportWidth).RoundUpTo300()) / 1000 + ExtraLength;
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
        public int Quantity { get; set; }
        public IGlobalWallInfo GlobalInfo { get; }
        public EngineerMemberInfo EngineerMemberInfo
        {
            get=> _engineerTimberInfo; 
            set=>SetProperty(ref _engineerTimberInfo, value);
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
            set=>SetProperty(ref _noItem,value);
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
            set => SetProperty(ref _thickness, value);
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
            set => SetProperty(ref _depth, value);

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
            set=>SetProperty(ref _materialType,value);
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
                        thickness = thickness;
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
                return EngineerMemberInfo != null ? EngineerMemberInfo.TimberGrade : _timberGrade;
            }
            set => SetProperty(ref _timberGrade, value);
        }

        #endregion

        #region Constructor

        public Beam(BeamType beamType,IGlobalWallInfo globalInfo)
        {
            Type = beamType;
            GlobalInfo = globalInfo;
            GlobalInfo.PropertyChanged += GlobalInfo_PropertyChanged;
            GlobalInfo.GlobalExtWallDetailInfo.BottomPlate.PropertyChanged += PlatePropertiesChanged;
            GlobalInfo.GlobalExtWallDetailInfo.TopPlate.PropertyChanged += PlatePropertiesChanged;
            GlobalInfo.GlobalExtWallDetailInfo.RibbonPlate.PropertyChanged += PlatePropertiesChanged;
            PropertyChanged += Beam_PropertyChanged;
            InitializedBeamSupportPoint();
            //GLobalSupportInfo = gLobalSupportInfo;
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
                    RaisePropertyChanged(nameof(Size));
                    RaisePropertyChanged(nameof(SizeGrade));
                    RaisePropertyChanged(nameof(MaterialType));
                    break;
                case nameof(MaterialType):
                    RaisePropertyChanged(nameof(TimberGrade));
                    RaisePropertyChanged(nameof(SizeGrade));

                    break;
            }
        }

        #endregion


        
}
}
