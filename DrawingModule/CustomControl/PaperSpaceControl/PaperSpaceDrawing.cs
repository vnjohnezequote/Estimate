using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using AppDataBase.DataBase;
using ApplicationInterfaceCore;
using AppModels;
using AppModels.EventArg;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.CustomControl.CanvasControl;
using MouseButton = System.Windows.Input.MouseButton;
using Point = System.Windows.Point;

namespace DrawingModule.CustomControl.PaperSpaceControl
{
    public class PaperSpaceDrawing: Drawings
    {
        #region Dependence Property
        
        #endregion
        #region Field

        #endregion

        #region Properties
        #endregion
        public event DrawingToolChanged ToolChanged;
        public ViewportList Viewports { get; }
        public int ActiveViewport { get; }
        public Point3D LastClickPoint { get; }
        public Plane DrawingPlane { get; }
        public Entity SelectedEntity { get; set; }
        private Point3D _startMovementPoint;
        private Point3D _currentMovementPoint;
        private bool _buttomPressed = false;

        
        public event EventHandler<EntityEventArgs> EntitiesSelectedChanged;

        #region Constructor

        public PaperSpaceDrawing() : base()
        {
           
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            _buttomPressed = false;
            if (_startMovementPoint!= null && _currentMovementPoint !=null && SelectedEntity!=null)
            {
                Vector3D movement = new Vector3D(_startMovementPoint,_currentMovementPoint);
                var x = _currentMovementPoint.X - _startMovementPoint.X;
                var y = _currentMovementPoint.Y - _startMovementPoint.Y;
                if (SelectedEntity is View view)
                {
                    view.X += x;
                    view.Y += y;
                }
                //SelectedEntity.Translate(movement);
                //SelectedEntity.TransformBy();
                Entities.Regen();
                this.Invalidate();
                _startMovementPoint = null;
                _currentMovementPoint = null;
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            var mousePosition = RenderContextUtility.ConvertPoint(GetMousePosition(e));
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                if (GetToolBar().Contains(mousePosition))
                {
                    base.OnMouseDown(e);

                    return;
                }
                var index = GetEntityUnderMouseCursor(mousePosition);
                if (index != -1)
                {
                    var view = Entities[index] as devDept.Eyeshot.Entities.View;
                    if (view!=null)
                    {
                        view.Selected = true;
                        SelectedEntity = view;
                    }
                    
                    var selectedViewArg = new EntityEventArgs(view);
                    if (EntitiesSelectedChanged != null)
                        EntitiesSelectedChanged.Invoke(this, selectedViewArg);
                }
                else
                {
                    if (SelectedEntity!=null)
                    {
                        SelectedEntity.Selected = false;
                        SelectedEntity = null;
                        EntitiesSelectedChanged.Invoke(this, null);
                    }
                    
                    
                }

            }
            else if(e.ChangedButton == System.Windows.Input.MouseButton.Middle)
            {
                if (SelectedEntity!=null)
                {
                    _buttomPressed = true;
                    this.ScreenToPlane(mousePosition, Plane.XY, out var currentPoint);
                    if (currentPoint!=null)
                    {
                        _startMovementPoint=currentPoint;
                    }
                }
            }

            base.OnMouseDown(e);

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var mousePosition = RenderContextUtility.ConvertPoint(GetMousePosition(e));
             this.ScreenToPlane(mousePosition, Plane.XY, out var currentPoint);
             _currentMovementPoint = currentPoint;

            PaintBackBuffer();
            SwapBuffers();
            base.OnMouseMove(e);
            
        }

        public void ProcessCommand(string commandString)
        {
            CommandThunk tempCommandThunk = null;
            if (ApplicationHolder.CommandClass.CommandThunks.Count > 0)
            {
                foreach (var commandThunk in ApplicationHolder.CommandClass.CommandThunks.Where(commandThunk => commandThunk.GlobalName == commandString))
                {
                    tempCommandThunk = commandThunk;
                    break;
                }
            }

            if (tempCommandThunk?.Invoker == null) return;
            tempCommandThunk.Invoker.DynamicInvoke();
            this.Focus();
            
        }

        protected override void DrawOverlay(Model.DrawSceneParams myParams)
        {
            if (SelectedEntity !=null && _startMovementPoint!=null && _buttomPressed && _currentMovementPoint!=null)
            {
                Entity tempEntity = (Entity)SelectedEntity.Clone();
                var x = _currentMovementPoint.X - _startMovementPoint.X;
                var y = _currentMovementPoint.Y - _startMovementPoint.Y;
                if (tempEntity is View view)
                {
                    view.X += x;
                    view.Y += y;
                }
                
                 //Vector3D movement = new Vector3D(_startMovementPoint,_currentMovementPoint);
                 //((Entity)tempEntity).Translate(movement);
                 DrawCurveOrBlockRef(tempEntity);
            }
            base.DrawOverlay(myParams);
        }

        private void DrawCurveOrBlockRef(Entity tempEntity)
        {
            if (tempEntity is BlockReference)
            {
                BlockReference br = (BlockReference)tempEntity;

                Entity[] entList = br.Explode(this.Blocks);

                foreach (var item in entList)
                {
                    if (item is ICurve iCurveItem)
                    {
                        Draw(iCurveItem);    
                    }
                    
                }
            }
        }

        private void Draw(ICurve theCurve)
        {
            if (theCurve is CompositeCurve)
            {
                CompositeCurve compositeCurve = theCurve as CompositeCurve;
                Entity[] explodedCurves = compositeCurve.Explode();
                foreach (Entity ent in explodedCurves)

                    DrawScreenCurve((ICurve)ent);
            }
            else
            {
                DrawScreenCurve(theCurve);
            }
        }
        private void DrawScreenCurve(ICurve curve)
        {
            const int subd = 100;

            Point3D[] pts = new Point3D[subd + 1];

            for (int i = 0; i <= subd; i++)
            {
                pts[i] = WorldToScreen(curve.PointAt(curve.Domain.ParameterAt((double)i / subd)));
            }

            renderContext.DrawLineStrip(pts);
        }
        #endregion

        #region private Method



        #endregion
        #region Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);

            return true;
        }
        protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);

            return true;
        }
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }
        #endregion
    }

}
