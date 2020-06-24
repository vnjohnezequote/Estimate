using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingModule.Control
{
    public static class Extensions
    {
        public static int Digits { get; set; } = 1;
        public static string ToRoundedString(this double value)
        {
            double num = value.Round();
            if (Extensions.Digits > 0)
            {
                return num.ToString(string.Format("F{0}", Extensions.Digits), CultureInfo.InvariantCulture);
            }
            return num.ToString(CultureInfo.InvariantCulture);
        }
        public static double Round(this double value)
        {
            if (Extensions.Digits >= 0)
            {
                return Math.Round(value, Extensions.Digits);
            }
            double num = Math.Pow(10.0, (double)(-(double)Extensions.Digits));
            return Math.Round(value / num, 0) * num;
        }
    }
}
