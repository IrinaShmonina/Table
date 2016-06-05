using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class Table : ITable
    {
        public int TableWidth { get; private set; }
        public int TableHeight {get; private set;}
        public int MaxChangedColumn {get; private set;}
        public int MaxChangedRow {get; private set;}
        public readonly Dictionary<Point, Cell> table;
        public readonly Dictionary<int, int> RowsHeight;
        public readonly Dictionary<int, int> ColumnsWidth;

        public Table()
        {
            TableWidth = 20;
            TableHeight = 20;
            MaxChangedColumn = 0;
            MaxChangedRow = 0;

            table = new Dictionary<Point, Cell>();
            ColumnsWidth = new Dictionary<int, int>();
            RowsHeight = new Dictionary<int, int>();

            for (int x = 1; x <= TableWidth; x++) // инициализация словаря
            {
                for (int y = 1; y <= TableHeight; y++)
                {
                    table.Add(new Point(x, y), new Cell(x, y));
                }
            }

            for (int i = 1; i <= TableWidth; i++)
                ColumnsWidth.Add(i, Cell.defaultWidth);

            for (int i = 1; i <= TableHeight; i++)
                RowsHeight.Add(i, Cell.defaultHeight);

        }

        public void ChangeColumnWidth(int number, int width)
        {
            ColumnsWidth[number] = width;
            for (int i = 1; i <= TableHeight; i++)
                table[new Point(number, i)].SetNewSize(width, RowsHeight[i]);

            if (number > MaxChangedColumn) MaxChangedColumn = number;
            NeedResize();
        }

        public void ChangeRowHeight(int number, int heigth)
        {
            RowsHeight[number] = heigth;
            for (int i = 1; i <= TableWidth; i++)
                table[new Point(i, number)].SetNewSize(ColumnsWidth[i], heigth);

            if (number > MaxChangedRow) MaxChangedRow = number;
            NeedResize();
        }

        public void IncreaseColumnsCount(int number)
        {
            for (int i = TableWidth - 1; i >= number; i--)
                ColumnsWidth[i + 1] = ColumnsWidth[i];
            ColumnsWidth[number] = Cell.defaultWidth;

            if (number <= MaxChangedColumn) MaxChangedColumn++;
            NeedResize();
        }

        public void IncreaseRowsCount(int number)
        {
            for (int i = TableHeight - 1; i >= number; i--)
                RowsHeight[i + 1] = RowsHeight[i];
            RowsHeight[number] = Cell.defaultHeight;

            if (number <= MaxChangedRow) MaxChangedRow++;
            NeedResize();
        }
        
        public void DecreaseColumnsCount(int number)
        {
            for (int i = number; i < TableWidth; i++)
                ColumnsWidth[i] = ColumnsWidth[i + 1];
            ColumnsWidth[TableWidth] = Cell.defaultWidth;

            if (number <= MaxChangedColumn) MaxChangedColumn--;
            NeedResize();
        }

        public void DecreaseRowsCount(int number)
        {
            for (int i = number; i < TableHeight; i++)
                RowsHeight[i] = RowsHeight[i + 1];
            RowsHeight[TableHeight] = Cell.defaultHeight;

            if (number <= MaxChangedRow) MaxChangedRow--;
            NeedResize();
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

        public Cell this[int x, int y]
        {
            get
            {
                return table[new Point(x, y)];
            }
            set
            {
                table[new Point(x, y)] = value;
                table[new Point(x, y)].SetNewCoords(x,y);
            }
        }
        public Cell this[Point point]
        {
            get
            {
                return table[point];
            }
            set
            {
                table[point] = value;
                table[point].SetNewCoords(point.X,point.Y);
            }
        }

        public void InsertRow(int rowIndex)
        {
            if (rowIndex < TableHeight)
            {
                IncreaseRowsCount(rowIndex);
                for (int r = TableHeight - 1; r >= rowIndex; r--)
                    for (int c = 1; c <= TableWidth; c++)
                            this[c, r + 1] = this[c, r];

                for (int c = 1; c <= TableWidth; c++)
                    table[new Point(c, rowIndex)] = new Cell(c,rowIndex);
            }
        }

        public void InsertColumn(int columnIndex)
        {
            if (columnIndex < TableWidth)
            {
                IncreaseColumnsCount(columnIndex);
                for (int c = TableWidth - 1; c >= columnIndex; c--)
                    for (int r = 1; r <= TableHeight; r++)
                            this[c + 1, r] = this[c, r];

                for (int r = 1; r <= TableHeight; r++)
                    table[new Point(columnIndex, r)] = new Cell(columnIndex,r);
            }
        }

        public void RemoveRow(int rowIndex)
        {
            if (rowIndex <= TableHeight)
            {
                DecreaseRowsCount(rowIndex);
                for (int r = rowIndex; r < TableHeight; r++)
                    for (int c = 1; c <= TableWidth; c++)
                            this[c, r] = this[c, r + 1];

                for (int c = 1; c <= TableWidth; c++)
                    table[new Point(c, TableHeight)] = new Cell(c, TableHeight);
            }
        }

        public void RemoveColumn(int columnIndex)
        {
            if (columnIndex < TableWidth)
            {
                DecreaseColumnsCount(columnIndex);
                for (int c = columnIndex; c < TableWidth; c++)
                    for (int r = 1; r <= TableHeight; r++)
                            this[c, r] = this[c + 1, r];
                for (int r = 1; r <= TableHeight; r++)
                    table[new Point(TableWidth, r)] = new Cell(TableWidth,r);
            }
        }
        public void PushData(Point point, string data)
        {
            
            table[point].PushData(data);
            if (point.X >= MaxChangedColumn) MaxChangedColumn = point.X;
            if (point.Y >= MaxChangedRow) MaxChangedRow = point.Y;
            NeedResize();
        }

        public void NeedResize()
        {
            if (MaxChangedColumn == TableWidth && MaxChangedRow == TableHeight)
                Resize(1, 0);
            else if (MaxChangedColumn == TableWidth)
                Resize(1, 0);
            else if (MaxChangedRow == TableHeight)
                Resize(0, 1);
        }

        public void Resize(int deltaX, int deltaY)
        {
            var oldTableWidth = TableWidth;
            var oldTableHeight = TableHeight;
            TableWidth += deltaX * TableWidth;
            TableHeight += deltaY * TableHeight;

            for (int x = 1; x <= TableWidth; x++)
                for (int y = 1; y <= TableHeight; y++)
                    if (x > oldTableWidth || y > oldTableHeight)
                        table.Add(new Point(x, y), new Cell(x, y));

            for (int i = oldTableWidth + 1; i <= TableWidth; i++)
                ColumnsWidth.Add(i, Cell.defaultWidth);

            for (int i = oldTableHeight + 1; i <= TableHeight; i++)
                RowsHeight.Add(i, Cell.defaultHeight);
        }
    }
}
