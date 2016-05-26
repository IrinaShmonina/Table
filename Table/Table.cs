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
        public readonly Dictionary<Point, Cell> table;//string заменится на cell везде
        public int RowsAmount = 20;
        public int ColumnsAmount = 20;
        public Dictionary<int, int> RowsHeigth;
        public Dictionary<int, int> ColumnsWidth;


        public void ChangeColumnWidth(int number, int width)
        {
            ColumnsWidth[number] = width;
            for (int i = 1; i<= ColumnsAmount; i++)
            {
                if (table.ContainsKey(new Point(number,i)))
                {
                    table[new Point(number, i)].SetNewSize(width, RowsHeigth[i]);
                }
            }
        }
        public void ChangeRowHeigth(int number, int heigth)
        {
            RowsHeigth[number] = heigth;
            for (int i = 1; i <= RowsAmount; i++)
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
            ColumnsWidth.Add(ColumnsAmount + 1, 0);
            for (int i = ColumnsAmount; i >= number; i--)
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
            for (int i = number; i < ColumnsAmount; i++)
            {
                ColumnsWidth[i] = ColumnsWidth[i + 1];
            }
            ColumnsWidth.Remove(ColumnsAmount);
        }

        public Table()
        {
            table = new Dictionary<Point, Cell>();
            RowsHeigth = new Dictionary<int,int>();
            ColumnsWidth = new Dictionary<int,int>();
            for (int i = 1; i <= RowsAmount; i++)
                RowsHeigth.Add(i, Cell.defaultHeigth);
            for (int i = 1; i <= ColumnsAmount; i++)
                ColumnsWidth.Add(i, Cell.defaultWidth);
        }
        public void AddCell(int x, int y)
        {
            table.Add(new Point(x, y), new Cell(x, y, RowsHeigth[x], ColumnsWidth[y]));
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
                if (x > ColumnsAmount) ColumnsAmount = x;

            }
        }

        public void InsertRow(int rowIndex)
        {
            if (rowIndex <= RowsAmount)
            {
                IncreaseRowsCount(rowIndex);
                for (int r = RowsAmount; r >= rowIndex; r--)
                    for (int c = 1; c <= ColumnsAmount; c++)
                    {
                        table.Remove(new Point(c, r + 1));
                        if (table.ContainsKey(new Point(c, r)))
                        {
                            this[c, r + 1] = this[c, r];
                        }
                    }
                for (int c = 1; c <= ColumnsAmount; c++)
                {
                    table.Remove(new Point(c, rowIndex));
                }
                RowsAmount++;
            }

        }

        public void InsertColumn(int columnIndex)
        {
            if (columnIndex <= ColumnsAmount)
            {
                IncreaseColumnsCount(columnIndex);
                for (int c = ColumnsAmount; c >= columnIndex; c--)
                    for (int r = 1; r <= RowsAmount; r++)
                    {
                        table.Remove(new Point(c + 1, r));
                        if (table.ContainsKey(new Point(c, r)))
                        {
                            this[c + 1, r] = this[c, r];
                        }
                    }
                for (int r = 1; r <= RowsAmount; r++)
                {
                    table.Remove(new Point(columnIndex, r));
                }
                ColumnsAmount++;
            }

        }

        public void DeleteRow(int rowIndex)
        {
            if (rowIndex <= RowsAmount)
            {
                DecreaseRowsCount(rowIndex);
                for (int r = rowIndex; r < RowsAmount; r++)
                    for (int c = 1; c <= ColumnsAmount; c++)
                    {
                        table.Remove(new Point(c, r));
                        if (table.ContainsKey(new Point(c, r + 1)))
                        {
                            this[c, r] = this[c, r + 1];
                        }
                    }
                for (int c = 1; c <= ColumnsAmount; c++)
                {
                    table.Remove(new Point(c, RowsAmount));
                }
                RowsAmount--;
            }

        }

        public void DeleteColumn(int columnIndex)
        {
            if (columnIndex <= ColumnsAmount)
            {
                DecreaseColumnsCount(columnIndex);
                for (int c = columnIndex; c < ColumnsAmount; c++)
                    for (int r = 1; r <= RowsAmount; r++)
                    {
                        table.Remove(new Point(c, r));
                        if (table.ContainsKey(new Point(c + 1, r)))
                        {
                            this[c, r] = this[c + 1, r];
                        }
                    }
                for (int r = 1; r <= RowsAmount; r++)
                {
                    table.Remove(new Point(ColumnsAmount, r));
                }
                ColumnsAmount--;
            }

        }



        public void Resize(int deltaX, int deltaY)
        {
            throw new NotImplementedException();
        }

        
    }
}
