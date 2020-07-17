using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModels.PocoDataModel
{
    public class TimberWallPoco : WallBasePoco
    {
        public double WallLength { get; set; }
        public WallLayerPoco WallLayer { get; set; }
        
    }
}
