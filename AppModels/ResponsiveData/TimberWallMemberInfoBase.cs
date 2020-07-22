using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public class TimberWallMemberInfoBase : BindableBase,IWallMemberInfo
    {
        #region Field

        private IGlobalWallInfo _globalWallInfo;
        protected string _thickness;
        protected string _depth;
        protected string _noItem;
        protected string _timberGrade;
        #endregion
        #region Property
        public IGlobalWallInfo GlobalWallInfo { get=>_globalWallInfo; private set=>SetProperty(ref _globalWallInfo,value); }
        public virtual string NoItem
        {
            get => string.IsNullOrEmpty( _noItem) ? "1" : _noItem;

            set
            {
                SetProperty(ref _noItem, value);
                CallBackPropertyChanged();
            } 
        }
        public virtual string Thickness
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
        public virtual string Depth
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
        public virtual string TimberGrade
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

        public TimberWallMemberInfoBase(IGlobalWallInfo globalWallInfo)
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


        protected virtual void CallBackPropertyChanged()
        {
            this.RaisePropertyChanged(nameof(this.Size));
            this.RaisePropertyChanged(nameof(this.SizeGrade));
           
        }

        #endregion
    }
}
