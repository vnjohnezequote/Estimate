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
        private bool _toleranceSuppressTralingZeros;
        private bool _toleranceSuppressLeadingZeros;
        private double _scalingForHeight;
        private double _lowerValue;
        private double _upperValue;
        private double _arrowHeadSize;
        private double _linearScale;
        private int _tolerancePrecision;
        private double _textGap;
        private string _dimStyle;
        private colorMethodType _textColorMethod;
        private toleranceType _toleranceMode;
        private double _scaleOverall;
        private elementPositionType _arrowLocation;
        private bool _upsideDown;
        private Color _textColor;
        private elementPositionType _textLocation;
        private Point3D _dimLinePosition;
        private string _textPrefix;
        private string _textOveride;
        private string _textSuffix;
        private linearDimensionUnitsType _linearDimensionUnits;
        private int _precision;
        private bool _suppressLeadingZeros;
        private bool _suppressTraillingZeros;

        public DimensionRollBack(Entity entity) : base(entity)
        {
            if (entity is Dimension dimension)
            {
                _toleranceSuppressTralingZeros = dimension.ToleranceSuppressLeadingZeros;
                _toleranceSuppressLeadingZeros = dimension.ToleranceSuppressTralingZeros;
                _scalingForHeight = dimension.ScalingForHeight;
                _lowerValue = dimension.LowerValue;
                _upperValue = dimension.UpperValue;
                _arrowHeadSize = dimension.ArrowheadSize;
                _linearScale = dimension.LinearScale;
                _tolerancePrecision = dimension.TolerancePrecision;
                _textGap = dimension.TextGap;
                _dimStyle = dimension.DimStyle ;
                _textColorMethod = dimension.TextColorMethod;
                _toleranceMode = dimension.ToleranceMode;
                _scaleOverall = dimension.ScaleOverall;
                _arrowLocation = dimension.ArrowsLocation;
                _upsideDown = dimension.UpsideDown;
                _textColor = dimension.TextColor;
                _textLocation = dimension.TextLocation;
                _dimLinePosition = dimension.DimLinePosition;
                _textPrefix = dimension.TextPrefix;
                _textOveride = dimension.TextOverride;
                _textSuffix = dimension.TextSuffix;
                _linearDimensionUnits = dimension.LinearDimensionUnits;
                _precision = dimension.Precision;
                _suppressLeadingZeros = dimension.SuppressLeadingZeros;
                _suppressTraillingZeros = dimension.SuppressTrailingZeros;
            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is Dimension dimension)
            {
                dimension.ToleranceSuppressLeadingZeros = _toleranceSuppressTralingZeros;
                dimension.ToleranceSuppressTralingZeros = _toleranceSuppressLeadingZeros;
                dimension.ScalingForHeight = _scalingForHeight;
                dimension.LowerValue = _lowerValue;
                dimension.UpperValue = _upperValue;
                dimension.ArrowheadSize = _arrowHeadSize;
                dimension.LinearScale = _linearScale;
                dimension.TolerancePrecision = _tolerancePrecision;
                dimension.TextGap = _textGap;
                dimension.DimStyle = _dimStyle;
                dimension.TextColorMethod = _textColorMethod;
                dimension.ToleranceMode = _toleranceMode;
                dimension.ScaleOverall = _scaleOverall;
                dimension.ArrowsLocation = _arrowLocation;
                dimension.UpsideDown = _upsideDown;
                dimension.TextColor = _textColor;
                dimension.TextLocation = _textLocation;
                dimension.DimLinePosition = _dimLinePosition;
                dimension.TextPrefix = _textPrefix;
                dimension.TextOverride = _textOveride;
                dimension.TextSuffix = _textSuffix;
                dimension.LinearDimensionUnits = _linearDimensionUnits;
                dimension.Precision = _precision;
                dimension.SuppressLeadingZeros = _suppressLeadingZeros;
                dimension.SuppressTrailingZeros = _suppressTraillingZeros;
            }
        }
    }
}
