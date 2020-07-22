﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using GeometryGym.Ifc;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public class GlobalWallInfo : BindableBase
    {
        #region Field
        private WallType _wallType;
        private NoggingMethodType _noggingMethodType;
        private int _wallSpacing;
        private string _thickness;
        private int _depth;
        private string _timberGrade;
        #endregion

        #region Property
        public JobWallDefaultInfo GlobalInfo { get; set; }
        public WallType WallType { get; set; }

        public NoggingMethodType NoggingMethodType => GlobalInfo.GlobalNoggingMethodType;
        public int WallSpacing { get=>_wallSpacing; set=>SetProperty(ref _wallSpacing,value); }

        public string Thickness
        {
            get => string.IsNullOrEmpty(_thickness) ? GlobalInfo.GlobalWallThickness.ToString() : _thickness;
            set=>SetProperty(ref _thickness,value);
        }

        public int Depth { get=>_depth; set=>SetProperty(ref _depth,value); }
        public string TimberGrade { get=>_timberGrade; set=>SetProperty(ref _timberGrade,value); }

        #endregion

        public GlobalWallInfo(WallType wallType,JobWallDefaultInfo globalDefaultInfo)
        {
            GlobalInfo = globalDefaultInfo;
            WallType = wallType;
            WallSpacing = 450;
            Depth = 35;
            TimberGrade = "MGP10";
            GlobalInfo.PropertyChanged += GlobalInfo_PropertyChanged;
        }

        private void GlobalInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName=="GlobalWallThickness")
            {
                this.RaisePropertyChanged(nameof(Thickness));
            }
        }
    }
}