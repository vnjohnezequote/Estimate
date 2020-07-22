

using System.ComponentModel;
using AppModels.Interaface;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public class WallMemberInfo : BindableBase, IWallMemberInfo
    {
        #region Field

        private string _noItem;
        private string _thickness;
        private string _depth;
        private string _timberGrade;

        #endregion
        #region Property
        //public IWallInfo WallInfo { get; set; }
        public IWallMemberInfo BaseMaterialInfo { get; set; }

        public string NoItem
        {
            get
            {
                if (string.IsNullOrEmpty(_noItem))
                {
                    return BaseMaterialInfo == null ? "Nil" : BaseMaterialInfo.NoItem;
                }
                return _noItem;
            }
            set => this.SetProperty(ref _noItem, value);
        }

        public string Thickness
        {
            get
            {
                if (string.IsNullOrEmpty(_thickness))
                {
                    return BaseMaterialInfo == null ? "Nil" : BaseMaterialInfo.Thickness;
                }
                return _thickness;
            }
            set => this.SetProperty(ref _thickness, value);
        }

        public string Depth
        {
            get
            {
                if (string.IsNullOrEmpty(_depth))
                {
                    return BaseMaterialInfo == null ? "Nil" : BaseMaterialInfo.Depth;
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

                var result = Thickness + "x" + Depth + " " + TimberGrade;
                if (NoItem =="1")
                {
                    return result;
                }

                return NoItem + "/" + result;
            }
        }

        #endregion

        #region Constructor
        public WallMemberInfo(IWallMemberInfo baseMaterialInfo)
        {
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
            NotifyPropertyChanged(e.PropertyName);
        }

        private void BaseMaterialInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }

        private void WallInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }



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
