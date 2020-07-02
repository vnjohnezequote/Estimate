using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RawInputManager.Enum;
using RawInputManager.Structure;

namespace RawInputManager.Class
{
    public class KeyboardInputEventArgs : RawInputEventArgs
    {
        public KeyboardInputEventArgs()
        {
        }
        internal KeyboardInputEventArgs(ref InputData rawInput, IntPtr hwnd) : base(ref rawInput, hwnd)
        {
            this.VirtualKey = rawInput.data.keyboard.VirtualKey;
            this.MakeCode = (int)rawInput.data.keyboard.MakeCode;
            this.ScanCodeFlags = rawInput.data.keyboard.Flags;
            this.Messages = rawInput.data.keyboard.Message;
            this.ExtraInformation = rawInput.data.keyboard.ExtraInformation;
        }

        internal KeyboardInputEventArgs(ref InputData rawInput, IntPtr hwd, KeyState keyState,Key key)
        {
            this.Device = rawInput.header.hDevice;
            this.WindowHandle = hwd;
            this.Key = key;
            this.MakeCode = (int)rawInput.data.keyboard.MakeCode;
            this.ScanCodeFlags = rawInput.data.keyboard.Flags;
            this.Messages = rawInput.data.keyboard.Message;
            this.ExtraInformation = rawInput.data.keyboard.ExtraInformation;
            this.KeyState = keyState;
            this.VirtualKey = rawInput.data.keyboard.VirtualKey;
        }
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>

        public Key Key { get; set; }

        /// <summary>
        /// Gets or sets the make code.
        /// </summary>
        /// <value>
        /// The make code.
        /// </value>

        public int MakeCode { get; set; }

        /// <summary>
        /// Gets or sets the scan code flags.
        /// </summary>
        /// <value>
        /// The scan code flags.
        /// </value>
        public RawKeyboardFlags ScanCodeFlags { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public KeyState KeyState { get; set; }
        public WindowsMessages Messages { get; set; }
        public VirtualKeys VirtualKey { get; private set; }
        public bool Handled { get; set; }

        /// <summary>
        /// Gets or sets the extra information.
        /// </summary>
        /// <value>
        /// The extra information.
        /// </value>

        public int ExtraInformation { get; set; }
    }
}
