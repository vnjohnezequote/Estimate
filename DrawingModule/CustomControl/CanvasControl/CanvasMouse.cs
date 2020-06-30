using System;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.EventArg;
using devDept.Eyeshot;
using devDept.Graphics;
using DrawingModule.Application;
using DrawingModule.DrawToolBase;
using DrawingModule.Helper;
using DrawingModule.Interface;
using DrawingModule.Views;
using MouseButton = System.Windows.Input.MouseButton;

namespace DrawingModule.CustomControl.CanvasControl
{
   public partial class CanvasDrawing
    {
        private event MouseEventHandler MouseMove_Drawing;
        private event MouseButtonEventHandler MouseUp_Drawing;
        private event MouseButtonEventHandler MouseDown_Drawing;
        private event DrawInteractiveDelegate DrawOverlay_Jigging;
        public event DrawingToolChanged ToolChanged;
        internal event Action UserInteraction;
        private System.Drawing.Point _mousePosition;
        private IDrawInteractive _currentTool;
        public IDynamicInputView DynamicInput { get; private set; }

        internal IDrawInteractive CurrentTool
        {
            get => _currentTool;
            private set
            {
                SetProperty(ref _currentTool, value);
                ToolChanged?.Invoke(this,new ToolChangedArgs(value));
                

            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            _mousePosition = RenderContextUtility.ConvertPoint(this.GetMousePosition(e));
            this.CurrentPoint = GetCurrentPoint(_mousePosition);
            CurrentIndex = this.GetEntityUnderMouseCursor(_mousePosition, false);
            _entityUnderMouse = CurrentIndex > -1 ? this.Entities[CurrentIndex] : null;
            if (!IsProcessingTool && this._selectTool.StartPoint != null)
            {
                this._selectTool.ProcessMouseMoveForSelection(e, this);
            }
            else if (this.IsProcessingTool && this._currentTool!=null)
            {
                if (_waitingForSelection && this._selectTool.StartPoint != null)
                {
                    this._selectTool.ProcessMouseMoveForSelection(e, this);
                }
                else 
                {
                    if(IsOrthoModeEnable)
                        if (this.CurrentTool.IsUsingOrthorMode && this.CurrentTool.BasePoint!=null)
                        {
                            CurrentPoint = Utils.GetEndPoint(this.CurrentTool.BasePoint, CurrentPoint);
                        }
                    if (IsSnappingEnable)
                        this.SetCurrentPoint(_mousePosition);
                }
                this.OnMouseMove_Drawing(e);
               
            }
            PaintBackBuffer();
            SwapBuffers();
            base.OnMouseMove(e);

        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            var mousePosition = RenderContextUtility.ConvertPoint(this.GetMousePosition(e));
            var ent = GetEntityUnderMouseCursor(mousePosition);
            this.LastClickPoint = CurrentPoint;
            var check = ent != -1;
            if (this.ActionMode == actionType.None && e.ChangedButton == MouseButton.Left)
            {
                if (IsProcessingTool)
                {
                    if (_waitingForSelection)
                    {
                        this.ProcessMouseDownForSelectionTool(e, check);
                        if (_waitingForPickSelection)
                        {
                            if (this.EntitiesManager.SelectedEntity!=null)
                            {
                                this.PromptStatus = PromptStatus.OK;
                                _waitingForPickSelection = false;
                            }
                        }
                    }
                    else
                    {
                        this.PromptStatus = PromptStatus.OK;
                        this.IsUserInteraction = true;
                    }

                    if (_currentTool!=null)
                    {
                        this.OnMouseDown_Drawing(e);
                    }
                }
                else
                {
                    this.ProcessMouseDownForSelectionTool(e, check);
                }

            }

            base.OnMouseDown(e);
        }
        protected void OnMouseMove_Drawing(MouseEventArgs e)
        {
            MouseMove_Drawing?.Invoke(this, e);
        }
        protected void OnMouseUp_Drawing(MouseButtonEventArgs e)
        {
            MouseUp_Drawing?.Invoke(this, e);
        }
        protected void OnMouseDown_Drawing(MouseButtonEventArgs e)
        {
            MouseDown_Drawing?.Invoke(this, e);
        }
        protected void DrawOverlay_Drawing(DrawInteractiveArgs data)
        {
            DrawOverlay_Jigging?.Invoke(this, data);
        }
        internal void SetDrawing(IDrawInteractive tool)
        {
            CurrentTool = tool;
            if (this.DynamicInput!=null)
            {
                CurrentTool.SetDynamicInput((IDynamicInputView)DynamicInput);
            }

            if (this.EntitiesManager!=null)
            {
                CurrentTool.SetEntitiesManager((IEntitiesManager) EntitiesManager);
            }

            if (this.LayersManager!=null)
            {
                CurrentTool.SetLayersManager((ILayerManager) LayersManager);
            }
            
            IsProcessingTool = true;
            _isUserInteraction = false;
            MouseMove_Drawing += tool.NotifyMouseMove;
            MouseUp_Drawing += tool.NotifyMouseUp;
            MouseDown_Drawing += tool.NotifyMouseDown;
            PreviewKeybordDown_Drawing += tool.NotifyPreviewKeyDown;
            DrawOverlay_Jigging += tool.OnJigging;
            
        }
        internal void ReleaseDrawing()
        {
            if (this._currentTool!=null)
            {
                MouseMove_Drawing -= _currentTool.NotifyMouseMove;
                MouseUp_Drawing -= _currentTool.NotifyMouseUp;
                MouseDown_Drawing -= _currentTool.NotifyMouseDown;
                DrawOverlay_Jigging -= _currentTool.OnJigging;
                PreviewKeybordDown_Drawing -= _currentTool.NotifyPreviewKeyDown;
                CurrentTool.SetDynamicInput(null);
                CurrentTool.SetEntitiesManager(null);
                CurrentTool.SetLayersManager(null);
                CurrentTool = null;
            }
            IsProcessingTool = false;
            this.ResetProcessingTool();


        }


        private void OnUserInteractionMessage()
        {
            UserInteraction?.Invoke();
        }

        






    }
}
