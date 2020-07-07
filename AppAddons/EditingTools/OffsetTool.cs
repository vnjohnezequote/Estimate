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
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.EditingTools
{
    public class OffsetTool : ToolBase,IOffsetDisance
    {
        public override Point3D BasePoint { get; protected set; }
        public override string ToolName => "Offset";
        private bool _waitingForSelection;
        private int _offsetDistance;

        public int OffsetDistance { get=>_offsetDistance; set=>SetProperty(ref _offsetDistance,value); }
        private Entity _selectedEntity;
        //private Entity _offetEntity;
        
        private Point3D _offsetPoint;

        public OffsetTool(): base()
        {
            IsUsingOffsetDistance = true;
            IsUsingOrthorMode = true;
            IsSnapEnable = true;
        }

        [CommandMethod("Offset")]
        public void Offset()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            while (true)
            {
                ToolMessage = "Please Select object to offset";
                var promptLineOption = new PromptSelectionOptions();
                var result = acDoc.Editor.GetSelection(promptLineOption);
                if (result.Status == PromptStatus.OK)
                {
                    this._selectedEntity = result.Value;
                }
                else
                {
                    return;
                }

                IsUsingLengthTextBox = true;
                DynamicInput?.FocusDynamicInputTextBox(FocusType.Length);

                var promptPointOption = new PromptPointOptions(ToolMessage);
                var resultPoint = acDoc.Editor.GetPoint(promptPointOption);
                if (resultPoint.Status == PromptStatus.OK)
                {
                    _offsetPoint = resultPoint.Value;
                }
                else
                {
                    return;
                }

                ProcessOffsetEntity();
            }
            
        }

        public override void NotifyMouseMove(object sender, MouseEventArgs e)
        {
            DynamicInput?.FocusDynamicInputTextBox(FocusType.Length);
        }

        private void ProcessOffsetEntity()
        {
            if (this._selectedEntity !=null && this._offsetPoint !=null)
            {
                var offetEntity = CalculatorOffsetEntity(_selectedEntity, _offsetPoint);
                if (offetEntity != null)
                {
                    EntitiesManager.AddAndRefresh(offetEntity, LayerManager.SelectedLayer.Name);
                }

            }

            ResetTool();
        }

        private void ResetTool()
        {
            //this._offetEntity = null;
            _offsetPoint = null;
            this._selectedEntity = null;
            this._waitingForSelection = true;
        }
        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            DrawInteractiveOffset((ICadDrawAble)sender, e);
        }

        private void DrawInteractiveOffset(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            if (this._selectedEntity == null)
            {
                return;
            }

            if (e.CurrentPoint==null)
            {
                return;
            }

            var offetEntity = CalculatorOffsetEntity(this._selectedEntity, e.CurrentPoint);
            DrawInteractiveUntilities.DrawCurveOrBlockRef(offetEntity,canvas);
            if (BasePoint!=null )
            {
                DrawInteractiveUntilities.DrawInteractiveSpotLine(BasePoint, e.CurrentPoint, canvas);
            }
           //DrawInteractiveUntilities.DrawPositionMark(e.CurrentPoint,canvas);
        }

        private Entity CalculatorOffsetEntity(Entity selEntity, Point3D offsetPoint)
        {
            ICurve selCurve = _selectedEntity as ICurve;
            double t;
            bool success = selCurve.Project(offsetPoint, out t);
            Point3D projectedPt = selCurve.PointAt(t);
            BasePoint = projectedPt;
            double offsetDist = 0;
            if (this.OffsetDistance ==0)
            {
                offsetDist = projectedPt.DistanceTo(offsetPoint);
            }
            else
            {
                offsetDist = this.OffsetDistance;
            }
            

            ICurve offsetCurve = selCurve.Offset(offsetDist, Vector3D.AxisZ, 0.01, true);
            success = offsetCurve.Project(offsetPoint, out t);
            projectedPt = offsetCurve.PointAt(t);
            if (projectedPt.DistanceTo(offsetPoint) > 1e-3)
                offsetCurve = selCurve.Offset(-offsetDist, Vector3D.AxisZ, 0.01, true);
            return offsetCurve as Entity;
        }

    }
}
