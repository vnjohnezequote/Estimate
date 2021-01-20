using System;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.WallMemberData;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.WallMemberData
{
    public abstract class WallMemberBase: BindableBase,IWallMemberDetailInfo
    {
        #region Field

        private int _noItem;
        protected int _thickness;
        private int _depth;
        private string _timberGrade;

        #endregion

        #region Property
        public WallTypePoco WallType => WallInfo.WallType;
        public abstract WallMemberType MemberType { get;protected set; }
        public IWallInfo WallInfo { get; set; }
        public abstract IWallMemberInfo BaseMaterialInfo { get; }
        public int NoItem
        {
            get
            {
                if (_noItem!= 0)
                {
                    return _noItem;
                }

                return BaseMaterialInfo?.NoItem ?? 0;
            }
            set
            {
                if (BaseMaterialInfo == null || value == BaseMaterialInfo.NoItem)
                {
                    value = 0;
                }
                SetProperty(ref _noItem, value);
                RaisePropertyChanged(nameof(SizeGrade));
            } 
        }
        public abstract int Thickness { get; set; }
        public int Depth
        {
            get
            {
                if (_depth != 0)
                {
                    return _depth;
                }

                return BaseMaterialInfo?.Depth ?? 0;
            }
            set
            {
                if (BaseMaterialInfo == null ||value == BaseMaterialInfo.Depth)
                {
                    value = 0;
                }
                SetProperty(ref _depth, value);
                RaisePropertyChanged(nameof(SizeGrade));
            } 
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
                if (BaseMaterialInfo == null || value == BaseMaterialInfo.TimberGrade)
                {
                    value = null;
                }

                SetProperty(ref _timberGrade, value);
                RaisePropertyChanged(nameof(SizeGrade));
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
                if (NoItem == 1)
                {
                    return result;
                }

                return NoItem + "/" + result;
            }
        }
        public virtual string SizeGrade
        {
            get
            {
                if (BaseMaterialInfo == null)
                    return "Nil";
                return this.Size + " " + this.TimberGrade;
                
            }
            set
            {
                var item = 0;
                var depth = 0;
                var grade = "";
                if (value == null || value =="Nil")
                {
                    return;
                }
                var words = value.Split(new char[] {'/', 'x', ' '});
                if (value.Contains("/"))
                {
                    item = Convert.ToInt32(words[0]);
                    depth = Convert.ToInt32(words[2]);
                    grade = words[3];

                }
                else
                {
                    item = 0;
                    depth = Convert.ToInt32(words[1]);
                    grade = words[2];
                }
                NoItem = item;
                Depth = depth;
                TimberGrade = grade;
                
            } 
        }

        #endregion

        #region Constructor

        protected WallMemberBase(IWallInfo wallInfo)
        {
            WallInfo = wallInfo;
            WallInfo.PropertyChanged += WallInfo_PropertyChanged;
            PropertyChanged += WallMemberBase_PropertyChanged;
            if (BaseMaterialInfo!=null)
            {
                BaseMaterialInfo.PropertyChanged += BaseMaterialInfo_PropertyChanged;
            }
        }

        private void BaseMaterialInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(BaseMaterialInfo.Depth):
                    RaisePropertyChanged(nameof(Depth));
                    RaisePropertyChanged(nameof(SizeGrade));
                    break;
                case nameof(BaseMaterialInfo.NoItem):
                    RaisePropertyChanged(nameof(NoItem));
                    RaisePropertyChanged(nameof(SizeGrade));
                    break;
                case nameof(BaseMaterialInfo.TimberGrade):
                    RaisePropertyChanged(nameof(TimberGrade));
                    RaisePropertyChanged(nameof(SizeGrade));
                    break;
            }
        }

        protected virtual void WallMemberBase_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        protected virtual void WallInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(WallType))
            {
                RaisePropertyChanged(nameof(BaseMaterialInfo));
                NoItem = 0;
                Depth = 0;
                TimberGrade = null;
            }

            RaisePropertyChanged(nameof(Thickness));
            RaisePropertyChanged(nameof(Size));
            RaisePropertyChanged(nameof(SizeGrade));
        }

        public virtual void LoadMemberInfo(WallMemberBasePoco wallMember)
        {
            NoItem = wallMember.NoItem;
            Thickness = wallMember.Thickness;
            Depth = wallMember.Depth;
            TimberGrade = wallMember.TimberGrade;
        }


        #endregion


    }
}
