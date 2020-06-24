// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiValueConverterAdapter.cs" company="John Nguyen">
//  John Nguyen
// </copyright>
// <summary>
//   Defines the MultiValueConverterAdapter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ApplicationConverter.Base
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// The multi value converter adapter.
    /// </summary>
    public class MultiValueConverterAdapter : IMultiValueConverter
    {
        /// <summary>
        /// Gets or sets the converter.
        /// </summary>
        public IValueConverter Converter { get; set; }

        #region IMultiValueConverter Members

        /// <summary>
        /// The last parameter.
        /// </summary>
        private object lastParameter;

        /// <summary>
        /// The last converter.
        /// </summary>
        private IValueConverter lastConverter;

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            this.lastConverter = this.Converter;
            if (values.Length > 1)
            {
                this.lastParameter = values[1];
            }

            if (values.Length > 2)
            {
                this.lastConverter = (IValueConverter)values[2];
            }

            return this.Converter == null ? values[0] : this.Converter.Convert(values[0], targetType, this.lastParameter, culture);
        }

        /// <summary>
        /// The convert back.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetTypes">
        /// The target types.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object[]"/>.
        /// </returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.lastConverter == null ? new object[] { value } : new object[] { this.lastConverter.ConvertBack(value, targetTypes[0], this.lastParameter, culture) };
        }

        #endregion
    }
}
