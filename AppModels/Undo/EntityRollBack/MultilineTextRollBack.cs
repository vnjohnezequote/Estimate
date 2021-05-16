using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Undo.EntityRollBack
{
    public class MultilineTextRollBack: TextRollBack
    {
        private double _lineSpaceDistance;
        private Text.alignmentType _alignment;
        private double[] _widthFactors;
        private double _rectHeight;
        private double _rectWidth;
        private bool _wrap;
        private string _contents;
        private  Point3D _insertionPoint;
        public MultilineTextRollBack(Entity entity) : base(entity)
        {
            if (EntityRollBack is MultilineText mtext)
            {
                _lineSpaceDistance = mtext.LineSpaceDistance;
                _alignment = mtext.Alignment;
                _widthFactors = mtext.WidthFactors;
                _rectHeight = mtext.RectHeight;
                _rectWidth = mtext.RectWidth;
                _wrap = mtext.Wrap;
                _contents = mtext.Contents;
                _insertionPoint = mtext.InsertionPoint.Clone() as Point3D;

            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is MultilineText mtext)
            {
                mtext.LineSpaceDistance = _lineSpaceDistance;
                mtext.Alignment = _alignment;
                mtext.WidthFactors = _widthFactors;
                mtext.RectHeight = _rectHeight;
                mtext.RectWidth = _rectWidth;
                mtext.Wrap = _wrap;
                mtext.Contents = _contents;
                mtext.InsertionPoint = _insertionPoint.Clone() as Point3D;
            }

        }
    }
}
