using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RawInputManager.Wind32
{
    public static class Win32Consts
    {
        
        public const int KEYBOARD_OVERRUN_MAKE_CODE = 255;
        public const int WM_APPCOMMAND = 793;
        public const int FAPPCOMMANDMASK = 61440;
        public const int FAPPCOMMANDMOUSE = 32768;
        public const int FAPPCOMMANDOEM = 4096;
        public const int WM_KEYDOWN = 256;
        public const int WM_KEYUP = 257;
        public const int WM_CHAR = 258;
        public const int WM_SYSKEYDOWN = 260;
        public const int WM_INPUT = 255;
        public const int WM_USB_DEVICECHANGE = 537;
        public const int WM_INPUT_DEVICE_CHANGE = 254;
        public const int PM_REMOVE = 1;
        public const int VK_SHIFT = 16;
        public const int RI_KEY_MAKE = 0;
        public const int RI_KEY_BREAK = 1;
        public const int RI_KEY_E0 = 2;
        public const int RI_KEY_E1 = 4;
        public const int VK_CONTROL = 17;
        public const int VK_MENU = 18;
        public const int VK_ZOOM = 251;
        public const int VK_LSHIFT = 160;
        public const int VK_RSHIFT = 161;
        public const int VK_LCONTROL = 162;
        public const int VK_RCONTROL = 163;
        public const int VK_LMENU = 164;
        public const int VK_RMENU = 165;
        public const int SC_SHIFT_R = 54;
        public const int SC_SHIFT_L = 42;
        public const int RIM_INPUT = 0;
        
    }
}
