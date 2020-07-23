using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;

namespace AppModels.ResponsiveData
{
    public class WallNoggingInfo : WallMemberInfo
    {
        public IWallInfo WallInfo { get; set; }

        public override int Thickness
        {
            get
            {
                if (_thickness !=0)
                    return _thickness;

                return WallInfo.NoggingMethod == NoggingMethodType.AsGlobal ? BaseMaterialInfo.Thickness : WallInfo.WallThickness;
            }
            set => SetProperty(ref _thickness, value);
        }

        public WallNoggingInfo(IWallMemberInfo baseMaterialInfo,IWallInfo wallInfo,WallMemberType memberType) : base(baseMaterialInfo,memberType)
        {
            WallInfo = wallInfo;
        }
    }
}
