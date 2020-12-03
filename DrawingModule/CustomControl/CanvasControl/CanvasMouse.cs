using System;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.EventArg;
using AppModels.Interaface;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.Application;
using DrawingModule.DrawToolBase;
using DrawingModule.Helper;
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
                if (CurrentTool != null) return;
                this.PolaTrackingPoints.Clear();
                PolaTrackedPoint = null;
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            //PaintBackBuffer();
            //SwapBuffers();
            _mousePosition = RenderContextUtility.ConvertPoint(this.GetMousePosition(e));
            this.CurrentPoint = GetCurrentPoint(_mousePosition);
            CurrentIndex = this.GetEntityUnderMouseCursor(_mousePosition, true);
            _entityUnderMouse = CurrentIndex > -1 ? this.Entities[CurrentIndex] : null;
            if (_entityUnderMouse is PictureEntity picture)
            {
                bool isPointInsidePicture = Utility.PointInRect(CurrentPoint,picture.BoxMin,picture.BoxMax);
                //picture.Visible = false;
                if (isPointInsidePicture)
                {
                    _entityUnderMouse = null;
                }
            }
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

        private bool GetEntitiesUnderMouse(System.Drawing.Point mousePosition)
        {
            var entIndex = GetEntityUnderMouseCursor(mousePosition);
            if (entIndex > -1)
            {
                if (entIndex > this.Entities.Count-1)
                {
                    _entityUnderMouse = null;
                    return false;
                }
                _entityUnderMouse = this.Entities[entIndex];
                if (_entityUnderMouse is Picture picture)
                {
                    bool isPointInsidePicture = Utility.PointInRect(CurrentPoint, picture.BoxMin, picture.BoxMax);
                    //picture.Visible = false;
                    if (isPointInsidePicture)
                    {
                        _entityUnderMouse = null;
                        return false;
                    }
                }

                return true;
            }
            else
            {
                _entityUnderMouse = null;
                return false;
            }
        }
        //private List<Point3D> test = new List<Point3D>();
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            var mousePosition = RenderContextUtility.ConvertPoint(this.GetMousePosition(e));
            var check = GetEntitiesUnderMouse(mousePosition);
            
            _isUserClicked = true;
            this.LastClickPoint = CurrentPoint;
            //test.Add(LastClickPoint);

            if (this.ActionMode == actionType.None && e.ChangedButton == MouseButton.Left)
            {
                this.PolaTrackingPoints.Clear();
                if (IsProcessingTool)
                {
                    if (_currentTool != null)
                    {
                        this.OnMouseDown_Drawing(e);
                    }
                    if (_waitingForSelection)
                    {
                        this.ProcessMouseDownForSelectionTool(e, check);
                        if (_waitingForPickSelection)
                        {
                            if (this.EntitiesManager.SelectedEntity!=null)
                            {
                                if (this.CurrentTool.IsUsingSwitchMode)
                                {
                                    if (Utils.IsEntityContainPoint3D(EntitiesManager.SelectedEntity.Entity as Entity, LastClickPoint))
                                    {
                                        this.PromptStatus = PromptStatus.SwitchMode;
                                        _waitingForPickSelection = false;
                                    }
                                    else
                                    {
                                        this.PromptStatus = PromptStatus.OK;
                                        _waitingForPickSelection = false;
                                    }
                                }
                                else
                                {
                                    this.PromptStatus = PromptStatus.OK;
                                    _waitingForPickSelection = false;
                                }
                                
                            }
                            else if(this.CurrentTool.IsUsingSwitchMode)
                            {
                                if (this._selectTool.StartPoint!= null)
                                {
                                    this._selectTool.StartPoint = null;
                                }
                                this.PromptStatus = PromptStatus.SwitchMode;
                                _waitingForPickSelection = false;
                            }
                        }
                    }
                    else
                    {
                        this.PromptStatus = PromptStatus.OK;
                        this.IsUserInteraction = true;
                    }
                }
                else
                {
                    if (!ActiveViewport.ToolBar.Contains(mousePosition))
                    {
                        this.ProcessMouseDownForSelectionTool(e, check);
                    }
                    
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

            CurrentTool.ActiveLevel = this.ActiveLevel;
            if (this.JobModel !=null)
            {
                CurrentTool.JobModel = this.JobModel;
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
