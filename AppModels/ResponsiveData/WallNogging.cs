using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Interaface;
using GeometryGym.Ifc;

namespace AppModels.ResponsiveData
{
    public class WallNogging: WallMemberBase
    {
        public int Length { get; set; }
        public WallNogging(IWallInfo wallInfo, IWallMemberInfo baseMaterialInfo) : base(wallInfo, baseMaterialInfo)
        {
        }
    }
}
