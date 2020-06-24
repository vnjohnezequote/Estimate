using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using CustomControls.Helper;

namespace DrawingModule.CommandLine
{
    public class NonTopmostPopup : Popup
    {
        public NonTopmostPopup()
        {
            this.ShowsBelowParent = false;
        }

        // Token: 0x17000290 RID: 656
        // (get) Token: 0x06000BE2 RID: 3042 RVA: 0x00028C8D File Offset: 0x00027C8D
        // (set) Token: 0x06000BE3 RID: 3043 RVA: 0x00028C95 File Offset: 0x00027C95
        public bool ShowsBelowParent { get; set; }

        // Token: 0x06000BE4 RID: 3044 RVA: 0x00028C9E File Offset: 0x00027C9E
        protected override void OnOpened(System.EventArgs e)
        {
            this.UpdateWindow();
            base.OnOpened(e);
        }

        // Token: 0x06000BE5 RID: 3045 RVA: 0x00028CB0 File Offset: 0x00027CB0
        private void UpdateWindow()
        {
            IntPtr handle = ((HwndSource)PresentationSource.FromVisual(base.Child)).Handle;
            IntPtr hwndInsertAfter = (IntPtr)(-2);
            if (this.ShowsBelowParent && base.Parent != null)
            {
                HwndSource hwndSource = PresentationSource.FromVisual(base.Parent as Visual) as HwndSource;
                if (hwndSource != null && hwndSource.RootVisual is Window)
                {
                    hwndInsertAfter = hwndSource.Handle;
                }
            }
            Win32.SetWindowPos(handle, hwndInsertAfter, 0, 0, 0, 0, 19u);
        }

        // Token: 0x06000BE6 RID: 3046 RVA: 0x00028D28 File Offset: 0x00027D28
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            bool isOpen = base.IsOpen;
            base.OnPreviewMouseLeftButtonDown(e);
            if (isOpen && !base.IsOpen)
            {
                e.Handled = true;
            }
        }

    }
}
