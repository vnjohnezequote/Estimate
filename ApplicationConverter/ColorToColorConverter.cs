using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ApplicationConverter
{
    public class ColorToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Drawing.Color color)
            {
                return System.Windows.Media.Color.FromArgb(
                    color.A,
                    color.R,
                    color.G,
                    color.B);
                
            }
            else
            {
                return System.Windows.Media.Color.FromRgb(255,255,255);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
            }

            return System.Drawing.Color.Red;
        }
    }
}
