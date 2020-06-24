using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using RawInputManager.Enum;
using RawInputManager.Structure;

namespace RawInputManager.Wind32
{
    internal static class Win32Methods
    {
        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PeekMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax,
            uint wRemoveMsg);

        [DllImport("user32", SetLastError = true)]
        public static extern int GetRawInputData(IntPtr hRawInput, DataCommand command, out InputData buffer,
            [In] [Out] ref int size, int cbSizeHeader);

        [DllImport("user32", SetLastError = true)]
        public static extern int GetRawInputData(IntPtr hRawInput, DataCommand command, [Out] IntPtr pData,
            [In] [Out] ref int size, int sizeHeader);

        [DllImport("user32", SetLastError = true)]
        public static extern uint GetRawInputDeviceInfo(IntPtr hDevice, RawInputDeviceInfo command, IntPtr pData,
            ref uint size);

        [DllImport("user32")]
        public static extern uint GetRawInputDeviceInfo(IntPtr hDevice, RawInputDeviceInfo command, ref DeviceInfo data,
            ref uint dataSize);

        [DllImport("user32", SetLastError = true)]
        public static extern uint GetRawInputDeviceList(IntPtr pRawInputDeviceList, ref uint numberDevices, uint size);

        [DllImport("user32", SetLastError = true)]
        public static extern bool RegisterRawInputDevices(RawInputDevice[] pRawInputDevice, uint numberDevices,
            uint size);

        [DllImport("user32", SetLastError = true)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr notificationFilter,
            DeviceNotification flags);

        [DllImport("user32", SetLastError = true)]
        public static extern bool UnregisterDeviceNotification(IntPtr handle);
        public static int LoWord(int dwValue)
        {
            return (dwValue & 0xFFFF);
        }

        public static int HiWord(Int64 dwValue)
        {
            return (int)(dwValue >> 16) & ~Win32Consts.FAPPCOMMANDMASK;
        }

        public static ushort LowWord(uint val)
        {
            return (ushort)val;
        }

        public static ushort HighWord(uint val)
        {
            return (ushort)(val >> 16);
        }

        public static uint BuildWParam(ushort low, ushort high)
        {
            return ((uint)high << 16) | low;
        }
        public static string GetDeviceDescription(string device)
        {
            var deviceKey = RegistryAccess.GetDeviceKey(device);
            if (deviceKey == null) return string.Empty;

            string text = deviceKey.GetValue("DeviceDesc").ToString();
            return text.Substring(text.IndexOf(';') + 1);
        }

        public static bool InputInForeground(IntPtr wparam)
        {
            return wparam.ToInt32() == 0;
        }
    }
}
