using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity.CustomEntitySurrogate;
using AppModels.Interaface;
using AppModels.ResponsiveData.Openings;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Serialization;

namespace AppModels.CustomEntity
{
    [Serializable]
    public class DoorCountEntity: Text, IEntityVmCreateAble
    {
        private Opening _doorReference;
        private string _levelName;

        public string LevelName
        {
            get => _levelName;
            set
            {
                OldLevelName = _levelName;
                _levelName = value;
            }
        }

        public string OldLevelName { get; private set; }
        public string DoorName { get; set; }
        public int DoorId { get; set; }

        public Opening DoorReference
        {
            get => _doorReference;
            set
            {
                if (_doorReference!=null)
                {
                    _doorReference.PropertyChanged -= DoorReference_PropertyChanged;
                }
                _doorReference = value;
                DoorReferenceId = value.Id;
                if (_doorReference!=null)
                {
                    _doorReference.PropertyChanged += DoorReference_PropertyChanged;
                }
            }
        }
        public int DoorReferenceId { get; set; }
        private void DoorReference_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(DoorReference.WallReference))
            {
                if (DoorReference.WallReference!=null)
                {
                    this.LevelName = DoorReference.WallReference.LevelName;
                    if (DoorReference.WallReference.WallColorLayer != null)
                    {
                        DoorReference.WallReference.PropertyChanged += WallReference_PropertyChanged;
                        this.LayerName = DoorReference.WallReference.WallColorLayer.Name;
                    }
                }
            }
        }

        private void WallReference_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }

        public DoorCountEntity(Text anotherText) : base(anotherText)
        {

        }

        public DoorCountEntity(DoorCountEntity another) : base(another)
        {
            this.LevelName = another.LevelName;
            this.DoorReferenceId = another.DoorReferenceId;
        }
        public DoorCountEntity(Point3D insPoint, string textString, double height, string levelName="", alignmentType alignment = alignmentType.MiddleCenter) : base(insPoint, textString, height, alignment)
        {
            if (!string.IsNullOrEmpty(levelName))
            {
                LevelName = levelName;
            }
        }
        protected override void Draw(DrawParams data)
        {
            base.Draw(data);
            var radius = Height*1.5 ;
            if (radius>1e-3)
            {
                var circle = new Circle(Plane.XY, InsertionPoint,radius);
                this.DrawCircle(circle,data);
            }
        }
        private void DrawCircle(ICurve circle, DrawParams data)
        {
            if (circle is CompositeCurve)
            {
                CompositeCurve compositeCurve = circle as CompositeCurve;
                Entity[] explodedCurves = compositeCurve.Explode();
                foreach (Entity ent in explodedCurves)

                    DrawScreenCurve((ICurve)ent,data);
            }
            else
            {
                DrawScreenCurve(circle,data);
            }
        }

        private void DrawScreenCurve(ICurve circle,DrawParams data)
        {

            const int subd = 100;

            Point3D[] pts = new Point3D[subd + 1];

            for (int i = 0; i <= subd; i++)
            {
                pts[i] = circle.PointAt(circle.Domain.ParameterAt((double)i / subd));
            }

            data.RenderContext.DrawLineStrip(pts);
        }

        public IEntityVm CreateEntityVm()
        {
            return new DoorCountEntityVm(this);
        }

        public override EntitySurrogate ConvertToSurrogate()
        {
            return new DoorCountEntitySurrogate(this);
        }
    }
}
