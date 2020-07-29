using AppModels.Enums;
using AppModels.Interaface;

namespace AppModels.ResponsiveData.WallMemberData
{
    public class WallTopPlate : WallMember
    {
        #region MyRegion

        public override int Thickness { get => WallInfo.WallThickness; set {} }
        public sealed override WallMemberType MemberType { get; protected set; }
        public override IWallMemberInfo BaseMaterialInfo => WallInfo.GlobalWallDetailInfo.TopPlate;

        #endregion

        #region Constructor
        public WallTopPlate(IWallInfo wallInfo) : base(wallInfo)
        {
            MemberType = WallMemberType.TopPlate;
        }


        #endregion




    }
}
