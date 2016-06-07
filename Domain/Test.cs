using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Domain
{
    [TestFixture]
    class Test
    {
        [TestCase("=сумм((1;1);(2;2))", "сумм((1;1);(2;2))")]
        public void TestSplite(string start, string end)
        {
            Assert.That(ExpressionCalculator.Split(start).Groups[1].ToString(),Is.EqualTo(end));
        }
    }
}
