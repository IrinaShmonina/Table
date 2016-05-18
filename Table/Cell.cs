using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Table
{
    class Cell
    {
        public readonly int ColumnNumber;
        public readonly int RowNumber;
        public readonly double Height;
        public readonly double Width;

        

        public Cell(int columnNumber, int rowNumber, int height = 15, int width = 15)
        {
            this.ColumnNumber = columnNumber;
            this.RowNumber = rowNumber;
            this.Height = height;
            this.Width = width;
        }

        public void PushData()
        {
            
        }
        
    }
}
