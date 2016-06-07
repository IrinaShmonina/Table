using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface ISerializer
    {
        void Serialise(object obj);
        object Deserialize(Type type);
    }
}
