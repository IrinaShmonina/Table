using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Table
{
    public class Table<T> : ITable<T>
    {
        private readonly Dictionary<Point, T> table;//string заменится на cell везде
        private int maxRowIndex;
        private int maxColumnIndex;

        public Table()
        {
            table = new Dictionary<Point, T>();
        }

        public T this[int x, int y]
        {
            get
            {
                return table[new Point(x, y)];
            }
            set
            {
                table.Add(new Point(x, y), value);
                if (y > maxRowIndex) maxRowIndex = y;
                if (x > maxColumnIndex) maxColumnIndex = x;
            }
        }

        public void InsertRow(int rowIndex)
        {
            if (rowIndex <= maxRowIndex)
            {
                for (int r = maxRowIndex; r >= rowIndex; r--)
                    for (int c = 0; c <= maxColumnIndex; c++)
                    {
                        if (table.ContainsKey(new Point(c, r)))
                        {
                            this[c, r + 1] = this[c, r];
                            table.Remove(new Point(c, r));
                        }
                    }
                for (int c = 0; c <= maxColumnIndex; c++)
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
                        if (table.ContainsKey(new Point(c, r)))
                        {
                            this[c + 1, r] = this[c, r];
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

        public void CutRow(int rowIndex)
        {
            if (rowIndex <= maxRowIndex)
            {
                for (int r = rowIndex; r < maxRowIndex; r++)
                    for (int c = 0; c <= maxColumnIndex; c++)
                    {
                        if (table.ContainsKey(new Point(c, r + 1)))
                        {
                            this[c, r] = this[c, r + 1];
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

        public void CutColumn(int columnIndex)
        {
            if (columnIndex <= maxColumnIndex)
            {
                for (int c = columnIndex; c < maxColumnIndex; c++)
                    for (int r = 0; r <= maxRowIndex; r++)
                    {
                        if (table.ContainsKey(new Point(c + 1, r)))
                        {
                            this[c, r] = this[c + 1, r];
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

        

        public void Resize(int deltaX, int deltaY)
        {
            throw new NotImplementedException();
        }

        
    }
}
