using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class Table : ITable<Cell>
    {
        private readonly Dictionary<Point, Cell> table;//string заменится на cell везде
        private int RowsAmount = 20;
        private int CollumsAmount = 20;
        public Dictionary<int, int> RowsHeigth;
        public Dictionary<int, int> ColumnsWidth;


        public void ChangeColumnWidth(int number, int width)
        {
            for (int i = 0; i< CollumsAmount; i++)
            {
                if (table.ContainsKey(new Point(number,i)))
                {
                    table[new Point(number, i)].SetNewSize(width, RowsHeigth[i]);
                }
            }
        }
        public void ChangeRowHeigth(int number, int heigth)
        {
            for (int i = 0; i < RowsAmount; i++)
            {
                if (table.ContainsKey(new Point(i,number)))
                {
                    table[new Point(i, number)].SetNewSize(ColumnsWidth[i], heigth);
                }
            }
        }
        public void IncreaseRowsCount(int number)
        {
            RowsHeigth.Add(RowsAmount + 1, 0);
            for (int i = RowsAmount; i >= number; i--)
            {
                RowsHeigth[i + 1] = RowsHeigth[i];
            }
            RowsHeigth[number] = Cell.defaultHeigth;
        }
        public void IncreaseColumnsCount(int number)
        {
            ColumnsWidth.Add(CollumsAmount + 1, 0);
            for (int i = CollumsAmount; i >= number; i--)
            {
                ColumnsWidth[i + 1] = ColumnsWidth[i];
            }
            ColumnsWidth[number] = Cell.defaultWidth;
        }

        public void DecreaseRowsCount(int number)
        {
            
            for (int i = number; i < RowsAmount; i++)
            {
                RowsHeigth[i] = RowsHeigth[i+1];
            }
            RowsHeigth.Remove(RowsAmount);
        }
        public void DecreaseColumnsCount(int number)
        {
            for (int i = number; i < CollumsAmount; i++)
            {
                ColumnsWidth[i] = ColumnsWidth[i + 1];
            }
            ColumnsWidth.Remove(CollumsAmount);
        }

        public Table()
        {
            table = new Dictionary<Point, Cell>();
            RowsHeigth = new Dictionary<int,int>();
            ColumnsWidth = new Dictionary<int,int>();
            for (int i = 1; i <= RowsAmount; i++)
                RowsHeigth[i] = Cell.defaultHeigth;
            for (int i = 1; i <= CollumsAmount; i++)
                ColumnsWidth[i] = Cell.defaultWidth;
        }

        public Cell this[int x, int y]
        {
            get
            {
                return table[new Point(x, y)];
            }
            set
            {

                table[new Point(x, y)] = value.SetNewCoords(x, y);
                if (y > RowsAmount) RowsAmount = y;
                if (x > CollumsAmount) CollumsAmount = x;

            }
        }

        public void InsertRow(int rowIndex)
        {
            if (rowIndex <= RowsAmount)
            {
                IncreaseRowsCount(rowIndex);
                for (int r = RowsAmount; r >= rowIndex; r--)
                    for (int c = 0; c <= CollumsAmount; c++)
                    {
                        table.Remove(new Point(c, r + 1));
                        if (table.ContainsKey(new Point(c, r)))
                        {
                            this[c, r + 1] = this[c, r];
                        }
                    }
                for (int c = 0; c <= CollumsAmount; c++)
                {
                    table.Remove(new Point(c, rowIndex));
                }
                RowsAmount++;
            }

        }

        public void InsertColumn(int columnIndex)
        {
            if (columnIndex <= CollumsAmount)
            {
                IncreaseColumnsCount(columnIndex);
                for (int c = CollumsAmount; c >= columnIndex; c--)
                    for (int r = 0; r <= RowsAmount; r++)
                    {
                        table.Remove(new Point(c + 1, r));
                        if (table.ContainsKey(new Point(c, r)))
                        {
                            this[c + 1, r] = this[c, r];
                        }
                    }
                for (int r = 0; r <= RowsAmount; r++)
                {
                    table.Remove(new Point(columnIndex, r));
                }
                CollumsAmount++;
            }

        }

        public void DeleteRow(int rowIndex)
        {
            if (rowIndex <= RowsAmount)
            {
                DecreaseRowsCount(rowIndex);
                for (int r = rowIndex; r < RowsAmount; r++)
                    for (int c = 0; c <= CollumsAmount; c++)
                    {
                        table.Remove(new Point(c, r));
                        if (table.ContainsKey(new Point(c, r + 1)))
                        {
                            this[c, r] = this[c, r + 1];
                        }
                    }
                for (int c = 0; c <= CollumsAmount; c++)
                {
                    table.Remove(new Point(c, RowsAmount));
                }
                RowsAmount--;
            }

        }

        public void DeleteColumn(int columnIndex)
        {
            if (columnIndex <= CollumsAmount)
            {
                DecreaseColumnsCount(columnIndex);
                for (int c = columnIndex; c < CollumsAmount; c++)
                    for (int r = 0; r <= RowsAmount; r++)
                    {
                        table.Remove(new Point(c, r));
                        if (table.ContainsKey(new Point(c + 1, r)))
                        {
                            this[c, r] = this[c + 1, r];
                        }
                    }
                for (int r = 0; r <= RowsAmount; r++)
                {
                    table.Remove(new Point(CollumsAmount, r));
                }
                CollumsAmount--;
            }

        }



        public void Resize(int deltaX, int deltaY)
        {
            throw new NotImplementedException();
        }


    }
}
