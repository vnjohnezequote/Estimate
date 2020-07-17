using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.ResponsiveData;

namespace AppModels.PocoDataModel
{
    public class WallLayerPoco: WallBasePoco
    {
        public WallTypePoco TimberWallTypePoco { get; set; }
        /// <summary>
        /// Gets or sets the wall thickness.
        /// </summary>
        public double WallThickness { get; set; }
        public LevelWallDefaultInfoPoco DefaultInfo { get; set; }
        public IntegerDimension StudSpacing { get; set; }
        public int IsStepDown { get; set; }
        public int IsRaisedCeiling { get; set; }
        public int SubStud { get; set; }
        public IntegerDimension LastWallHeight { get; set; }
        public double WallLength { get; set; }
        public double ExtraLength { get; set; }
        public double TempLength { get; set; }
    }
}
