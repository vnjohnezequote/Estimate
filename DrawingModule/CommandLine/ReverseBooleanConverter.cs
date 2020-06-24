using System;
using System.Globalization;
using System.Windows.Data;

namespace DrawingModule.CommandLine
{
    public class ReverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if (!(value is bool))
            {
                throw new NotSupportedException();
            }
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if (!(value is bool))
            {
                throw new NotSupportedException();
            }
            return !(bool)value;
        }
    }
}
