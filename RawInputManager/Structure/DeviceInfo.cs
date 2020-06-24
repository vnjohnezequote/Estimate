using System.Runtime.InteropServices;
using RawInputManager.Enum;

namespace RawInputManager.Structure
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct DeviceInfo
    {
        [FieldOffset(0)] public int Size;
        [FieldOffset(4)] public RawInputType Type;

        [FieldOffset(8)] public DeviceInfoMouse MouseInfo;
        [FieldOffset(8)] public DeviceInfoKeyboard KeyboardInfo;
        [FieldOffset(8)] public DeviceInfoHid HIDInfo;

        public override string ToString()
        {
            return string.Format("DeviceInfo\n Size: {0}\n Type: {1}\n", Size, Type);
        }
    }
}