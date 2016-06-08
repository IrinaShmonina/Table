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
    public class TestExpressionCalculator
    {
        [TestCase("=add((1;1);(2;2))", true)]
        [TestCase("=add(add((1;1);(2;2));(2;2))", true)]
        [TestCase("=(1;1)", true)]
        [TestCase("(11)", false)]
        [TestCase("(1;1)", false)]
        [TestCase("=(1;", false)]
        [TestCase("=(11)", false)]
        [TestCase("==;", false)]
        [TestCase("==add((1;1);(2;2));", false)]
        public void TestIsCorrect(string start, bool result)
        {
            Assert.That(ExpressionCalculator.IsCorrect(start), Is.EqualTo(result));
        }


        [TestCase("add((1;1);(2;2))", "add", "(1;1)", "(2;2)")]
        [TestCase("add(add((1;1);(2;2));(2;2))", "add", "add((1;1);(2;2))", "(2;2)")]
        [TestCase("add(mult((1;1);(2;2));(2;2))", "add", "mult((1;1);(2;2))", "(2;2)")]
        public void TestGetNameAndArgs(string str, string name, string arg1, string arg2)
        {
            Assert.That(ExpressionCalculator.GetNameAndArgs(str).Item1, Is.EqualTo(name));
            Assert.That(ExpressionCalculator.GetNameAndArgs(str).Item2, Is.EqualTo(arg1));
            Assert.That(ExpressionCalculator.GetNameAndArgs(str).Item3, Is.EqualTo(arg2));
        }



        [Test, TestCaseSource(nameof(DivideCases))]
        public void TestTryParse(string str, bool resulte, Point pointResulte)
        {
            Point p = default(Point);
            Assert.That(ExpressionCalculator.TryParse(str, out p), Is.EqualTo(resulte));
            Assert.That(p, Is.EqualTo(pointResulte));
        }
        static object[] DivideCases =
        {
            new object[] { "(1;1)", true, new Point(1,1) },
            new object[] { "(123;123)", true, new Point(123,123) },
            new object[] { "(d;1)",false,default(Point)  },
            new object[] { "(12d;1)",false,default(Point)  },
        };

        [TestCase("add((1;1);(2;2))", 3.0)]
        [TestCase("add(1;1)", 2.0)]
        [TestCase("add(add((1;1);(2;2));(2;2))", 4.0)]
        [TestCase("add(mult((1;1);(2;2));mult((1;1);(2;2)))", 4.0)]
        [TestCase("div(add((1;1);(2;2));mult((1;1);(2;2)))", 1.5)]
        [TestCase("div(add(sub((1;1);(2;2));(2;2));mult((1;1);(2;2)))", 1.0)]
        public void TestCount(string str, double number)
        {
            Dictionary<Point,Cell> table=new Dictionary<Point, Cell>()
            {
                [new Point(1,1)]=new Cell(1,1,"2",""),
                [new Point(2,2)]=new Cell(2,2,"1",""),


            };
            Assert.That(ExpressionCalculator.Count(str,table), Is.EqualTo(number));
        }
    }
}
