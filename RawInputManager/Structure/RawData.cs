using System.Runtime.InteropServices;

namespace RawInputManager.Structure
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct RawData
    {
        [FieldOffset(0)] internal RawMouse mouse;
        [FieldOffset(0)] internal RawKeyboard keyboard;
        [FieldOffset(0)] internal RawHid hid;
    }
}