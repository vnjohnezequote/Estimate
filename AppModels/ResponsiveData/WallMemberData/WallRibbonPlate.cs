using AppModels.Enums;
using AppModels.Interaface;

namespace AppModels.ResponsiveData.WallMemberData
{
    public class WallRibbonPlate: WallMember
    {
        public override int Thickness { get=>WallInfo.WallThickness; set{} }

        public WallRibbonPlate(IWallInfo wallInfo) : base(wallInfo)
        {
            MemberType = WallMemberType.RibbonPlate;
        }

        public override WallMemberType MemberType { get; protected set; }
        public override IWallMemberInfo BaseMaterialInfo => WallInfo.GlobalWallDetailInfo.TopPlate;
    }
}
