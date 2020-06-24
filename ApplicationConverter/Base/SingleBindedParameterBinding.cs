// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleBindedParameterBinding.cs" company="John nguyen">
//   John nguyen
// </copyright>
// <summary>
//   Defines the SingleBindedParameterBinding type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ApplicationConverter.Base
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// The single binded parameter binding.
    /// </summary>
    public class SingleBindedParameterBinding : MarkupExtension
    {
        public Binding Binding { get; set; }
        public IValueConverter Converter { get; set; }
        public Binding ConverterParameter { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            try
            {
                IProvideValueTarget pvt = serviceProvider.GetService(
                  typeof(IProvideValueTarget)) as IProvideValueTarget;
                DependencyObject TargetObject = pvt.TargetObject as DependencyObject;

                BindingOperations.SetBinding(TargetObject,
                  CustomBindingUtil.SingleBindingProperty, Binding);
                BindingOperations.SetBinding(TargetObject,
                  CustomBindingUtil.ConverterParameterProperty, ConverterParameter);
                TargetObject.SetValue(CustomBindingUtil.ConverterProperty, this.Converter);
                Binding b = new Binding();
                b.Source = TargetObject;
                b.Path = new PropertyPath(CustomBindingUtil.BResultProperty);
                return b.ProvideValue(serviceProvider);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
