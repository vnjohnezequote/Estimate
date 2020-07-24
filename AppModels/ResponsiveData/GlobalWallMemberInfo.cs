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
    public class GlobalWallMemberInfo : BindableBase, IWallMemberInfo
    {
        #region Field
        protected int _thickness;
        protected int _depth;
        protected int _noItem;
        protected string _timberGrade;
        #endregion
        #region Property
        public IBasicWallInfo GlobalWallInfo { get; private set; }
        public IWallMemberInfo BaseMaterialInfo { get; private set; }
        public WallType WallType => GlobalWallInfo.WallType;
        public virtual int NoItem
        {
            get
            {
                if (_noItem != 0)
                    return _noItem;
                return BaseMaterialInfo?.NoItem ?? 1;
            }

            set
            {
                SetProperty(ref _noItem, value);
                CallBackPropertyChanged();
            }
        }
        public virtual int Thickness
        {
            get
            {
                if (_thickness != 0)
                {
                    return _thickness;
                }
                if (GlobalWallInfo.NoggingMethod == NoggingMethodType.AsGlobal && MemberType == WallMemberType.Nogging)
                {
                    return BaseMaterialInfo?.Thickness ?? GlobalWallInfo.WallThickness;
                }

                return GlobalWallInfo.WallThickness;
            }
            set
            {
                this.SetProperty(ref this._thickness, value);
                this.CallBackPropertyChanged();
            }
        }
        public virtual int Depth
        {
            get
            {
                if (_depth != 0)
                {
                    return this._depth;
                }

                return BaseMaterialInfo?.Depth ?? GlobalWallInfo.Depth;
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
                if (!string.IsNullOrEmpty(_timberGrade))
                {
                    return _timberGrade;
                }

                return BaseMaterialInfo != null ? BaseMaterialInfo.TimberGrade : GlobalWallInfo.TimberGrade;
            }
            set
            {
                if (value == GlobalWallInfo.TimberGrade)
                {
                    value = string.Empty;
                }

                if (BaseMaterialInfo != null)
                {
                    //if (MemberType == WallMemberType.Nogging && value == BaseMaterialInfo.TimberGrade)
                    //{
                    //    value = string.Empty;
                    //}

                    //if (MemberType == WallMemberType.Trimmer && value== BaseMaterialInfo.TimberGrade)
                    //{
                    //    value = string.Empty;
                    //}
                    if (value == BaseMaterialInfo.TimberGrade)
                    {
                        value = string.Empty;
                    }
                }


                this.SetProperty(ref this._timberGrade, value);
                CallBackPropertyChanged();
            }
        }

        public WallMemberType MemberType { get; private set; }
        public string Size => this.NoItem == 1 ? this.Thickness + "x" + this.Depth : this.NoItem + "/" + this.Thickness + "x" + this.Depth;
        public string SizeGrade => this.Size + " " + this.TimberGrade;

        #endregion

        #region Constructor

        public GlobalWallMemberInfo(IBasicWallInfo globalWallInfo, WallMemberType memberType, IWallMemberInfo baseMaterialInfo = null)
        {
            MemberType = memberType;
            this.GlobalWallInfo = globalWallInfo;
            BaseMaterialInfo = baseMaterialInfo;
            GlobalWallInfo.PropertyChanged += DefaultInfo_PropertyChanged;
            if (BaseMaterialInfo != null)
            {
                BaseMaterialInfo.PropertyChanged += BaseMaterialInfo_PropertyChanged;
            }
        }

        private void BaseMaterialInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }

        private void DefaultInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case "WallThickness":
                case "Thickness":
                    RaisePropertyChanged(nameof(Thickness));
                    break;
                case "Depth":
                    RaisePropertyChanged(nameof(Depth));
                    break;
                case "TimberGrade":
                    RaisePropertyChanged(nameof(TimberGrade));
                    break;
                case "NoItem":
                    RaisePropertyChanged(nameof(NoItem));
                    break;
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
