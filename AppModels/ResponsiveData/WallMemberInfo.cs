

using System.ComponentModel;
using AppModels.Enums;
using AppModels.Interaface;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public class WallMemberInfo : BindableBase, IWallMemberInfo
    {
        #region Field

        private int _noItem;
        // ReSharper disable once InconsistentNaming
        protected int _thickness;
        private int _depth;
        private string _timberGrade;

        #endregion
        #region Property
        public WallType WallType { get; set; }
        //public IWallInfo WallInfo { get; set; }
        public IWallMemberInfo BaseMaterialInfo { get; set; }
        public WallMemberType MemberType { get; set; }

        public int NoItem
        {
            get
            {
                if (_noItem==0)
                {
                    return BaseMaterialInfo?.NoItem ?? 0;
                }
                return _noItem;
            }
            set => this.SetProperty(ref _noItem, value);
        }

        public virtual int Thickness
        {
            get
            {
                if (_thickness==0)
                {
                    return BaseMaterialInfo?.Thickness ?? 0;
                }
                return _thickness;
            }
            set => this.SetProperty(ref _thickness, value);
        }

        public int Depth
        {
            get
            {
                if (_depth==0)
                {
                    return BaseMaterialInfo?.Depth ?? 0;
                }
                return _depth;
            }
            set => this.SetProperty(ref _depth, value);
        }

        public string TimberGrade
        {
            get
            {
                if (string.IsNullOrEmpty(_timberGrade))
                {
                    return BaseMaterialInfo == null ? "Nil" : BaseMaterialInfo.TimberGrade;
                }
                return _timberGrade;
            }
            set
            {
                if (value == BaseMaterialInfo.TimberGrade)
                {
                    value = null;
                }

                SetProperty(ref _timberGrade, value);
            }
        }

        

        public string Size
        {
            get
            {
                if (BaseMaterialInfo == null)
                {
                    return "Nil";
                }

                var result = Thickness + "x" + Depth;
                if (NoItem ==1)
                {
                    return result;
                }

                return NoItem + "/" + result;
            }
        }
        public string SizeGrade
        {
            get
            {
                if (BaseMaterialInfo == null)
                    return "Nil";
                return this.Size + " " + this.TimberGrade;

            }
        }

        #endregion

        #region Constructor
        public WallMemberInfo(IWallMemberInfo baseMaterialInfo,WallMemberType memberType)
        {
            MemberType = memberType;
            //WallInfo = wallInfo;
            BaseMaterialInfo = baseMaterialInfo;
            PropertyChanged += WallMember_PropertyChanged;
            //WallInfo.PropertyChanged += WallInfo_PropertyChanged;
            if (BaseMaterialInfo != null)
            {
                BaseMaterialInfo.PropertyChanged += BaseMaterialInfo_PropertyChanged;
            }

        }

        private void WallMember_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BaseMaterialInfo))
            {
             RaisePropertyChanged(nameof(NoItem));
             RaisePropertyChanged(nameof(Thickness));
             RaisePropertyChanged(nameof(Depth));
             RaisePropertyChanged(nameof(TimberGrade));
             RaisePropertyChanged(nameof(Size));
            }
            NotifyPropertyChanged(e.PropertyName);
        }

        private void BaseMaterialInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
            RaisePropertyChanged(nameof(Size));
        }

        //private void WallInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    RaisePropertyChanged(e.PropertyName);
        //}



        #endregion


        #region Protedted Method

        protected void NotifyPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(BaseMaterialInfo):
                    RaisePropertyChanged(nameof(NoItem));
                    RaisePropertyChanged(nameof(Thickness));
                    RaisePropertyChanged(nameof(Depth));
                    RaisePropertyChanged(nameof(TimberGrade));
                    RaisePropertyChanged(nameof(Size));
                    break;
                case nameof(NoItem):
                case nameof(Thickness):
                case nameof(Depth):
                case nameof(TimberGrade):
                    RaisePropertyChanged(propertyName);
                    RaisePropertyChanged(nameof(Size));
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
