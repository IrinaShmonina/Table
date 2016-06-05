using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    interface ITable
    {
        Cell this[int x, int y] { get; set; }
        Cell this[Point point] { get; set; }
        void InsertRow(int y);
        void InsertColumn(int x);
        void RemoveRow(int y);
        void RemoveColumn(int x);
        void Resize(int deltaX, int deltaY);
    }
}
