using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using RawInputManager.Enum;
using RawInputManager.Structure;
using RawInputManager.Wind32;

namespace RawInputManager.Class
{
    public sealed class RawInputDriver : IDisposable
    {
        public int NumberOfKeyboards { get; private set; }
        public int NumberOfMouses { get; private set; }
        private IntPtr _devNotifyHandle;
        private readonly Dictionary<IntPtr, DeviceInfo> _deviceList = new Dictionary<IntPtr, DeviceInfo>();
        private readonly object _lock = new object();
        public event EventHandler<KeyboardInputEventArgs> KeyPressed;
        public event EventHandler<MouseInputEventArgs> MouseClick;
        public RawInputDriver(IntPtr hwnd, bool captureOnlyInForeground)
        {
            RawInputDevice[] array =
            {
                new RawInputDevice
                {
                    UsagePage = HidUsagePage.Generic,
                    Usage = HidUsage.Keyboard,
                    Flags = (captureOnlyInForeground ? RawInputDeviceFlags.NONE : RawInputDeviceFlags.INPUTSINK) | RawInputDeviceFlags.DEVNOTIFY,
                    Target = hwnd
                },
                new RawInputDevice
                {
                    UsagePage = HidUsagePage.Generic,
                    Usage =  HidUsage.Mouse,
                    Flags = (captureOnlyInForeground ? RawInputDeviceFlags.NONE : RawInputDeviceFlags.INPUTSINK) | RawInputDeviceFlags.DEVNOTIFY,
                    Target = hwnd
                },
            };
            if (!Win32Methods.RegisterRawInputDevices(array, (uint)array.Length, (uint)Marshal.SizeOf(array[0])))
            {
                throw new ApplicationException("Failed to register raw input device(s).", new Win32Exception());
            }
            if (!Win32Methods.RegisterRawInputDevices(array, (uint)array.Length, (uint)Marshal.SizeOf(array[1])))
            {
                throw new ApplicationException("Failed to register raw input device(s).", new Win32Exception());
            }
            EnumerateDevices();
        }

        private void EnumerateDevices()
        {
            lock (_lock)
            {
                _deviceList.Clear();
                uint devices = 0u;
                int size = Marshal.SizeOf(typeof(RawInputDeviceList));
                if (Win32Methods.GetRawInputDeviceList(IntPtr.Zero, ref devices, (uint)size) != 0u)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                IntPtr pRawInputDeviceList = Marshal.AllocHGlobal((int)(size * devices));
                try
                {
                    Win32Methods.GetRawInputDeviceList(pRawInputDeviceList, ref devices, (uint)size);
                    int index = 0;
                    int mouseCount = 0;
                    int keyboardCount = 0;
                    while (index < devices)
                    {
                        DeviceInfo device = this.GetDevice(pRawInputDeviceList, size, index);
                        if (device != null && !_deviceList.ContainsKey(device.DeviceHandle))
                        {
                            _deviceList.Add(device.DeviceHandle, device);
                            if (device.DeviceType == RawInputType.Keyboard)
                            {
                                keyboardCount++;
                            }
                            else if (device.DeviceType == RawInputType.Mouse)
                            {
                                mouseCount++;
                            }
                        }
                        index++;
                    }

                    this.NumberOfKeyboards = keyboardCount;
                    this.NumberOfMouses = mouseCount;
                }
                finally
                {
                    Marshal.FreeHGlobal(pRawInputDeviceList);
                }
                
            }

        }

        private DeviceInfo GetDevice(IntPtr pRawInputDeviceList, int dwSize, int index)
        {
            uint size = 0u;
            // On Window 8 64bit when compiling against .Net > 3.5 using .ToInt32 you will generate an arithmetic overflow. Leave as it is for 32bit/64bit applications
            var rawInputDeviceList = (RawInputDeviceList)Marshal.PtrToStructure(new IntPtr(pRawInputDeviceList.ToInt64() + dwSize * index), typeof(RawInputDeviceList));
            Win32Methods.GetRawInputDeviceInfo(rawInputDeviceList.hDevice, RawInputDeviceInfo.RIDI_DEVICENAME, IntPtr.Zero, ref size);
            if (size <= 0u)
            {
                return null;
            }
            IntPtr intPtr = Marshal.AllocHGlobal((int)size);

            try
            {
                Win32Methods.GetRawInputDeviceInfo(rawInputDeviceList.hDevice, RawInputDeviceInfo.RIDI_DEVICENAME, intPtr, ref size);

                string device = Marshal.PtrToStringAnsi(intPtr);
                if (rawInputDeviceList.dwType == DeviceType.RimTypekeyboard ||
                    rawInputDeviceList.dwType == DeviceType.RimTypeHid || rawInputDeviceList.dwType == DeviceType.RimTypemouse)
                {
                    string deviceDescription = Win32Methods.GetDeviceDescription(device);
                    DeviceInfo deviceInfor = null;

                    uint infoSize = (uint)(Marshal.SizeOf(typeof(Structure.DeviceInfo)));
                    Win32Methods.GetRawInputDeviceInfo(rawInputDeviceList.hDevice, RawInputDeviceInfo.RIDI_DEVICEINFO, IntPtr.Zero, ref infoSize);
                    if (infoSize > 0)
                    {
                        IntPtr pInfoData = Marshal.AllocHGlobal((int)infoSize);
                        try
                        {
                            Win32Methods.GetRawInputDeviceInfo(rawInputDeviceList.hDevice, RawInputDeviceInfo.RIDI_DEVICEINFO, pInfoData, ref infoSize);
                            Structure.DeviceInfo rawDeviceInfo = (Structure.DeviceInfo)Marshal.PtrToStructure(pInfoData, typeof(Structure.DeviceInfo));
                            deviceInfor = DeviceInfo.Convert(ref rawDeviceInfo, (string)Marshal.PtrToStringAnsi(intPtr), rawInputDeviceList.hDevice);
                            deviceInfor.Description = deviceDescription;
                            deviceInfor.DeviceType = GetDeviceType((int)rawInputDeviceList.dwType);
                            return deviceInfor;
                        }
                        finally
                        {
                            if (pInfoData != IntPtr.Zero)
                            {
                                Marshal.FreeHGlobal(pInfoData);
                            }
                        }

                    }


                }
            }
            finally
            {
                if (intPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(intPtr);
                }
            }
            return null;
        }

        private RawInputType GetDeviceType(int device)
        {

            switch (device)
            {
                case (int)RawInputType.Mouse: return RawInputType.Mouse;
                case (int)RawInputType.Keyboard: return RawInputType.Keyboard;
                case (int)RawInputType.HID: return RawInputType.HID;
                default: return RawInputType.Other;
            }

        }

        public bool HandleMessage(int msg, IntPtr wparam, IntPtr lparam)
        {
            switch (msg)
            {

                case Win32Consts.WM_INPUT:
                    return ProcessRawInput(lparam);
            }
            return false;
        }

        private bool ProcessRawInput(IntPtr hdevice)
        {
            if (_deviceList.Count == 0)
            {
                return false;
            }

            int size = 0;
            Win32Methods.GetRawInputData(hdevice, DataCommand.RID_INPUT, IntPtr.Zero, ref size, Marshal.SizeOf(typeof(RawInputHeader)));
            InputData rawBuffer;
            if (Win32Methods.GetRawInputData(hdevice, DataCommand.RID_INPUT, out rawBuffer, ref size, Marshal.SizeOf(typeof(RawInputHeader))) != size)
            {
                Debug.WriteLine("Error getting the rawinput buffer");
                return false;
            }

            if (rawBuffer.header.dwType == RawInputType.Keyboard)
            {
                if (rawBuffer.data.keyboard.Message == WindowsMessages.KEYDOWN || rawBuffer.data.keyboard.Message == WindowsMessages.SYSKEYDOWN)
                {
                    int makecode = (int)rawBuffer.data.keyboard.MakeCode;
                    int flags = (int)rawBuffer.data.keyboard.Flags;
                    int vKey = (int)rawBuffer.data.keyboard.VirtualKey;

                    // On most keyboards, "extended" keys such as the arrow or 
                    // page keys return two codes - the key's own code, and an
                    // "extended key" flag, which translates to 255. This flag
                    // isn't useful to us, so it can be disregarded.
                    if (vKey == Win32Consts.KEYBOARD_OVERRUN_MAKE_CODE)
                    {
                        return false;
                    }
                    DeviceInfo keyBoard;
                    lock (_lock)
                    {
                        if (!_deviceList.TryGetValue(rawBuffer.header.hDevice, out keyBoard))
                        {
                            Debug.WriteLine("Handle: {0} was not in the device list.", rawBuffer.header.hDevice);
                            return false;
                        }
                    }
                    var isE0BitSet = ((flags & Win32Consts.RI_KEY_E0) != 0);
                    bool isBreakBitSet = (flags & Win32Consts.RI_KEY_BREAK) != 0;
                    WindowsMessages message = rawBuffer.data.keyboard.Message;
                    Key key = KeyInterop.KeyFromVirtualKey(AdjustVirtualKey(rawBuffer, vKey, isE0BitSet, makecode));

                    EventHandler<KeyboardInputEventArgs> keyPressed = KeyPressed;
                    if (keyPressed != null)
                    {
                        var rawKeyBoardInputEventArgs = new KeyboardInputEventArgs(ref rawBuffer,rawBuffer.header.hDevice, isBreakBitSet ? KeyState.KeyDown : KeyState.KeyUp,
                           key);
                        keyPressed(this, rawKeyBoardInputEventArgs);
                        if (rawKeyBoardInputEventArgs.Handled)
                        {
                            MSG msg;
                            Win32Methods.PeekMessage(out msg, IntPtr.Zero, Win32Consts.WM_KEYDOWN, Win32Consts.WM_KEYUP, Win32Consts.PM_REMOVE);
                        }

                        return rawKeyBoardInputEventArgs.Handled;
                    }

                }

                if (rawBuffer.data.keyboard.Message == WindowsMessages.KEYUP || rawBuffer.data.keyboard.Message == WindowsMessages.SYSKEYUP)
                {

                }
            }
            else if (rawBuffer.header.dwType == RawInputType.Mouse)
            {
                RawMouseButtons wasMouseDown = rawBuffer.data.mouse.ButtonFlags;
                if (wasMouseDown == 0 || wasMouseDown == RawMouseButtons.LeftUp || wasMouseDown == RawMouseButtons.MiddleUp || wasMouseDown == RawMouseButtons.RightUp || wasMouseDown == RawMouseButtons.Button4Up || wasMouseDown == RawMouseButtons.Button5Up || wasMouseDown == RawMouseButtons.None)
                {
                    return false;
                }
                DeviceInfo mouse;
                lock (_lock)
                {
                    if (!_deviceList.TryGetValue(rawBuffer.header.hDevice,out mouse))
                    {
                        Debug.WriteLine("Handle: {0} was not in the device list.", rawBuffer.header.hDevice);
                        return false;
                    }
                }


                EventHandler<MouseInputEventArgs> mouseClick = MouseClick;
                if (mouseClick !=null)
                {
                    var mouseInputEventArgs = new MouseInputEventArgs(ref rawBuffer,rawBuffer.header.hDevice);
                    mouseClick(this, mouseInputEventArgs);
                }

            }


            return false;
        }

        private static int AdjustVirtualKey(InputData rawBuffer, int virtualKey, bool isE0BitSet, int makeCode)
        {
            var adjustedKey = virtualKey;

            if (rawBuffer.header.hDevice == IntPtr.Zero)
            {
                // When hDevice is 0 and the vkey is VK_CONTROL indicates the ZOOM key
                if (rawBuffer.data.keyboard.VirtualKey == (VirtualKeys)Win32Consts.VK_CONTROL)
                {
                    adjustedKey = Win32Consts.VK_ZOOM;
                }
            }
            else
            {
                switch (virtualKey)
                {
                    // Right-hand CTRL and ALT have their e0 bit set 
                    case Win32Consts.VK_CONTROL:
                        adjustedKey = isE0BitSet ? Win32Consts.VK_RCONTROL : Win32Consts.VK_LCONTROL;
                        break;
                    case Win32Consts.VK_MENU:
                        adjustedKey = isE0BitSet ? Win32Consts.VK_RMENU : Win32Consts.VK_LMENU;
                        break;
                    case Win32Consts.VK_SHIFT:
                        adjustedKey = makeCode == Win32Consts.SC_SHIFT_R ? Win32Consts.VK_RSHIFT : Win32Consts.VK_LSHIFT;
                        break;
                    default:
                        adjustedKey = virtualKey;
                        break;
                }
            }

            return adjustedKey;
        }


        ~RawInputDriver()
        {
            Dispose();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if (_devNotifyHandle != IntPtr.Zero)
            {
                Win32Methods.UnregisterDeviceNotification(_devNotifyHandle);
                _devNotifyHandle = IntPtr.Zero;
            }
        }
    }
}
