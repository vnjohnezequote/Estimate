using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Undo.EntityRollBack
{
    public class Framing2DRollBack: PlannarRollBack
    {
        private Point3D _outerStartPoint;
        private Point3D _outerEndPoint;

        public Framing2DRollBack(Entity entity) : base(entity)
        {
            if (EntityRollBack is FramingRectangle2D framingRectangle2D)
            {
                _outerStartPoint = framingRectangle2D.OuterStartPoint;
                _outerEndPoint = framingRectangle2D.OuterEndPoint;
            }
            
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is FramingRectangle2D framingRectangle2D)
            {
                framingRectangle2D.OuterStartPoint = _outerStartPoint;
                framingRectangle2D.OuterEndPoint = _outerEndPoint;
            }
            
        }
    }
}
