using AppModels.Enums;
using AppModels.Interaface;

namespace AppModels.ResponsiveData.WallMemberData
{
    public class WallTopPlate : WallMember
    {
        public override int Thickness { get=>WallInfo.WallThickness; set{} }

        public WallTopPlate(IWallInfo wallInfo, IWallMemberInfo baseMaterialInfo) : base(wallInfo)
        {
            MemberType = WallMemberType.TopPlate;
        }

        public sealed override WallMemberType MemberType { get; protected set; }
        public override IWallMemberInfo BaseMaterialInfo => WallInfo.GlobalWallDetailInfo.TopPlate;
    }
}
