using System;
using System.Runtime.InteropServices;

namespace DrawingModule.CommandLine
{
    public sealed class DocumentActivationChangedEventArgs: System.EventArgs
    {
        private bool m_newvalue;

        public bool NewValue
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                return m_newvalue;
            }
        }

        internal DocumentActivationChangedEventArgs([MarshalAs(UnmanagedType.U1)] bool newvalue)
        {
            this.m_newvalue = newvalue;
        }
    }
}
