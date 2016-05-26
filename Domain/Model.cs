using System;
using System.Collections.Generic;
using App.Table;

namespace model
{

    public static class StringExtensions
    {
        public static String[] SmartSplit(this String str)
        {
            var s = new List<String>();
            var q = "";
            var quote = 0;
            for (var i = 0; i < str.Length; i++)
            {
                if (quote < 0)
                    throw new Exception();
                switch (str[i])
                {
                    case '(':
                        {
                            q += str[i];
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
                                q += str[i];
                            break;
                        }
                    case ')':
                        {
                            q += str[i];
                            quote--;
                            break;
                        }
                    default:
                        {
                            q += str[i];
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
        static public Table table;
        public Dictionary<String, Func<String, String, String>> funcDict;

        public TextParser()
        {
            this.funcDict = new Dictionary<string, Func<string, string, string>>();
            funcDict["СУММ"] = (x, y) => (int.Parse(x) + int.Parse(y)).ToString();
            funcDict["КОНК"] = (x, y) => x + y;
            funcDict["УМН"] = (x, y) => (int.Parse(x) * int.Parse(y)).ToString();
        }

        public Tuple<List<Cell>, String> parseText(String text)
        {
            var list = new List<Cell>();
            if (text[0] != '=')
                return Tuple.Create(new List<Cell>(), text);
            try
            {
                if (text.Length == 3)
                {
                    var cell = table[text[1], int.Parse(text[2].ToString())];
                    return Tuple.Create(new List<Cell>(new Cell[] { cell }), cell.text.ToString());
                }
                var newText = text.Substring(1).Replace(" ", "");
                foreach (var funcname in funcDict.Keys)
                    if (newText.StartsWith(funcname))
                    {
                        if (!newText.EndsWith(")") || newText[funcname.Length] != '(')
                            throw new Exception();
                        var substr = newText.Substring(funcname.Length + 1, newText.Length - funcname.Length - 1 - 1).SmartSplit();
                        var v = new String[substr.Length];
                        for (var i = 0; i < substr.Length; i++)
                        {
                            var textParser = new TextParser().parseText("=" + substr[i]);
                            v[i] = textParser.Item2;
                            foreach (var e in textParser.Item1)
                                list.Add(e);
                        }
                        return Tuple.Create(list, funcDict[funcname](v[0], v[1]));
                    }
                return Tuple.Create(new List<Cell>(), newText);
            }
            catch (Exception e)
            {
                return Tuple.Create(list, "error");
            }
        }
    }
}