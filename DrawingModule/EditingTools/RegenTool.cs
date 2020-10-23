using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;

namespace DrawingModule.EditingTools
{
    public class RegenTool: ToolBase
    {
        public override Point3D BasePoint { get; protected set; }

        [CommandMethod("Regen")]
        public void Regen()
        {
            this.EntitiesManager.Refresh();
        }
    }
}
