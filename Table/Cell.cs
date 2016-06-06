using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class Cell
    {
        public const int Width = 60;
        public const int Height = 20;
        public int ColumnNumber { get; private set; }
        public int RowNumber { get; private set; }
        public string Data { get; private set; }
        public string text; //{ get; private set; }

        public Cell(int columnNumber, int rowNumber, string data = "")
        {
            this.ColumnNumber = columnNumber;
            this.RowNumber = rowNumber;
            this.Data = data;
        }
        public void PushData(string x)
        {
            Data = x;
        }
        //добавил
        public void SetNewCoords(int x, int y)
        {
            ColumnNumber = x;
            RowNumber = y;
        }
        
    }
}
