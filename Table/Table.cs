using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Table
{
    public class Table
    {
        private readonly Dictionary<Point, string> table;//string заменится на cell везде
        private int maxRowIndex;
        private int maxColumnIndex;

        public Table()
        {
            table = new Dictionary<Point, string>();
        }

        public void Put(int row, int column, string value)
        {
            table.Add(new Point(column, row), value);
            if (row > maxRowIndex) maxRowIndex = row;
            if (column > maxColumnIndex) maxColumnIndex = column;           
        }
        public void InsertRow(int rowIndex)
        {
            if (rowIndex<=maxRowIndex)
            {
                for (int r = maxRowIndex;r>=rowIndex;r--)
                    for (int c = 0;c<=maxColumnIndex;c++)
                    {
                        if (table.ContainsKey(new Point(c, r)))
                        {
                            table[new Point(c, r + 1)] = table[new Point(c,r)];
                            table.Remove(new Point(c, r));
                        }
                    }
                for (int c = 0;c<=maxColumnIndex;c++)
                {
                    table.Remove(new Point(c, rowIndex));
                }
            }
            maxRowIndex++;
        }

        public void InsertColumn(int columnIndex)
        {
            if (columnIndex <= maxColumnIndex)
            {
                for (int c = maxColumnIndex; c >= columnIndex; c--)
                    for (int r = 0; r <= maxRowIndex; r++)
                    {
                        if (table.ContainsKey(new Point(c,r)))
                        {
                            table[new Point(c + 1, r)] = table[new Point(c, r)];
                            table.Remove(new Point(c, r));
                        }
                    }
                for (int r = 0; r <= maxRowIndex; r++)
                {
                    table.Remove(new Point(columnIndex, r));
                }
            }
            maxColumnIndex++;
        }

        public void DeleteRow(int rowIndex)
        {
            if (rowIndex <= maxRowIndex)
            {
                for (int r = rowIndex; r < maxRowIndex;r++ )
                    for (int c = 0; c <= maxColumnIndex; c++)
                    {
                        if (table.ContainsKey(new Point(c, r + 1)))
                        {
                            table[new Point(c, r)] = table[new Point(c, r + 1)];
                            table.Remove(new Point(c, r + 1));
                        }
                    }
                for (int c = 0; c <= maxColumnIndex; c++)
                {
                    table.Remove(new Point(c, maxRowIndex));
                }
            }
            maxRowIndex--;
        }

        public void DeleteColumn(int columnIndex)
        {
            if (columnIndex <= maxColumnIndex)
            {
                for (int c = columnIndex; c < maxColumnIndex; c++)
                    for (int r = 0; r <= maxRowIndex; r++)
                    {
                        if (table.ContainsKey(new Point(c + 1, r)))
                        {
                            table[new Point(c, r)] = table[new Point(c + 1, r)];
                            table.Remove(new Point(c + 1, r));
                        }
                    }
                for (int r = 0; r <= maxRowIndex; r++)
                {
                    table.Remove(new Point(maxColumnIndex, r));
                }
            }
            maxColumnIndex--;
        }

        public string Get(int row, int column)
        {
            return table[new Point(column, row)];
        }
    }
}
