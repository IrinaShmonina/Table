using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class FileLoader : ILoader
    {
        public void UpLoad(string data, string path)
        {
            File.WriteAllText(path, data);
        }

        public string DownLoad(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
