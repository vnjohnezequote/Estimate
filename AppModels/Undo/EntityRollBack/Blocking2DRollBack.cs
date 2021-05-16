using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo.EntityRollBack
{
    public class Blocking2DRollBack: TextRollBack
    {
        public Blocking2DRollBack(Entity entity) : base(entity)
        {
            if (EntityRollBack is Blocking2D blocking)
            {
                
            }
        }
    }
}
