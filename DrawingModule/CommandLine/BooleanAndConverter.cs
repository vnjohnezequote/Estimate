using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DrawingModule.CommandLine
{
    public class BooleanAndConverter: IMultiValueConverter
    {
        
        public bool AsVisibility { get; set; }

        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int i = 0;
            while (i < values.Length)
            {
                object obj = values[i];
                if (obj == null || !(obj is bool) || !(bool)obj)
                {
                    if (this.AsVisibility)
                    {
                        return Visibility.Collapsed;
                    }
                    return false;
                }
                else
                {
                    i++;
                }
            }
            if (this.AsVisibility)
            {
                return Visibility.Visible;
            }
            return true;
        }

        
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
