using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels;
using AppModels.CustomEntity;
using AppModels.EntityCreator;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.EditingTools
{
    public class BreakJoist : ToolBase
    {
        //private bool _watingForSelection;
        private List<FramingRectangle2D> _framingList = new List<FramingRectangle2D>();
        private Beam2D _breakFraming;
        private Line _lineBreak;
        public override string ToolName => "Break Joist";
        public override Point3D BasePoint { get; protected set; }

        public BreakJoist()
        {
            //_watingForSelection = true;
            EntityUnderMouseDrawingType = UnderMouseDrawingType.ByObject;
            IsUsingOrthorMode = false;
            IsSnapEnable = false;
        }

        [CommandMethod("Break Framing")]
        public void BreakJ()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            while (true)
            {
                ToolMessage = "Please select Joist To Break";
                var promptEntities = new PromptEntityOptions();
                var result = acDoc.Editor.GetEntities(promptEntities,true);
                if (result.Status == PromptStatus.OK)
                {
                    foreach (var resultEntity in result.Entities)
                    {
                        if (resultEntity is FramingRectangle2D framingRect)
                        {
                            if (!(resultEntity is OutTrigger2D))
                            {
                                _framingList.Add(framingRect);
                            }
                        }
                    }

                    if (_framingList.Count==0)
                    {
                        continue;
                    }
                }
                else
                {
                    return;
                }

                while (true)
                {
                    ToolMessage = "Please select Line/Beam to break";
                    var promtSelect = new PromptSelectionOptions();
                    var resultEnt = acDoc.Editor.GetSelection(promtSelect);
                    if (resultEnt.Status == PromptStatus.OK)
                    {
                        if (resultEnt.Value is Line line)
                        {
                            _lineBreak = line;
                            break;
                        }

                        if (resultEnt.Value is Beam2D beam)
                        {
                            _breakFraming = beam;
                            break;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                if (_framingList.Count>0 &&(_breakFraming!=null)|| _lineBreak !=null)
                {
                    if (_lineBreak==null)
                    {
                        ProcessBreakTool(_framingList, _breakFraming);
                    }
                    else
                    {
                        ProcessBreakTool(_framingList, _lineBreak);
                    }
                    return;
                }
            }
        }

        private Point3D FindClosePoint(Point3D point1,Point3D point2, Point3D checkPoint)
        {
            if (Point3D.Distance(checkPoint,point1)>Point3D.Distance(checkPoint,point2))
            {
                return point2;
            }

            return point1;
        }

        public void ProcessBreakTool(List<FramingRectangle2D> framingList,Beam2D beam)
        {
            if (beam.IsBeamUnder)
            {
                return;
            }

            foreach (var framing2D in framingList)
            {
                var intersectPoints = new List<Point3D>();
                if (framing2D.IntersectWith(beam, intersectPoints,out var intersectionType))
                {
                    switch (intersectionType)
                    {
                        case FramingIntersectionType.Cross:
                        case FramingIntersectionType.TCross:
                        case FramingIntersectionType.TOverlap:
                            if (framing2D is Joist2D joist)
                            {
                                Point3D endPoint1 = null;
                                Point3D endPoint2 = null;
                                if (joist.OuterStartPoint.DistanceTo(intersectPoints[0])>joist.OuterStartPoint.DistanceTo(intersectPoints[1]))
                                {
                                    endPoint1 = intersectPoints[1];
                                    endPoint2 = intersectPoints[0];
                                }
                                else
                                {
                                    endPoint1 = intersectPoints[0];
                                    endPoint2 = intersectPoints[1];
                                }

                                var joistCreator = new Joist2DCreator(joist, joist.OuterStartPoint, endPoint1);
                                this.EntitiesManager.AddAndRefresh((Joist2D)joistCreator.GetFraming2D(), joist.LayerName);
                                joistCreator = new Joist2DCreator(joist, endPoint2, joist.OuterEndPoint);
                                this.EntitiesManager.AddAndRefresh((Joist2D)joistCreator.GetFraming2D(), joist.LayerName);
                                this.EntitiesManager.RemoveEntity(joist);
                            }
                            break;
                        case FramingIntersectionType.TEndCross:
                            if (intersectPoints.Contains(framing2D.OuterStartPoint))
                            {
                                framing2D.OuterStartPoint = FindClosePoint(intersectPoints[0], intersectPoints[1],
                                    framing2D.OuterEndPoint);
                            }
                            else
                            {
                                framing2D.OuterEndPoint = FindClosePoint(intersectPoints[0], intersectPoints[1],
                                    framing2D.OuterStartPoint);
                            }
                            break;
                        case FramingIntersectionType.TEndOverlap:
                        case FramingIntersectionType.LEndOverlap:
                            if (beam.IsPointInside(framing2D.StartCenterLinePoint))
                            {
                                framing2D.OuterStartPoint = intersectPoints[0];
                            }
                            else
                            {
                                framing2D.OuterEndPoint = intersectPoints[0];
                            }
                            break;
                        case FramingIntersectionType.LEndCrossTouch:
                            if (beam.IsPointIn(framing2D.StartCenterLinePoint))
                            {
                                framing2D.OuterStartPoint = intersectPoints[0];
                            }
                            else
                            {
                                framing2D.OuterEndPoint = intersectPoints[0];
                            }
                            break;
                        case FramingIntersectionType.LOverlap:
                            if (intersectPoints[1]==framing2D.OuterStartPoint || intersectPoints[1]==framing2D.InnerStartPoint)
                            {
                                framing2D.OuterStartPoint = intersectPoints[0];
                            }
                            else
                            {
                                framing2D.OuterEndPoint = intersectPoints[0];
                            }
                            break;
                        case FramingIntersectionType.LEndCross:
                            if (beam.IsPointInside(framing2D.OuterStartPoint) || beam.IsPointInside(framing2D.InnerStartPoint))
                            {
                                framing2D.OuterStartPoint = intersectPoints[0];
                            }
                            else
                            {
                                framing2D.OuterEndPoint = intersectPoints[0];
                            }
                            break;
                        case FramingIntersectionType.ParallelOverlap:
                            if (beam.IsPointInside(framing2D.OuterStartPoint) || beam.IsPointInside(framing2D.InnerStartPoint))
                            {
                                framing2D.OuterStartPoint = intersectPoints[0];
                            }
                            else
                            {
                                framing2D.OuterEndPoint = intersectPoints[0];
                            }
                            

                            break;
                        case FramingIntersectionType.ColinearOverlap:
                            Point3D intersectPoint1 = null;
                            if (intersectPoints[0]== framing2D.StartCenterLinePoint || intersectPoints[0] == framing2D.EndCenterLinePoint)
                            {
                                intersectPoint1 = intersectPoints[1];
                            }
                            else
                            {
                                intersectPoint1 = intersectPoints[0];
                            }
                            var framingOutnerSegment2 =
                                new Segment2D(framing2D.OuterStartPoint, framing2D.OuterEndPoint);
                            var d2 = framingOutnerSegment2.Project(intersectPoint1);
                            var breakPoint2 = framingOutnerSegment2.PointAt(d2);
                            if (breakPoint2!=null)
                            {
                                if (beam.IsPointInside(framing2D.StartCenterLinePoint))
                                {
                                    framing2D.OuterStartPoint = breakPoint2.ConvertPoint2DtoPoint3D();
                                }
                                else
                                {
                                    framing2D.OuterEndPoint = breakPoint2.ConvertPoint2DtoPoint3D();
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            EntitiesManager.EntitiesRegen();
        }

        public void ProcessBreakJoist(Joist2D joist, Point3D breakPoint)
        {
                var joistCreator = new Joist2DCreator(joist, joist.OuterStartPoint, breakPoint);
                this.EntitiesManager.AddAndRefresh((Joist2D) joistCreator.GetFraming2D(), joist.LayerName);
                joistCreator = new Joist2DCreator(joist, breakPoint, joist.OuterEndPoint);
                this.EntitiesManager.AddAndRefresh((Joist2D)joistCreator.GetFraming2D(), joist.LayerName);
                this.EntitiesManager.RemoveEntity(joist);
        }

        public void ProcessBreakTool(List<FramingRectangle2D> framingList, Line breakLine)
        {
            
            foreach (var framing2D in framingList)
            {
                if (framing2D.IntersectWith(breakLine,out var intersecPoint,out var intersectype))
                {
                    switch (intersectype)
                    {
                        case segmentIntersectionType.Cross:
                            if (framing2D is Joist2D joist)
                            {
                                var segment = new Segment2D(framing2D.OuterStartPoint, framing2D.OuterEndPoint);
                                var disc = segment.Project(intersecPoint);
                                var breakPoint = segment.PointAt(disc);
                                if (breakPoint!=null)
                                {
                                    ProcessBreakJoist(joist, breakPoint.ConvertPoint2DtoPoint3D());
                                }
                            }
                            
                            break;
                        default: break;
                    }
                }
            }
            
        }




    }
}
