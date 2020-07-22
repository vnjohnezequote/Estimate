using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public class LevelGlobalWallInfo: BindableBase
    {
        #region Field
        private string _thickness;
        private string _depth;
        private string _timberGrade;
        private string _wallSpacing;
        #endregion

        #region Property
        public GlobalWallInfo GlobalInfo { get; set; }
        public NoggingMethodType NoggingMethodType => GlobalInfo.NoggingMethodType;

        public WallType WallType => GlobalInfo.WallType;

        public string Thickness
        {
            get => string.IsNullOrEmpty(_thickness) ? GlobalInfo.Thickness : _thickness;
            set => SetProperty(ref _thickness, value);
        }

        public string Depth
        {
            get => string.IsNullOrEmpty(_depth) ? GlobalInfo.Depth.ToString() : _depth;
            set => SetProperty(ref _thickness, value);
        }
        public string TimberGrade
        {
            get => string.IsNullOrEmpty(_timberGrade) ? GlobalInfo.TimberGrade : _timberGrade;
            set
            {
                if (value == GlobalInfo.TimberGrade)
                {
                    value = null;
                }
                SetProperty(ref _timberGrade, value);
            } 
        }
        public string WallSpacing
        {
            get => string.IsNullOrEmpty(_wallSpacing) ? GlobalInfo.WallSpacing.ToString() : _wallSpacing;
            set
            {
                if (value == GlobalInfo.WallSpacing.ToString())
                {
                    value = null;
                }
                SetProperty(ref _wallSpacing, value);
            }
        }
        #endregion

        public LevelGlobalWallInfo(WallType wallType, GlobalWallInfo globalDefaultInfo)
        {
            GlobalInfo = globalDefaultInfo;
        }

    }
}
