using AppModels.CustomEntity;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAddons.EditingTools
{
    public class ClearFramingReference: ToolBase
    {
        public override Point3D BasePoint { get; protected set; }

        [CommandMethod("Clear Framing2D")]
        public void ClearIFraming2D()
        {
            var i = 0;
            while(i<this.EntitiesManager.Entities.Count)
            {
                var entity = this.EntitiesManager.Entities[i];
                if (entity is IFraming2D framing2D && framing2D.FramingReference == null)
                    this.EntitiesManager.Entities.Remove(entity);
                else if (entity is FramingNameEntity framingName && framingName.FramingReference == null)
                    this.EntitiesManager.Entities.Remove(entity);
                else
                    i++;
            }
            this.EntitiesManager.EntitiesRegen();
        }

    }
}
