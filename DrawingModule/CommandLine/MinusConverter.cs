using System;
using System.Globalization;
using System.Windows.Data;

namespace DrawingModule.CommandLine
{
    class MinusConverter : IMultiValueConverter
    {
        public MinusConverter()
        {
            this.Minimum = double.MinValue;
            this.Maximum = double.MaxValue;
            this.Append = 0.0;
        }

        
        public double Minimum { get; set; }

        
        public double Maximum { get; set; }

        
        public double Append { get; set; }

        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 0)
            {
                return 0.0;
            }
            double num = (double)values[0];
            for (int i = 1; i < values.Length; i++)
            {
                num -= (double)values[i];
            }
            num += this.Append;
            if (num > this.Maximum)
            {
                return this.Maximum;
            }
            if (num < this.Minimum)
            {
                return this.Minimum;
            }
            return num;
        }

        
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
