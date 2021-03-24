using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Undo.EntityRollBack
{
    public class TextRollBack: PlannarRollBack
    {
        private Point3D _insertPoint;
        private Text.alignmentType _alignment;
        private bool _backward;
        private bool _billboard;
        private string _styleName;
        private string _textString;
        private bool _upSideDown;
        private double _widthFactor;
        private double _height;

        public TextRollBack(Entity entity) : base(entity)
        {
            if (entity is Text text)
            {
                _insertPoint = (Point3D) text.InsertionPoint.Clone();
                _alignment = text.Alignment;
                _backward = text.Backward;
                _billboard = text.Billboard;
                _styleName = text.StyleName;
                _textString = text.TextString;
                _upSideDown = text.UpsideDown;
                _widthFactor = text.WidthFactor;
                _height = text.Height;
            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is Text text)
            {
                text.InsertionPoint=_insertPoint;
                text.Alignment = _alignment; 
                text.Backward = _backward ;
                text.Billboard =_billboard;
                text.StyleName = _styleName;
                if (!(text is Dimension))
                {
                    text.TextString = _textString;
                }
                text.UpsideDown = _upSideDown;
                text.WidthFactor=_widthFactor;
                text.Height = _height;
            }
        }
    }
}
