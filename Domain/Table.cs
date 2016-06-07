using Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Table : ITable
    {
        public int ColumnsCount { get; set; }
        public int RowsCount { get; set; }
        public int MaxChangedColumn { get; set; }
        public int MaxChangedRow { get; set; }

        private Dictionary<Point, Cell> table;

        public IBuffer buffer;
        public ISerializer serializer;

        public Table(IBuffer buffer, ISerializer serializer)
        {

            ColumnsCount = 200;
            RowsCount = 200;
            MaxChangedColumn = 0;
            MaxChangedRow = 0;

            table = new Dictionary<Point, Cell>();

            for (int x = 1; x <= ColumnsCount; x++) // инициализация словаря
            {
                for (int y = 1; y <= RowsCount; y++)
                {
                    table.Add(new Point(x, y), new Cell(x, y));
                }
            }

            this.buffer = buffer;
            this.serializer = serializer;
        }

        public Dictionary<int, int> GetShiftedRowsCoords(int yShiftInPixels, int yShiftInCells)
        {
            var result = new Dictionary<int, int>();
            var yCoord = yShiftInPixels;
            for (int i = yShiftInCells + 1; i <= RowsCount; i++)
            {
                result.Add(i, yCoord);
                yCoord += Cell.Height;
            }
            return result;
        }

        public Dictionary<int, int> GetShiftedColumnsCoords(int xShiftInPixels, int xShiftInCells)
        {
            var result = new Dictionary<int, int>();
            var xCoord = xShiftInPixels;
            for (int i = xShiftInCells + 1; i <= ColumnsCount; i++)
            {
                result.Add(i, xCoord);
                xCoord += Cell.Width;
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
                table[new Point(x, y)].SetNewCoords(x, y);
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
                table[point].SetNewCoords(point.X, point.Y);
            }
        }

        public void AddRow(int rowIndex)
        {
            for (int r = RowsCount - 1; r >= rowIndex; r--)
                for (int c = 1; c <= ColumnsCount; c++)
                    this[c, r + 1] = this[c, r];

            for (int c = 1; c <= ColumnsCount; c++)
                table[new Point(c, rowIndex)] = new Cell(c, rowIndex);

            if (rowIndex <= MaxChangedRow) MaxChangedRow++;
            Resize();
        }

        public void AddColumn(int columnIndex)
        {
            for (int c = ColumnsCount - 1; c >= columnIndex; c--)
                for (int r = 1; r <= RowsCount; r++)
                    this[c + 1, r] = this[c, r];

            for (int r = 1; r <= RowsCount; r++)
                table[new Point(columnIndex, r)] = new Cell(columnIndex, r);

            if (columnIndex <= MaxChangedColumn) MaxChangedColumn++;
            Resize();
        }

        public void RemoveRow(int rowIndex)
        {
            for (int r = rowIndex; r < RowsCount; r++)
                for (int c = 1; c <= ColumnsCount; c++)
                    this[c, r] = this[c, r + 1];

            for (int c = 1; c <= ColumnsCount; c++)
                table[new Point(c, RowsCount)] = new Cell(c, RowsCount);

            if (rowIndex <= MaxChangedRow) MaxChangedRow--;
            Resize();
        }

        public void RemoveColumn(int columnIndex)
        {
            for (int c = columnIndex; c < ColumnsCount; c++)
                for (int r = 1; r <= RowsCount; r++)
                    this[c, r] = this[c + 1, r];

            for (int r = 1; r <= RowsCount; r++)
                table[new Point(ColumnsCount, r)] = new Cell(ColumnsCount, r);

            if (columnIndex <= MaxChangedColumn) MaxChangedColumn--;
            Resize();
        }
        public void PushData(Point point, string data)
        {

            table[point].PushData(data);
            if (point.X >= MaxChangedColumn) MaxChangedColumn = point.X;
            if (point.Y >= MaxChangedRow) MaxChangedRow = point.Y;
            Resize();
        }
        public void SetFormula(Point point, string data)
        {
            table[point].SetFormula(data);
            Resize();
        }

        public void Resize()
        {
            if (MaxChangedColumn == ColumnsCount && MaxChangedRow == RowsCount)
                Resize(0.5, 0.5);
            else if (MaxChangedColumn == ColumnsCount)
                Resize(0.5, 0);
            else if (MaxChangedRow == RowsCount)
                Resize(0, 0.5);
            else return;

        }

        public void Resize(double deltaX, double deltaY)
        {
            var oldTableWidth = ColumnsCount;
            var oldTableHeight = RowsCount;
            ColumnsCount += (int)(deltaX * ColumnsCount);
            RowsCount += (int)(deltaY * RowsCount);

            for (int x = 1; x <= ColumnsCount; x++)
                for (int y = 1; y <= RowsCount; y++)
                    if (x > oldTableWidth || y > oldTableHeight)
                        table.Add(new Point(x, y), new Cell(x, y));
        }




        public void CutRow(int rowIndex)
        {
            var bufferedRow = new Dictionary<int, string>();
            for (int c = 1; c <= ColumnsCount; c++)
            {
                bufferedRow.Add(c, this[c, rowIndex].Data);
                table[new Point(c, rowIndex)].PushData("");
            }
            buffer.AddRow(bufferedRow);
        }

        public void CutColumn(int columnIndex)
        {
            var bufferedColumn = new Dictionary<int, string>();
            for (int r = 1; r <= RowsCount; r++)
            {
                bufferedColumn.Add(r, this[columnIndex, r].Data);
                table[new Point(columnIndex, r)].PushData("");
            }
            buffer.AddColumn(bufferedColumn);
        }

        public void CopyRow(int rowIndex)
        {
            var bufferedRow = new Dictionary<int, string>();
            for (int c = 1; c <= ColumnsCount; c++)
            {
                bufferedRow.Add(c, this[c, rowIndex].Data);
            }
            buffer.AddRow(bufferedRow);
        }

        public void CopyColumn(int columnIndex)
        {
            var bufferedColumn = new Dictionary<int, string>();
            for (int r = 1; r <= RowsCount; r++)
            {
                bufferedColumn.Add(r, this[columnIndex, r].Data);
            }
            buffer.AddColumn(bufferedColumn);
        }

        public void PastRow(int rowIndex)
        {
            var bufferedRow = buffer.GetRow();
            for (int c = 1; c <= ColumnsCount; c++)
            {
                table[new Point(c, rowIndex)] = new Cell(c, rowIndex);
                if (bufferedRow.ContainsKey(c))
                    table[new Point(c, rowIndex)].PushData(bufferedRow[c]);
                else table[new Point(c, rowIndex)].PushData("");
            }
        }

        public void PastColumn(int columnIndex)
        {
            var bufferedColumn = buffer.GetColumn();
            for (int r = 1; r <= RowsCount; r++)
            {
                table[new Point(columnIndex, r)] = new Cell(columnIndex, r);
                if (bufferedColumn.ContainsKey(r))
                    table[new Point(columnIndex, r)].PushData(bufferedColumn[r]);
                else table[new Point(columnIndex, r)].PushData("");
            }
        }


        public void Serialize()
        {
            serializer.Serialise(table);
        }

        public void Deserialize()
        {
            table = (Dictionary<Point, Cell>)serializer.Deserialize(table.GetType());
        }



    }
}
