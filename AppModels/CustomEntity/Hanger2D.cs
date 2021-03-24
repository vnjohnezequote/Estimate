using devDept.Eyeshot.Entities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using AppModels.CustomEntity.CustomEntitySurrogate;
using AppModels.Interaface;
using AppModels.ResponsiveData.Openings;
using AppModels.Undo;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Geometry;
using devDept.Serialization;

namespace AppModels.CustomEntity
{
    public sealed class Hanger2D: Text,IEntityVmCreateAble,IFraming2D,ICloneAbleToUndo
    {
        private IFraming _framingReference;
        public Guid FramingSheetId { get; set; }
        public IFraming2DContaintHangerAndOutTrigger Framing2D { get; set; }
        public IFraming FramingReference
        {
            get => _framingReference;
            set
            {
                _framingReference = value;
                if (_framingReference!=null)
                {
                    _framingReference.PropertyChanged += HangerRefOnPropertyChanged;
                }
            }
        }
        public double FullLength { get; set; }
        public Guid Id{ get; set; }
        public Guid LevelId { get; set; }
        public Guid FramingReferenceId { get; set; }
        
        
        public Hanger2D(Point3D insPoint, string textString, double height,Hanger reference,IFraming2DContaintHangerAndOutTrigger framing2D) : base(insPoint, textString, height)
        {
            Id = Guid.NewGuid();
            LevelId = reference.LevelId;
            FramingSheetId = reference.FramingSheetId;
            FramingReference = reference;
            this.TextString = reference.Name;
            Color = Color.FromArgb(127,127,63);
            Framing2D = framing2D;
        }
        //public Hanger2D(Point3D insPoint, string textString, double height):base(insPoint,textString, height)
        //{
            
        //}

        public Hanger2D(Text another) : base(another)
        {
        }

        public Hanger2D(Hanger2D another) :base(another)
        {
            Id = Guid.NewGuid();
            LevelId = another.LevelId;
            FramingSheetId = another.FramingSheetId;
            FramingReference = (IFraming)another.FramingReference.Clone();
            FramingReferenceId = FramingReference.Id;
            Framing2D = another.Framing2D;
        }
        
        private void HangerRefOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HangerMaterial")
            {
                RegenerationHangeName();
            }
            if (e.PropertyName == nameof(FramingReference.Name))
            {
                TextString = FramingReference.Name;
            }
        }

        private void RegenerationHangeName()
        {
            if (FramingReference!=null && FramingReference.FramingSheet!=null)
            {
               
                Helper.RegenerationHangerName(FramingReference.FramingSheet.Hangers.ToList());
                var items = FramingReference.FramingSheet.Hangers.OrderBy(hanger => hanger.Name);
                var sortedList = items.ToList();
                FramingReference.FramingSheet.Hangers.Clear();
                FramingReference.FramingSheet.Hangers.AddRange(sortedList);
            }
        }
        protected override void Draw(DrawParams data)
        {
            base.Draw(data);
            var radius = Height;
            if (radius > 1e-3)
            {
                var circle = new Circle(Plane.XY, InsertionPoint, radius);
                this.DrawCircle(circle, data);
            }
        }
        private void DrawCircle(ICurve circle, DrawParams data)
        {
            if (circle is CompositeCurve)
            {
                CompositeCurve compositeCurve = circle as CompositeCurve;
                Entity[] explodedCurves = compositeCurve.Explode();
                foreach (Entity ent in explodedCurves)

                    DrawScreenCurve((ICurve)ent, data);
            }
            else
            {
                DrawScreenCurve(circle, data);
            }
        }

        private void DrawScreenCurve(ICurve circle, DrawParams data)
        {
            const int subd = 100;
            Point3D[] pts = new Point3D[subd + 1];

            for (int i = 0; i <= subd; i++)
            {
                pts[i] = circle.PointAt(circle.Domain.ParameterAt((double)i / subd));
            }

            data.RenderContext.DrawLineStrip(pts);
        }
        public IEntityVm CreateEntityVm(IEntitiesManager entitiesManager)
        {
            return new Hanger2DVm(this,entitiesManager);
        }

        public override EntitySurrogate ConvertToSurrogate()
        {
            return new Hanger2DSurrogate(this);
        }

        public override Object Clone()
        {
            return new Hanger2D(this);
        }
    }
}
