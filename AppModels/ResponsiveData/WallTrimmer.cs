using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModels.ResponsiveData
{
    public class WallTrimmer: TimberWallMemberBase
    {
        public WallTrimmer(GlobalWallInfo globalWallInfo, TimberWallMemberBase topPlateInfo) : base(globalWallInfo)
        {
        }
    }
}
