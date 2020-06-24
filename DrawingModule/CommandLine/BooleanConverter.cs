using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DrawingModule.CommandLine
{
    public class BooleanConverter: IValueConverter
    {
       
        public bool InvertBoolean { get; set; }

        
        public bool AsVisibilityObject { get; set; }

        
        public BooleanConverter()
        {
            this.InvertBoolean = false;
            this.AsVisibilityObject = false;
        }

        
        public object Convert(object value, Type typeTarget, object param, CultureInfo culture)
        {
            bool flag = (bool)value ^ this.InvertBoolean;
            if (this.AsVisibilityObject)
            {
                return flag ? Visibility.Visible : Visibility.Collapsed;
            }
            return flag;
        }

        
        public object ConvertBack(object value, Type typeTarget, object param, CultureInfo culture)
        {
            return (bool)value ^ this.InvertBoolean;
        }
    }
}
