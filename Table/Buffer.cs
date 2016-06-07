using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class Buffer : IBuffer
    {
        private Dictionary<int, string> bufferedRow;
        private Dictionary<int, string> bufferedColumn;

        public Buffer()
        {
            bufferedRow = new Dictionary<int, string>();
            bufferedColumn = new Dictionary<int, string>();
        }

        public void AddRow(Dictionary<int, string> row)
        {
            bufferedRow = row;
        }

        public void AddColumn(Dictionary<int, string> column)
        {
            bufferedColumn = column;
        }

        public Dictionary<int, string> GetRow()
        {
            return bufferedRow;
        }

        public Dictionary<int, string> GetColumn()
        {
            return bufferedColumn;
        }
    }
}

