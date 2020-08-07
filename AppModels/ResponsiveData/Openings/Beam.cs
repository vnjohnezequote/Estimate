// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Beam.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The beam.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
    public class Beam: BindableBase, IBeam
{
        #region Field

        private int _id;
        private string _name;
        private int _numberOfSupport;
        private BeamType _beamType;
        private SupportType _pointSupportType;
        private int _spanLength;
        private double _extraLength;
        private EngineerMemberInfo _supportReference;
        private EngineerMemberInfo _engineerTimberInfo;
        private WallBase _wallReference;

        private int _supportHeight;
        //private IWallMemberInfo _globalSupportInfo;
        #endregion


        #region Property

        public IWallMemberInfo GLobalSupportInfo { get; private set; }

        public ObservableCollection<SupportPoint> LoadPointSupports { get; set; } 
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

        public Suppliers Supplier { get; }
        public int Id { get=>_id; set=>SetProperty(ref _id,value); }
        
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the beam grade.
        /// </summary>
        public string BeamGrade { get; set; }

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
                if (WallReference != null)
                {
                    return WallReference.StudHeight;
                }

                return SupportHeight-GlobalThicknessTBT;
                //GlobalInfo.GlobalExtWallDetailInfo.RibbonPlate.Depth
            }
        }

        public int GlobalThicknessTBT
        {
            get
            {
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
            set => SetProperty(ref _wallReference,value);
        }
        #endregion

        #region Constructor

        public Beam(BeamType beamType,IGlobalWallInfo globalInfo)
        {
            Type = beamType;
            GlobalInfo = globalInfo;
            GlobalInfo.GlobalExtWallDetailInfo.BottomPlate.PropertyChanged += PlatePropertiesChanged;
            GlobalInfo.GlobalExtWallDetailInfo.TopPlate.PropertyChanged += PlatePropertiesChanged;
            GlobalInfo.GlobalExtWallDetailInfo.RibbonPlate.PropertyChanged += PlatePropertiesChanged;
            PropertyChanged += Beam_PropertyChanged;
            InitializedBeamSupportPoint();
            //GLobalSupportInfo = gLobalSupportInfo;
        }

        private void PlatePropertiesChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Depth" || e.PropertyName == "NoItem")
            {
                RaisePropertyChanged(nameof(GlobalThicknessTBT));
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
            }
        }

        #endregion


        
}
}
