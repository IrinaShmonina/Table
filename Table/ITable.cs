using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public interface ITable
    {
        int TableWidth { get; set; }
        int TableHeight { get; set; }
        int MaxChangedColumn { get; set; }
        int MaxChangedRow { get; set; }

        Cell this[int x, int y] { get; set; }
        Cell this[Point point] { get; set; }

        void ChangeColumnWidth(int number, int width);
        void ChangeRowHeight(int number, int heigth);
        int GetRowHeight(int number);
        int GetColumnWidth(int number);

        void IncreaseColumnsCount(int number);
        void IncreaseRowsCount(int number);
        void DecreaseColumnsCount(int number);
        void DecreaseRowsCount(int number);

        Dictionary<int, int> GetShiftedRowsCoords(int yShiftInPixels, int yShiftInCells);
        Dictionary<int, int> GetShiftedColumnsCoords(int xShiftInPixels, int xShiftInCells);

        void InsertRow(int y);
        void InsertColumn(int x);
        void RemoveRow(int y);
        void RemoveColumn(int x);
        void PushData(Point point, string data);
        void Resize();
        void Resize(int deltaX, int deltaY);        
    }
}
