﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppModels
{
    public static class Helper
    {
        public static int RoundUpTo300(this int number)
        {
            return (int)(Math.Ceiling((double)number / 300)*300);
        }

        public static int RoundUpto5(this double number)
        {
            return (int)(Math.Ceiling(number / 5) * 5);
        }
        public static bool ConvertStringToDouble(string inputString, out double outputDouble)
        {
            outputDouble = 0;

            if (string.IsNullOrEmpty(inputString))
            {
                outputDouble = 0;
                return false;
            }
            var match = Regex.Match(inputString, @"([+|-]?[\d+]?[+|\-|\*|\/]?\d+)+");
            if (match.Success)
            {
                var dataTable = new DataTable();
                var output = dataTable.Compute(match.Value, String.Empty);
                outputDouble = Convert.ToDouble(output);

            }

            if (Math.Abs(outputDouble) > 0.0001)
            {
                return true;
            }

            return false;
        }
    }
}
