using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels;
using AppModels.EventArg;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.Application;
using DrawingModule.Enums;
using DrawingModule.UserInteractive;
using Line = devDept.Eyeshot.Entities.Line;

namespace DrawingModule.DrawToolBase
{
    public abstract class DimToolBase : ToolBase
    {

        public override Point3D BasePoint { get; protected set; }
        protected Point3D _startPoint;
        protected Point3D _endPoint;
        protected Point3D _dimPoint;
        protected Point3D _extPt1;
        protected Point3D _extPt2;
        protected Plane _storePlane;
        public DimToolBase()
        {
            this.EntityUnderMouseDrawingType = UnderMouseDrawingType.BySegment;
            this.IsSnapEnable = true;
            this.IsUsingOrthorMode = true;
            this.IsUsingSwitchMode = true;

        }

        public override void NotifyPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (this._startPoint == null && e.Key == Key.Space)
            {
                if (this.EntityUnderMouseDrawingType == UnderMouseDrawingType.ByObject)
                {
                    this.EntityUnderMouseDrawingType = UnderMouseDrawingType.BySegment;
                    return;
                }
                this.EntityUnderMouseDrawingType = UnderMouseDrawingType.ByObject;
            }
        }

        protected void OnProcessCommand()
        {
            var acEditor = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument.Editor;
            while (true)
            {
                this.ResetTool();
                var prePairForDim = PrepairForDim(acEditor);
                if (prePairForDim == CommandLoopType.BreakLoop) return;
                if (prePairForDim == CommandLoopType.ResetLoop) continue;
                if (_extPt1 != null && _extPt2 != null && _storePlane != null)
                {
                    ProcessedDim(_extPt1, _extPt2, _dimPoint, _storePlane);
                }
            }

        }
        protected virtual void ResetTool()
        {
            this._startPoint = null;
            this._endPoint = null;
            this._extPt1 = null;
            this._extPt2 = null;
            this._storePlane = null;
            this._dimPoint = null;
            IsUsingSwitchMode = true;
        }
        protected void ProcessedDim(Point3D p1, Point3D p2, Point3D p3, Plane storePlane, double dimTextHeight = 100.0)
        {
            var linearEntity = new LinearDim(storePlane, p1, p2, p3, dimTextHeight);
            linearEntity.LineTypeMethod = colorMethodType.byLayer;
            linearEntity.TextSuffix = "mm";
            //var testLinear = new LinearTest(linearEntity);

            EntitiesManager.AddAndRefresh(linearEntity, this.LayerManager.SelectedLayer.Name);
            //EntitiesManager.AddAndRefresh(testLinear, this.LayerManager.SelectedLayer.Name);
        }

        private CommandLoopType  PrepairEntityForDim(Editor editor)
        {
            ToolMessage = "Please Select object segment for dim, press SpaceBar to switch dim by Point Mode";
            var entityOption = new PromptSelectionOptions();
            var result = editor.GetSelection(entityOption);
            switch (result.Status)
            {
                case PromptStatus.Cancel:
                    return CommandLoopType.BreakLoop;
                case PromptStatus.SwitchMode:
                    if (result.Value == null)
                    {
                        if (result.ClickedPoint == null)
                        {
                            return CommandLoopType.ResetLoop;
                        }
                        this._startPoint = result.ClickedPoint;
                        //this.EntityUnderMouseDrawingType = UnderMouseDrawingType.ByObject;
                        var check = PrepairPointForDim(editor);
                        switch (check)
                        {
                            case CommandLoopType.BreakLoop:
                                return CommandLoopType.BreakLoop;
                            case CommandLoopType.Continues:
                                return CommandLoopType.Continues;
                            case CommandLoopType.ResetLoop:
                                return CommandLoopType.ResetLoop;
                        }
                    }
                    else 
                    {

                        this._startPoint = result.ClickedPoint;
                        var check = PrepairPointForDim(editor);
                        switch (check)
                        {
                            case CommandLoopType.BreakLoop:
                                return CommandLoopType.BreakLoop;
                            case CommandLoopType.Continues:
                                return CommandLoopType.Continues;
                            case CommandLoopType.ResetLoop:
                                return CommandLoopType.ResetLoop;
                        }

                    }

                    return CommandLoopType.ResetLoop;
            }

            var entityForDim = result.Value;
                if (entityForDim != null && entityForDim is Line line)
                {
                    this._startPoint = line.StartPoint;
                    this._endPoint = line.EndPoint;
                    return CommandLoopType.Continues;
                }
                else
                {
                    this._startPoint = result.ClickedPoint;
                var check = PrepairPointForDim(editor);
                switch (check)
                {
                    case CommandLoopType.BreakLoop:
                        return CommandLoopType.BreakLoop;
                    case CommandLoopType.Continues:
                        return CommandLoopType.Continues;
                    case CommandLoopType.ResetLoop:
                        return CommandLoopType.ResetLoop;
                }
            }

            return CommandLoopType.Continues;
        }

        public virtual CommandLoopType PrepairForDim(Editor editor)
        {
            if (EntityUnderMouseDrawingType == UnderMouseDrawingType.BySegment)
            {
                var checkMode = PrepairEntityForDim(editor);
                switch (checkMode)
                {
                    case CommandLoopType.BreakLoop:
                        return CommandLoopType.BreakLoop;
                    case CommandLoopType.ResetLoop:
                        return CommandLoopType.ResetLoop;
                }
            }
            else
            {
                var checkMode2 = PrepairPointForDim(editor);
                switch (checkMode2)
                {
                    case CommandLoopType.BreakLoop:
                        return CommandLoopType.BreakLoop;
                    case CommandLoopType.ResetLoop:
                        return CommandLoopType.ResetLoop;
                }
            }


            IsUsingSwitchMode = false;
                ToolMessage = "Please enter Dim Point Location";
                var promptPointOp = new PromptPointOptions(ToolMessage);
                var res = editor.GetPoint(promptPointOp);
                switch (res.Status)
                {
                    case PromptStatus.Cancel:
                        return CommandLoopType.BreakLoop;
                    case PromptStatus.SwitchMode:
                        return CommandLoopType.ResetLoop;
                    default:
                        _dimPoint = res.Value;
                        return CommandLoopType.Continues;
                }

        }
        public virtual CommandLoopType PrepairPointForDim(Editor editor)
        {
            var promptPointOp = new PromptPointOptions(ToolMessage);
            PromptPointResult res = null;
            if (this._startPoint == null)
            {
                ToolMessage = "Please enter first Point for Dim, press SpaceBar to switch dim by Entity mode";
                res = editor.GetPoint(promptPointOp);
                if (res.Status == PromptStatus.Cancel)
                {
                    //Application.DocumentManager.MdiActiveDocument.Editor.UnRegisterDrawInteractive(this);
                    return CommandLoopType.BreakLoop;
                }

                else if (res.Status == PromptStatus.SwitchMode)
                {
                    return CommandLoopType.ResetLoop;
                }
                _startPoint = res.Value;
            }
            
            IsUsingSwitchMode = false;
            ToolMessage = "Please enter Second Point for Dim";
            res = editor.GetPoint(promptPointOp);
            if (res.Status == PromptStatus.Cancel)
            {
                //Application.DocumentManager.MdiActiveDocument.Editor.UnRegisterDrawInteractive(this);
                return CommandLoopType.BreakLoop;
            }
            else if (res.Status == PromptStatus.SwitchMode)
            {
                return CommandLoopType.ResetLoop;
            }
            _endPoint = res.Value;
            return CommandLoopType.Continues;
        }
        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {

            DrawInteractiveDim((ICadDrawAble)sender, e);
        }

        protected abstract void DrawInteractiveDim(ICadDrawAble canvas, DrawInteractiveArgs e);
    }
}
