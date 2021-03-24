using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo.EntityRollBack
{
    public class Beam2DRollBack: FramingRectangleContainHangerAndOutTriggerRollBack
    {
        public Beam2DRollBack(Entity entity) : base(entity)
        {
        }
    }
}
