using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AppModels.Enums;
using AppModels.Interaface;
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
        public WallType WallType => _globallWallInfo.WallType;
        public NoggingMethodType NoggingMethod => GlobalWallInfo.NoggingMethod;
        public IBasicWallInfo GlobalWallInfo { get; private set; }
        public IWallMemberInfo RibbonPlate { get; private set; }
        public IWallMemberInfo TopPlate { get; private set; }
        public IWallMemberInfo Stud { get;private set; }
        public IWallMemberInfo BottomPlate { get;private set; }
        public IWallMemberInfo NoggingInfo { get;private set; }
        public IWallMemberInfo Trimmer { get;private set; }

        #endregion

        #region Constructor

        public GlobalWallDetailInfo(IBasicWallInfo globalWallInfo,IWallMemberInfo globalNoggingInfo)
        {
            GlobalWallInfo = globalWallInfo;
            if (GlobalWallInfo.WallType == WallType.LBW)
            {
                RibbonPlate = new GlobalWallMemberInfo(globalWallInfo,WallMemberType.RibbonPlate);
            }
            TopPlate = new GlobalWallMemberInfo(globalWallInfo,WallMemberType.TopPlate);
            Stud = new GlobalWallMemberInfo(globalWallInfo,WallMemberType.Stud);
            BottomPlate = new GlobalWallMemberInfo(globalWallInfo,WallMemberType.BottomPlate);
            NoggingInfo = new GlobalWallMemberInfo(globalWallInfo,WallMemberType.Nogging, globalNoggingInfo);
            Trimmer = new GlobalWallMemberInfo(globalWallInfo,WallMemberType.Trimmer, TopPlate);
        }


        #endregion
    }
}
