using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.WallMemberData;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Openings
{
    public class LoadPointSupport : BindableBase
    {
        #region Field

        private IBeam _supportMemberInfo;
        private SupportType? _pointSupportType = null;
        private IWallInfo _wall;
        private ITimberInfo _timberInfo;
        #endregion

        public IBeam SupportMemberInfor => _supportMemberInfo;

        public ITimberInfo TimberInfo
        {
            get=>_timberInfo;
            set=>SetProperty(ref _timberInfo,value);
        }
        #region Properties

        public SupportType? PoinSupportType
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

        public LoadPointSupport(IBeam supportMember)
        {
            _supportMemberInfo = supportMember;
            PropertyChanged += LoadPointSupport_PropertyChanged;
            SupportMemberInfor.PropertyChanged += SupportMemberInfor_PropertyChanged;
        }

        private void LoadPointSupport_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Wall))
            {
                if (PoinSupportType == SupportType.Jamb)
                {
                    
                }
                else
                {
                    return;
                }
            }

        }

        private void SupportMemberInfor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PoinSupportType))
            {
                RaisePropertyChanged(nameof(PoinSupportType));
            }

        }
        #endregion


    }
}
