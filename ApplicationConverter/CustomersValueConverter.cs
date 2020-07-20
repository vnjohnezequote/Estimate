using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using AppModels.Enums;

namespace ApplicationConverter
{
    public class CustomersValueConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StickFrameCustomers format)
            {
                return GetString(format);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                return Enum.Parse(typeof(StickFrameCustomers), s.Substring(0, s.IndexOf(':')));
            }
            return null;
        }

        public string[] Strings => GetStrings();

        public static string GetString(StickFrameCustomers format)
        {
            return format.ToString() + ": " + GetDescription(format);
        }

        public static string GetDescription(StickFrameCustomers format)
        {
            return format.GetType().GetMember(format.ToString())[0].GetCustomAttribute<DescriptionAttribute>().Description;

        }
        public static string[] GetStrings()
        {
            List<string> list = new List<string>();
            foreach (StickFrameCustomers format in Enum.GetValues(typeof(StickFrameCustomers)))
            {
                list.Add(GetString(format));
            }

            return list.ToArray();
        }

    }
}
