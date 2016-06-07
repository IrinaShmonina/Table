using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    class FormulaParser
    {
        public static double Eval(char[] expr)
        {
            return parseSummands(expr, 0);
        }

        private static double parseSummands(char[] expr, int index)
        {
            double x = parseFactors(expr, ref index);
            while (true)
            {
                char op = expr[index];
                if (op != '+' && op != '-')
                    return x;
                index++;
                double y = parseFactors(expr, ref index);
                if (op == '+')
                    x += y;
                else
                    x -= y;
            }
        }
        private static double parseFactors(char[] expr, ref int index)
        {
            double x;
            if (expr[index] == '[')
                x = GetCellData(expr, ref index);
            else
                x = GetDouble(expr, ref index);
            while (true)
            {
                char op = expr[index];
                if (op != '/' && op != '*' && op != '^')
                    return x;
                index++;
                double y;
                if (expr[index] == '[')
                    y = GetCellData(expr, ref index);
                else
                    y = GetDouble(expr, ref index);
                if (op == '/' && y != 0)
                    x /= y;
                else if (op == '*')
                    x *= y;
                else if (op == '^')
                    x = Math.Pow(x, y);
            }

        }

        private static double GetDouble(char[] expr, ref int index)
        {
            string dbl = "";
            while (((int)expr[index] >= 48 && (int)expr[index] <= 57) || (int)expr[index] == 46)
            {
                dbl += expr[index].ToString();
                index++;
                if (index == expr.Length)
                {
                    index--;
                    break;
                }
            }
            return double.Parse(dbl);
        }

        private static double GetCellData(char[] expr, ref int index)
        {
            double dbl;
            string a = "";
            string b = "";
            var flag = true;

            while (true)//((int)expr[index] >= 48 && (int)expr[index] <= 57) || (int)expr[index] == 46
            {

                if (expr[index] == '[')
                    index++;
                else if (expr[index] == ']')
                {
                    index++;
                    if (index == expr.Length)
                    {
                        index--;
                    }
                    break;
                }
                else if (expr[index] == ';')
                {
                    index++;
                    flag = false;
                }
                else if (flag)
                {
                    a += expr[index].ToString();
                    index++;
                }
                else if (!flag)
                {
                    b += expr[index].ToString();
                    index++;
                }

                if (index == expr.Length)
                {
                    index--;
                    break;
                }
            }
            dbl = double.Parse(a) + double.Parse(b);
            return dbl;
        }
    }
}
