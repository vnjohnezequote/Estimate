using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.ResponsiveData.Openings;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo.EntityRollBack
{
    public class BeamRollBack: BlockRefRollBack
    {
        public BeamRollBack(Entity entity) : base(entity)
        {
            if (entity is BeamEntity beam)
            {
                
            }
        }

        public override void Undo()
        {
            base.Undo();

        }
    }
}
