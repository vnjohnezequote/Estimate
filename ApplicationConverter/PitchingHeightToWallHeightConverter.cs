// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PitchingHeightToWallHeightConverter.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The pitching height to wall height converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ApplicationConverter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    using AppModels;

    /// <summary>
    /// The pitching height to wall height converter.
    /// </summary>
    public class PitchingHeightToWallHeightConverter : IValueConverter
    {
        /// <summary>
        /// The temp.
        /// </summary>
        private List<IntegerDimension> temp;

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value">
        /// The value.
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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = 0;
            if (value== DependencyProperty.UnsetValue || value == null)
            {
                // Do nothing
            }
            else 
            {
                if (value is List<IntegerDimension> dimensions)
                {
                    if (!(dimensions.Count < 2))
                    {
                        dimensions[1].Size = dimensions[0].Size + (int)parameter;
                        result = dimensions[1].Size;
                        this.temp = dimensions;
                    }
                    
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// The convert back.
        /// </summary>
        /// <param name="value">
        /// The value.
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
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            this.temp[0].Size = System.Convert.ToInt32(value) - (int)parameter;

            return this.temp;
        }
    }
}
