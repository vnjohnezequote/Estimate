using System;
using System.Globalization;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;

namespace ApplicationConverter
{
    public class StringToPackIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string iconString = value as string;
            if (string.IsNullOrEmpty(iconString))
            {
                return null;
            }
            var iconKind = GetIconKind(iconString);
            
            return iconKind;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private PackIconKind GetIconKind(string iconKind)
        {
            var icon = (PackIconKind)System.Enum.Parse(typeof(PackIconKind), iconKind);
            return icon;
        }
    }
}