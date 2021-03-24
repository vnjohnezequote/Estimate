using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Undo.EntityRollBack
{
    public class DimensionRollBack: TextRollBack
    {
        private double _arrowHeadSize;
        private elementPositionType _arrowLocation;
        private Point3D _dimLinePosition;
        private string _dimStyle;
        private linearDimensionUnitsType _linearDimensionUnits;
        private double _linearScale;
        private double _lowerValue;
        private int _precision;
        private double _scaleOverall;
        private double _scalingForHeight;
        private bool _suppressLeadingZeros;
        private bool _suppressTraillingZeros;
        private Color _textColor;
        private colorMethodType _textColorMethod;
        private double _textGap;
        private elementPositionType _textLocation;
        private string _textOveride;
        private string _textPrefix;

        public DimensionRollBack(Entity entity) : base(entity)
        {
            if (entity is Dimension dimension)
            {
                _arrowHeadSize = dimension.ArrowheadSize;
                _arrowLocation = dimension.ArrowsLocation;
                _dimLinePosition = (Point3D)dimension.DimLinePosition.Clone();
                _dimStyle = dimension.DimStyle;
                _linearDimensionUnits = dimension.LinearDimensionUnits;
                _linearScale = dimension.LinearScale;
                _lowerValue = dimension.LowerValue;
                _precision = dimension.Precision;
                _scaleOverall = dimension.ScaleOverall;
                _scalingForHeight = dimension.ScalingForHeight;
                _suppressLeadingZeros = dimension.SuppressLeadingZeros;
                _suppressTraillingZeros = dimension.SuppressTrailingZeros;
                _textColor = dimension.TextColor;
                _textColorMethod = dimension.TextColorMethod;
                _textGap = dimension.TextGap;
                _textLocation = dimension.TextLocation;
                _textOveride = dimension.TextOverride;
                _textPrefix = dimension.TextPrefix;
            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is Dimension dimension)
            {
                dimension.ArrowheadSize = _arrowHeadSize;
                dimension.ArrowsLocation = _arrowLocation;
                dimension.DimLinePosition = _dimLinePosition;
                dimension.DimStyle = _dimStyle;
                dimension.LinearDimensionUnits = _linearDimensionUnits;
                dimension.LinearScale = _linearScale;
                dimension.LowerValue = _lowerValue;
                dimension.Precision = _precision;
                dimension.ScaleOverall = _scaleOverall;
                dimension.ScalingForHeight = _scalingForHeight;
                dimension.SuppressLeadingZeros = _suppressLeadingZeros;
                dimension.SuppressTrailingZeros = _suppressTraillingZeros;
                dimension.TextColor = _textColor;
                dimension.TextColorMethod = _textColorMethod;
                dimension.TextGap = _textGap;
                dimension.TextLocation = _textLocation;
                dimension.TextOverride = _textOveride;
                dimension.TextPrefix = _textPrefix;
            }
        }
    }
}
