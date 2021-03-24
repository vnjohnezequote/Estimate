using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;

namespace AppAddons.EditingTools
{
    public class UndoTool: ToolBase
    {
        public override Point3D BasePoint { get; protected set; }

        [CommandMethod("Undo")]
        public void Undo()
        {
            if (this.UndoEngineer!=null)
            {
                this.UndoEngineer.Undo();
            }
        }
    }
}
