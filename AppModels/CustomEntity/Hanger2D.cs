using devDept.Eyeshot.Entities;
using System;
using System.ComponentModel;
using AppModels.CustomEntity.CustomEntitySurrogate;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.Openings;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Geometry;
using devDept.Serialization;

namespace AppModels.CustomEntity
{
    public sealed class Hanger2D: Text,IEntityVmCreateAble,IFraming2D
    {
        private IFraming _framingReference;
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
        public Guid FramingReferenceId { get; set; }
        
        
        public Hanger2D(Point3D insPoint, string textString, double height,Hanger reference) : base(insPoint, textString, height)
        {
            Id = Guid.NewGuid();
            FramingReference = reference;
            this.TextString = reference.Name;
        }
        public Hanger2D(Point3D insPoint, string textString, double height):base(insPoint,textString, height)
        {
            
        }

        public Hanger2D(Text another) : base(another)
        {

        }
        
        private void HangerRefOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FramingReference.Name))
            {
                TextString = FramingReference.Name;
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


    }
}
