// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindableParameterConverter.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the BindableParameterConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ApplicationConverter.Base
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// The bindable parameter converter.
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "Reviewed. Suppression is OK here.")]
    public class BindableParameterConverter : MarkupExtension
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        public Binding Binding { get; set; }

        /// <summary>
        /// Gets or sets the converter.
        /// </summary>
        public IValueConverter Converter { get; set; }

        /// <summary>
        /// Gets or sets the converter parameter binding.
        /// </summary>
        public Binding ConverterParameterBinding { get; set; }

        #endregion
        #region Overridden Methods

        /// <summary>
        /// The provide value.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var multiBinding = new MultiBinding();
            multiBinding.Bindings.Add(Binding);
            multiBinding.Bindings.Add(this.ConverterParameterBinding);
            var adapter = new MultiValueConverterAdapter
            {
                Converter = this.Converter
            };
            multiBinding.Converter = adapter;
            return multiBinding.ProvideValue(serviceProvider);
        }
        #endregion
    }
    
       
}
