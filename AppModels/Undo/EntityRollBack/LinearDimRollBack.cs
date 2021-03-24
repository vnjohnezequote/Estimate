using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo.EntityRollBack
{
    public class LinearDimRollBack: RollBackEntity
    {
        public LinearDimRollBack(Entity entity) : base(entity)
        {
        }

        public override void Undo()
        {
            base.Undo();
        }
    }
}
