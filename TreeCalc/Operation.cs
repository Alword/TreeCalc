using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TreeCalc
{
    public class Operation
    {
        [DllImport(@"ExpressionCalculator.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern double Calculate(string inputExpression);

        public enum Operations
        {
            asin,
            acos,
            atan,
            log,
            power,
            sin,
            cos,
            tan,
            exp,
            addition,
            subtraction,
            multiplication,
            division,
            leftBracket,
            rightBracket,
            factorial,
        }

        /// <summary>
        /// Посчитать выражение
        /// </summary>
        /// <param name="x">Выражение</param>
        /// <returns>Результат выражения</returns>
        public static string TryCalc(string x)
        {
            string res = Calculate(x).ToString();
            return res;
        }
    }
}
