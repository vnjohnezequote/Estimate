// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorToBrushConverter.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the ColorToBrushConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;

namespace ApplicationConverter
{
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;
    using Color = System.Drawing.Color;
    /// <summary>
    /// The color to brush converter.
    /// </summary>
    public class ColorToBrushConverter : IValueConverter
    {
        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Drawing.Color color)
            {
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(
                    color.A,
                    color.R,
                    color.G,
                    color.B);
                System.Windows.Media.Brush returnColorBrush = new SolidColorBrush(newColor);
                return returnColorBrush;
            }
            else
            {
                return new SolidColorBrush(Colors.White);
            }
        }
        /// <summary>
        /// The convert back.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            if (value is Brush brush)
            {
                var colorHex = brush.ToString();
                var color = ((SolidColorBrush) brush).Color;
                return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B); ;
            }
            else
            {
                return Color.Red;
            }
        }

        
        
    }
}
