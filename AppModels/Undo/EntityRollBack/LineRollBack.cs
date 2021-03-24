using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Undo.EntityRollBack
{
    public class LineRollBack:RollBackEntity
    {
        private Point3D _startPoint;
        private Point3D _endPoint;
        public LineRollBack(Entity entity) : base(entity)
        {
            if (entity is Line line)
            {
                _startPoint = (Point3D)line.StartPoint.Clone();
                _endPoint = (Point3D)line.EndPoint.Clone();
            }
        }

        public override void Undo()
        {
            if (EntityRollBack is Line line)
            {
                line.StartPoint = _startPoint;
                line.EndPoint = _endPoint;
            }
        }
    }
}
