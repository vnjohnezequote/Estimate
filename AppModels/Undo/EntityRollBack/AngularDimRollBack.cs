using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Undo.EntityRollBack
{
    public class AngularDimRollBack: DimensionRollBack
    {
        private double _extLineExt;
        private double _extLineOffset;
        private bool _showExtLine1;
        private bool _showExtLine2;
        private arrowheadType _leftArrowhead;
        private Point3D _extLine1;
        private Point3D _origin;
        private angleFormatType _angleFormat;
        private Point3D _extLine2;
        private arrowheadType _rightArrowhead;
        public AngularDimRollBack(Entity entity) : base(entity)
        {
            if (entity is AngularDim angularDim)
            {
                _extLineExt = angularDim.ExtLineExt;
                _extLineOffset = angularDim.ExtLineOffset;
                _showExtLine1 = angularDim.ShowExtLine1;
                _showExtLine2 = angularDim.ShowExtLine2;
                _leftArrowhead = angularDim.LeftArrowhead;
                _extLine1 = angularDim.ExtLine1;
                _origin = angularDim.Origin;
                _angleFormat = angularDim.AngleFormat ;
                _extLine2 = angularDim.ExtLine2;
                _rightArrowhead = angularDim.RightArrowhead;
            }
        }
        public override void Undo()
        {
            base.Undo();
            if(EntityRollBack is AngularDim angularDim)
            {
                angularDim.ExtLineExt = _extLineExt;
                angularDim.ExtLineOffset = _extLineOffset;
                angularDim.ShowExtLine1 = _showExtLine1;
                angularDim.ShowExtLine2 = _showExtLine2;
                angularDim.LeftArrowhead = _leftArrowhead;
                angularDim.ExtLine1 = _extLine1;
                angularDim.Origin = _origin;
                angularDim.AngleFormat = _angleFormat;
                angularDim.ExtLine2 = _extLine2;
                angularDim.RightArrowhead = _rightArrowhead;
            }
        }
    }
}
