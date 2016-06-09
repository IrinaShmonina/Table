using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain
{
    public static class ExpressionParser
    {
        public static Tuple<string, string[]> GetNameAndArgs(string expression)
        {
            string pattern = @"(.*?)[(](.*)[)]";
            var result = Regex.Match(expression, pattern);

            var args = GetArgs(result.Groups[2].ToString());
            return new Tuple<string, string[]>(result.Groups[1].ToString(), args);
        }

        public static string[] GetArgs(string expression)
        {
            List<string> list = new List<string>();
            var balance = 0;
            var counter = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == ';' && balance == 0)
                {
                    list.Add(expression.Substring(counter, i - counter));
                    counter = i + 1;
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
