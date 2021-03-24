using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Undo.EntityRollBack
{
    public class PlannarRollBack: RollBackEntity
    {
        private Plane _plane;
        private float _symbolSize;
        
        public PlannarRollBack(Entity entity) : base(entity)
        {
            if (EntityRollBack is PlanarEntity plannar)
            {
                _plane = (Plane)plannar.Plane.Clone();
                _symbolSize = plannar.SymbolSize;
            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is PlanarEntity plannar)
            {
                plannar.Plane = _plane;
                plannar.SymbolSize = _symbolSize;
            }
        }
    }
}
