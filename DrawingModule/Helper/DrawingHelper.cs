using System;
using devDept.Geometry;
using DrawingModule.Views;

namespace DrawingModule.Helper
{
    public static class DrawingHelper
    {
        public static Tuple<double, double> GetFormatSize(linearUnitsType units, formatType formatType)
        {
            // Values on this method are millimeters so it uses this factor to get converted values according to the current units.
            double conversionFactor = Utility.GetLinearUnitsConversionFactor(linearUnitsType.Millimeters, units);

            switch (formatType)
            {
                case formatType.A0_ISO:
                    return new Tuple<double, double>(1189 * conversionFactor, 841 * conversionFactor);
                case formatType.A1_ISO:
                    return new Tuple<double, double>(841 * conversionFactor, 594 * conversionFactor);
                case formatType.A2_ISO:
                    return new Tuple<double, double>(594 * conversionFactor, 420 * conversionFactor);
                case formatType.A3_ISO:
                    return new Tuple<double, double>(420 * conversionFactor, 297 * conversionFactor);
                case formatType.A4_ISO:
                    return new Tuple<double, double>(210 * conversionFactor, 297 * conversionFactor);
                case formatType.A4_LANDSCAPE_ISO:
                    return new Tuple<double, double>(297 * conversionFactor, 210 * conversionFactor);
                case formatType.A_ANSI:
                    return new Tuple<double, double>(215.9 * conversionFactor, 279.4 * conversionFactor);
                case formatType.A_LANDSCAPE_ANSI:
                    return new Tuple<double, double>(279.4 * conversionFactor, 215.9 * conversionFactor);
                case formatType.B_ANSI:
                    return new Tuple<double, double>(431.8 * conversionFactor, 279.4 * conversionFactor);
                case formatType.C_ANSI:
                    return new Tuple<double, double>(558.8 * conversionFactor, 431.8 * conversionFactor);
                case formatType.D_ANSI:
                    return new Tuple<double, double>(863.6 * conversionFactor, 558.8 * conversionFactor);
                case formatType.E_ANSI:
                    return new Tuple<double, double>(1117.6 * conversionFactor, 863.6 * conversionFactor);
                default:
                    return new Tuple<double, double>(210 * conversionFactor, 297 * conversionFactor);
            }
        }
    }
}
