using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using App;

namespace Domain
{

    public static class StringExtensions
    {
        public static String[] SmartSplit(this String str)
        {
            var s = new List<String>();
            var q = "";
            var quote = 0;
            foreach (char t in str)
            {
                if (quote < 0)
                    throw new Exception();
                switch (t)
                {
                    case '(':
                    {
                        q += t;
                        quote++;
                        break;
                    }
                    case ';':
                    {
                        if (quote == 0)
                        {
                            s.Add(q);
                            q = "";
                        }
                        else
                            q += t;
                        break;
                    }
                    case ')':
                    {
                        q += t;
                        quote--;
                        break;
                    }
                    default:
                    {
                        q += t;
                        break;
                    }
                }
            }
            s.Add(q);
            return s.ToArray();
        }
    }

    public class TextParser
    {
        public static Table Table;
        public Dictionary<String, Func<String, String, String>> FuncDict;

        public TextParser()
        {
            this.FuncDict = new Dictionary<string, Func<string, string, string>>();
            FuncDict["СУММ"] = (x, y) => (int.Parse(x) + int.Parse(y)).ToString();
            FuncDict["КОНК"] = (x, y) => x + y;
            FuncDict["УМН"] = (x, y) => (int.Parse(x) * int.Parse(y)).ToString();
        }

        public Tuple<List<Cell>, String> ParseText(String text)
        {
            var list = new List<Cell>();
            if (text[0] != '=')
                return Tuple.Create(new List<Cell>(), text);
            try
            {
                if (text.Length == 3)
                {
                    var cell = Table[new Point( text[1], int.Parse(text[2].ToString()))];
                    return Tuple.Create(new List<Cell>(new Cell[] { cell }), cell.text);
                }
                var newText = text.Substring(1).Replace(" ", "");
                foreach (var funcname in FuncDict.Keys)
                    if (newText.StartsWith(funcname))
                    {
                        if (!newText.EndsWith(")") || newText[funcname.Length] != '(')
                            throw new Exception();
                        var substr = newText.Substring(funcname.Length + 1, newText.Length - funcname.Length - 1 - 1).SmartSplit();
                        var v = new String[substr.Length];
                        for (var i = 0; i < substr.Length; i++)
                        {
                            var textParser = new TextParser().ParseText("=" + substr[i]);
                            v[i] = textParser.Item2;
                            list.AddRange(textParser.Item1);
                        }
                        return Tuple.Create(list, FuncDict[funcname](v[0], v[1]));
                    }
                return Tuple.Create(new List<Cell>(), newText);
            }
            catch (Exception)
            {
                return Tuple.Create(list, "error");
            }
        }
    }
}