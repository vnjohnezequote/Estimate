// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomBindingUtil.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the CustomBindingUtil type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ApplicationConverter.Base
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// The custom binding util.
    /// </summary>
    public class CustomBindingUtil
    {
        #region Single Binding Attached-Properties

        public static object GetSingleBinding(DependencyObject obj)
        {
            return (object)obj.GetValue(SingleBindingProperty);
        }

        public static void SetSingleBinding(DependencyObject obj, object value)
        {
            obj.SetValue(SingleBindingProperty, value);
        }

        // Using a DependencyProperty as the backing store for SingleBinding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SingleBindingProperty =
            DependencyProperty.RegisterAttached("SingleBinding", typeof(object), typeof(CustomBindingUtil), new PropertyMetadata(null, SingleBindingChanged));

        private static void SingleBindingChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                object convparam = obj.GetValue(ConverterParameterProperty);
                object binding = obj.GetValue(SingleBindingProperty);
                obj.SetValue(BResultProperty, (obj.GetValue(ConverterProperty) as IValueConverter).Convert(binding, null, convparam, null));
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public static object GetBResult(DependencyObject obj)
        {
            return (object)obj.GetValue(BResultProperty);
        }

        public static void SetBResult(DependencyObject obj, object value)
        {
            obj.SetValue(BResultProperty, value);
        }

        // Using a DependencyProperty as the backing store for BResult.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BResultProperty =
            DependencyProperty.RegisterAttached("BResult", typeof(object), typeof(CustomBindingUtil), new UIPropertyMetadata(null));


        #endregion

        #region Multi Binding Attached-Properties
        #region Predefined Bindings


        public static Object GetBinding1(DependencyObject obj)
        {
            return (Object)obj.GetValue(Binding1Property);
        }
        public static void SetBinding1(DependencyObject obj, Object value)
        {
            obj.SetValue(Binding1Property, value);
        }
        // Using a DependencyProperty as the backing store for Binding1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Binding1Property =
            DependencyProperty.RegisterAttached("Binding1", typeof(Object), typeof(CustomBindingUtil), new PropertyMetadata(null, BindingChanged));


        public static Object GetBinding2(DependencyObject obj)
        {
            return (Object)obj.GetValue(Binding2Property);
        }
        public static void SetBinding2(DependencyObject obj, Object value)
        {
            obj.SetValue(Binding2Property, value);
        }
        // Using a DependencyProperty as the backing store for Binding2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Binding2Property =
            DependencyProperty.RegisterAttached("Binding2", typeof(Object), typeof(CustomBindingUtil), new PropertyMetadata(null, BindingChanged));


        public static Object GetBinding3(DependencyObject obj)
        {
            return (Object)obj.GetValue(Binding3Property);
        }
        public static void SetBinding3(DependencyObject obj, Object value)
        {
            obj.SetValue(Binding3Property, value);
        }
        // Using a DependencyProperty as the backing store for Binding3.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Binding3Property =
            DependencyProperty.RegisterAttached("Binding3", typeof(Object), typeof(CustomBindingUtil), new PropertyMetadata(null, BindingChanged));

        #endregion

        /// <summary>
        /// update result property with converted (& Weighted) all available sub-binding results
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private static void BindingChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            object[] Values = new Object[] { obj.GetValue(Binding1Property), obj.GetValue(Binding2Property), obj.GetValue(Binding3Property) };
            try
            {
                object convparam = obj.GetValue(ConverterParameterProperty);
                obj.SetValue(MBResultProperty, (obj.GetValue(MultiConverterProperty) as IMultiValueConverter).Convert(Values, null, convparam, null));
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public static IMultiValueConverter GetMultiConverter(DependencyObject obj)
        {
            return (IMultiValueConverter)obj.GetValue(MultiConverterProperty);
        }

        public static void SetMultiConverter(DependencyObject obj, IMultiValueConverter value)
        {
            obj.SetValue(MultiConverterProperty, value);
        }

        // Using a DependencyProperty as the backing store for MultiConverter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MultiConverterProperty =
            DependencyProperty.RegisterAttached("MultiConverter", typeof(IMultiValueConverter), typeof(CustomBindingUtil), new UIPropertyMetadata(null));





        public static Object GetMBResult(DependencyObject obj)
        {
            return (Object)obj.GetValue(MBResultProperty);
        }
        public static void SetMBResult(DependencyObject obj, Object value)
        {
            obj.SetValue(MBResultProperty, value);
        }
        // Using a DependencyProperty as the backing store for MBResult.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MBResultProperty =
            DependencyProperty.RegisterAttached("MBResult", typeof(Object), typeof(CustomBindingUtil), new PropertyMetadata(null));


        #endregion

        #region Shared Attached-Properties

        public static IValueConverter GetConverter(DependencyObject obj)
        {
            return (IValueConverter)obj.GetValue(ConverterProperty);
        }
        public static void SetConverter(DependencyObject obj, IValueConverter value)
        {
            obj.SetValue(ConverterProperty, value);
        }
        // Using a DependencyProperty as the backing store for Converter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConverterProperty =
            DependencyProperty.RegisterAttached("Converter", typeof(IValueConverter), typeof(CustomBindingUtil), new UIPropertyMetadata(null));




        public static object GetConverterParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(ConverterParameterProperty);
        }

        public static void SetConverterParameter(DependencyObject obj, object value)
        {
            obj.SetValue(ConverterParameterProperty, value);
        }

        // Using a DependencyProperty as the backing store for ConverterParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConverterParameterProperty =
            DependencyProperty.RegisterAttached("ConverterParameter", typeof(object), typeof(CustomBindingUtil), new PropertyMetadata(null));

        #endregion

    }
}
