using System;
using System.Globalization;
using System.Windows.Data;

namespace DrawingModule.CommandLine
{
    public class HintCategoryIsExpandedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HintCategory hintCategory = value as HintCategory;
            HintCategory hintCategory2 = parameter as HintCategory;
            if (hintCategory == hintCategory2)
            {
                return true;
            }
            return false;
        }

        // Token: 0x06000D15 RID: 3349 RVA: 0x0002C694 File Offset: 0x0002B694
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HintCategory result = parameter as HintCategory;
            if ((bool)value)
            {
                return result;
            }
            return null;
        }
    }
}