﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface ISerializer
    {
        string Serialize(object obj);
        object Deserialize(Type type, string data);
    }
}
