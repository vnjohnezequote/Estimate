using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RawInputManager.Enum;
using RawInputManager.Structure;

namespace RawInputManager.Class
{
    public abstract class RawInput : IDisposable
    {
        private readonly RawInputDriver _inputDriver;
        public int NumberOfKeyboards
        {
            get { return InputDriver.NumberOfKeyboards; }
        }
        public int NumberOfMouses
        {
            get { return InputDriver.NumberOfMouses; }
        }
        public event  EventHandler<KeyboardInputEventArgs> KeyPress
        {
            add { InputDriver.KeyPressed += value;}
            remove { InputDriver.KeyPressed -= value; }
        }
        public event  EventHandler<MouseInputEventArgs> MousePress
        {
            add { InputDriver.MouseClick += value; }
            remove { InputDriver.MouseClick -= value; }
        }
        protected RawInputDriver InputDriver
        {
            get { return _inputDriver; }
        }
        protected RawInput(IntPtr handle, RawInputCaptureMode captureMode)
        {
            _inputDriver = new RawInputDriver(handle, captureMode == RawInputCaptureMode.Foreground);
        }
        public void Dispose()
        {
            
        }
        public abstract void AddMessageFilter();
        public abstract void RemoveMessageFilter();
    }
}
