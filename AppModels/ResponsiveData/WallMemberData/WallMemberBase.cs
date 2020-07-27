using System;
using System.Text.RegularExpressions;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
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
        private string _sizeGrade;
        
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
            set => SetProperty(ref _noItem, value);
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
            set => SetProperty(ref _depth, value);
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
            set
            {

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
                if (value =="Nil")
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
                this.SetProperty(ref _noItem, item);
                this.SetProperty(ref _depth, depth);
                this.SetProperty(ref _timberGrade, grade);
            } 
        }

        #endregion

        #region Constructor

        protected WallMemberBase(IWallInfo wallInfo)
        {
            WallInfo = wallInfo;
            WallInfo.PropertyChanged += WallInfo_PropertyChanged;
            //PropertyChanged += WallMemberBase_PropertyChanged;
        }

        //private int GetItemNumberInString(string value)
        //{
        //    var pattern = @"^\d(?=\/)";
        //    var result = 0;
        //    foreach (var m in Regex.Matches(value, pattern))
        //    {
        //        result = Convert.ToInt32(m);
                
        //    }
        //    return result;
        //}

        

        protected virtual void WallMemberBase_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Size));
            RaisePropertyChanged(nameof(SizeGrade));
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




        #endregion


    }
}
