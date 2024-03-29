﻿using System.Windows;

namespace DrawingModule.CommandLine
{
    public static class ClassExtensions
    {
        public static Size? GetLocalSize(this FrameworkElement fe)
        {
            object obj = fe.ReadLocalValue(FrameworkElement.WidthProperty);
            object obj2 = fe.ReadLocalValue(FrameworkElement.HeightProperty);
            if (obj != DependencyProperty.UnsetValue && obj2 != DependencyProperty.UnsetValue)
            {
                return new Size?(new Size((double)obj, (double)obj2));
            }
            return null;
        }
    }
}
