using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using Environment = devDept.Eyeshot.Environment;

namespace AppModels.CustomEntity
{
    public class WallRegion: Text
    {
        public Point3D StartPoint { get; set; }
        public Point3D EndPoint { get; set; }
        public int WallThickness { get; set; }
        public bool IsLoadBearingWall { get; set; }
        public bool ShowWallDimension { get; set; }
        public Segment2D CenterLine { get; set; }
        public Segment2D WallLine1 { get; set; }
        public Segment2D WallLine2 { get; set; }
        public Segment2D DimensionLine { get; set; }
        public Point3D StartPoint1 { get; set; }
        public Point3D StartPoint2 { get; set; }
        public Point3D EndPoint1 { get; set; }
        public Point3D EndPoint2 { get; set; }
        public bool ShowStartWallLine { get; set; }
        public bool ShowEndWallLine { get; set; }

        public Segment2D WallStartLine { get; set; }
        public Segment2D WallEndLine { get; set; }


        public WallRegion(Plane wallPlan, Point3D insPoint, Point3D startPoint, Point3D endPoint,
            int wallThickness = 90, bool isLoadBearingWall = true, bool showDimension = false, double textHeight = 90) :
            base(wallPlan, insPoint, textHeight, Text.alignmentType.BaselineCenter)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            WallThickness = wallThickness;
            IsLoadBearingWall = isLoadBearingWall;
            ShowWallDimension = showDimension;
            CenterLine = new Segment2D(startPoint, endPoint);
            ShowStartWallLine = true;
            ShowEndWallLine = true;
            InitializerWallLine(startPoint, endPoint);
            this.LineTypeScale = 10;
            this.InsertionPoint = new Point3D(DimensionLine.MidPoint.X,DimensionLine.MidPoint.Y, 0);
            var distace = StartPoint.DistanceTo(EndPoint);
            this.TextString = ((int)(StartPoint.DistanceTo(EndPoint)-WallThickness)).ToString();
        }
        private void InitializerWallLine(Point3D startPoint, Point3D endPoint)
        {
            //var offsetLine = CenterLine.Offset(WallThickness / 2, Vector3D.AxisZ);
            var offsetLine = CenterLine.Offset((double)WallThickness / 2);
            offsetLine.ExtendBy((double)WallThickness/2,(double)WallThickness/2);
            WallLine1 = offsetLine;
            offsetLine = CenterLine.Offset(-(double) WallThickness / 2);
            offsetLine.ExtendBy((double)WallThickness / 2, (double)WallThickness/2);
            WallLine2 = offsetLine;
            StartPoint1 = WallLine1.P0.ConvertPoint2DtoPoint3D();
            EndPoint1 = WallLine1.P1.ConvertPoint2DtoPoint3D();
            StartPoint2 = WallLine2.P0.ConvertPoint2DtoPoint3D();
            EndPoint2 = WallLine2.P1.ConvertPoint2DtoPoint3D();
            offsetLine = WallLine2.Offset(-(double)WallThickness/2 );
            DimensionLine = offsetLine;
            //Utility.OffsetPoint(endPoint, startPoint, 45, out var startPoint1);
            //WallLine1 = new Line(startPoint1, endPoint1);
            //offsetLine = CenterLine.Offset(-WallThickness / 2, Vector3D.AxisZ);
            //Utility.OffsetPoint(offsetLine.StartPoint, offsetLine.EndPoint, WallThickness / 2, out var endPoint2);
            //Utility.OffsetPoint(endPoint, startPoint, 45, out var startPoint2);
            //WallLine2 = new Line(startPoint2, endPoint2);
            //StartWallLine = new Line(WallLine1.StartPoint, WallLine2.StartPoint);
            //EndWallLine = new Line(WallLine1.EndPoint, WallLine1.EndPoint);
        }
        protected override void Draw(DrawParams data)
        {
            //base.Draw(data);
            
            var points = new Point3D[]{StartPoint1,EndPoint1,EndPoint2,StartPoint2};
            data.RenderContext.SetState(depthStencilStateType.DepthTestAlways);
            data.RenderContext.Draw(this.drawData,primitiveType.Undefined);
            data.RenderContext.PushRasterizerState();
            data.RenderContext.SetRasterizerState(rasterizerPolygonDrawingType.Fill, rasterizerCullFaceType.None);
            data.RenderContext.PushModelView();
            //data.RenderContext.SetColorWireframe(Color.Chartreuse,true);
            //data.RenderContext.SetMaterialFrontAmbient(Color.CadetBlue,false);
            data.RenderContext.DrawQuads(points, new Vector3D[]
            {
                Vector3D.AxisZ
            });
            data.RenderContext.PopModelView();
            data.RenderContext.PopRasterizerState();
            if (ShowWallDimension)
            {
                this.DrawText(data);
            }
            
            data.RenderContext.SetLineSize(2);
            //data.RenderContext.SetColorWireframe(Color.Crimson);
            
            if (!this.IsLoadBearingWall)
            {
                if (ShowStartWallLine)
                {
                    data.RenderContext.SetColorWireframe(Color.Crimson);
                    data.RenderContext.DrawBufferedLine(StartPoint1,StartPoint2);
                }

                if (ShowEndWallLine)
                {
                    data.RenderContext.SetColorWireframe(Color.Crimson);
                    data.RenderContext.DrawBufferedLine(EndPoint1, EndPoint2);
                }
                data.RenderContext.SetColorWireframe(Color.Crimson);
                this.DrawDashWallLine(data,StartPoint1,EndPoint1);
                data.RenderContext.SetColorWireframe(Color.LimeGreen);
                this.DrawDashWallLine(data, StartPoint2, EndPoint2);

            }
            else
            {
                if (ShowStartWallLine)
                {
                    data.RenderContext.SetColorWireframe(Color.Crimson);
                    data.RenderContext.DrawBufferedLine(StartPoint1,EndPoint1);
                }

                if (ShowEndWallLine)
                {
                    data.RenderContext.SetColorWireframe(Color.Crimson);
                    data.RenderContext.DrawBufferedLine(EndPoint1,EndPoint2);
                }
                data.RenderContext.SetColorWireframe(Color.Crimson);
                data.RenderContext.DrawBufferedLine(StartPoint1,EndPoint1);
                data.RenderContext.SetColorWireframe(Color.LimeGreen);
                data.RenderContext.DrawBufferedLine(StartPoint2,EndPoint2);
            }
            //data.RenderContext.DrawTriangles2D(points);
        }
        private void DrawDashWallLine(DrawParams data, Point3D startPoint, Point3D endPoint)
        {
            var attributes = data.Attributes;
            if (!string.IsNullOrEmpty((attributes != null) ? attributes.LineTypeName : null))
            {
                var lineTypePattern = data.LineTypes[attributes.LineTypeName];
                if (lineTypePattern.Length * this.LineTypeScale > data.ScreenToWorld4Times && data.ScreenToWorld > 0f)
                {
                    float num = 1f;
                    if (data.FullParents != null && data.FullParents.Count > 0)
                    {
                        num = (float)data.FullParents.Peek().GetFullTransformation(data.Blocks).ScaleFactorX;
                    }

                    List<Point3D> list;
                    List<Point3D> list2;
                    var vertices = new Point3D[] { startPoint, endPoint };
                    lineTypePattern.GetPatternVertices(data.MaxPatternRepetitions, vertices, this.LineTypeScale / num, out list, out list2);
                    for (int i = 0; i < list.Count; i += 2)
                    {
                        data.RenderContext.DrawBufferedLine(list[i], list[i + 1]);
                    }
                    data.RenderContext.DrawPointsOnTheFly(list2.ToArray());
                }
            }
        }
       
    }
}
