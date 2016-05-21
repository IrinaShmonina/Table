using System;

namespace model
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var table = new Table(3, 3);
            TextParser.table = table;
            table.setCellText('A', 1, "1");
            table.setCellText('B', 1, "3");
            table.increaseRowsCount();
            table.increaseColumnCount();
            table.setCellText('A', 4, "=яслл(A1; B1)");
            table.setCellText('B', 4, "=слм(A1;       B1)");
            Console.WriteLine(table);
            Console.WriteLine();
            table.setCellText('A', 1, "3");
            table.setCellText('D', 1, "=яслл(яслл(A4; A1)  ;   яслл(B4; B1))");
            Console.WriteLine(table);
        }
    }
}
