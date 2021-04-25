using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Undo.EntityRollBack
{
    public class LinearDimRollBack: RollBackEntity
    {
        private bool _showExtLine2;
        private bool _showExtLine1;
        private Point3D _extLine2;
        private Point3D _extLine1;
        private arrowheadType _rightArrowhead;
        private arrowheadType _leftArrowhead;
        private double _extLineOffset;
        private double _extLineExt;
        public LinearDimRollBack(Entity entity) : base(entity)
        {
            if(EntityRollBack is LinearDim dimension)
            {
                _showExtLine2 = dimension.ShowExtLine1;
                _showExtLine1 = dimension.ShowExtLine2;
                _extLine2 = dimension.ExtLine2;
                _extLine1 = dimension.ExtLine1;
                _rightArrowhead = dimension.RightArrowhead;
                _leftArrowhead = dimension.LeftArrowhead;
                _extLineOffset = dimension.ExtLineOffset;
                _extLineExt = dimension.ExtLineExt;
            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is LinearDim dimension)
            {
                dimension.ShowExtLine1 = _showExtLine2;
                dimension.ShowExtLine2 = _showExtLine1;
                dimension.ExtLine2 = _extLine2;
                dimension.ExtLine1 = _extLine1;
                dimension.RightArrowhead = _rightArrowhead;
                dimension.LeftArrowhead = _leftArrowhead;
                dimension.ExtLineOffset = _extLineOffset;
                dimension.ExtLineExt = _extLineExt;
            }
        }
    }
}
