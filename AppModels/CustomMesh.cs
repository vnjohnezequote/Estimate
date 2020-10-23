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

namespace AppModels
{
    public class CustomMesh: Mesh
    {
        public CustomMesh()
        {

        }
        public override void Compile(CompileParams data)
        {
            base.Compile(data);
            data.RenderContext.Compile(this.drawEdgesData, new DrawEntityCallBack(this.DrawEdgesTest), null);
        }

        public static CustomMesh CreateMeshTest(IList<Point3D> points, Mesh.natureType meshNature)
        {
            return Mesh.CreatePlanar<CustomMesh>(points, null, meshNature);
        }
        private void DrawEdgesTest(RenderContextBase renderContextBase_0, object object_1)
        {
            renderContextBase_0.SetColorWireframe(Color.Brown);
            renderContextBase_0.DrawLineLoop(this.Vertices);
        }
    }
}
