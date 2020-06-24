using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DrawingModule.CommandLine
{
    public class ObjectConverter :IValueConverter
    {
       
        public bool InvertBoolean { get; set; }

        
        public bool AsVisibilityObject { get; set; }

        
        public object Convert(object value, Type typeTarget, object param, CultureInfo culture)
        {
            bool flag = false;
            try
            {
                int num = System.Convert.ToInt32(value);
                flag = (num > ((param == null) ? 0 : System.Convert.ToInt32(param)));
            }
            catch (Exception)
            {
                flag = false;
            }
            bool flag2 = flag ^ this.InvertBoolean;
            if (this.AsVisibilityObject)
            {
                return flag2 ? Visibility.Visible : Visibility.Collapsed;
            }
            return flag2;
        }
        
        public object ConvertBack(object value, Type typeTarget, object param, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}