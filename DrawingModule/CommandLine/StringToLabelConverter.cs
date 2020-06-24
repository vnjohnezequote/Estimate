using System;
using System.Globalization;
using System.Windows.Data;

namespace DrawingModule.CommandLine
{
    public class StringToLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return ((string)value).Replace("_", "__");
            }
            return value;
        }

        // Token: 0x06000612 RID: 1554 RVA: 0x00012196 File Offset: 0x00011196
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
