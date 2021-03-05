using System;
using System.Collections.Generic;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ResponsiveData.Openings;
using devDept.Eyeshot.Entities;
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
        public Guid? FramingNameId { get; set; }
        public bool IsShowFramingName { get; set; }
        public FramingNameEntity FramingName { get; set; }

        public List<FramingRectangleContainHangerAndOutTrigger> ConectedFramings =
            new List<FramingRectangleContainHangerAndOutTrigger>();
        #endregion
        #region Constructor

        public FramingRectangleContainHangerAndOutTrigger(Point3D outerStartPoint, Point3D outerEndPoint,IFraming framingReference, int thickness = 90, bool flipped = false,bool centerCreater = false) : base(outerStartPoint, outerEndPoint, framingReference, thickness, flipped,centerCreater)
        {
        }

        public FramingRectangleContainHangerAndOutTrigger(Point3D outerStartPoint, Point3D outerEndPoint, int thickness, bool flipped,bool centerCreater = false) : base(outerStartPoint, outerEndPoint, thickness, flipped,centerCreater)
        {
        }

        protected FramingRectangleContainHangerAndOutTrigger(FramingRectangleContainHangerAndOutTrigger another) : base(another)
        {
            if (another.IsHangerA)
            {
                var hangerA = (Hanger2D)another.HangerA.Clone();
                hangerA.Framing2D = this;
                HangerAId = hangerA.Id;
                this.HangerA = hangerA;
                this.IsHangerA = true;
                ((IContaintHanger) FramingReference).HangerA = (Hanger)hangerA.FramingReference;
            }
            if (another.IsHangerB)
            {
                var hangerB = (Hanger2D) another.HangerB.Clone();
                hangerB.Framing2D = this;
                HangerBId = hangerB.Id;
                this.HangerB = hangerB;
                this.IsHangerB = true;
                ((IContaintHanger) FramingReference).HangerB = (Hanger)hangerB.FramingReference;
            }
            IsShowFramingName = another.IsShowFramingName;
            if (another.FramingName != null)
            {
                FramingName = (FramingNameEntity)another.FramingName.Clone();
                FramingName.FramingReference = this.FramingReference;
                FramingReferenceId = this.FramingReference.Id;
            }

            if (another.IsOutTriggerA)
            {
                OutTriggerA = (OutTrigger2D)another.OutTriggerA.Clone();
                OutTriggerA.Framing2D = this;
                ((OutTrigger) OutTriggerA.FramingReference).Parrent = this.FramingReference;
                OutTriggerAId = OutTriggerA.Id;
                IsOutTriggerA = true;
                ((IContaintOutTrigger) FramingReference).OutTriggerA = (OutTrigger) OutTriggerA.FramingReference;
            }

            if (another.IsOutTriggerB)
            {
                OutTriggerB = (OutTrigger2D)another.OutTriggerB.Clone();
                OutTriggerB.Framing2D = this;
                ((OutTrigger)OutTriggerB.FramingReference).Parrent = this.FramingReference;
                OutTriggerBId = OutTriggerB.Id;
                IsOutTriggerB = true;
                ((IContaintOutTrigger)FramingReference).OutTriggerB = (OutTrigger)OutTriggerB.FramingReference;
            }
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

        protected override void HangerACenterPointChanged()
        {
            if (HangerA!=null)
            {
                HangerA.InsertionPoint = HangerACenterPoint;
            }
        }

        protected override void HangerBCenterPointChanged()
        {
            if (HangerB!=null)
            {
                HangerB.InsertionPoint = HangerBCenterPoint;
            }
        }

        public override void Translate(double dx, double dy, double dz = 0)
        {
            base.Translate(dx, dy, dz);
            FramingName?.Translate(dx,dy,dz);
            OutTriggerA?.Translate(dx,dy,dz);
            OutTriggerB?.Translate(dx,dy,dz);
        }

        public override IRectangleSolid Offset(double amount, Vector3D planeNormal, double tolerance, bool sharp)
        {
            var framing = (IRectangleSolid)Clone();
            Line framingLine = new Line(framing.OuterStartPoint, framing.OuterEndPoint);
            Vector3D vector3D = Vector3D.Cross(framingLine.Tangent, planeNormal);
            vector3D.Normalize();
            Vector3D v = vector3D * amount;
            framingLine.Translate(v);
            framing.OuterStartPoint = framingLine.StartPoint;
            framing.OuterEndPoint = framingLine.EndPoint;
            if (framing is IFraming2DContaintHangerAndOutTrigger framingContaintFraming)
            {
                if (framingContaintFraming.FramingName!=null)
                {
                    framingContaintFraming.FramingName.Translate(v);
                }

                if (framingContaintFraming.OutTriggerA!=null)
                {
                    framingContaintFraming.OutTriggerA.Translate(v);
                }

                if (framingContaintFraming.OutTriggerB != null)
                {
                    framingContaintFraming.OutTriggerB.Translate(v);
                }    

            }

            

            return framing;
        }

    }
}
