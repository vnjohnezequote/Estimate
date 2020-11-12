using devDept.Eyeshot.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AppModels.Interaface;
using AppModels.ResponsiveData.Openings;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Geometry;
using Prism.Mvvm;

namespace AppModels.CustomEntity
{
    public class Hanger2D: Text,IEntityVmCreateAble
    {
        public Hanger HangerReference { get; set; }
        public Guid Id{ get; set; }
        public Hanger2D(Point3D insPoint, string textString, double height,Hanger hangerRef) : base(insPoint, textString, height)
        {
            Id = Guid.NewGuid();
            HangerReference = hangerRef;
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

        

        
    }
}
