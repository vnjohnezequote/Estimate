// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Opening.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Opening type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData.EngineerMember;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Openings
{
    /// <summary>
    /// The opening.
    /// </summary>
    public class Opening : BindableBase
    {
        private WallBase _wallReference;
        //private string _noOfJambSupport;
        private int _id;
        private int _doorHeaderHeight;
        private WallLocationTypes? _doorTypeLocation;
        private int _width;
        private int _height;
        private string _location;
        private LintelBeam _lintel;
        private OpeningInfo _doorWindowInfo;
        private OpeningType? _openingType = null;
        //private bool _isCavitySlidingDoor=false;
        private DoorTypes _doorType;
        private NumberOfDoors _doorNumberType;
        private int _noDoor;
        private bool _isGarageDoor;
        private int _supportSpan;

        public WallBase WallReference
        {
            get=>_wallReference;
            set
            {
                SetProperty(ref _wallReference, value);
                _wallReference.PropertyChanged += WallReferencePropertyChanged;
                if (this.Lintel!=null)
                {
                    this.Lintel.WallReference = value;

                }
            }
        }
        public OpeningInfo DoorWindowInfo {
            get=>_doorWindowInfo;
            set
            {
                SetProperty(ref _doorWindowInfo, value);
                if (DoorWindowInfo!=null)
                {
                    DoorWindowInfo.PropertyChanged += DoorWindowInfo_PropertyChanged;
                }
                
            }
        }
        public int SupportSpan { get=>_supportSpan; set=>SetProperty(ref _supportSpan,value); }
        public int NoDoor { get=>_noDoor; set=>SetProperty(ref _noDoor,value); }
        public bool IsGarageDoor { get=>_isGarageDoor; set=>SetProperty(ref _isGarageDoor,value); }

        public DoorTypes DoorType
        {
            get => _doorType;
            set => SetProperty(ref _doorType, value);

        }

        public NumberOfDoors DoorNumberType
        {
            get => _doorNumberType;
            set => SetProperty(ref _doorNumberType, value);
        }

        public int DoorQty
        {
            get
            {
                switch (DoorNumberType)
                {
                    case NumberOfDoors.Single:
                        return 1;
                    case NumberOfDoors.Double:
                        return 2;
                    case NumberOfDoors.Triple:
                        return 3;
                    case NumberOfDoors.Quadruple:
                        return 4;
                    default:
                        return 1;
                }
            }
        }

        public WallLocationTypes? DoorTypeLocation
        {
            get => _doorTypeLocation ?? DoorWindowInfo?.DoorTypeLocation;
            set
            {
                if (DoorWindowInfo!=null && DoorWindowInfo.DoorTypeLocation== value)
                {
                    value = null;
                }

                SetProperty(ref _doorTypeLocation, value);
            }
        }

        public IGlobalWallInfo GlobalWallInfo { get; set; }
        public Suppliers Suppliers => GlobalWallInfo.GlobalInfo.Supplier;
        public int Id { get=>_id; set=>SetProperty(ref _id,value); }
        public OpeningType? OpeningType
        {
            get
            {
                if (_openingType != null)
                    return (OpeningType)_openingType;
                return DoorWindowInfo?.DoorType ?? Enums.OpeningType.Door;
            }
            set
            {
                if (DoorWindowInfo!=null && DoorWindowInfo.DoorType == value)
                {
                    value = null;
                }
                SetProperty(ref _openingType, value);
            }
        }
        public LevelWall Level { get; set; }
        public string Location { get=>_location; set=>SetProperty(ref _location,value); }
        //public string FullName { get; set; }
        //public string ShortName { get; set; }
        public int Height
        {
            get
            {
                if (_height != 0) return _height;
                if (DoorWindowInfo!=null)
                {
                    return DoorWindowInfo.Height;
                }

                return DoorTypeLocation==WallLocationTypes.External ? GlobalWallInfo.ExternalDoorHeight : GlobalWallInfo.InternalDoorHeight;
            }
            set
            {
                if (DoorWindowInfo!=null && DoorWindowInfo.Height==value)
                {
                    value = 0;
                }
                SetProperty(ref _height, value);
            }
        }

        public int Width
        {
            get
            {
                if (_width!=0)
                {
                    return _width;
                }

                return DoorWindowInfo?.Width ?? 0;
            }

            set
            {
                if (DoorWindowInfo != null && DoorWindowInfo.Width == value)
                    value = 0;
                SetProperty(ref _width, value);
            } 
        }

        public int DoorHeaderHeight
        {
            get
            {
                if (_doorHeaderHeight!=0)
                {
                    return _doorHeaderHeight;
                }

                if (GlobalWallInfo!=null)
                {
                    return DoorTypeLocation == WallLocationTypes.External ? GlobalWallInfo.ExternalDoorHeight : GlobalWallInfo.InternalDoorHeight;
                }

                return 0;
            }
            set
            {
                if (GlobalWallInfo!=null)
                {
                    if (DoorTypeLocation == WallLocationTypes.External)
                    {
                        if (value==GlobalWallInfo.ExternalDoorHeight)
                        {
                            value = 0;
                        }
                    }
                    else
                    {
                        if (value==GlobalWallInfo.InternalDoorHeight)
                        {
                            value = 0;
                        }
                    }
                }

                SetProperty(ref _doorHeaderHeight, value);
            }
        }
        public int WallThickness => WallReference?.WallThickness ?? 0;
        public LintelBeam Lintel { get=>_lintel; set=>SetProperty(ref _lintel,value); }
        public bool IsDoorUnderLbw => WallReference?.WallType != null && WallReference.WallType.IsLoadBearingWall;

        public Opening(IGlobalWallInfo globalWallInfo)
        {
            this.GlobalWallInfo = globalWallInfo;
            GlobalWallInfo.PropertyChanged += GlobalWallInfo_PropertyChanged;
            PropertyChanged += Opening_PropertyChanged;
            Lintel = new LintelBeam(this);
        }

        private void DoorWindowInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(DoorWindowInfo.Width):
                    RaisePropertyChanged(nameof(Width));
                    break;
                case nameof(DoorWindowInfo.Height):
                    RaisePropertyChanged(nameof(Height));
                    break;
                case nameof(DoorWindowInfo.DoorType):
                    RaisePropertyChanged(nameof(OpeningType));
                    break;
                case nameof(DoorWindowInfo.DoorTypeLocation):
                    RaisePropertyChanged(nameof(DoorTypeLocation));
                    break;
            }
        }

        private void Opening_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(DoorTypeLocation):
                    RaisePropertyChanged(nameof(DoorHeaderHeight));
                    break;
                case nameof(WallReference):
                    RaisePropertyChanged(nameof(IsDoorUnderLbw));
                    RaisePropertyChanged(nameof(WallThickness));
                    break;
                case nameof(DoorWindowInfo):
                    RaisePropertyChanged(nameof(Width));
                    RaisePropertyChanged(nameof(Height));
                    RaisePropertyChanged(nameof(OpeningType));
                    RaisePropertyChanged(nameof(DoorTypeLocation));
                    break;
                case nameof(DoorNumberType):
                    RaisePropertyChanged(nameof(DoorNumberType));
                    break;
            }
        }

        private void GlobalWallInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ExternalDoorHeight":
                case "InternalDoorHeight":
                    RaisePropertyChanged(nameof(DoorHeaderHeight));
                    break;
                case nameof(DoorNumberType):
                    RaisePropertyChanged(nameof(DoorQty));
                    break;
            }
        }

        private void WallReferencePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(WallReference.WallThickness):
                case nameof(WallReference.WallType):
                    RaisePropertyChanged(nameof(WallThickness));
                    RaisePropertyChanged(nameof(IsDoorUnderLbw));
                    break;
            }
        }

        public void LoadOpeningInfo(OpeningPoco openingInfo, List<OpeningInfo> doorSchedules,List<WallBase> walls,List<EngineerMemberInfo> engineerSchedules)
        {
            foreach (var doorSchedule in doorSchedules.Where(doorSchedule => doorSchedule.Id == openingInfo.DoorWindowInfoId))
            {
                this.DoorWindowInfo = doorSchedule;
            }

            foreach (var wallBase in walls.Where(wallBase => openingInfo.WallReferenceId == wallBase.Id))
            {
                WallReference = wallBase;
            }
            Id = openingInfo.Id;
            NoDoor = openingInfo.NoDoor;
            IsGarageDoor = openingInfo.IsGarageDoor;
            DoorType = openingInfo.DoorType;
            DoorNumberType = openingInfo.DoorNumberType;
            DoorTypeLocation = openingInfo.DoorTypeLocation;
            OpeningType = openingInfo.OpeningType;
            Location = openingInfo.Location;
            Height = openingInfo.Height;
            Width = openingInfo.Width;
            SupportSpan = openingInfo.SupportSpan;
            DoorHeaderHeight = openingInfo.DoorHeaderHeight;
            Lintel.LoadLintelInfo(openingInfo.Lintel,engineerSchedules);
        }
    }
}
