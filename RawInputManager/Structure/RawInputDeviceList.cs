using System;
using System.Runtime.InteropServices;

namespace RawInputManager.Structure
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawInputDeviceList
    {
        public IntPtr hDevice;
        public uint dwType;
    }
}