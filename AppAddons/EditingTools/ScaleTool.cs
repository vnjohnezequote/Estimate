using System;
using System.Linq;
using System.Windows.Input;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.EventArg;
using AppModels.Interaface;
using AppModels.Undo;
using AppModels.Undo.Backup;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;
using Environment = devDept.Eyeshot.Environment;

namespace AppAddons.EditingTools
{
    public class ScaleTool : EditingToolBase
    {
        public override string ToolName => "Scale";
        //private List<Point3D> _clickPoints;
        private double _scaleFactor;
        public override double ScaleFactor { get=>_scaleFactor; set=>SetProperty(ref _scaleFactor,value); }
        public double ReferenceScaleFactor { get; private set; }

        public ScaleTool()
        {
            DefaultDynamicInputTextBoxToFocus = FocusType.ScaleFactor;
            //_clickPoints = new List<Point3D>();
        }
        [CommandMethod("Scale")]
        public void Scale()
        {
            OnProcessCommand();
        }

        protected override bool PrepairPoint(Editor editor)
        {
            ToolMessage = "Please enter origin point to " + ToolName.ToLower();
            this.IsSnapEnable = true;
            var promptPointOption = new PromptPointOptions("Please enter origin point to " + ToolName.ToLower());
            var promptPointResult = editor.GetPoint(promptPointOption);
            switch (promptPointResult.Status)
            {
                case PromptStatus.OK:
                    BasePoint = promptPointResult.Value;
                    //this.BasePoint = promptPointResult.Value;
                    break;
                case PromptStatus.Cancel:
                    return false;
            }

            IsUsingScaleFactorTextBox = true;
            this.DynamicInput?.FocusDynamicInputTextBox(FocusType.ScaleFactor);
            ToolMessage = "Select enter Scale Factor " + ToolName.ToLower();
            promptPointResult = editor.GetPoint(promptPointOption);
            switch (promptPointResult.Status)
            {
                case PromptStatus.OK:
                    this._endPoint = promptPointResult.Value;
                    break;
                case PromptStatus.Cancel:
                    return false;
            }

            
            return true;

        }
        protected override void OnMoveNextTab()
        {
            if (IsUsingScaleFactorTextBox)
            {
                this.DynamicInput?.FocusDynamicInputTextBox(FocusType.ScaleFactor);
            }
            
        }
        public override void NotifyMouseMove(object sender, MouseEventArgs e)
        {
            if (IsUsingScaleFactorTextBox)
            {
                this.DynamicInput?.FocusDynamicInputTextBox(FocusType.ScaleFactor);
            }
        }
        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            DrawInteractiveScale((ICadDrawAble)sender, e);
        }

        private void DrawInteractiveScale(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            if (BasePoint== null)
            {
                return;
            }
            
            //var worldToScreenVertices = _clickPoints.Select(point3D => canvas.WorldToScreen(point3D)).ToList();
            //renderContext.DrawLineStrip(worldToScreenVertices.ToArray());


            //if (worldToScreenVertices.Count > 0)
            //{
            //    canvas.renderContext.DrawLineStrip(new Point3D[]
            //    {
            //        canvas.WorldToScreen(_clickPoints.First()),canvas.WorldToScreen(e.CurrentPoint)
            //    });
            //}
            if (e.CurrentPoint == null) return;
            //_scaleFactor = BasePoint.DistanceTo(e.CurrentPoint) / _clickPoints[0].DistanceTo(_clickPoints[1]);
            if (this.SelectedEntities.Count==0)
            {
                return;
            }

            CalculatorScaleFactor(e.CurrentPoint);

            foreach (var tempEntity in this.SelectedEntities.Select(selEntity => (Entity)selEntity.Clone()))
            {
                tempEntity.Scale(BasePoint, ScaleFactor);
                if (tempEntity is Text)
                {
                    tempEntity.Regen(new RegenParams(0, (Environment)canvas));

                }
                DrawInteractiveUntilities.DrawCurveOrBlockRef(tempEntity, canvas);
            }

        }

        protected override void ProcessEntities()
        {
            //CalculatorScaleFactor(_endPoint);
            var undoItem = new UndoList() { ActionType = ActionTypes.Edit };
            foreach (var selEntity in this.SelectedEntities)
            {
                if (selEntity is IFraming2D|| selEntity is JoistArrowEntity)
                {
                    continue;
                }
                var backup = BackupEntitiesFactory.CreateBackup(selEntity, undoItem, EntitiesManager);
                backup?.Backup();
                selEntity.Scale(BasePoint, ScaleFactor);
            }
            this.UndoEngineer.SaveSnapshot(undoItem);
            this.EntitiesManager.Refresh();
        }

        protected override void ResetTool()
        {
            base.ResetTool();
            IsUsingScaleFactorTextBox = false;
            ScaleFactor = 0;
        }

        private void CalculatorScaleFactor(Point3D currentPoint)
        {
            var maxlength = 0.0;
            foreach (var selectedEntity in SelectedEntities)
            {
                if (selectedEntity.BoxSize.Max > maxlength)
                    maxlength = selectedEntity.BoxSize.Max;
            }

            var d1 = BasePoint.DistanceTo(currentPoint);
            if (Math.Abs(maxlength) < 0.0001)
            {
                maxlength = 1;
            }

            _scaleFactor = d1 / maxlength;
            if (Math.Abs(_scaleFactor) < 0.0001)
            {
                _scaleFactor = 1;
            }
            //this.RaisePropertyChanged(nameof(ScaleFactor));
        }
    }
}
