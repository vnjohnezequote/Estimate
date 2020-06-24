using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DrawingModule.CommandLine
{
    public class RevertVisibilityMultiValueConverter: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 0)
            {
                return Visibility.Visible;
            }
            foreach (object obj in values)
            {
                if (!(obj is bool) || !(bool)obj)
                {
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
