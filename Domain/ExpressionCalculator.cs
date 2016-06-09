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
        private static Dictionary<string, Func<double, double, double>> nameToFunc =
            new Dictionary<string, Func<double, double, double>>()
            {
                {"add",new Func<double, double, double>((x, y) => x + y)},
                {"mult",new Func<double, double, double>((x, y) => x * y)},
                {"sub",new Func<double, double, double>((x, y) => x - y)},
                {"div",new Func<double, double, double>((x, y) => x / y)},
                {"pow",new Func<double,double,double>((x, y) => Math.Pow(x,y))},
                {"sqrt",new Func<double,double,double>((x, y) => Math.Pow(x,1/y))}
            };


        public static bool IsCorrect(this string str)
        {
            var pattern = @"^=\w*[(](.*?[;].*?)[)]$";
            var result = Regex.IsMatch(str, pattern);
            return result;
        }

        public static double Count(string expression, Dictionary<Point, Cell> table)
        {
            var point = default(Point);
            if (TryParse(expression, out point))
            {
                table[point].SetChangedAnotherCell();
                return double.Parse(table[point].Data);
            }
            if (IsNumber(expression)) return double.Parse(expression.Trim('(',')'));
            var nameAndArgs = GetNameAndArgs(expression);
            var func = nameToFunc[nameAndArgs.Item1];
            return func(Count(nameAndArgs.Item2, table), Count(nameAndArgs.Item3, table));
        }

        public static Tuple<string, string, string> GetNameAndArgs(string expression)
        {
            string pattern = @"(.*?)[(](.*)[)]";
            var result = Regex.Match(expression, pattern);

            var args = GetArgs(result.Groups[2].ToString());
            return new Tuple<string, string, string>(result.Groups[1].ToString(),
                args.Item1, args.Item2);
        }

        public static Tuple<string, string> GetArgs(string expression)
        {
            var balance = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == ';' && balance == 0)
                    return Tuple.Create(expression.Substring(0, i), expression.Substring(i + 1));
                if (expression[i] == '(') balance++;
                if (expression[i] == ')') balance--;
            }
            return null;
        }
        public static bool TryParse(string s, out Point result)
        {
            string pattern = @"^\((\d+);(\d+)\)$";
            if (Regex.IsMatch(s, pattern))
            {
                var m = Regex.Match(s, pattern);
                result = new Point(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
                return true;
            }
            result = default(Point);
            return false;
        }

        public static bool IsNumber(string s)
        {
            string pattern = @"^[(]?[-]?\d+[)]?$";
            return Regex.IsMatch(s, pattern);
        }

    }
}
