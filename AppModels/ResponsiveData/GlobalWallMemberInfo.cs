using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.WallMemberData;
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
        public WallTypePoco WallType => GlobalWallInfo.WallType;
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
                if (BaseMaterialInfo!=null && value == BaseMaterialInfo.NoItem)
                {
                    value = 0;
                }
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
                if (GlobalWallInfo.NoggingMethod == NoggingMethodType.AsGlobal && MemberType == WallMemberType.Nogging)
                {
                    if (BaseMaterialInfo!= null && value == BaseMaterialInfo.Thickness)
                    {
                        value = 0;
                    }
                    else if (value == GlobalWallInfo.WallThickness)
                    {
                        value = 0;
                    }
                }
                else if(value == GlobalWallInfo.WallThickness )
                {
                    value = 0;
                }
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
                if (BaseMaterialInfo!=null && value == BaseMaterialInfo.Depth)
                {
                    value = 0;
                }
                else if (value == GlobalWallInfo.Depth)
                {
                    value = 0;
                }
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
                if (BaseMaterialInfo != null)
                {
                    if (value == BaseMaterialInfo.TimberGrade)
                    {
                        value = string.Empty;
                    }
                } 
                else if (value == GlobalWallInfo.TimberGrade)
                {
                    value = string.Empty;
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
            if (GlobalWallInfo!=null)
            {
                GlobalWallInfo.PropertyChanged += DefaultInfo_PropertyChanged;
            }
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
            if (e.PropertyName == "NoggingMethod")
            {
                RaisePropertyChanged(nameof(Thickness));
            }
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

        public void LoadMemberInfo(WallMemberBasePoco wallMember)
        {
            NoItem = wallMember.NoItem;
            Depth = wallMember.Depth;
            Thickness = wallMember.Thickness;
            TimberGrade = wallMember.TimberGrade;
        }

        #endregion
    }
}
