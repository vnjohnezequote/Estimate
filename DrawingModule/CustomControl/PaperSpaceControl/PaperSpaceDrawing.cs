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
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.CustomControl.CanvasControl;

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
        public event EventHandler<EntityEventArgs> EntitiesSelectedChanged;

        #region Constructor

        public PaperSpaceDrawing()
        {

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
                    }
                    
                    var selectedViewArg = new EntityEventArgs(view);
                    if (EntitiesSelectedChanged != null)
                        EntitiesSelectedChanged.Invoke(this, selectedViewArg);
                }

            }

            base.OnMouseDown(e);

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
