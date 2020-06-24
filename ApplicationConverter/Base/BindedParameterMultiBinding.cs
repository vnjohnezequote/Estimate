// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindedParameterMultiBinding.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the BindedParameterMultiBinding type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ApplicationConverter.Base
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// The binded parameter multi binding.
    /// </summary>
    public class BindedParameterMultiBinding : MarkupExtension
    {
        public Binding Binding1 { get; set; }
        public Binding Binding2 { get; set; }
        public Binding ConverterParameter { get; set; }
        public IMultiValueConverter Converter { get; set; }


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            try
            {
                IProvideValueTarget pvt = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
                DependencyObject TargetObject = pvt.TargetObject as DependencyObject;
                BindingOperations.SetBinding(TargetObject, CustomBindingUtil.Binding1Property, Binding1);
                BindingOperations.SetBinding(TargetObject, CustomBindingUtil.Binding2Property, Binding2);
                BindingOperations.SetBinding(TargetObject, CustomBindingUtil.ConverterParameterProperty, ConverterParameter);
                TargetObject.SetValue(CustomBindingUtil.MultiConverterProperty, this.Converter);

                Binding b = new Binding();
                b.Source = TargetObject;
                b.Path = new PropertyPath(CustomBindingUtil.MBResultProperty);
                return b.ProvideValue(serviceProvider);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
