using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Cell
    {
        public const int Width = 60;
        public const int Height = 20;
        public int ColumnNumber { get; private set; }
        public int RowNumber { get; private set; }
        public string Data { get; private set; }
        public string Formula { get; private set; }

        public Cell(int columnNumber, int rowNumber, string data = "", string formula = "")
        {
            this.ColumnNumber = columnNumber;
            this.RowNumber = rowNumber;
            this.Data = data;
            this.Formula = formula;
        }
        public void PushData(string x)
        {
            Data = x;
        }
        public void SetFormula(string f)
        {
            Formula = f;
        }
        //добавил
        public void SetNewCoords(int x, int y)
        {
            ColumnNumber = x;
            RowNumber = y;
        }

    }
}
