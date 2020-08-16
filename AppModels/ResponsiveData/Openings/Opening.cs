// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Opening.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Opening type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using AppModels.Enums;
using AppModels.Interaface;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Openings
{
    /// <summary>
    /// The opening.
    /// </summary>
    public class Opening : BindableBase
    {
        private WallBase _wallReference;
        private string _noOfJambSupport;
        private int _id;
        private int _doorHeaderHeight;
        private WallLocationTypes _doorTypeLocation;
        private int _width;
        private int _height;
        private string _location;
        private LintelBeam _lintel;
        private OpeningInfo _doorWindowInfo;
        private OpeningType? _openingType = null;

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
        public int NoDoor { get; set; }
        public bool IsGarageDoor { get; set; }
        public bool IsCavitySlidingDoor { get; set; }
        public WallLocationTypes DoorTypeLocation { get=>_doorTypeLocation; set=>SetProperty(ref _doorTypeLocation,value); }
        public IGlobalWallInfo GlobalWallInfo { get; set; }
        public Suppliers Suppliers => GlobalWallInfo.GlobalInfo.Supplier;
        public int Id { get=>_id; set=>SetProperty(ref _id,value); }

        public OpeningType OpeningType
        {
            get
            {
                if (_openingType != null)
                    return (OpeningType)_openingType;
                if (DoorWindowInfo != null)
                    return DoorWindowInfo.DoorType;
                return OpeningType.Door;
            }
            set
            {
                SetProperty(ref _openingType, value);
            }
        }

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
            set=>SetProperty(ref _height,value);
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
            
            set => SetProperty(ref _width, value);
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
            }
        }

        private void GlobalWallInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ExternalDoorHeight" || e.PropertyName == "InternalDoorHeight")
            {
                RaisePropertyChanged(nameof(DoorHeaderHeight));
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
    }
}
