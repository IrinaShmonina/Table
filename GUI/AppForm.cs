using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App;

namespace GUI
{
    class AppForm : Form
    {
        private Cell table;
        public AppForm(Table table)
        {
            this.table = table;
            this.Text = "Электронная Таблица";
            this.Size = new Size(500, 500);
            //this.ClientSize = new Size(300, 300);

        }
    }
}
