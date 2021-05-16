using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Undo.EntityRollBack
{
    public class LinearPathRollBack: RollBackEntity
    {
        private double _globalWidth;
        private Point3D[] _vertices;
        public LinearPathRollBack(Entity entity) : base(entity)
        {
            if (entity is LinearPath line)
            {
                _globalWidth = line.GlobalWidth;
                _vertices = line.Vertices.Clone() as Point3D[];
            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is LinearPath linearPath)
            {
                linearPath.GlobalWidth = _globalWidth;
                linearPath.Vertices = _vertices;
            }
        }
    }
}
