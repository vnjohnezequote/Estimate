// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemHelper.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the SystemHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace CustomControls.Helper
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Forms;


    /// <summary>
    /// The system helper.
    /// </summary>
    internal static class SystemHelper
    {
        /// <summary>
        /// The get current dpi.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int GetCurrentDPI()
        {
            return (int)typeof(SystemParameters).GetProperty("Dpi", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null, null);
        }

        /// <summary>
        /// The get current dpi scale factor.
        /// </summary>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        public static double GetCurrentDPIScaleFactor()
        {
            return (double)SystemHelper.GetCurrentDPI() / 96;
        }

        /// <summary>
        /// The get mouse screen position.
        /// </summary>
        /// <returns>
        /// The <see cref="Point"/>.
        /// </returns>
        public static Point GetMouseScreenPosition()
        {
            System.Drawing.Point point = Control.MousePosition;
            return new Point(point.X, point.Y);
        }
    }
}
