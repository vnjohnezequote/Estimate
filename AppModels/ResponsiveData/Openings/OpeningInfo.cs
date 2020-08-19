﻿using System;
using AppModels.Enums;
using AppModels.Interaface;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Openings
{
    public class OpeningInfo: BindableBase
    {
        private string _name;
        private int _width;
        private int _height;
        private OpeningType _doorType;
        private WallLocationTypes _doorTypeLocation;

        public IGlobalWallInfo GlobalWallInfo { get; set; }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        public int Height
        {
            get
            {
                if (_height != 0) return _height;

                return DoorTypeLocation == WallLocationTypes.External ? GlobalWallInfo.ExternalDoorHeight : GlobalWallInfo.InternalDoorHeight;
            }
            set => SetProperty(ref _height, value);
        }
        public WallLocationTypes DoorTypeLocation { get => _doorTypeLocation; set => SetProperty(ref _doorTypeLocation, value); }

        public OpeningType DoorType
        {
            get => _doorType;
            set => SetProperty(ref _doorType, value);
        }

        public OpeningInfo(IGlobalWallInfo globalWallInfo)
        {
            GlobalWallInfo = globalWallInfo;
            PropertyChanged += OpeningInfo_PropertyChanged;
            GlobalWallInfo.PropertyChanged += GlobalWallInfo_PropertyChanged;
        }

        private void GlobalWallInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(GlobalWallInfo.ExternalDoorHeight):
                case nameof(GlobalWallInfo.InternalDoorHeight):
                    RaisePropertyChanged(nameof(Height)); 
                    break;
            }
        }

        private void OpeningInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(DoorTypeLocation):
                    RaisePropertyChanged(nameof(Height));
                    break;
            }
        }
    }
}