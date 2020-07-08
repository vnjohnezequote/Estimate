using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using devDept.Eyeshot;
using devDept.Geometry;
using DrawingModule.Application;
using DrawingModule.CommandClass;

namespace DrawingModule.CustomControl.PaperSpaceControl
{
    public class PaperSpaceDrawing: Drawings
    {
        public event DrawingToolChanged ToolChanged;
        //private IDrawInteractive _currentTool;
        //internal IDrawInteractive CurrentTool
        //{
        //    get => _currentTool;
        //    private set
        //    {
        //        SetProperty(ref _currentTool, value);
        //        ToolChanged?.Invoke(this, new ToolChangedArgs(value));
        //    }
        //}
        public ViewportList Viewports { get; }
        public int ActiveViewport { get; }
        public Point3D LastClickPoint { get; }
        public Plane DrawingPlane { get; }

        #region Constructor

        public PaperSpaceDrawing()
        {

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
