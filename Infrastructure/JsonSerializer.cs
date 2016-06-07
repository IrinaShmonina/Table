using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class JsonSerializer : ISerializer
    {
        public void Serialise(object obj)
        {
            var serializedData = JsonConvert.SerializeObject(obj);
            File.WriteAllText("doc.txt", serializedData);
        }
        public object Deserialize(Type type)
        {
            var deserializedData = JsonConvert.DeserializeObject(File.ReadAllText("doc.txt"), type);
            return deserializedData;

        }
    }
}
