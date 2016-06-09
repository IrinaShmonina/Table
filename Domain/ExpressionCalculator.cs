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
                        double result = 0;
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
            if (TryParse(expression, out point))
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

            if (IsNumber(expression)) return double.Parse(expression.Trim('(', ')'));
            var nameAndArgs = GetNameAndArgs(expression);
            var func = nameToFunc[nameAndArgs.Item1];
            var args = new double[nameAndArgs.Item2.Length];
            for (int i = 0; i < nameAndArgs.Item2.Length;i++ )
            {
                args[i] = Count(nameAndArgs.Item2[i], table);
            }
            return func(args);
        }

        public static Tuple<string, string[]> GetNameAndArgs(string expression)
        {
            string pattern = @"(.*?)[(](.*)[)]";
            var result = Regex.Match(expression, pattern);

            var args = GetArgs(result.Groups[2].ToString());
            return new Tuple<string, string[]>(result.Groups[1].ToString(),args);
        }

        public static string[] GetArgs(string expression)
        {
            List<string> list= new List<string>();
            var balance = 0;
            var counter=0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == ';' && balance == 0)
                {
                    list.Add(expression.Substring(counter, i - counter));
                    counter = i+1;
                }
                if (expression[i] == '(') balance++;
                if (expression[i] == ')') balance--;
                if (i == expression.Length - 1)
                {
                    list.Add(expression.Substring(counter, i + 1 - counter));
                    return list.ToArray();
                }
                
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
