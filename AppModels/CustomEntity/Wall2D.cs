using System.Collections.Generic;
using System.Drawing;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;

namespace AppModels.CustomEntity
{
    public class Wall2D: Text, IEntityVmCreateAble
    {
        private Point3D _startPoint1;
        private Point3D _startPoint2;
        private Point3D _endPoint1;
        private Point3D _endPoint2;
        private int _wallThickness;
        private bool _isLoadBearingWall;
        private bool _showDimension;
        private Point3D _startPoint;
        private Point3D _endPoint;
        //private bool _showStartWallLine;
        //private bool _showEndWallLine;

        public Point3D StartPoint
        {
            get => _startPoint;
            set
            {
                _startPoint = value;
                this.RegenMode = regenType.RegenAndCompile;
            }
        }

        public Point3D EndPoint
        {
            get => _endPoint;
            set
            {
                _endPoint = value;
                this.RegenMode = regenType.RegenAndCompile;
            }
        }

        public int WallThickness
        {
            get => _wallThickness;
            set
            {
                _wallThickness = value;
                this.RegenEntityGeometry(StartPoint,EndPoint);
                this.RegenMode = regenType.RegenAndCompile;
            }
        }

        public bool IsLoadBearingWall
        {
            get => _isLoadBearingWall;
            set
            {
                _isLoadBearingWall = value;
                this.RegenMode = regenType.RegenAndCompile;
            }
        }

        public bool ShowWallDimension
        {
            get => _showDimension;
            set
            {
                _showDimension = value;
                this.RegenMode = regenType.RegenAndCompile;
            }
        }

        public Segment2D CenterLine { get; set; }
        public Segment2D WallLine1 { get; set; }
        public Segment2D WallLine2 { get; set; }
        public Segment2D DimensionLine { get; set; }

        public List<Point3D> WallBoxVerticesStart{ get;set; }
        public List<Point3D> WallBoxVerticesEnd { get; set; }

        public Point3D StartPoint1
        {
            get => _startPoint1;
            set
            {
                _startPoint1 = value;
                this.RegenMode = regenType.RegenAndCompile;
            }
        }

        public Point3D StartPoint2
        {
            get => _startPoint2;
            set
            {
                _startPoint2 = value;
                this.RegenMode = regenType.RegenAndCompile;
            }
        }

        public Point3D EndPoint1
        {
            get => _endPoint1;
            set
            {
                _endPoint1 = value;
                this.RegenMode = regenType.RegenAndCompile;
            }
        }

        public Point3D EndPoint2
        {
            get => _endPoint2;
            set
            {
                _endPoint2 = value;
                this.RegenMode = regenType.RegenAndCompile;
            }

        }

        public bool ShowStartWallLine { get; set; }
        public bool ShowEndWallLine { get; set; }

        public Segment2D WallStartLine { get; set; }
        public Segment2D WallEndLine { get; set; }
        public List<Point3D> WallVertices { get; set; } = new List<Point3D>();
        public List<Point3D> CenterlineVertices { get; set; } = new List<Point3D>();
        //public EntityGraphicsData DrawEdgesData { get; set; } = new EntityGraphicsData();


        public Wall2D(Plane wallPlan, Point3D startPoint, Point3D endPoint,
            int wallThickness = 90, bool isLoadBearingWall = true,bool ccMode = true, bool showDimension = false, double textHeight = 90) :
            base(wallPlan, startPoint, textHeight, Text.alignmentType.BaselineCenter)
        {
            _wallThickness = wallThickness;
            _isLoadBearingWall = isLoadBearingWall;
            _showDimension = showDimension;
            if (ccMode)
            {
                _startPoint = startPoint;
                _endPoint = endPoint;
                CenterlineVertices = new List<Point3D>() { StartPoint, EndPoint };
                CenterLine = new Segment2D(startPoint, endPoint);
                ShowStartWallLine = true;
                ShowEndWallLine = true;
                RegenEntityGeometry(startPoint, endPoint);
            }
            else
            {
                var wallLine = new Segment2D(startPoint, endPoint);
                var centerLine = wallLine.Offset((double) WallThickness / 2);
                centerLine.ExtendBy(-(double)WallThickness/2, -(double)WallThickness / 2);
                CenterLine = centerLine;
                CenterlineVertices = new List<Point3D>(){ CenterLine.P0.ConvertPoint2DtoPoint3D(), CenterLine.P1.ConvertPoint2DtoPoint3D() };
                _startPoint = CenterLine.P0.ConvertPoint2DtoPoint3D();
                _endPoint = CenterLine.P1.ConvertPoint2DtoPoint3D();
                ShowStartWallLine = true;
                ShowEndWallLine = true;
                RegenEntityGeometry(CenterLine.P0.ConvertPoint2DtoPoint3D(),CenterLine.P1.ConvertPoint2DtoPoint3D());
            }
            //this.LineTypeScale = 10;

            //this.InsertionPoint = new Point3D(DimensionLine.MidPoint.X,DimensionLine.MidPoint.Y, 0);
            //var distace = StartPoint.DistanceTo(EndPoint);
            //this.TextString = ((int)(StartPoint.DistanceTo(EndPoint)-Thickness)).ToString();
        }
        private void RegenEntityGeometry(Point3D startPoint, Point3D endPoint)
        {
            //var offsetLine = CenterLine.Offset(Thickness / 2, Vector3D.AxisZ);
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
            
            var segment1 = new Segment2D((Point3D)StartPoint1.Clone(),(Point3D)EndPoint1.Clone());
            segment1.ExtendBy(-WallThickness,-WallThickness);
            var segment2 = new Segment2D((Point3D)StartPoint2.Clone(),(Point3D)EndPoint2.Clone());
            segment2.ExtendBy(-WallThickness,-WallThickness);
            var p1 = segment1.P0;
            var p2 = segment2.P0;
            var p3 = segment1.P1;
            var p4 = segment2.P1;
            
            WallBoxVerticesStart = new List<Point3D>(){ StartPoint1, p1.ConvertPoint2DtoPoint3D(), p2.ConvertPoint2DtoPoint3D(), StartPoint2 };
            WallBoxVerticesEnd = new List<Point3D>(){EndPoint1,EndPoint2,p4.ConvertPoint2DtoPoint3D(),p3.ConvertPoint2DtoPoint3D()};

            var tempLine = new Segment2D(StartPoint1, StartPoint2);
            WallBoxVerticesStart.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(p1, p2);
            WallBoxVerticesStart.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(StartPoint1, p1);
            WallBoxVerticesStart.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(StartPoint2, p2);
            WallBoxVerticesStart.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(EndPoint1, EndPoint2);
            WallBoxVerticesEnd.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(p3, p4);
            WallBoxVerticesEnd.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(EndPoint1, p3);
            WallBoxVerticesEnd.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(EndPoint2, p4);
            WallBoxVerticesEnd.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
        }
        protected override void Draw(DrawParams data)
        {
            //base.Draw(data);
            var points = new Point3D[]{StartPoint1,EndPoint1,EndPoint2,StartPoint2}; 
            //data.RenderContext.SetState(depthStencilStateType.DepthTestOff);
            data.RenderContext.PushRasterizerState();
            data.RenderContext.SetRasterizerState(rasterizerPolygonDrawingType.Fill, rasterizerCullFaceType.None);
            data.RenderContext.PushModelView();

            data.RenderContext.DrawQuads(points, new Vector3D[]
                {
                    Vector3D.AxisZ
                });
                //data.RenderContext.Draw(this.drawData, primitiveType.Undefined);
            data.RenderContext.PopModelView();
            data.RenderContext.PopRasterizerState();
            if (ShowWallDimension)
            {
                this.DrawText(data);
            }
            //DrawWallBorder(data);
            //this.DrawEdges(data);
            
        }
        private void DrawDashWallLine(DrawParams data, Point3D startPoint, Point3D endPoint)
        {
            //var attributes = data.Attributes;
            //if (!string.IsNullOrEmpty((attributes != null) ? attributes.LineTypeName : null))
            //{
            //    var lineTypePattern = data.LineTypes[attributes.LineTypeName];
            //    if (lineTypePattern.Length * this.LineTypeScale > data.ScreenToWorld4Times && data.ScreenToWorld > 0f)
            //    {
            //        float num = 1f;
            //        if (data.FullParents != null && data.FullParents.Count > 0)
            //        {
            //            num = (float)data.FullParents.Peek().GetFullTransformation(data.Blocks).ScaleFactorX;
            //        }

            //        List<Point3D> list;
            //        List<Point3D> list2;
            //        var vertices = new Point3D[] { startPoint, endPoint };
            //        lineTypePattern.GetPatternVertices(data.MaxPatternRepetitions, vertices, this.LineTypeScale / num, out list, out list2);
            //        for (int i = 0; i < list.Count; i += 2)
            //        {
            //            data.RenderContext.DrawBufferedLine(list[i], list[i + 1]);
            //        }
            //        data.RenderContext.DrawPointsOnTheFly(list2.ToArray());
            //    }
            //}
            var lineTypePattername = this.LineTypeName;
            var lineTypePattern = data.LineTypes[lineTypePattername];
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

        

        protected override void DrawEdges(DrawParams data)
        {
            base.DrawEdges(data);
            //var currentState = data.RenderContext.CurrentDepthStencilState;
            //data.RenderContext.PushDepthStencilState();
            //var currentState = data.RenderContext.CurrentDepthStencilState;
            //data.RenderContext.SetState(depthStencilStateType.DepthMaskFalse_DepthTestGreater);
            
            data.RenderContext.SetLineSize(4);
            data.RenderContext.SetColorWireframe(Color.Black);
            //data.RenderContext.SetColorWireframe(Color.Crimson);
            data.RenderContext.PushModelView();
            if (!this.IsLoadBearingWall)
            {
                if (ShowStartWallLine)
                {
                    //data.RenderContext.SetColorWireframe(Color.Crimson);
                    data.RenderContext.DrawBufferedLine(StartPoint1, StartPoint2);
                    //data.RenderContext.DrawLine(OuterStartPoint, InnerStartPoint);
                }

                if (ShowEndWallLine)
                {
                  //  data.RenderContext.SetColorWireframe(Color.Crimson);
                    data.RenderContext.DrawBufferedLine(EndPoint1, EndPoint2);
                    //data.RenderContext.DrawLine(OuterEndPoint, InnerEndPoint);
                }

                //data.RenderContext.SetColorWireframe(Color.Crimson, true);
                this.DrawDashWallLine(data, StartPoint1, EndPoint1);
                //data.RenderContext.SetColorWireframe(Color.Crimson);
                this.DrawDashWallLine(data, StartPoint2, EndPoint2);
                //data.RenderContext.DrawLine(WallBoxVerticesStart[1], WallBoxVerticesStart[2]);
                //data.RenderContext.DrawLine(WallBoxVerticesEnd[2], WallBoxVerticesEnd[3]);
                //data.RenderContext.SetColorWireframe(Color.Crimson);
                data.RenderContext.DrawBufferedLine(WallBoxVerticesStart[1], WallBoxVerticesStart[2]);
                data.RenderContext.DrawBufferedLine(WallBoxVerticesEnd[2], WallBoxVerticesEnd[3]);

            }
            else
            {
                if (ShowStartWallLine)
                {
                    //data.RenderContext.SetColorWireframe(Color.Crimson);
                    data.RenderContext.DrawBufferedLine(StartPoint1, StartPoint2);
                }

                if (ShowEndWallLine)
                {
                  //  data.RenderContext.SetColorWireframe(Color.Crimson);
                    data.RenderContext.DrawBufferedLine(EndPoint1, EndPoint2);
                }
                //data.RenderContext.SetColorWireframe(Color.Crimson);
                data.RenderContext.DrawBufferedLine(StartPoint1, EndPoint1);
                //data.RenderContext.SetColorWireframe(Color.Crimson);
                data.RenderContext.DrawBufferedLine(StartPoint2, EndPoint2);
                //data.RenderContext.SetColorWireframe(Color.Crimson);
                data.RenderContext.DrawBufferedLine(WallBoxVerticesStart[1], WallBoxVerticesStart[2]);
                data.RenderContext.DrawBufferedLine(WallBoxVerticesEnd[2], WallBoxVerticesEnd[3]);
            }
            data.RenderContext.PopModelView();
            
            //data.RenderContext.PopDepthStencilState();
            //data.RenderContext.SetState(currentState);
        }

       

        public override void Regen(RegenParams data)
        {
            var distance = (int)StartPoint.DistanceTo(EndPoint)-WallThickness;
            this.TextString = distance.ToString();
            this.InsertionPoint = new Point3D(DimensionLine.MidPoint.X, DimensionLine.MidPoint.Y, 0);
            base.Regen(data);
            var listPoint = new List<Point3D>(Vertices);
            if (!listPoint.Contains(this.StartPoint))
            {
                listPoint.Add(StartPoint);
            }
            if (!listPoint.Contains(this.EndPoint))
            {
                listPoint.Add(EndPoint);
            }

            if (!listPoint.Contains(StartPoint1))
            {
                listPoint.Add(StartPoint1);
            }
            if (!listPoint.Contains(StartPoint2))
            {
                listPoint.Add(StartPoint2);
            }
            if (!listPoint.Contains(EndPoint1))
            {
                listPoint.Add(EndPoint1);
            }
            if (!listPoint.Contains(EndPoint2))
            {
                listPoint.Add(EndPoint2);
            }
            Vertices = listPoint.ToArray();
            WallVertices.Clear();
            WallVertices.Add(StartPoint1);
            WallVertices.Add(StartPoint2);
            WallVertices.Add(EndPoint2);
            WallVertices.Add(EndPoint1);
            base.UpdateBoundingBox(data);
            this.RegenMode = regenType.CompileOnly;
        }

        public WallIntersectionType GetInterSection(Wall2D other,out Point3D intersectionPoint,out LinearPath centerLinePath )
        {
            intersectionPoint = null;
            centerLinePath = null;
            foreach (var otherCenterlineVertex in other.CenterlineVertices)
            {
                if (this.CenterlineVertices.Contains(otherCenterlineVertex))
                {
                    //var points = new List<Point3D>(){ this.StartPoint, EndPoint, other.EndPoint};
                    //centerLinePath = new LinearPath(points.ToArray());
                    intersectionPoint = otherCenterlineVertex;
                    if (intersectionPoint == this.StartPoint && intersectionPoint == other.StartPoint)
                    {
                        if (intersectionPoint == other.StartPoint)
                        {
                            var points = new List<Point3D>() { this.EndPoint, intersectionPoint, other.EndPoint };
                            centerLinePath = new LinearPath(points.ToArray());
                        }
                        else
                        {
                            var points = new List<Point3D>() { this.EndPoint, intersectionPoint, other.StartPoint };
                            centerLinePath = new LinearPath(points.ToArray());
                        }
                    }
                    else if(intersectionPoint == this.EndPoint )
                    {
                        if (intersectionPoint== other.StartPoint)
                        {
                            var points = new List<Point3D>() { this.StartPoint, intersectionPoint, other.EndPoint };
                            centerLinePath = new LinearPath(points.ToArray());
                        }
                        else
                        {
                            var points = new List<Point3D>() { this.StartPoint, intersectionPoint, other.StartPoint };
                            centerLinePath = new LinearPath(points.ToArray());
                        }
                    } 
                    return WallIntersectionType.AtVertice;
                }
            }
            return WallIntersectionType.None;
        }

        public bool SplitWallAt(Wall2D otherWall,out List<Wall2D> regions)
        {
            switch (Segment2D.Intersection(CenterLine,otherWall.CenterLine, out var point1, out var point2,0.001))
            {
                case segmentIntersectionType.Cross:
                    regions = new List<Wall2D>();
                    return true;
                case segmentIntersectionType.Touch:
                    regions = new List<Wall2D>();
                    return true;
                default:
                    regions = null;
                    return false;
            }
        }
        public bool SplitWallAtAndNew(Wall2D otherWall, List<Wall2D> walls,out segmentIntersectionType segmentIntersectionType,List<Point3D> intersectPoints)
        {
            switch (Segment2D.Intersection(CenterLine, otherWall.CenterLine, out var point1, out var point2, 0.1))
            {
                case segmentIntersectionType.Cross:
                    segmentIntersectionType = segmentIntersectionType.Cross;
                    var wall = new Wall2D(Plane.XY, this.StartPoint,point1.ConvertPoint2DtoPoint3D(),this.WallThickness,this.IsLoadBearingWall,true,this.ShowWallDimension);
                    //wall.Color = Color.FromArgb(127, Color.CornflowerBlue);
                    wall.Color =  Color.CornflowerBlue;
                    wall.ColorMethod = colorMethodType.byEntity;
                    wall.LineTypeName = "Dash Space";
                    wall.LineTypeMethod = colorMethodType.byEntity;
                    walls.Add(wall);
                    wall = new Wall2D(Plane.XY, point1.ConvertPoint2DtoPoint3D(),this.EndPoint,this.WallThickness,this.IsLoadBearingWall,true,this.ShowWallDimension);
                    //wall.Color = Color.FromArgb(127, Color.CornflowerBlue);
                    wall.Color = Color.CornflowerBlue;
                    wall.ColorMethod = colorMethodType.byEntity;
                    wall.LineTypeName = "Dash Space";
                    wall.LineTypeMethod = colorMethodType.byEntity;
                    walls.Add(wall);
                    intersectPoints.Insert(intersectPoints.Count - 1, point1.ConvertPoint2DtoPoint3D());
                    return true;
                case segmentIntersectionType.Touch:
                    segmentIntersectionType = segmentIntersectionType.Touch;
                    wall = new Wall2D(Plane.XY, this.StartPoint, point1.ConvertPoint2DtoPoint3D(), this.WallThickness, this.IsLoadBearingWall,true, this.ShowWallDimension);
                    //wall.Color = Color.FromArgb(127, Color.CornflowerBlue);
                    wall.Color = Color.CornflowerBlue;
                    wall.ColorMethod = colorMethodType.byEntity;
                    wall.LineTypeName = "Dash Space";
                    wall.LineTypeMethod = colorMethodType.byEntity;
                    walls.Add(wall);
                    wall = new Wall2D(Plane.XY, point1.ConvertPoint2DtoPoint3D(), this.EndPoint, this.WallThickness, this.IsLoadBearingWall,true, this.ShowWallDimension);
                    //wall.Color = Color.FromArgb(127, Color.CornflowerBlue);
                    wall.Color = Color.CornflowerBlue;
                    wall.ColorMethod = colorMethodType.byEntity;
                    wall.LineTypeName = "Dash Space";
                    wall.LineTypeMethod = colorMethodType.byEntity;
                    walls.Add(wall);
                    if (!otherWall.CenterlineVertices.Contains(StartPoint1.ConvertPoint2DtoPoint3D()))
                    {
                        intersectPoints.Insert(intersectPoints.Count-1,point1.ConvertPoint2DtoPoint3D());
                    }
                    return true;
                case segmentIntersectionType.EndPointTouch:
                    segmentIntersectionType = segmentIntersectionType.EndPointTouch;
                    return false;
                case segmentIntersectionType.OverlapInSegment:
                    segmentIntersectionType = segmentIntersectionType.OverlapInSegment;
                    return false;
                case segmentIntersectionType.CollinearEndPointTouch:
                    segmentIntersectionType = segmentIntersectionType.CollinearEndPointTouch;
                    return false;
                default:
                    segmentIntersectionType = segmentIntersectionType.Disjoint;
                    walls.Add(otherWall);
                    return false;
            }
        }

        
        public IEntityVm CreateEntityVm(IEntitiesManager entitiesManager)
        {
            return new Wall2DVm(this);
        }
    }
}
