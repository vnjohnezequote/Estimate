using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public class GlobalWallDetailInfo : BindableBase,IGlobalWallDetail
    {
        #region Field
        private IBasicWallInfo _globallWallInfo;
        private IWallMemberInfo _ribbonPlate;
        private IWallMemberInfo _topPlate;
        private IWallMemberInfo _stud;
        private IWallMemberInfo _bottomPlate;
        private IWallMemberInfo _nogging;
        private IWallMemberInfo _trimmer;
        #endregion

        #region Property
        public WallTypePoco WallType {
            get
            {
                if (_globallWallInfo!=null)
                {
                    return _globallWallInfo.WallType;
                }

                return null;

            }
           
        } 
        public NoggingMethodType NoggingMethod => GlobalWallInfo.NoggingMethod;
        public IBasicWallInfo GlobalWallInfo { get; private set; }
        public IWallMemberInfo RibbonPlate { get; private set; }
        public IWallMemberInfo TopPlate { get; private set; }
        public IWallMemberInfo Stud { get;private set; }
        public IWallMemberInfo BottomPlate { get;private set; }
        public IWallMemberInfo Nogging { get;private set; }
        public IWallMemberInfo Trimmer { get;private set; }
        public int WallSpacing => GlobalWallInfo.WallSpacing;

        #endregion

        #region Constructor

        public GlobalWallDetailInfo(IBasicWallInfo globalWallInfo,IWallMemberInfo globalNoggingInfo)
        {
            GlobalWallInfo = globalWallInfo;
            GlobalWallInfo.PropertyChanged += GlobalWallInfo_PropertyChanged;
            if (GlobalWallInfo.WallType.IsLoadBearingWall)
            {
                RibbonPlate = new GlobalWallMemberInfo(globalWallInfo,WallMemberType.RibbonPlate);
            }
            TopPlate = new GlobalWallMemberInfo(globalWallInfo,WallMemberType.TopPlate);
            Stud = new GlobalWallMemberInfo(globalWallInfo,WallMemberType.Stud);
            BottomPlate = new GlobalWallMemberInfo(globalWallInfo,WallMemberType.BottomPlate);
            Nogging = new GlobalWallMemberInfo(globalWallInfo,WallMemberType.Nogging, globalNoggingInfo);
            Trimmer = new GlobalWallMemberInfo(globalWallInfo,WallMemberType.Trimmer, TopPlate);
        }

        private void GlobalWallInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName== "WallSpacing")
            {
                RaisePropertyChanged(nameof(WallSpacing));
            }
        }


        #endregion
    }
}
