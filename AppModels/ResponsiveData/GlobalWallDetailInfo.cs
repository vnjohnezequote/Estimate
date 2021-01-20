using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public class GlobalWallDetailInfo : BindableBase,IGlobalWallDetail
    {
        #region Field
        //private IBasicWallInfo _globallWallInfo;
        //private IWallMemberInfo _ribbonPlate;
        //private IWallMemberInfo _topPlate;
        //private IWallMemberInfo _stud;
        //private IWallMemberInfo _bottomPlate;
        //private IWallMemberInfo _nogging;
        //private IWallMemberInfo _trimmer;
        #endregion

        #region Property
        public WallTypePoco WallType => GlobalWallInfo?.WallType;
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

            if (e.PropertyName == nameof(NoggingMethod))
            {
                RaisePropertyChanged(nameof(NoggingMethod));
            }
        }

        public void LoadWallDetailInfo(GlobalWallDetailPoco globalWallDetailInfo)
        {
            if (GlobalWallInfo.WallType.IsLoadBearingWall)
            {
                RibbonPlate.LoadMemberInfo(globalWallDetailInfo.RibbonPlate);
            }

            TopPlate.LoadMemberInfo(globalWallDetailInfo.TopPlate);
            Stud.LoadMemberInfo(globalWallDetailInfo.Stud);
            BottomPlate.LoadMemberInfo(globalWallDetailInfo.BottomPlate);
            Nogging.LoadMemberInfo(globalWallDetailInfo.Nogging);
            Trimmer.LoadMemberInfo(globalWallDetailInfo.Trimmer);
        }

        #endregion
    }
}
