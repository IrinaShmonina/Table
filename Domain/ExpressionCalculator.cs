using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain
{
    public static class ExpressionCalculator
    {
        private static Dictionary<string, Func<double[], double>> nameToFunc =
            new Dictionary<string, Func<double[], double>>()
            {
                {"add",new Func<double[], double>(x =>
                    {
                        double result = 0;
                        for (int i = 0; i < x.Length;i++)
                        {
                            result += x[i];
                        }
                        return result;
                    })},
                {"mult",new Func<double[], double>(x =>
                    {
                        double result = 1;
                        for (int i = 0; i < x.Length;i++)
                        {
                            result *= x[i];
                        }
                        return result;
                    })},
                {"sub",new Func<double[], double>(x => x[0] - x[1])},
                {"div",new Func<double[], double>(x => x[0] / x[1])},
                {"pow",new Func<double[], double>(x => Math.Pow(x[0],x[1]))},
                {"sqrt",new Func<double[], double>(x => Math.Pow(x[0],1/x[1]))},
                {"inv",new Func<double[], double>(x => -x[0])},
            };

        public static bool IsCorrect(this string str)
        {
            var pattern = @"^=\w*[(]((.*?[;])*.*?)[)]$";
            var twoArgresult = Regex.IsMatch(str, pattern);
            return twoArgresult;
        }

        public static double Count(string expression, Dictionary<Point, Cell> table)
        {
            var point = default(Point);
            if (ExpressionParser.TryParse(expression, out point))
            {
                table[point].SetChangedAnotherCell();
                try
                {
                    return double.Parse(table[point].Data);
                }
                catch(System.FormatException)
                {
                    return 0;
                }
            }
            if (expression == "" || expression == null) return 0;

            if (ExpressionParser.IsNumber(expression)) return double.Parse(expression.Trim('(', ')'));
            var nameAndArgs = ExpressionParser.GetNameAndArgs(expression);
            var func = nameToFunc[nameAndArgs.Item1];
            var args = new double[nameAndArgs.Item2.Length];
            for (int i = 0; i < nameAndArgs.Item2.Length;i++ )
            {
                args[i] = Count(nameAndArgs.Item2[i], table);
            }
            return func(args);
        }

        

    }
}
