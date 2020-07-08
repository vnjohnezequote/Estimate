using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;

namespace DrawingModule.Entities
{
    public class PictureEntity : Picture
    {
        public PictureEntity(Plane pln, Point3D basePoint, double width, double height, Image image) : base(pln, basePoint, width, height, image)
        {
            Lighted = false;
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

        //protected override void DrawFlatSelected(DrawParams data)
        //{
        //   this.DrawFlat(data);
        //}

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
            base.DrawWireframeSelected(data);
        }
    }
}
