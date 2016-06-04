﻿using System;
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
        public int RowsAmount = 100;
        public int ColumnsAmount = 100;
        public Dictionary<int, int> RowsHeight;
        public Dictionary<int, int> ColumnsWidth;


        public void ChangeColumnWidth(int number, int width)
        {
            ColumnsWidth[number] = width;
            for (int i = 1; i<= ColumnsAmount; i++)
            {
                    table[new Point(number, i)].SetNewSize(width, RowsHeight[i]);
                }
            }

        public void ChangeRowHeight(int number, int heigth)
        {
            RowsHeight[number] = heigth;
            for (int i = 1; i <= RowsAmount; i++)
            {
                    table[new Point(i, number)].SetNewSize(ColumnsWidth[i], heigth);
                }
            }

        public void IncreaseRowsCount(int number)
        {
            for (int i = RowsAmount - 1; i >= number; i--)
            {
                RowsHeight[i + 1] = RowsHeight[i];
            }
            RowsHeight[number] = Cell.defaultHeight;
        }

        public void IncreaseColumnsCount(int number)
        {
            for (int i = ColumnsAmount - 1; i >= number; i--)
            {
                ColumnsWidth[i + 1] = ColumnsWidth[i];
            }
            ColumnsWidth[number] = Cell.defaultWidth;
        }

        public void DecreaseRowsCount(int number)
        {
            for (int i = number; i < RowsAmount; i++)
            {
                RowsHeight[i] = RowsHeight[i+1];
            }
            RowsHeight[RowsAmount] = Cell.defaultHeight;
        }

        public void DecreaseColumnsCount(int number)
        {
            for (int i = number; i < ColumnsAmount; i++)
            {
                ColumnsWidth[i] = ColumnsWidth[i + 1];
            }
            ColumnsWidth[ColumnsAmount] = Cell.defaultWidth;
        }

        public Dictionary<int, int> GetShiftedRowsCoords(int yShift)
        {
            var result = new Dictionary<int, int>();
            var yCoord = yShift;
            for (int i = 1; i <= RowsHeight.Count; i++)
            {
                result.Add(i, yCoord);
                yCoord += RowsHeight[i];
            }
            return result;
        }

        public Dictionary<int, int> GetShiftedColumnsCoords(int xShift)
        {
            var result = new Dictionary<int, int>();
            var xCoord = xShift;
            for (int i = 1; i <= ColumnsWidth.Count; i++)
            {
                result.Add(i, xCoord);
                xCoord += ColumnsWidth[i];
            }
            return result;
        }

        //public Dictionary<Point, Point> GetShiftedCellsCoords(int xShift, int yShift)
        //{
        //    var result = new Dictionary<Point, Point>();
        //    var xCoord = xShift;
        //    var yCoord = yShift;
        //    for (int i = 1; i <= ColumnsWidth.Count; i++)
        //    {               
        //        for (int j = 1; j <= RowsHeigth.Count; j++)
        //        {
        //            result.Add(new Point(i,j),new Point(xCoord, yCoord));
        //            yCoord += RowsHeigth[j];
        //        }
        //        xCoord += ColumnsWidth[i];
        //        yCoord = yShift;
        //    }
        //    return result;
        //}

        public Table()
        {
            table = new Dictionary<Point, Cell>();
            ColumnsWidth = new Dictionary<int, int>();
            RowsHeight = new Dictionary<int, int>();
            
            for (int x = 1; x <= ColumnsAmount; x++) // инициализация словаря
            {
                for (int y = 1; y <= RowsAmount; y++)
                {
                    table.Add(new Point(x, y), new Cell(x, y));
                }
            }

            for (int i = 1; i <= ColumnsAmount; i++)
                ColumnsWidth.Add(i, Cell.defaultWidth);

            for (int i = 1; i <= RowsAmount; i++)
                RowsHeight.Add(i, Cell.defaultHeight);
            
        }
        //public Cell TryAddCell(int x, int y)
        //{
        //    if (!table.ContainsKey(new Point(x, y)))
        //    {
        //        table.Add(new Point(x, y), new Cell(x, y, RowsHeigth[x], ColumnsWidth[y]));
        //    }
        //    return table[new Point(x, y)];
        //}
        public Cell this[int x, int y]
        {
            get
            {
                return table[new Point(x, y)];
            }
            set
            {

                table[new Point(x, y)] = value; //.SetNewCoords(x, y)
                table[new Point(x, y)] = table[new Point(x, y)].SetNewCoords(x, y);
                //if (y > RowsAmount) RowsAmount = y;
                //if (x > ColumnsAmount) ColumnsAmount = x;

            }
        }

        public void InsertRow(int rowIndex)
        {
            if (rowIndex < RowsAmount)
            {
                IncreaseRowsCount(rowIndex);
                for (int r = RowsAmount - 1; r >= rowIndex; r--)
                    for (int c = 1; c <= ColumnsAmount; c++)
                            this[c, r + 1] = this[c, r];

                for (int c = 1; c <= ColumnsAmount; c++)
                    table[new Point(c, rowIndex)] = new Cell(c,rowIndex);

            }

        }

        public void InsertColumn(int columnIndex)
        {
            if (columnIndex < ColumnsAmount)
            {
                IncreaseColumnsCount(columnIndex);
                for (int c = ColumnsAmount - 1; c >= columnIndex; c--)
                    for (int r = 1; r <= RowsAmount; r++)
                            this[c + 1, r] = this[c, r];

                for (int r = 1; r <= RowsAmount; r++)
                    table[new Point(columnIndex, r)] = new Cell(columnIndex,r);
            }

        }

        public void DeleteRow(int rowIndex)
        {
            if (rowIndex <= RowsAmount)
            {
                DecreaseRowsCount(rowIndex);
                for (int r = rowIndex; r < RowsAmount; r++)
                    for (int c = 1; c <= ColumnsAmount; c++)
                            this[c, r] = this[c, r + 1];

                for (int c = 1; c <= ColumnsAmount; c++)
                    table[new Point(c, RowsAmount)] = new Cell(c, RowsAmount);

                //RowsAmount--;
            }

        }

        public void DeleteColumn(int columnIndex)
        {
            if (columnIndex < ColumnsAmount)
            {
                DecreaseColumnsCount(columnIndex);
                for (int c = columnIndex; c < ColumnsAmount; c++)
                    for (int r = 1; r <= RowsAmount; r++)
                            this[c, r] = this[c + 1, r];
                for (int r = 1; r <= RowsAmount; r++)
                    table[new Point(ColumnsAmount, r)] = new Cell(ColumnsAmount,r);
                //ColumnsAmount--;
            }

        }



        public void Resize(int deltaX, int deltaY)
        {
            throw new NotImplementedException();
        }


    }
}
