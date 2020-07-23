using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData;

namespace AppModels.Factories
{
    public class WallMemberFactory
    {
        public static IWallMemberInfo CreateWallMemberInfo(IWallMemberInfo wallMemberInfo,IWallInfo wallInfo)
        {
            switch (wallMemberInfo.MemberType)
            {
                case WallMemberType.BottomPlate:
                case WallMemberType.RibbonPlate:
                case WallMemberType.TopPlate:
                case WallMemberType.Stud:
                case WallMemberType.Trimmer:
                    return new WallMemberInfo(wallMemberInfo, wallMemberInfo.MemberType);
                    break;
                case WallMemberType.Nogging:
                    return new WallNoggingInfo(wallMemberInfo,wallInfo, wallMemberInfo.MemberType);
                default:
                    return new WallMemberInfo(wallMemberInfo, wallMemberInfo.MemberType);
            }
            
        }
    }
}
