using System.Drawing;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.CustomEntity
{
    public class CustomVectorView : VectorView
    {
        public bool KeepEntityLineWeight { get; set; }
        public bool KeepEntityLineType { get; set; }
        public new Point3D WindowCenter { get; private set; }
        public new RectangleF Window { get; private set; }
        public new double Width { get; private set; }
        public new double Height { get; private set; }

        public viewType MyViewtype{ get; set; }

        public CustomVectorView(double x, double y, viewType standardView, double scale, string name, double width = 0, double height = 0) : base(x, y, standardView, scale, name, width, height)
        {
            MyViewtype = standardView;
        }
        internal bool CheckViewTypeIsCustomView()
        {
            return this.MyViewtype != viewType.Other;
        }

        
        internal bool CheckViewTypeIs2DView()
        {
            return this.MyViewtype == viewType.Right || this.MyViewtype == viewType.Bottom || this.MyViewtype == viewType.Left || this.MyViewtype == viewType.Top || this.MyViewtype == viewType.Front || this.MyViewtype == viewType.Rear;
        }
        internal void SetWindowCenter(Point3D windowCenterPoint)
        {
            this. WindowCenter = windowCenterPoint;
        }
        internal void SetWindowRectangle(RectangleF windowRectangle)
        {
            this.Window  = windowRectangle;
        }
        internal void SetWidth(double width)
        {
            this.Width  = width;
        }
        internal void SetHeight(double height)
        {
            this.Height = height;
        }
    }
}