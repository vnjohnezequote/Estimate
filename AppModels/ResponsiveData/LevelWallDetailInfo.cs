using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public class LevelWallDetailInfo: BindableBase
    {
        #region Field
        private LevelGlobalWallInfo _globalWallInfo;
        private TimberWallMemberInfoBase _ribbonPlate;
        private TimberWallMemberInfoBase _topPlate;
        private TimberWallMemberInfoBase _stud;
        private TimberWallMemberInfoBase _bottomPlate;
        private TimberWallMemberInfoBase _nogging;
        private TimberWallMemberInfoBase _trimmer;
        #endregion

        #region Property

        public LevelGlobalWallInfo GlobalWallInfo { get; private set; }
        public TimberWallMemberInfoBase RibbonPlate { get; private set; }
        public TimberWallMemberInfoBase TopPlate { get; private set; }
        public TimberWallMemberInfoBase Stud { get; private set; }
        public TimberWallMemberInfoBase BottomPlate { get; private set; }
        public WallNogging Nogging { get; private set; }
        public WallMemberInfo Trimmer { get; private set; }

        #endregion

        #region Constructor

        public LevelWallDetailInfo(GlobalWallInfo globalWallInfo, TimberWallMemberInfoBase globalNoggingInfo, TimberWallMemberInfoBase globalDoorInfor)
        {
            if (globalWallInfo.WallType == WallType.External_LBW || globalWallInfo.WallType == WallType.Internal_LBW)
            {
                RibbonPlate = new TimberWallMemberInfoBase(globalWallInfo);
            }
            TopPlate = new TimberWallMemberInfoBase(globalWallInfo);
            Stud = new TimberWallMemberInfoBase(globalWallInfo);
            BottomPlate = new TimberWallMemberInfoBase(globalWallInfo);
            Nogging = new WallNogging(globalWallInfo, globalNoggingInfo);
            Trimmer = new WallMemberInfo(globalWallInfo, TopPlate);

        }


        #endregion
    }
}
