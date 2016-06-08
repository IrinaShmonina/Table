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


        [TestCase("сумм((1;1);(2;2))","сумм","(1;1)","(2;2)")]
        [TestCase("сумм(сумм((1;1);(2;2));(2;2))", "сумм","сумм((1;1);(2;2))","(2;2)")]
        [TestCase("сумм(умн((1;1);(2;2));(2;2))", "сумм", "умн((1;1);(2;2))", "(2;2)")]
        public void TestGetNameAndArgs(string str, string name, string arg1, string arg2)
        {
            Assert.That(ExpressionCalculator.GetNameAndArgs(str).Item1,Is.EqualTo(name));
            Assert.That(ExpressionCalculator.GetNameAndArgs(str).Item2,Is.EqualTo(arg1));
            Assert.That(ExpressionCalculator.GetNameAndArgs(str).Item3,Is.EqualTo(arg2));
        }



        [Test,TestCaseSource(nameof(DivideCases))]
        public void TestTryParse(string str, bool resulte, Point pointResulte)
        {
            Point p=default(Point);
            Assert.That(ExpressionCalculator.TryParse(str,out p), Is.EqualTo(resulte));
            Assert.That(p,Is.EqualTo(pointResulte));
        }
        static object[] DivideCases =
        {
            new object[] { "(1;1)", true, new Point(1,1) },
            new object[] { "(123;123)", true, new Point(123,123) },
            new object[] { "(d;1)",false,default(Point)  },
            new object[] { "(12d;1)",false,default(Point)  },
        };

        [TestCase("сумм((1;1);(2;2))", 2.0)]
        [TestCase("сумм(1;1)", 2.0)]
        [TestCase("сумм(сумм((1;1);(2;2));(2;2))", 3.0)]
        [TestCase("сумм(умн((1;1);(2;2));умн((1;1);(2;2)))", 2.0)]
        [TestCase("дел(сумм((1;1);(2;2));умн((1;1);(2;2)))", 2.0)]
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
