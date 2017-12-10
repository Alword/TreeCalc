using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TreeCalc
{
    public class Operation
    {
        [DllImport(@"ExpressionCalculator.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern double Calculate(string inputExpression);

        /// <summary>
        /// Посчитать выражение
        /// </summary>
        /// <param name="x">Выражение</param>
        /// <returns>Результат выражения</returns>
        public static string TryCalc(string x)
        {
            string result = "";
            try
            {
                if (x.CountWords("∞") == 0)
                {
                    result = Calculate(x).ToString();
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
    }
}
