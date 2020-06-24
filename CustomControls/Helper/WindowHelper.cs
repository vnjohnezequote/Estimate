using System.Windows;

namespace CustomControls.Helper
{
    public static class WindowHelper
    {
        /// <summary>
        /// Get Parent of userControl
        /// </summary>
        /// <param name="userControl"></param>
        /// <returns></returns>
        public static FrameworkElement GetWindowParent(FrameworkElement userControl)
        {
            var parentWindow = userControl;
            while (parentWindow?.Parent != null)
            {
                parentWindow = parentWindow.Parent as FrameworkElement;
            }
            return parentWindow;
        }
    }
}
