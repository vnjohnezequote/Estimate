using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.EventArg;
using AppModels.Interaface;
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
                    if (offetEntity is Joist2D joist)
                    {
                        if (joist.FramingReference!=null && joist.FramingReference.FramingSheet!=null)
                        {
                            joist.FramingReference.FramingSheet.Joists.Add(joist.FramingReference);
                            EntitiesManager.AddAndRefresh(offetEntity, LayerManager.SelectedLayer.Name);
                        }

                    }
                    else
                    {
                        EntitiesManager.AddAndRefresh(offetEntity, LayerManager.SelectedLayer.Name);
                    }
                    
                   
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
            
            
            if (offetEntity is ICurve)
            {
                DrawInteractiveUntilities.DrawCurveOrBlockRef(offetEntity, canvas);
            }
            
            if (BasePoint!=null )
            {
                DrawInteractiveUntilities.DrawInteractiveSpotLine(BasePoint, e.CurrentPoint, canvas);
            }
            //DrawInteractiveUntilities.DrawPositionMark(e.CurrentPoint,canvas);
        }

        private Entity CalculatorOffsetEntity(Entity selEntity, Point3D offsetPoint)
        {
            if (selEntity is ICurve selCurve)
            {
                double t;
                bool success = selCurve.Project(offsetPoint, out t);
                Point3D projectedPt = selCurve.PointAt(t);
                BasePoint = projectedPt;
                double offsetDist = 0;
                if (this.OffsetDistance != 0)
                {
                    var trackingLine = new Line(projectedPt, offsetPoint);
                    offsetPoint = trackingLine.PointAt(OffsetDistance);
                }
               

                offsetDist = projectedPt.DistanceTo(offsetPoint);

                //if (selEntity is Line line)
                //{
                //    var pline = new LinearPath(line.Vertices);
                //    var offsetCurve = pline.Offset(offsetDist, Vector3D.AxisZ, 0.01, true);
                //    success = offsetCurve.Project(offsetPoint, out t);
                //    projectedPt = offsetCurve.PointAt(t);
                //    if (projectedPt.DistanceTo(offsetPoint) > 1e-3)
                //        offsetCurve = pline.Offset(-offsetDist, Vector3D.AxisZ, 0.01, true);
                //    return offsetCurve as Entity;
                //}

                //return null;
                ICurve offsetCurve = selCurve.Offset(offsetDist, Vector3D.AxisZ, 0.01, true);
                success = offsetCurve.Project(offsetPoint, out t);
                projectedPt = offsetCurve.PointAt(t);
                var disc = projectedPt.DistanceTo(offsetPoint);
                if (disc > 1e-3)
                    offsetCurve = selCurve.Offset(-offsetDist, Vector3D.AxisZ, 0.01, true);
                return offsetCurve as Entity;
            }
            else if(selEntity is Joist2D joist)
            {
                double t;
                bool success = joist.Project(offsetPoint, out t);
                Point3D projectedPt = joist.PointAt(t);
                BasePoint = projectedPt;
                double offsetDist = 0;
                if (this.OffsetDistance != 0)
                {
                    var trackingLine = new Line(projectedPt, offsetPoint);
                    offsetPoint = trackingLine.PointAt(OffsetDistance);
                }
                offsetDist = projectedPt.DistanceTo(offsetPoint);
                
                var offsetJoist = joist.Offset(offsetDist, Vector3D.AxisZ, 0.01, true);
                success = offsetJoist.Project(offsetPoint, out t);
                projectedPt = offsetJoist.PointAt(t);
                if (projectedPt.DistanceTo(offsetPoint)>1e-3)
                {
                    offsetJoist = joist.Offset(-offsetDist, Vector3D.AxisZ, 0.01, true);
                }
                return offsetJoist as Entity;
            }
            

            return null;

        }

    }
}
