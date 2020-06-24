using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DrawingModule.CommandLine
{
    class MultiBooleanOrConverter: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                return false;
            }
            foreach (object obj in values)
            {
                if (obj != null && obj != DependencyProperty.UnsetValue && (bool)obj)
                {
                    return true;
                }
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
