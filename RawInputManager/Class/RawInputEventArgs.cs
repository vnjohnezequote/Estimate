using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RawInputManager.Structure;

namespace RawInputManager.Class
{
    public abstract class RawInputEventArgs : EventArgs
    {
        protected RawInputEventArgs()
        {
        }
        internal RawInputEventArgs(ref InputData rawInput, IntPtr hwnd)
        {
            this.Device = rawInput.header.hDevice;
            this.WindowHandle = hwnd;
        }
        /// <summary>
        /// Gets or sets the RawInput device.
        /// </summary>
        /// <value>
        /// The device.
        /// </value>
        public IntPtr Device { get; set; }

        /// <summary>
        /// Gets or sets the handle of the window that received the RawInput mesage.
        /// </summary>
        /// <value>
        /// The window handle.
        /// </value>
        public IntPtr WindowHandle { get; set; }
    }
}
