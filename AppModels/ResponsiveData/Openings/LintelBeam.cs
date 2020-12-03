using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.Framings.FloorAndRafter;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Framings.Beams;
using AppModels.ResponsiveData.Support;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Openings
{
    public class LintelBeam : BeamBase
    {
        #region Field
        private Opening _openingInfo;
        private string _noOfJambSupport;

        #endregion

        #region Properties
        public override double QuoteLength
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
                    return (double)((FramingSpan * cavityFactor + supportWidth).RoundUpTo300()) / 1000 + ExtraLength;
                for (var i = 0; i < 2; i++)
                {
                    supportWidth += LoadPointSupports[i].SupportWidth;
                }

                return (double)((FramingSpan * cavityFactor + supportWidth).RoundUpTo300()) / 1000 + ExtraLength;
            }
        }
        public override int TotalSupportWidth
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

                return supportWidth * Convert.ToInt32(StandardDoorJambSupport);
            }
        }
        public Dictionary<int[], int> DoorJambStandardSupportTable { get; }
        public Opening OpeningInfo
        {
            get => _openingInfo; 
            set => SetProperty(ref _openingInfo, value);
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

                if (FramingInfo.MaterialType == MaterialTypes.Steel || SupportReference==null)
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
        
        #endregion

        #region Contructor

        public LintelBeam(Opening openingInfo) : base(FramingTypes.LintelBeam,openingInfo.Level)
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
            GlobalInfo = OpeningInfo.GlobalWallInfo;
            OpeningInfo.PropertyChanged += OpeningInfo_PropertyChanged;
            OpeningInfo.GlobalWallInfo.PropertyChanged += GlobalWallInfo_PropertyChanged;
            PropertyChanged += LintelBeam_PropertyChanged;
            OpeningInfo.GlobalWallInfo.GlobalExtWallDetailInfo.BottomPlate.PropertyChanged += PlatePropertiesChanged;
            OpeningInfo.GlobalWallInfo.GlobalExtWallDetailInfo.TopPlate.PropertyChanged += PlatePropertiesChanged;
            OpeningInfo.GlobalWallInfo.GlobalExtWallDetailInfo.RibbonPlate.PropertyChanged += PlatePropertiesChanged;
            InitializedBeamSupportPoint();
        }

        public LintelBeam(LintelBeam another) : base(another)
        {

        }

        private void LintelBeam_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(EngineerMember):
                    //RaisePropertyChanged(nameof(MaterialType));
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
                //case nameof(MaterialType):
                //    RaisePropertyChanged(nameof(StandardDoorJambSupport));
                //    RaisePropertyChanged(nameof(TimberGrade));
                //    RaisePropertyChanged(nameof(SizeGrade));
                //    break;
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
                RaisePropertyChanged(nameof(ThicknessTbt));
                RaisePropertyChanged(nameof(RealSupportHeight));
            }
        }
        private void OpeningInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Index":
                    RaisePropertyChanged(nameof(Index));
                    RaisePropertyChanged(nameof(Name));
                    break;
                case "Width":
                    RaisePropertyChanged(nameof(StandardDoorJambSupport));
                    RaisePropertyChanged(nameof(FramingSpan));
                    RaisePropertyChanged(nameof(QuoteLength));
                    break;
                case nameof(OpeningInfo.DoorQty):
                    RaisePropertyChanged(nameof(FramingSpan));
                    RaisePropertyChanged(nameof(QuoteLength));
                    break;
                case nameof(OpeningInfo.DoorType):
                    RaisePropertyChanged(nameof(FramingSpan));
                    RaisePropertyChanged(nameof(QuoteLength));
                    break;
                case nameof(OpeningInfo.NoDoor):
                    RaisePropertyChanged(nameof(FramingSpan));
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
       

        public void LoadLintelInfo(LintelBeamPoco lintel,List<EngineerMemberInfo> engineerSchedules)
        {
            foreach (var engineerMemberInfo in engineerSchedules)
            {
                if (engineerMemberInfo.Id == lintel.EngineerMemberInfoId)
                {
                    EngineerMember = engineerMemberInfo;
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
            Index = lintel.Id;
            Name = lintel.Name;
            StandardDoorJambSupport = lintel.StandardDoorJambSupport;
            NoItem = lintel.NoItem;
            Thickness = lintel.Thickness;
            Depth = lintel.Depth;
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



        public override object Clone()
        {
            return new LintelBeam(this);
        }
    }
}
