using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public class GlobalWallDetailInfo : BindableBase
    {
        #region Field
        private GlobalWallInfo _globalWallInfo;
        private TimberWallMemberBase _ribbonPlate;
        private TimberWallMemberBase _topPlate;
        private TimberWallMemberBase _stud;
        private TimberWallMemberBase _bottomPlate;
        private TimberWallMemberBase _nogging;
        private TimberWallMemberBase _trimmer;
        #endregion

        #region Property

        public GlobalWallInfo GlobalWallInfo { get; private set; }
        public TimberWallMemberBase RibbonPlate { get; private set; }
        public TimberWallMemberBase TopPlate { get; private set; }
        public TimberWallMemberBase Stud { get;private set; }
        public TimberWallMemberBase BottomPlate { get;private set; }
        public WallNogging Nogging { get;private set; }
        public WallTrimmer Trimmer { get;private set; }

        #endregion

        #region Constructor

        public GlobalWallDetailInfo(GlobalWallInfo globalWallInfo, TimberWallMemberBase globalNoggingInfo,TimberWallMemberBase globalDoorInfor)
        {
            if (globalWallInfo.WallType==WallType.External_LBW || globalWallInfo.WallType == WallType.Internal_LBW)
            {
                RibbonPlate = new TimberWallMemberBase(globalWallInfo);
            }
            
            TopPlate = new TimberWallMemberBase(globalWallInfo);
            Stud = new TimberWallMemberBase(globalWallInfo);
            BottomPlate = new TimberWallMemberBase(globalWallInfo);
            Nogging = new WallNogging(globalWallInfo,globalNoggingInfo);
            Trimmer = new WallTrimmer(globalWallInfo,TopPlate);
        }
        

        #endregion
    }
}
