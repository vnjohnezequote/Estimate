using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.WallMemberData;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Openings
{
    public class SupportPoint : BindableBase
    {
        #region Field
        private IBeam _supportMemberInfo;
        private SupportType? _pointSupportType = null;
        private IWallInfo _wall;
        private EngineerMemberInfo _engineerMemberInfo;
        #endregion

        public IBeam SupportMemberInfor => _supportMemberInfo;
        public LoadPointLocation PointLocation { get;}
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
        public int SupportHeight { get; set; }
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
            }
        }

        public IWallInfo Wall
        {
            get => _wall;
            set => SetProperty(ref _wall, value);
        }
        #endregion

        #region Constructor

        public SupportPoint(IBeam supportMember,LoadPointLocation location)
        {
            PointLocation = location;
            _supportMemberInfo = supportMember;
            PropertyChanged += LoadPointSupport_PropertyChanged;
            SupportMemberInfor.PropertyChanged += SupportMemberInfor_PropertyChanged;
        }

        private void LoadPointSupport_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(PointSupportType):
                case nameof(EngineerMemberInfo):
                    RaisePropertyChanged(nameof(SupportWidth));
                    break;
            }

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
            }
        }
        #endregion


    }
}
