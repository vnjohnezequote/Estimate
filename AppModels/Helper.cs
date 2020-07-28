using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
