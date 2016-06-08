using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface ITable
    {
        int ColumnsCount { get; set; }
        int RowsCount { get; set; }
        int MaxChangedColumn { get; set; }
        int MaxChangedRow { get; set; }
        Dictionary<Point, Cell> GetTable();

        Cell this[int x, int y] { get; set; }
        Cell this[Point point] { get; set; }

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
        void SetFormula(Point point, string data);
        void Resize();
        void Resize(double deltaX, double deltaY);
        void Serialize();
        void Deserialize();
    }
}
