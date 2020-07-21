using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public class TimberWallMemberBase : BindableBase
    {
        #region Field

        private GlobalWallInfo _globalWallInfo;
        private string _thickness;
        private string _depth;
        private string _noItem = "1";
        private string _timberGrade;
        #endregion
        #region Property
        public GlobalWallInfo GlobalWallInfo { get=>_globalWallInfo; private set=>SetProperty(ref _globalWallInfo,value); }
        public string NoItem
        {
            get => _noItem;
            set
            {
                SetProperty(ref _noItem, value);
                CallBackPropertyChanged();
            } 
        }
        public string Thickness
        {
            get
            {
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
        public string Depth
        {
            get
            {
                if (string.IsNullOrEmpty(_depth))
                {
                    return GlobalWallInfo.Depth.ToString();
                }
                return this._depth;
            } 
            set
            {
                this.SetProperty(ref this._depth, value);
                this.CallBackPropertyChanged();
            }
        }
        public string TimberGrade
        {
            get
            {
                if (string.IsNullOrEmpty(_timberGrade))
                {
                    return GlobalWallInfo.TimberGrade;
                }

                return _timberGrade;
            } 
            set
            {
                if (value == GlobalWallInfo.TimberGrade)
                {
                    value = string.Empty;
                }
                this.SetProperty(ref this._timberGrade, value);
                CallBackPropertyChanged();
            }
        }
        public string Size => this.NoItem == "1" ? this.Thickness + "x" + this.Depth : this.NoItem + "/" + this.Thickness + "x" + this.Depth;
        public string SizeGrade => this.Size + " " + this.TimberGrade;

        #endregion

        #region Constructor

        public TimberWallMemberBase(GlobalWallInfo globalWallInfo)
        {
            this.GlobalWallInfo = globalWallInfo;
            GlobalWallInfo.PropertyChanged += DefaultInfo_PropertyChanged;
        }

        private void DefaultInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName =="Thickness")
            {
                RaisePropertyChanged(nameof(Thickness));
            }

            if (e.PropertyName=="Depth")
            {
                RaisePropertyChanged(nameof(Depth));
            }

            if (e.PropertyName== "TimberGrade")
            {
                RaisePropertyChanged(nameof(TimberGrade));
            }
            CallBackPropertyChanged();
        }


        private void CallBackPropertyChanged()
        {
            this.RaisePropertyChanged(nameof(this.Size));
            this.RaisePropertyChanged(nameof(this.SizeGrade));
           
        }

        #endregion
    }
}
