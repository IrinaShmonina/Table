using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public interface IBuffer 
    {
        void AddRow(Dictionary<int, string> row);
        void AddColumn(Dictionary<int, string> column);
        Dictionary<int, string> GetRow();
        Dictionary<int, string> GetColumn();
    }
}
