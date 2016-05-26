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
        private TableLayoutPanel tab;
        public AppForm(Table table)
        {
            this.table = table;
            this.Text = "Электронная Таблица";
            this.MinimumSize = new Size(800, 600);
            this.AutoSize = true;
            this.WindowState = FormWindowState.Maximized;
            this.Resize += (sender, args) => { Invalidate(); };
            DoubleBuffered = true;

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
                        new ToolStripMenuItem("Изменить ширину столбца", null,ChangeColumnWidth),
                        new ToolStripMenuItem("Изменить высоту строки", null,ChangeRowHeigth)
                    }
                    ),
                new ToolStripMenuItem("Формулы"),
                new ToolStripMenuItem("О программе", null,(sender,args) => MessageBox.Show("Электронная таблица, версия 1.0\nРазработчики:\nЕрмаков Степан\nШмонина Ирина\nЛевшин Михаил","О программе"))

            };
            
            mainMenu.Items.AddRange(menuItems);
            Controls.Add(mainMenu);

            var startXcoord = 50;
            var startYcoord = 150;
            tab = new TableLayoutPanel();
            tab.Location = new Point(50, 100);
            tab.Size = new Size(1200, 800);
            tab.BackColor = Color.Yellow;
            tab.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            for (int i = 1; i <= table.RowsAmount; i++)//
            {
                tab.RowStyles.Add(new RowStyle(SizeType.Absolute, table.RowsHeigth[i]));
            }
            for (int i = 1; i <= table.ColumnsAmount; i++)//
            {
                tab.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, table.ColumnsWidth[i]));
            }
            for (int i = 1; i <= table.ColumnsAmount; i++)
                for (int j = 1; j <= table.RowsAmount; j++)
                {
                    var x = i;
                    var y = j;
                        var textbox = new TextBox();
                        textbox.TextChanged += (sender, args) =>
                            {

                                if (!table.table.ContainsKey(new Point(x, y)))
                                {
                                    table.AddCell(x, y);
                                }
                            };
                        tab.Controls.Add(textbox, x, y);
                }
            Controls.Add(tab);

            Paint += (sender, args) =>
                {
                    //var graphics = args.Graphics;
                    //var pen = new Pen(Color.Black, 1);
                    //var startXcoord = 50;
                    //var startYcoord = 150;
                    //var currentXcoord = startXcoord;
                    //var currentYcoord = startYcoord;
                    //var totalWidth = 0;
                    //var totalHeigth = 0;
                    //for (int i = 1; i <= table.RowsAmount; i++) totalHeigth += table.RowsHeigth[i];
                    //for (int i = 1; i <= table.ColumnsAmount; i++) totalWidth += table.ColumnsWidth[i];
                    //for (int i = 1; i <= table.RowsAmount; i++)
                    //{
                    //    currentYcoord += table.RowsHeigth[i];
                    //    graphics.DrawLine(pen, new Point(startXcoord, currentYcoord), new Point(totalWidth + startXcoord, currentYcoord));
                    //}
                    //for (int i = 1; i <= table.ColumnsAmount; i++)//
                    //{
                    //    currentXcoord += table.ColumnsWidth[i];
                    //    graphics.DrawLine(pen, new Point(currentXcoord, startYcoord), new Point(currentXcoord, totalHeigth + startYcoord));
                    //}
                };
        }
        void InsertRow(object sender, EventArgs e)
        {
            var i = 5;
            table.InsertRow(i);
            //tab.RowStyles.Insert(i, new RowStyle(SizeType.Absolute, table.RowsHeigth[i]));
            //for (int j = 1; j <= table.ColumnsAmount; j++)
            //{
            //    var x = j;
            //    var textbox = new TextBox();
            //    textbox.TextChanged += (s, a) =>
            //    {

            //        if (!table.table.ContainsKey(new Point(x, i)))
            //        {
            //            table.AddCell(x, i);
            //        }
            //    };
            //    tab.Controls.Add(textbox, x, i);
            //}
        }
        void InsertColumn(object sender, EventArgs e)
        {
            var i = 5;
            table.InsertColumn(i);
            //tab.ColumnStyles.Insert(i, new ColumnStyle(SizeType.Absolute, table.ColumnsWidth[i]));
            //for (int j = 1; j <= table.RowsAmount; j++)
            //{
            //    var y = j;
            //    var textbox = new TextBox();
            //    textbox.TextChanged += (s, a) =>
            //    {

            //        if (!table.table.ContainsKey(new Point(i, y)))
            //        {
            //            table.AddCell(i, y);
            //        }
            //    };
            //    tab.Controls.Add(textbox, i, y);
            //}


            
            //Invalidate();
        }
        void RemoveRow(object sender, EventArgs e)
        {
            var i = 5;
            table.DeleteRow(i);
            tab.RowStyles.RemoveAt(i);
            //Invalidate();
        }
        void RemoveColumn(object sender, EventArgs e)
        {
            var i = 5;
            table.DeleteColumn(i);
            tab.ColumnStyles.RemoveAt(i);
            //Invalidate();
        }
        void ChangeRowHeigth(object sender, EventArgs e)
        {
            table.ChangeRowHeigth(8,30);
            Invalidate();
        }
        void ChangeColumnWidth(object sender, EventArgs e)
        {
            table.ChangeColumnWidth(8,30);
            Invalidate();
        }

    }
}
