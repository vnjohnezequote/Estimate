using System;
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
                ToolMessage = "Please Select first line";
                var promptLineOption = new PromptSelectionOptions();
                var result = acDoc.Editor.GetSelection(promptLineOption);
                if (result.Status == PromptStatus.OK)
                {
                    this._firstLineForRunJoist= result.Value as Line;
                }
                else
                {
                    return;
                }

                ToolMessage = "Please Select Second line";
                result = acDoc.Editor.GetSelection(promptLineOption);
                if (result.Status == PromptStatus.OK)
                {
                    this._secondLineForRunJoist = result.Value as Line;
                }
                else
                {
                    return;
                }

                if (!IsTwoLineParallel(_firstLineForRunJoist,_secondLineForRunJoist))
                {
                    ResetTool();
                    continue;
                }
                _waitingForSelection = false;
                _secondLineForRunJoist.Project(_firstLineForRunJoist.MidPoint,out var distance);
                var projectPoint = _secondLineForRunJoist.PointAt(distance);
                var distanceRunLength = _firstLineForRunJoist.MidPoint.DistanceTo(projectPoint);
                var loopCheck = (int)distanceRunLength / JobModel.DefaultJoistSpacing;
                this.ProcessAutorunJoist(_firstLineForRunJoist, _secondLineForRunJoist, distanceRunLength, loopCheck);
                return;
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
