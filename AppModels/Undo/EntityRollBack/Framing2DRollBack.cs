using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Undo.EntityRollBack
{
    public class Framing2DRollBack: PlannarRollBack
    {
        public Framing2DRollBack(Entity entity) : base(entity)
        {
            if(EntityRollBack is IFraming2D)
            {

            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is IFraming2D)
            {

            }
        }
    }
}
