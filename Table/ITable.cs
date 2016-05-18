using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Table
{
    interface ITable<T>
    {
        T this[int x, int y] { get; set; }
        void InsertRow(int y);
        void InsertColomn(int x);
        void Resize(int deltaX, int deltaY);
    }
}
