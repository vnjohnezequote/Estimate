using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using RawInputManager.Enum;

namespace RawInputManager.Class
{
    public class WPFRawInputManager : RawInput
    {
        private bool _hasFilter;

        public WPFRawInputManager(HwndSource hwndSource, RawInputCaptureMode captureMode)
            : base(hwndSource.Handle, captureMode)
        {
            hwndSource.AddHook(Hook);
        }

        public WPFRawInputManager(Visual visual, RawInputCaptureMode captureMode)
            : this(GetHwndSource(visual), captureMode)
        {
        }

        private static HwndSource GetHwndSource(Visual visual)
        {
            var source = PresentationSource.FromVisual(visual) as HwndSource;
            if (source == null)
            {
                throw new InvalidOperationException("Cannot find a valid HwndSource");
            }
            return source;
        }

        private IntPtr Hook(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            InputDriver.HandleMessage(msg, wparam, lparam);
            return IntPtr.Zero;
        }
        public override void AddMessageFilter()
        {
            if (_hasFilter)
            {
                return;
            }
            ComponentDispatcher.ThreadFilterMessage += OnThreadFilterMessage;
            _hasFilter = true;
        }
        public override void RemoveMessageFilter()
        {
            ComponentDispatcher.ThreadFilterMessage -= OnThreadFilterMessage;
            _hasFilter = false;
        }
        // ReSharper disable once RedundantAssignment
        private void OnThreadFilterMessage(ref MSG msg, ref bool handled)
        {
            handled = InputDriver.HandleMessage(msg.message, msg.wParam, msg.lParam);
            
        }
    }
}
