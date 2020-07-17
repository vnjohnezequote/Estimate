using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace AppThemes.Helper
{
    public static class MyTextFieldAssist
    {
        /// <summary>
        /// PrefixText dependency property
        /// </summary>
        public static readonly DependencyProperty PrefixTextProperty = DependencyProperty.RegisterAttached(
            "PrefixText", typeof(string), typeof(MyTextFieldAssist), new PropertyMetadata(default(string)));

        public static void SetPrefixText(DependencyObject element, string value) => element.SetValue(PrefixTextProperty, value);

        public static string GetPrefixText(DependencyObject element) => (string)element.GetValue(PrefixTextProperty);
    }
}
