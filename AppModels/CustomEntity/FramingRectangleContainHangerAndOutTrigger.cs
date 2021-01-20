using System;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings;
using devDept.Geometry;

namespace AppModels.CustomEntity
{
    public class FramingRectangleContainHangerAndOutTrigger: FramingRectangle2D,IFraming2DContaintHangerAndOutTrigger
    {
        #region Private

        private bool _outTriggerAFlipped;
        private bool _outTriggerBFlipped;

        #endregion


        #region Properties
        public Guid? HangerAId { get; set; }
        public Guid? HangerBId { get; set; }
        public Hanger2D HangerA { get; set; }
        public Hanger2D HangerB { get; set; }
        public bool IsHangerA { get; set; }
        public bool IsHangerB { get; set; }
        public Guid? OutTriggerAId { get; set; }
        public Guid? OutTriggerBId { get; set; }
        public OutTrigger2D OutTriggerA { get; set; }
        public OutTrigger2D OutTriggerB { get; set; }
        public bool IsOutTriggerA { get; set; }
        public bool IsOutTriggerB { get; set; }
        public bool OutTriggerAFlipped
        {
            get => _outTriggerAFlipped;
            set
            {
                if (this.OutTriggerA == null) return;
                _outTriggerAFlipped = value;

                OutTriggerA?.Flipper(StartPoint, EndPoint);

            }
        }
        public bool OutTriggerBFlipped
        {
            get => _outTriggerBFlipped;
            set
            {
                if (this.OutTriggerB == null) return;
                _outTriggerBFlipped = value;

                OutTriggerB?.Flipper(StartPoint, EndPoint);
            }
        }

        #endregion
        #region Constructor

        public FramingRectangleContainHangerAndOutTrigger(Point3D outerStartPoint, Point3D outerEndPoint, FramingBase framingReference, int thickness = 90, bool flipped = false) : base(outerStartPoint, outerEndPoint, framingReference, thickness, flipped)
        {
        }

        public FramingRectangleContainHangerAndOutTrigger(Point3D outerStartPoint, Point3D outerEndPoint, int thickness, bool flipped) : base(outerStartPoint, outerEndPoint, thickness, flipped)
        {
        }

        protected FramingRectangleContainHangerAndOutTrigger(FramingRectangle2D another) : base(another)
        {
        }

        #endregion


        public void SetFlippedOutriggerA(bool outTriggerAFlipped)
        {
            _outTriggerAFlipped = outTriggerAFlipped;
        }
        public void SetFlippedOutriggerB(bool outTriggerBFlipped)
        {
            _outTriggerBFlipped = outTriggerBFlipped;
        }


        protected override void SetFramingColor(int thickness)
        {
            
        }
    }
}
