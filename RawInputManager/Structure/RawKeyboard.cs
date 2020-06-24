using System.Runtime.InteropServices;
using RawInputManager.Enum;

namespace RawInputManager.Structure
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RawKeyboard
    {
        /// <summary>Scan code for key depression.</summary>
        public short MakeCode;
        /// <summary>Scan code information.</summary>
        public RawKeyboardFlags Flags;
        /// <summary>Reserved.</summary>
        public short Reserved;
        /// <summary>Virtual key code.</summary>
        public VirtualKeys VirtualKey;
        /// <summary>Corresponding window message.</summary>
        public WindowsMessages Message;
        /// <summary>Extra information.</summary>
        public int ExtraInformation;
        public override string ToString()
        {
            return
                string.Format(
                    "Rawkeyboard\n Makecode: {0}\n Makecode(hex) : {0:X}\n Flags: {1}\n Reserved: {2}\n VKeyName: {3}\n Message: {4}\n ExtraInformation {5}\n",
                    MakeCode, Flags, Reserved, VirtualKey, Message, ExtraInformation);
        }
    }
}