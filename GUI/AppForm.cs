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
        private Table table;
        private MenuItem mainMenu;
        public AppForm(Table table)
        {
            this.table = table;
            this.Text = "Электронная Таблица";
            this.MinimumSize = new Size(800, 600);
            this.AutoSize = true;
            this.WindowState = FormWindowState.Maximized;
            this.Resize += (sender, args) => { };


        }
    }
}
