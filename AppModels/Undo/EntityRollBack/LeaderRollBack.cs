using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Undo.EntityRollBack
{
    public class LeaderRollBack: RollBackEntity
    {
        private double _scale;
        private bool _showArrowHead;
        private double _arrowHeadSize;
        private Point3D[] _vertices;
        public LeaderRollBack(Entity entity) : base(entity)
        {
            if (EntityRollBack is Leader leader)
            {
                _scale = leader.Scale;
                _showArrowHead = leader.ShowArrowHead;
                _arrowHeadSize = leader.ArrowheadSize;
                _vertices = leader.Vertices.Clone() as Point3D[];
            }

        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is Leader leader)
            {
                leader.Scale = _scale;
                leader.ShowArrowHead = _showArrowHead;
                leader.ArrowheadSize = _arrowHeadSize;
                leader.Vertices = _vertices;
            }

        }
    }
}
