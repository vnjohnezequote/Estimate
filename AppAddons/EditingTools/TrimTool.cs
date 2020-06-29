using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Geometry;
using DrawingModule.DrawToolBase;

namespace AppAddons.EditingTools
{
    public class TrimTool : ToolBase
    {
        public override Point3D BasePoint { get; protected set; }
    }
}
