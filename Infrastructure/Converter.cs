using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Infrastructure
{
    public static class Converter
    {
        public static string GetCorrectString(string input)
        {
            var output = input;
            var pattern = new Regex(@"(?<letters>[A-Z]+)(?<numbers>[0123456789]+)");
            foreach (Match m in pattern.Matches(input))
            {
                var str = "(" + ConvertLettersToNumbers(m.Groups["letters"].Value).ToString() + ";" + m.Groups["numbers"].Value + ")";
                output = pattern.Replace(output, str, 1);
            }
            return output;
        }
        public static string ConvertNumbersToLetters(int number)
        {
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var str = "";
            while (number != 0)
            {
                number--;
                str = alphabet[number % alphabet.Length] + str;
                number /= alphabet.Length;
            }
            return str;
        }

        public static int ConvertLettersToNumbers(string letters)
        {
            var number = 0;
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (var i = 0; i < letters.Length; i++)
            {
                var a = (int)Math.Pow(alphabet.Length, i);
                var b = letters[letters.Length - i - 1];
                var c = alphabet.IndexOf(b) + 1;
                number += a * c;
            }
            return number;
        }
    }
}
