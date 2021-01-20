using AppModels.Enums;
using AppModels.Interaface;

namespace AppModels.ResponsiveData.WallMemberData
{
    public class WallBottomPlate: WallMember
    {
        public override int Thickness
        {
            get => WallInfo.WallThickness;
            set
            {
                
            }
        }

        public WallBottomPlate(IWallInfo wallInfo) : base(wallInfo)
        {
            MemberType = WallMemberType.BottomPlate;
        }

        public override WallMemberType MemberType { get; protected set; }
        public override IWallMemberInfo BaseMaterialInfo => WallInfo.GlobalWallDetailInfo.BottomPlate;
    }
}
