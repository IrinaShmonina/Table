using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IBuffer
    {
        void AddRow(Dictionary<int, string> row);
        void AddColumn(Dictionary<int, string> column);
        Dictionary<int, string> GetRow();
        Dictionary<int, string> GetColumn();
    }
}
