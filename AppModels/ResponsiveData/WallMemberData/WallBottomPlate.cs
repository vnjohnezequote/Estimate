using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        public override WallMemberType MemberType { get; protected set; }
        public override IWallMemberInfo BaseMaterialInfo => WallInfo.GlobalWallDetailInfo.BottomPlate;
    }
}
