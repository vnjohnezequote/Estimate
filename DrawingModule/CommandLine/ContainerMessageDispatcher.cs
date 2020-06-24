using System;
using System.Windows.Interop;

namespace DrawingModule.CommandLine
{
    public delegate void ContainerMessageEventHandler(ref MSG msg, ref bool handled);
    public static class ContainerMessageDispatcher
    {
        public static void RaiseWin32Message(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            MSG msg = default(MSG);
            msg.hwnd = hWnd;
            msg.message = message;
            msg.wParam = wParam;
            msg.lParam = lParam;
            if (ContainerMessageDispatcher.Win32Message != null)
            {
                ContainerMessageDispatcher.Win32Message(ref msg, ref handled);
            }
        }
        public static event ContainerMessageEventHandler Win32Message;
    }
}
