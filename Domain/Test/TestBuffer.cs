using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Domain.Test
{
    [TestFixture]
    public class TestBuffer
    {
        [Test, TestCaseSource(nameof(DivideCases))]
        public void TestAddRow(Dictionary<int, string> row)
        {
            Buffer buffer = new Buffer();
            buffer.AddRow(row);
            Assert.IsNotNull(buffer);
        }

        [Test, TestCaseSource(nameof(DivideCases))]
        public void TestAddColumn(Dictionary<int, string> column)
        {
            Buffer buffer = new Buffer();
            buffer.AddRow(column);
            Assert.IsNotNull(buffer);
        }


        [Test, TestCaseSource(nameof(DivideCases))]
        public void TestGetRow(Dictionary<int, string> row)
        {
            Buffer buffer = new Buffer();
            buffer.AddRow(row);
            Assert.That(buffer.GetRow().Count, Is.EqualTo(row.Count));
        }
        public void TestGetColumn(Dictionary<int, string> column)
        {
            Buffer buffer = new Buffer();
            buffer.AddRow(column);
            Assert.That(buffer.GetRow().Count, Is.EqualTo(column.Count));
        }

        static object[] DivideCases =
        {
            new object[]
            {
                new Dictionary<int, string>()
                {
                    [1] = "1",
                    [2] = "2"
                }
            }
        };
    }
}
