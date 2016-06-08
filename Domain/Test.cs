using System;
using System.Collections.Generic;
using System.Drawing;
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
        [TestCase("=сумм((1;1);(2;2))",true)]
        [TestCase("=сумм(сумм((1;1);(2;2));(2;2))",true)]
        [TestCase("=(1;1)",true)]
        [TestCase("(11)",false)]
        [TestCase("(1;1)",false)]
        [TestCase("=(1;",false)]
        [TestCase("=(11)",false)]
        [TestCase("==;",false)]
        [TestCase("==сумм((1;1);(2;2));", false)]
        public void TestIsCorrect(string start,bool result)
        {
            Assert.That(ExpressionCalculator.IsCorrect(start),Is.EqualTo(result));
        }
        [TestCase("сумм((1;1);(2;2))","(1;1)")]
        [TestCase("сумм(сумм((1;1);(2;2));(2;2))", "сумм((1;1);(2;2))")]
        public void TestGetNameAndArgs(string str, string end)
        {
            Assert.That(ExpressionCalculator.GetNameAndArgs(str).Item2,Is.EqualTo(end));
        }

        [TestCase("(1;1)")]
        public void TestTryParse(string str)
        {
            Point p=default(Point);
            Assert.That(ExpressionCalculator.TryParse(str,out p), Is.EqualTo(true));
            Assert.That(p,Is.EqualTo(new Point(1,1)));
        }


        [TestCase("сумм((1;1);(2;2))", 2.0)]
        [TestCase("сумм(сумм((1;1);(2;2));(2;2))", 3.0)]
        public void TestCount(string str, double number)
        {
            Dictionary<Point,Cell> table=new Dictionary<Point, Cell>()
            {
                [new Point(1,1)]=new Cell(1,1,"1",""),
                [new Point(2,2)]=new Cell(2,2,"1",""),


            };
            Assert.That(ExpressionCalculator.Count(str,table), Is.EqualTo(number));
        }
    }
}
