using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domain;
using Ninject;
using Infrastructure;

namespace GUI
{
    class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var container = new StandardKernel();//DI container
            container.Bind<IBuffer>().To<Domain.Buffer>();
            container.Bind<ITable>().To<Table>();
            container.Bind<ISerializer>().To<JsonSerializer>();
            container.Bind<ILoader>().To<FileLoader>();
            var form = container.Get<AppForm>();
            Application.Run(form);
        }
    }
}
