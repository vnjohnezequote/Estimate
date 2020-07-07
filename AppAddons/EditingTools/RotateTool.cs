
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.EventArg;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.Control;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;
using Environment = devDept.Eyeshot.Environment;

namespace AppAddons.EditingTools
{
    public class RotateTool : EditingToolBase
    {
        public override string ToolName => "Rotate";
        private Point3D _startPoint;
        private Point3D _refPoint2;
        private Point3D _currentPoint;
        private Point3D _endPoint;
        private double _currentAngle;
        private List<Point3D> _clickPoints;
        public override double CurrentAngle { 
            get => Utility.RadToDeg(_currentAngle).Round();
            set => SetProperty(ref _currentAngle, Utility.RadToDeg(value));
        }
        public RotateTool() : base()
        {
            _clickPoints = new List<Point3D>();
            DefaultDynamicInputTextBoxToFocus = FocusType.Angle;
        }
        [CommandMethod("Rotate")]
        public void Rotate()
        {
            OnProcessCommand();
        }

        protected override bool PrepairPoint(Editor editor)
        {
            ToolMessage = "Please enter basePoint to" + ToolName.ToLower();
            this.IsSnapEnable = true;
            IsUsingOrthorMode = false;
            var promptPointOption = new PromptPointOptions("Please enter basePoint to " + ToolName.ToLower());
            var promptPointResult = editor.GetPoint(promptPointOption);
            switch (promptPointResult.Status)
            {
                case PromptStatus.OK:
                    this._startPoint = promptPointResult.Value;
                    this._clickPoints.Add(_startPoint);
                    this.BasePoint = promptPointResult.Value;
                    break;
                case PromptStatus.Cancel:
                    return false;
            }
            ToolMessage = "Please enter first referent point to " + ToolName.ToLower();
            promptPointResult = editor.GetPoint(promptPointOption);
            switch (promptPointResult.Status)
            {
                case PromptStatus.OK:
                    this._refPoint2 = promptPointResult.Value;
                    this._clickPoints.Add(_refPoint2);
                    this.ReferencePoint = _refPoint2;
                    break;
                case PromptStatus.Cancel:
                    return false;
            }

            IsUsingAngleTextBox = true;
            //IsUsingLengthTextBox = true;
            this.DynamicInput?.FocusDynamicInputTextBox(FocusType.Angle);
            ToolMessage = "Please enter second reference point to " + ToolName.ToLower();
            promptPointResult = editor.GetPoint(promptPointOption);
            switch (promptPointResult.Status)
            {
                case PromptStatus.OK:
                    this._endPoint = promptPointResult.Value;
                    this._clickPoints.Add(_endPoint);
                    break;
                case PromptStatus.Cancel:
                    return false;
            }
            return true;
        }

        public override void NotifyMouseMove(object sender, MouseEventArgs e)
        {
            if (IsUsingAngleTextBox)
            {
                this.DynamicInput?.FocusDynamicInputTextBox(FocusType.Angle);
            }
        }

        protected override void OnMoveNextTab()
        {
            DynamicInput?.FocusAngle();
        }

        protected override void ProcessEntities()
        {
            var vector1 = new Vector2D(BasePoint,_refPoint2);
            vector1.Normalize();
            var vector2 = new Vector2D(BasePoint, _endPoint);
            vector2.Normalize();
            var angle = Vector2D.SignedAngleBetween(vector1, vector2);
            this._currentAngle = angle;
            foreach (var selEntity in this.SelectedEntities)
            {
                selEntity.Rotate(_currentAngle, Vector3D.AxisZ, _clickPoints[0]);
            }
            EntitiesManager.Refresh();
            CurrentAngle = 0;
        }
        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            DrawInteractiveRotate((ICadDrawAble)sender, e);
        }
        private void DrawInteractiveRotate(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            if (WaitingForSelection)
            {
                return;
            }

            if (_clickPoints.Count ==0) return;
            if(this._endPoint !=null ) return;

            DrawInteractiveUntilities.DrawInteractiveArc(canvas,_clickPoints,e.CurrentPoint, out _currentAngle);
            //this.RaisePropertyChanged(nameof(CurrentAngle));
            // Show temp entities for current rotation state
            foreach (var ent in this.SelectedEntities)
            {
                Entity tempEntity = (Entity)ent.Clone();
                tempEntity.Rotate(_currentAngle, Vector3D.AxisZ, _startPoint);

                if (tempEntity is Text)
                    tempEntity.Regen(new RegenParams(0, (Environment)canvas));
                canvas.renderContext.EnableXOR(true);
                DrawInteractiveUntilities.DrawCurveOrBlockRef(tempEntity,canvas);
                canvas.renderContext.EnableXOR(false);
                //this.Draw((ICurve)tempEntity); 
            }
        }

        protected override void ResetTool()
        {
            base.ResetTool();
            this._clickPoints.Clear();
            this._refPoint2 = null;
            this._startPoint = null;
            this._endPoint = null;
            this.ReferencePoint = null;
            IsUsingAngleTextBox = false;
        }
    }
}
