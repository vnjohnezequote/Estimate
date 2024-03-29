﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.EntityCreator;
using AppModels.Enums;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.DrawingTools
{
    public class RunJoist: ToolBase
    {
        private bool  _waitingForSelection;
        private Line _firstLineForRunJoist;
        private Line _secondLineForRunJoist;
        public override string ToolName => "Auto Run Joist";
        public override Point3D BasePoint { get; protected set; }

        public RunJoist()
        {
            _waitingForSelection = true;
            EntityUnderMouseDrawingType = UnderMouseDrawingType.BySegment;
            IsUsingOrthorMode = false;
            IsSnapEnable = false;
        }

        [CommandMethod("Run Joist")]
        public void AutoRunJoist()
        {

            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            while (true)
            {
                if (JobModel.ActiveFloorSheet == null)
                {
                    ToolMessage = "Please select Floor Sheet before run Joist/Rafter";
                    continue;
                }
                var promptLineOption = new PromptSelectionOptions();
                PromptSelectionResult result = null;
                while(_firstLineForRunJoist == null)
                {
                    ToolMessage = "Please Select First line to Run Joist";
                    result = acDoc.Editor.GetSelection(promptLineOption);
                    if (result.Status == PromptStatus.OK)
                    {
                        if (result.Value is Line line)
                            this._firstLineForRunJoist = line;
                        else continue;
                    }
                    else
                    {
                        return;
                    }
                }
                while (_secondLineForRunJoist == null)
                {
                    ToolMessage = "Please Select Second line";
                    result = acDoc.Editor.GetSelection(promptLineOption);
                    if (result.Status == PromptStatus.OK)
                    {
                        if (result.Value is Line line)
                            this._secondLineForRunJoist = line;
                        else continue;
                    }
                    else
                    {
                        return;
                    }
                }
                

                if (!IsTwoLineParallel(_firstLineForRunJoist,_secondLineForRunJoist))
                {
                    ResetTool();
                    continue;
                }
                _waitingForSelection = false;

                var firstLine = GetFirstRunLine(_firstLineForRunJoist, _secondLineForRunJoist);

                _secondLineForRunJoist.Project(_firstLineForRunJoist.MidPoint,out var distance);
                var projectPoint = _secondLineForRunJoist.PointAt(distance);
                var distanceRunLength = _firstLineForRunJoist.MidPoint.DistanceTo(projectPoint);
                var loopCheck = (int)distanceRunLength / JobModel.DefaultJoistSpacing;
                this.ProcessAutorunJoist(firstLine, _secondLineForRunJoist, distanceRunLength, loopCheck);
                return;
            }
        }

        private enum LineDirectionType
        {
            HorizontalLine,
            VerticalLine,
            FredomLine
        }
        private LineDirectionType GetLineDirectionType (Line line)
        {
            
            if (line.StartPoint.Y.CompareTo(line.EndPoint.Y) == 0)
                return LineDirectionType.HorizontalLine;
            if (line.StartPoint.X.CompareTo(line.EndPoint.X) == 0)
                return LineDirectionType.VerticalLine;
            return LineDirectionType.FredomLine;
        }

        private Line GetFirstRunLine(Line firstLine, Line secondLine)
        {
            var lineDirect = GetLineDirectionType(firstLine);
            if(lineDirect == LineDirectionType.HorizontalLine)
            {
               if(firstLine.StartPoint.X<firstLine.EndPoint.X)
                {
                    if (firstLine.StartPoint.Y > secondLine.StartPoint.Y)
                        return firstLine;
                    return new Line(firstLine.EndPoint, firstLine.StartPoint);
                }
                else
                {
                    if(firstLine.StartPoint.Y < secondLine.StartPoint.Y)
                        return firstLine;
                    return new Line(firstLine.EndPoint, firstLine.StartPoint);
                }
            }
            else if(lineDirect == LineDirectionType.VerticalLine)
            {
               
                if(firstLine.StartPoint.Y> firstLine.EndPoint.Y)
                {
                    if (firstLine.StartPoint.X > secondLine.StartPoint.X)
                        return firstLine;
                    else return new Line(firstLine.EndPoint, firstLine.StartPoint);
                }
                else
                {
                    if (firstLine.StartPoint.X < secondLine.StartPoint.X)
                        return firstLine;
                    else return new Line(firstLine.EndPoint, firstLine.StartPoint);
                }
            }
            else
            {
                secondLine.Project(firstLine.StartPoint,out var projectPointDistance);
                var projectPoint = secondLine.PointAt(projectPointDistance);
                if (firstLine.StartPoint.X < firstLine.EndPoint.X)
                {
                    if (firstLine.StartPoint.Y > projectPoint.Y)
                    {
                        return firstLine;
                    }
                    else return new Line(firstLine.EndPoint, firstLine.StartPoint);
                }
                else
                {
                    if (firstLine.StartPoint.Y < projectPoint.Y)
                    {
                        return firstLine;
                    }
                    else return new Line(firstLine.EndPoint, firstLine.StartPoint);
                }

            }
        }
        private void ProcessAutorunJoist(Line firstLine, Line secondLine, double distanceRun, int loopCheck)
        {
            Line joistLine = null;
            Joist2D firstJoist = null;
            for (var i = 0; i < loopCheck+1; i++)
            {
                if (joistLine == null)
                {
                    joistLine = firstLine;
                    var joistCreator = new Joist2DCreator(JobModel.ActiveFloorSheet, joistLine.StartPoint,
                        joistLine.EndPoint,FramingTypes.FloorJoist,JobModel.SelectedJoitsMaterial);
                    joistCreator.GetFraming2D().ColorMethod = colorMethodType.byEntity;
                    this.EntitiesManager.AddAndRefresh((Joist2D)joistCreator.GetFraming2D(),LayerManager.SelectedLayer.Name);
                    firstJoist = (Joist2D) joistCreator.GetFraming2D();
                }
                else
                {
                    joistLine = (Line)joistLine.Offset(JobModel.DefaultJoistSpacing, Vector3D.AxisZ);
                    var joistCreator = new Joist2DCreator(firstJoist, joistLine.StartPoint, joistLine.EndPoint, false);
                    joistCreator.GetFraming2D().ColorMethod = colorMethodType.byEntity;
                    this.EntitiesManager.AddAndRefresh((Joist2D)joistCreator.GetFraming2D(),LayerManager.SelectedLayer.Name);

                }
            }

            var lastDistance = distanceRun - (loopCheck) * JobModel.DefaultJoistSpacing;

            if (lastDistance > 100)
            {
                if (joistLine != null)
                {
                    joistLine = (Line) joistLine.Offset((int)lastDistance, Vector3D.AxisZ);
                    var joistCreator = new Joist2DCreator(firstJoist, joistLine.StartPoint, joistLine.EndPoint, false);
                    joistCreator.GetFraming2D().ColorMethod = colorMethodType.byEntity;
                    this.EntitiesManager.AddAndRefresh((Joist2D) joistCreator.GetFraming2D(),
                        LayerManager.SelectedLayer.Name);
                }
            }
            var orderedEnumerable = this.JobModel.ActiveFloorSheet.Joists.OrderBy(framing => framing.Name);
            var sortedList = orderedEnumerable.ToList();
            this.JobModel.ActiveFloorSheet.Joists.Clear();
            this.JobModel.ActiveFloorSheet.Joists.AddRange(sortedList);

        }
        
        private bool IsTwoLineParallel(Line line1, Line line2)
        {
            Segment2D seg1 = new Segment2D(this._firstLineForRunJoist.StartPoint, this._firstLineForRunJoist.EndPoint);
            Segment2D seg2 = new Segment2D(this._secondLineForRunJoist.StartPoint, this._secondLineForRunJoist.EndPoint);
            return !Segment2D.IntersectionLine(seg1, seg2, out var centerPoint);
        }
        private void ResetTool()
        {
            this._waitingForSelection = true;
            this._firstLineForRunJoist= null;
            this._secondLineForRunJoist= null;
        }

    }
}
