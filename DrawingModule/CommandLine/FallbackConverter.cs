using System;
using System.Globalization;
using System.Windows.Data;

namespace DrawingModule.CommandLine
{
    public class FallbackConverter : IValueConverter
    {
        // Token: 0x170000F8 RID: 248
        // (get) Token: 0x0600060C RID: 1548 RVA: 0x00013D9F File Offset: 0x00012D9F
        // (set) Token: 0x0600060D RID: 1549 RVA: 0x00013DA7 File Offset: 0x00012DA7
        public object FallbackValue { get; set; }

        // Token: 0x0600060E RID: 1550 RVA: 0x00013DB0 File Offset: 0x00012DB0
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (!(value is bool) || (bool)value))
            {
                return value;
            }
            if (parameter == null)
            {
                return this.FallbackValue;
            }
            return parameter;
        }

        // Token: 0x0600060F RID: 1551 RVA: 0x00012196 File Offset: 0x00011196
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
