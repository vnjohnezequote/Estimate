using System;
using System.Drawing;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;

namespace AppModels.CustomEntity
{
    [Serializable]
    public class PictureEntity : Picture
    {
        public PictureEntity(Plane pln, Point3D basePoint, double width, double height, Image image) : base(pln, basePoint, width, height, image)
        {
            Lighted = false;
        }

        public PictureEntity(Picture another): base(another)
        {

        }

        protected override void DrawFlatSelected(DrawParams data)
        {
            var currentState = data.RenderContext.CurrentDepthStencilState;
            data.RenderContext.SetState(depthStencilStateType.DepthTestOff);
            base.DrawFlatSelected(data);
            data.RenderContext.SetState(currentState);

        }


        protected override void DrawWireframe(DrawParams data)
        {
            var currentState = data.RenderContext.CurrentDepthStencilState;
            data.RenderContext.SetState(depthStencilStateType.DepthTestOff);
            base.DrawWireframe(data);
            this.DrawFlat(data);
            data.RenderContext.SetState(currentState);

        }


    }
}
