using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    interface ITable<T>
    {
        T this[int x, int y] { get; set; }
        void InsertRow(int y);
        void InsertColumn(int x);
        void CutRow(int y);
        void CutColumn(int x);
        void Resize(int deltaX, int deltaY);
    }
}
