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
        //protected override void DrawForSelection(GfxDrawForSelectionParams data)
        //{
        //    //data.RenderContext.DrawLineLoop(this.Vertices);
        //}

        //protected override void DrawForSelection(GfxDrawForSelectionParams data)
        //{

        //}

        //protected override void DrawEntity(RenderContextBase context, object myParams)
        //{

        //}

        //protected override void DrawFlat(DrawParams data)
        //{

        //}

        protected override void DrawFlatSelected(DrawParams data)
        {
            data.RenderContext.SetState(depthStencilStateType.DepthTestEqual);
            this.DrawFlat(data);
        }

        //protected override void DrawOnScreenWireframe(DrawOnScreenWireframeParams myParams)
        //{

        //}

        //protected override void DrawWireEntity(RenderContextBase context, object myParams)
        //{
        //    this.DrawF
        //}
        protected override void DrawWireframe(DrawParams data)
        {
            data.RenderContext.SetState(depthStencilStateType.DepthTestOff);
            base.DrawWireframe(data);
            this.DrawFlat(data);
            
        }
        protected override void DrawWireframeSelected(DrawParams data)
        {
            //data.RenderContext.SetState(depthStencilStateType.DepthTestOff);
            base.DrawWireframeSelected(data);
        }
    }
}
