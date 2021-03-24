using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo.EntityRollBack
{
    public class AngularDimRollBack: DimensionRollBack
    {
        public AngularDimRollBack(Entity entity) : base(entity)
        {
            if (entity is AngularDim angularDim)
            {
                
            }
        }
    }
}
