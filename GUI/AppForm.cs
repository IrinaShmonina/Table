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
        private MenuStrip mainMenu;
        public AppForm(Table table)
        {
            this.table = table;
            this.Text = "Электронная Таблица";
            this.MinimumSize = new Size(800, 600);
            this.AutoSize = true;
            this.WindowState = FormWindowState.Maximized;
            this.Resize += (sender, args) => { };


            mainMenu = new MenuStrip();
            var menuItems = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("Файл"),
                new ToolStripMenuItem("Редактирование",null,
                    new ToolStripMenuItem[]
                    {
                        new ToolStripMenuItem("Вставить строку", null,InsertRow),
                        new ToolStripMenuItem("Вставить столбец", null,InsertColumn),
                        new ToolStripMenuItem("Удалить строку", null,RemoveRow),
                        new ToolStripMenuItem("Удалить столбец", null,RemoveColumn),
                        new ToolStripMenuItem("Изменить ширину столбца", null,(sender,args) => MessageBox.Show("О программе")),
                        new ToolStripMenuItem("Изменить высоту строки", null,(sender,args) => MessageBox.Show("О программе"))
                    }
                    ),
                new ToolStripMenuItem("Формулы"),
                new ToolStripMenuItem("О программе", null,(sender,args) => MessageBox.Show("Электронная таблица, версия 1.0\nРазработчики:\nЕрмаков Степан\nШмонина Ирина\nЛевшин Михаил","О программе"))
            };
            
            //ToolStripMenuItem fileItem = new ToolStripMenuItem("Файл");
            //fileItem.DropDownItems.Add("Создать");
            //fileItem.DropDownItems.Add("Сохранить");
            //mainMenu.Items.Add(fileItem);
            //ToolStripMenuItem aboutItem = new ToolStripMenuItem("О программе");
            //aboutItem.Click += (sender,args) => MessageBox.Show("О программе");
            mainMenu.Items.AddRange(menuItems);

            Controls.Add(mainMenu);
        }
        void InsertRow(object sender, EventArgs e)
        {
            MessageBox.Show("О программе");
        }
        void InsertColumn(object sender, EventArgs e)
        {
            MessageBox.Show("О программе");
        }
        void RemoveRow(object sender, EventArgs e)
        {
            MessageBox.Show("О программе");
        }
        void RemoveColumn(object sender, EventArgs e)
        {
            MessageBox.Show("О программе");
        }

    }
}
