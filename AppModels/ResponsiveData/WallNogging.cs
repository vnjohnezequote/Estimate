using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;

namespace AppModels.ResponsiveData
{
    public class WallNogging : TimberWallMemberBase
    {
        public TimberWallMemberBase GlobalNoggingInfo { get; set; }
        public override string Thickness
        {
            get
            {
                if (GlobalWallInfo.GlobalInfo.GlobalNoggingMethodType == NoggingMethodType.AsGlobal)
                {
                    return GlobalNoggingInfo.Thickness;
                }
                if (string.IsNullOrEmpty(_thickness))
                {
                    return GlobalWallInfo.Thickness.ToString();
                }
                return this._thickness;
            }
            set
            {
                this.SetProperty(ref this._thickness, value);
                this.CallBackPropertyChanged();
            }
        }
        public override string Depth
        {
            get
            {
                if (string.IsNullOrEmpty(_depth))
                {
                    return GlobalNoggingInfo.Depth;
                }
                return this._depth;
            }
            set
            {
                this.SetProperty(ref this._depth, value);
                this.CallBackPropertyChanged();
            }
        }

        public override string TimberGrade
        {
            get
            {
                if (string.IsNullOrEmpty(_timberGrade))
                {
                    return GlobalWallInfo.GlobalInfo.GlobalNoggingInfo.TimberGrade;
                }

                return _timberGrade;
            }
            set
            {
                if (value == GlobalWallInfo.GlobalInfo.GlobalNoggingInfo.TimberGrade)
                {
                    value = string.Empty;
                }
                this.SetProperty(ref this._timberGrade, value);
                CallBackPropertyChanged();
            }
        }
        public WallNogging(GlobalWallInfo globalWallInfo, TimberWallMemberBase globalNoggingInfo) : base(globalWallInfo)
        {
            this.GlobalNoggingInfo = globalNoggingInfo;
            GlobalWallInfo.GlobalInfo.PropertyChanged += GlobalWallInfo_PropertyChanged;
            GlobalNoggingInfo.PropertyChanged += GlobalNoggingInfo_PropertyChanged;
        }

        private void GlobalNoggingInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Thickness")
            {
                RaisePropertyChanged(nameof(Thickness));
            }
            if (e.PropertyName == "Depth")
            {
                RaisePropertyChanged(nameof(Depth));
            }
            if (e.PropertyName == "TimberGrade")
            {
                RaisePropertyChanged(nameof(TimberGrade));
            }
        }

        private void GlobalWallInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName== "GlobalNoggingMethodType")
            {
                RaisePropertyChanged(nameof(Thickness));
            }
        }
    }
}
