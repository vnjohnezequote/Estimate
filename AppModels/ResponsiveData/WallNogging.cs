using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;

namespace AppModels.ResponsiveData
{
    public class WallNogging : WallMemberInfo
    {
        public WallNogging(IWallMemberInfo baseMaterialInfo) : base(baseMaterialInfo)
        {
        }
    }
}
