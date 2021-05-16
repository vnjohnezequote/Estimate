using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo.EntityRollBack
{
    public class FramingNameRollBack: TextRollBack
    {
        public FramingNameRollBack(Entity entity) : base(entity)
        {
            if (EntityRollBack is FramingNameEntity)
            {

            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is FramingNameEntity)
            {

            }

        }
    }
}
