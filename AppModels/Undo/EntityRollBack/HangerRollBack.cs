using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo.EntityRollBack
{
    public class HangerRollBack: TextRollBack
    {
        public HangerRollBack(Entity entity) : base(entity)
        {
            if (EntityRollBack is Hanger2D hanger)
            {
                
            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is Hanger2D hanger)
            {

            }

        }
    }
}
