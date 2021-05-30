using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Undo.EntityRollBack
{
    public abstract class FramingRectangle2DRollBack: Framing2DRollBack
    {
        private Point3D _outerStartPoint;
        private Point3D _outerEndPoint;
        private IFraming _framingReference;
        private Guid _framingReferenceId;
        private int _thickness;
        private bool _isShowLeader;
        private FramingNameEntity _framingName;
        
        private Point3D _framingNamePoint;
        private bool _flipped;
        
        
        public FramingRectangle2DRollBack(Entity entity) : base(entity)
        {
            if (EntityRollBack is FramingRectangle2D framingRectangle2D)
            {
                _outerStartPoint = framingRectangle2D.OuterStartPoint.Clone() as Point3D;
                _outerEndPoint = framingRectangle2D.OuterEndPoint.Clone() as Point3D;
                _framingReference = framingRectangle2D.FramingReference;
                _thickness = framingRectangle2D.Thickness;
                _isShowLeader = framingRectangle2D.IsShowLeader;
                _framingName = framingRectangle2D.FramingName;
                _flipped = framingRectangle2D.Flipped = _flipped;
                _framingReferenceId = framingRectangle2D.FramingReferenceId;
                if (framingRectangle2D.FramingName != null)
                {
                    _framingNamePoint = framingRectangle2D.FramingName.InsertionPoint;
                }
            }
        }
        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is FramingRectangle2D framingRectangle2D)
            {
                framingRectangle2D.ResetPoint(_outerStartPoint, _outerEndPoint);
                framingRectangle2D.FramingReference = _framingReference;
                framingRectangle2D.Thickness = _thickness;
                framingRectangle2D.IsShowLeader = _isShowLeader;
                framingRectangle2D.FramingName = _framingName;
                framingRectangle2D.Flipped = _flipped;
                framingRectangle2D.FramingReferenceId = _framingReferenceId;

                if (framingRectangle2D.FramingName != null)
                {
                    framingRectangle2D.FramingName.InsertionPoint = _framingNamePoint;
                }
            }
        }
    }
}
