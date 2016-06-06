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
        int ColumnsCount { get; set; }
        int RowsCount { get; set; }
        int MaxChangedColumn { get; set; }
        int MaxChangedRow { get; set; }

        Cell this[int x, int y] { get; set; }
        Cell this[Point point] { get; set; }

        Dictionary<int, int> GetShiftedRowsCoords(int yShiftInPixels, int yShiftInCells);
        Dictionary<int, int> GetShiftedColumnsCoords(int xShiftInPixels, int xShiftInCells);

        void AddRow(int y);
        void AddColumn(int x);
        void RemoveRow(int y);
        void RemoveColumn(int x);

        void CutRow(int y);
        void CutColumn(int x);
        void CopyRow(int y);
        void CopyColumn(int x);
        void PastRow(int y);
        void PastColumn(int x);

        void PushData(Point point, string data);
        void Resize();
        void Resize(double deltaX, double deltaY);        
    }
}
