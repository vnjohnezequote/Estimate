using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.CustomEntity;
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
                        else if (resultEnt.Value is Beam2D beam)
                        {
                            _breakFraming = beam;
                            break;
                        }
                        else
                        {
                            continue;
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
                        case segmentIntersectionType.Cross:
                            if(framing2D is Joist2D joist)
                            {
                                joist.FramingReference.FramingSheet.Joists.Remove(joist.FramingReference);
                                this.EntitiesManager.RemoveEntity(joist);
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

                                var joistRef1 = new Joist(joist.FramingReference.FramingSheet);
                                joistRef1.Index = 1;
                                joistRef1.FramingType = joist.FramingReference.FramingType;
                                var joistRef2 = new Joist(joist.FramingReference.FramingSheet);
                                joistRef2.Index = 1;
                                joistRef2.FramingType = joist.FramingReference.FramingType;
                                if (joist.FramingReference.FramingInfo !=null)
                                {
                                    joistRef1.FramingInfo = joist.FramingReference.FramingInfo;
                                    joistRef2.FramingInfo = joist.FramingReference.FramingInfo;
                                }
                                var joistThickness = joist.Thickness;
                                var framingSpan1 = joist.OuterStartPoint.DistanceTo(endPoint1);
                                var framingSpan2 = joist.OuterEndPoint.DistanceTo(endPoint2);
                                joistRef1.FramingSpan = (int)framingSpan1-90;
                                joistRef2.FramingSpan = (int)framingSpan2-90;
                                var joist1 = new Joist2D(joist.OuterStartPoint, endPoint1, joistRef1,
                                    joistThickness);
                                joist1.ColorMethod = colorMethodType.byEntity;
                                var joist2 = new Joist2D( endPoint2,joist.OuterEndPoint, joistRef2,
                                    joistThickness);
                                joist2.ColorMethod = colorMethodType.byEntity;
                                GeneralJoistName(joistRef1);
                                GeneralJoistName(joistRef2);
                                joist.FramingReference.FramingSheet.Joists.Add(joistRef1);
                                joist.FramingReference.FramingSheet.Joists.Add(joistRef2);
                                var orderedEnumerable = joist.FramingReference.FramingSheet.Joists.OrderBy(framing => framing.Name);
                                var sortedList = orderedEnumerable.ToList();
                                joist.FramingReference.FramingSheet.Joists.Clear();
                                joist.FramingReference.FramingSheet.Joists.AddRange(sortedList);
                                this.EntitiesManager.AddAndRefresh(joist1,joist.LayerName);
                                this.EntitiesManager.AddAndRefresh(joist2, joist.LayerName);

                            }
                            break;
                        case segmentIntersectionType.EndPointTouch:
                        case segmentIntersectionType.Touch:
                            if (beam.IsPointInside(framing2D.OuterStartPoint))
                            {
                                framing2D.OuterStartPoint = intersectPoints[0];
                                EntitiesManager.Refresh();

                            }
                            else
                            {
                                framing2D.OuterEndPoint = intersectPoints[0];
                                EntitiesManager.Refresh();
                            }
                            break;
                    }
                }
            }
        }

        public void ProcessBreakJoist(Joist2D joist, Point3D breakPoint)
        {
                joist.FramingReference.FramingSheet.Joists.Remove(joist.FramingReference);
                this.EntitiesManager.RemoveEntity(joist);

                var joistRef1 = new Joist(joist.FramingReference.FramingSheet);
                joistRef1.Index = 1;
                joistRef1.FramingType = joist.FramingReference.FramingType;
                var joistRef2 = new Joist(joist.FramingReference.FramingSheet);
                joistRef2.Index = 1;
                joistRef2.FramingType = joist.FramingReference.FramingType;
                if (joist.FramingReference.FramingInfo != null)
                {
                    joistRef1.FramingInfo = joist.FramingReference.FramingInfo;
                    joistRef2.FramingInfo = joist.FramingReference.FramingInfo;
                }
                var joistThickness = joist.Thickness;
                var framingSpan1 = joist.OuterStartPoint.DistanceTo(breakPoint);
                var framingSpan2 = joist.OuterEndPoint.DistanceTo(breakPoint);
                joistRef1.FramingSpan = (int)framingSpan1 - 90;
                joistRef2.FramingSpan = (int)framingSpan2 - 90;
                var joist1 = new Joist2D(joist.OuterStartPoint, breakPoint, joistRef1,
                    joistThickness);
                joist1.ColorMethod = colorMethodType.byEntity;
                var joist2 = new Joist2D(breakPoint, joist.OuterEndPoint, joistRef2,
                    joistThickness);
                joist2.ColorMethod = colorMethodType.byEntity;
                GeneralJoistName(joistRef1);
                GeneralJoistName(joistRef2);
                joist.FramingReference.FramingSheet.Joists.Add(joistRef1);
                joist.FramingReference.FramingSheet.Joists.Add(joistRef2);
                var orderedEnumerable = joist.FramingReference.FramingSheet.Joists.OrderBy(framing => framing.Name);
                var sortedList = orderedEnumerable.ToList();
                joist.FramingReference.FramingSheet.Joists.Clear();
                joist.FramingReference.FramingSheet.Joists.AddRange(sortedList);
                this.EntitiesManager.AddAndRefresh(joist1, joist.LayerName);
                this.EntitiesManager.AddAndRefresh(joist2, joist.LayerName);
            
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
                                ProcessBreakJoist(joist,intersecPoint);
                            }
                            
                            break;
                        case segmentIntersectionType.Touch:
                            if (intersecPoint != framing2D.OuterStartPoint && intersecPoint != framing2D.OuterEndPoint)
                            {
                                if (framing2D is Joist2D joist2)
                                {
                                    ProcessBreakJoist(joist2, intersecPoint);
                                }
                            }
                            break;
                        default: break;
                    }
                }
            }

        }

        private void GeneralJoistName(Joist joist)
        {
            if (joist.FramingSheet.Joists.Count>0)
            {
                var maximumSubFixIndex = 0;
                IFraming maxJoist = null;
                IFraming maxIndexJoist = null;
                var maximumIndex = 0;
                foreach (var existJoist in JobModel.ActiveFloorSheet.Joists)
                {
                    if (existJoist.FramingType != joist.FramingType)
                    {
                        continue;
                    }
                    if (existJoist.FramingInfo != null && joist.FramingInfo != null && existJoist.FramingInfo == joist.FramingInfo)
                    {
                        if (Math.Abs(existJoist.QuoteLength - joist.QuoteLength) > 0.001)
                        {
                            maxJoist = existJoist;
                            if (maximumIndex < existJoist.SubFixIndex)
                            {
                                maximumIndex = existJoist.SubFixIndex;
                            }
                        }
                    }
                    else
                    {
                        if (existJoist.NamePrefix == joist.NamePrefix)
                        {
                            maxIndexJoist = existJoist;

                            if (maximumIndex < existJoist.Index)
                            {
                                maximumIndex = existJoist.Index;
                            }
                        }

                    }
                }

                if (maxJoist != null)
                {
                    joist.SubFixIndex = maximumSubFixIndex + 1;
                }

                else if (maxIndexJoist != null)
                {
                    joist.Index = maximumIndex + 1;
                }
            }
        }
        

       

    }
}
