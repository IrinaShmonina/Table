using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App;
using System.Threading;

namespace GUI
{

    class AppForm : Form
    {
        private Table table;
        private MenuStrip mainMenu;
        //private TableLayoutPanel tab;
        private Dictionary<Point, TextBox> textBoxes;
        private Dictionary<Point, Label> labels;
        //public Dictionary<Point, Point> CellsCoords;
        public Dictionary<int, int> RowsCoords;
        public Dictionary<int, int> ColumnsCoords;
        const int startXcoord = 40;
        const int startYcoord = 120;

        public AppForm(Table table)
        {
            //System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            this.table = table;
            this.Text = "Электронная Таблица";
            this.MinimumSize = new Size(800, 600);
            this.AutoSize = true;
            this.MaximizeBox = false;
            this.WindowState = FormWindowState.Maximized;
            this.SizeChanged += (sender, args) => { DrawTable(); };//DrawTable();
            this.Resize += (sender, args) => { DrawTable(); };
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

            /////////////////////////////



            textBoxes = new Dictionary<Point, TextBox>();
            labels = new Dictionary<Point, Label>();
            DrawTable();





            Paint += (sender, args) =>
                {


                };
        }
        void DrawTable()
        {
            //foreach (var e in dict.Values)
            //{
            //    Controls.Remove(e);
            //}
            //CellsCoords = table.GetShiftedCellsCoords(startXcoord, startYcoord);
            RowsCoords = table.GetShiftedRowsCoords(startYcoord);
            ColumnsCoords = table.GetShiftedColumnsCoords(startXcoord);
            for (int x = 1; x <= table.ColumnsAmount; x++)
                if (ColumnsCoords[x] < this.Right - 100)
                {
                    var i = x;
                    if (!labels.ContainsKey(new Point(i, 0)))
                    {
                        var label = new Label();
                        label.Text = i.ToString();
                        label.Location = new Point(ColumnsCoords[i], startYcoord - 20);
                        label.Size = new Size(table.ColumnsWidth[i], 20);
                        labels.Add(new Point(i, 0), label);
                        Controls.Add(label);
                    }
                    else
                    {
                        var label = labels[new Point(i, 0)];
                        label.Text = i.ToString();
                        label.Location = new Point(ColumnsCoords[i], startYcoord - 20);
                        label.Size = new Size(table.ColumnsWidth[i], 20);
                    }
                }

            for (int y = 1; y <= table.RowsAmount; y++)
                if (RowsCoords[y] < this.Bottom - 100)
                {
                    var j = y;
                    if (!labels.ContainsKey(new Point(0, j)))
                    {
                        var label = new Label();
                        label.Text = j.ToString();
                        label.Location = new Point(startXcoord - 20, RowsCoords[j]);
                        label.Size = new Size(20, table.RowsHeigth[j]);
                        labels.Add(new Point(0, j), label);
                        Controls.Add(label);
                    }
                    else
                    {
                        var label = labels[new Point(0, j)];
                        label.Text = j.ToString();
                        label.Location = new Point(startXcoord - 20, RowsCoords[j]);
                        label.Size = new Size(20, table.RowsHeigth[j]);
                    }
                }


            //Parallel.For(1, table.ColumnsAmount, x =>
            for (int x = 1; x <= table.ColumnsAmount; x++)
            {
                if (ColumnsCoords[x] < this.Right - 100)
                {
                    //var thread = new Thread();
                    for (int y = 1; y <= table.RowsAmount; y++)
                    {
                        if (RowsCoords[y] < this.Bottom - 100)
                        {
                            var i = x;
                            var j = y;

                            if (!textBoxes.ContainsKey(new Point(i, j)))
                            {
                                var textbox = new TextBox();
                                textbox.Location = new Point(ColumnsCoords[i], RowsCoords[j]);
                                textbox.Width = table.ColumnsWidth[i];
                                textbox.Height = table.RowsHeigth[j];
                                textbox.Text = table[i, j].data;
                                textbox.TextChanged += (s, a) =>
                                {
                                    table[i, j].PushData(textbox.Text);
                                };
                                textBoxes.Add(new Point(i, j), textbox);
                                Controls.Add(textbox);
                            }
                            else
                            {
                                var textbox = textBoxes[new Point(i, j)];
                                textbox.Location = new Point(ColumnsCoords[i], RowsCoords[j]);
                                textbox.Width = table.ColumnsWidth[i];
                                textbox.Height = table.RowsHeigth[j];
                                textbox.Text = table[i, j].data;
                                textbox.TextChanged += (s, a) =>
                                {
                                    table[i, j].PushData(textBoxes[new Point(i, j)].Text);
                                };
                            }

                        }
                    }


                }

            }//);
        }
        void InsertRow(object sender, EventArgs e)
        {
            var i = 5;
            table.InsertRow(i);
            DrawTable();
        }
        void InsertColumn(object sender, EventArgs e)
        {
            var i = 5;
            table.InsertColumn(i);
            DrawTable();
        }
        void RemoveRow(object sender, EventArgs e)
        {
            var i = 5;
            table.DeleteRow(i);
            DrawTable();
        }
        void RemoveColumn(object sender, EventArgs e)
        {
            var i = 5;
            table.DeleteColumn(i);
            DrawTable();
        }
        void ChangeRowHeigth(object sender, EventArgs e)
        {
            table.ChangeRowHeigth(8, 30);
            DrawTable();
        }
        void ChangeColumnWidth(object sender, EventArgs e)
        {
            table.ChangeColumnWidth(8, 30);
            DrawTable();
        }

    }
}
