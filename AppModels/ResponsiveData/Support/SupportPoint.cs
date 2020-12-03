using System;
using System.Collections.Generic;
using System.Linq;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData.EngineerMember;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Support
{
    public class SupportPoint : BindableBase,ICloneable
    {
        #region Field
        private IBeam _supportMemberInfo;
        private SupportType? _pointSupportType = null;
        //private WallBase _wall;
        private EngineerMemberInfo _engineerMemberInfo;
        #endregion

        public IBeam SupportMemberInfor
        {
            get=>_supportMemberInfo;
            private set
            {
                SetProperty(ref _supportMemberInfo, value);
                if (value!=null)
                {
                    value.PropertyChanged += SupportMemberInfor_PropertyChanged;
                }
            }
        } 
        public LoadPointLocation PointLocation { get; private set; }
        public EngineerMemberInfo EngineerMemberInfo
        {
            get
            {
                if (_engineerMemberInfo!=null)
                {
                    return _engineerMemberInfo;
                }
                
                return _supportMemberInfo.SupportReference ?? null;
            }
            set
            {
                if (_supportMemberInfo.SupportReference !=null)
                {
                    if (_supportMemberInfo.SupportReference == value)
                    {
                        value = null;
                    }
                }
                SetProperty(ref _engineerMemberInfo, value);
                RaisePropertyChanged(nameof(SupportWidth));
                RaisePropertyChanged(nameof(SupportHeight));
            }
        }
        
        public int SupportWidth
        {
            get
            {
                if (PointSupportType == SupportType.Post || PointSupportType == SupportType.SteelPost)
                    return 90;
                if (EngineerMemberInfo == null) return 90;
                if (EngineerMemberInfo.MaterialType==MaterialTypes.Steel)
                {
                    return 90;
                }

                return EngineerMemberInfo.Depth * EngineerMemberInfo.NoItem;

            }
        }

        public int SupportHeight
        {
            get
            {
                if (PointSupportType == SupportType.Jamb)
                {
                    return SupportMemberInfor.RealSupportHeight;
                }
                else
                {
                    return SupportMemberInfor.SupportHeight;
                }

            }
        }

        #region Properties

        public SupportType? PointSupportType
        {
            get
            {
                if (_pointSupportType != null)
                {
                    return (SupportType)_pointSupportType;
                }

                return SupportMemberInfor.PointSupportType;
            }
            set
            {
                if (value == SupportMemberInfor.PointSupportType)
                {
                    value = null;
                }

                SetProperty(ref _pointSupportType, value);
                RaisePropertyChanged(nameof(SupportWidth));
                RaisePropertyChanged(nameof(SupportHeight));
            }
        }

        //public WallBase Wall2D
        //{
        //    get => _wall;
        //    set => SetProperty(ref _wall, value);
        //}
        #endregion

        #region Constructor

        public SupportPoint(IBeam supportMember,LoadPointLocation location)
        {
            PointLocation = location;
            _supportMemberInfo = supportMember;
        }

        public SupportPoint(SupportPoint another)
        {
            PointLocation = another.PointLocation;
            EngineerMemberInfo = another.EngineerMemberInfo;
            PointSupportType = another.PointSupportType;
        }

        private void SupportMemberInfor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(PointSupportType):
                    RaisePropertyChanged(nameof(PointSupportType));
                    RaisePropertyChanged(nameof(SupportWidth));
                    break;
                case "SupportReference":
                    RaisePropertyChanged(nameof(EngineerMemberInfo));
                    break;
                case "RealSupportHeight":
                    RaisePropertyChanged(nameof(SupportHeight));
                    break;
            }
        }

        public void LoadSupportPoint(SupportPointPoco supportPoint,List<EngineerMemberInfo> engineerSchedules)
        {
            PointLocation = supportPoint.PointLocation;
            if (engineerSchedules!=null)
            {
                foreach (var engineerMemberInfo in engineerSchedules.Where(engineerMemberInfo => engineerMemberInfo.Id == supportPoint.EngineerReferenceId))
                {
                    this.EngineerMemberInfo = engineerMemberInfo;
                }
            }
            
            PointSupportType = supportPoint.PointSupportType;
        }
        
        #endregion


        public object Clone()
        {
            return new SupportPoint(this);
        }
    }
}
