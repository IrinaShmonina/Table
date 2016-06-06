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
        private Size currentShift;
        private ITable table;
        private MenuStrip mainMenu;
        private TextBox focusedCell;
        private TextBox focusedCellCoords;
        private TextBox currentTextBox;
        private TextBox MaxXCoord;
        private TextBox MaxYCoord;
        private VScrollBar vScroller;
        private HScrollBar hScroller;

        private Dictionary<Point, TextBox> textBoxes;
        private Dictionary<Point, Label> labels;
        public Dictionary<int, int> RowsCoords;
        public Dictionary<int, int> ColumnsCoords;

        const int LeftTopX_Pixel = 50;
        const int LeftTopY_Pixel = 100;

        private int maxX;
        private int maxY;

        public AppForm(ITable table)
        {
            currentShift = new Size(0, 0);
            //System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            this.table = table;
            this.Text = "Электронная Таблица";
            this.MinimumSize = new Size(800, 600);
            //this.AutoSize = true;
            this.WindowState = FormWindowState.Maximized;
            //this.SizeChanged += (sender, args) => { DrawTable(); };//DrawTable();
            this.Resize += (sender, args) => { DrawTable(); };
            //this.BorderStyle = BorderStyle.FixedSingle;
            DoubleBuffered = true;

            mainMenu = new MenuStrip();
            var menuItems = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("Файл"),
                new ToolStripMenuItem("Редактирование",null,
                    new ToolStripMenuItem[]
                    {
                        //new ToolStripMenuItem("Вставить строку", null,InsertRow),
                        //new ToolStripMenuItem("Вставить столбец", null,InsertColumn),
                        //new ToolStripMenuItem("Удалить строку", null,RemoveRow),
                        //new ToolStripMenuItem("Удалить столбец", null,RemoveColumn),
                        //new ToolStripMenuItem("Изменить ширину столбца", null,ChangeColumnWidth),
                        //new ToolStripMenuItem("Изменить высоту строки", null,ChangeRowHeigth)
                    }
                    ),
                new ToolStripMenuItem("Формулы"),
                new ToolStripMenuItem("О программе", null,(sender,args) => MessageBox.Show("Электронная таблица, версия 1.0\nРазработчики:\nЕрмаков Степан\nШмонина Ирина\nЛевшин Михаил","О программе"))

            };

            mainMenu.Items.AddRange(menuItems);
            Controls.Add(mainMenu);

            focusedCell = new TextBox();
            focusedCell.Location = new Point(150, 50);
            focusedCell.Width = 400;
            focusedCell.Height = 20;
            focusedCell.TextChanged += (sender, args) =>
                {
                    if (currentTextBox != null)
                        currentTextBox.Text = focusedCell.Text;
                };
            Controls.Add(focusedCell);

            focusedCellCoords = new TextBox();
            focusedCellCoords.Location = new Point(30, 50);
            focusedCellCoords.Width = 110;
            focusedCellCoords.Height = 20;
            focusedCellCoords.Enabled = false;
            Controls.Add(focusedCellCoords);

            MaxXCoord = new TextBox();
            MaxXCoord.Location = new Point(600, 40);
            MaxXCoord.Size = new Size(30, 30);
            MaxXCoord.Text = table.MaxChangedColumn.ToString();
            Controls.Add(MaxXCoord);

            MaxYCoord = new TextBox();
            MaxYCoord.Location = new Point(630, 40);
            MaxYCoord.Size = new Size(30, 30);
            MaxYCoord.Text = table.MaxChangedRow.ToString();
            Controls.Add(MaxYCoord);

            vScroller = new VScrollBar();
            vScroller.Dock = DockStyle.Right;
            vScroller.Width = 20;
            vScroller.Height = 200;
            vScroller.Minimum = 0;
            vScroller.Maximum = table.RowsCount;
            vScroller.Scroll += (sender, args) =>
                {
                    currentShift.Height = vScroller.Value;
                    if (vScroller.Value > vScroller.Maximum-10)
                        table.Resize(0, 0.5);
                    DrawTable();

                };
            Controls.Add(vScroller);

            hScroller = new HScrollBar();
            hScroller.Dock = DockStyle.Bottom;
            hScroller.Width = 200;
            hScroller.Height = 20;
            hScroller.Minimum = 0;
            hScroller.Maximum = table.ColumnsCount;
            hScroller.Scroll += (sender, args) =>
                {
                    currentShift.Width = hScroller.Value;
                    if (hScroller.Value > hScroller.Maximum-10)
                        table.Resize(0.5, 0);
                    DrawTable();
                };
            Controls.Add(hScroller);





            textBoxes = new Dictionary<Point, TextBox>();
            labels = new Dictionary<Point, Label>();

            DrawTable();





            Paint += (sender, args) =>
                {


                };
        }
        void ShowMaxCoords()
        {
            MaxXCoord.Text = table.MaxChangedColumn.ToString();
            MaxYCoord.Text = table.MaxChangedRow.ToString();
        }
        void DrawTable()
        {
            ColumnsCoords = table.GetShiftedColumnsCoords(LeftTopX_Pixel, currentShift.Width);
            RowsCoords = table.GetShiftedRowsCoords(LeftTopY_Pixel, currentShift.Height);

            maxX = 0;//shift.Width;
            maxY = 0;// shift.Height;
            for (int x = 1; x <= table.ColumnsCount; x++)
            {
                var i = x;
                if (ColumnsCoords[currentShift.Width + i] + Cell.Width < this.Right - 50)
                {
                    maxX++;
                    var contextMenu = new ContextMenuStrip();
                    ToolStripMenuItem addColumn = new ToolStripMenuItem("Добавить столбец", null, (s, a) => { AddColumn(currentShift.Width + i); });
                    ToolStripMenuItem removeColumn = new ToolStripMenuItem("Удалить столбец", null, (s, a) => { RemoveColumn(currentShift.Width + i); });

                    ToolStripMenuItem cutColumn = new ToolStripMenuItem("Вырезать столбец", null, (s, a) => { CutColumn(currentShift.Width + i); });
                    ToolStripMenuItem copyColumn = new ToolStripMenuItem("Копировать столбец", null, (s, a) => { CopyColumn(currentShift.Width + i); });
                    ToolStripMenuItem pastColumn = new ToolStripMenuItem("Вставить столбец", null, (s, a) => { PastColumn(currentShift.Width + i); });
                    contextMenu.Items.AddRange(new[] { addColumn, removeColumn, cutColumn, copyColumn, pastColumn});

                    Label label;
                    if (!labels.ContainsKey(new Point(i, 0)))
                    {
                        label = new Label();
                        labels.Add(new Point(i, 0), label);
                        Controls.Add(label);
                    }

                    label = labels[new Point(i, 0)];

                    label.Location = new Point(ColumnsCoords[currentShift.Width + i], LeftTopY_Pixel - 20);
                    label.Size = new Size(Cell.Width, 20);
                    label.Text = (currentShift.Width + i).ToString();
                    label.BorderStyle = BorderStyle.Fixed3D;
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.ContextMenuStrip = contextMenu;
                }
                else
                {
                    if (labels.ContainsKey(new Point(i, 0)))
                        labels.Remove(new Point(i, 0));
                    else
                        break;
                }
            }


            for (int y = 1; y <= table.RowsCount; y++)
            {
                var j = y;
                if (RowsCoords[currentShift.Height + j] + Cell.Height < this.Bottom - 50)
                {
                    maxY++;
                    var contextMenu = new ContextMenuStrip();
                    ToolStripMenuItem addRow = new ToolStripMenuItem("Добавить строку", null, (s, a) => { AddRow(currentShift.Height + j); });
                    ToolStripMenuItem removeRow = new ToolStripMenuItem("Удалить строку", null, (s, a) => { RemoveRow(currentShift.Height + j); });

                    ToolStripMenuItem cutRow = new ToolStripMenuItem("Вырезать строку", null, (s, a) => { CutRow(currentShift.Height + j); });
                    ToolStripMenuItem copyRow = new ToolStripMenuItem("Копировать строку", null, (s, a) => { CopyRow(currentShift.Height + j); });
                    ToolStripMenuItem pastRow = new ToolStripMenuItem("Вставить строку", null, (s, a) => { PastRow(currentShift.Height + j); });
                    contextMenu.Items.AddRange(new[] { addRow, removeRow, cutRow, copyRow, pastRow });

                    Label label;
                    if (!labels.ContainsKey(new Point(0, j)))
                    {
                        label = new Label();
                        labels.Add(new Point(0, j), label);
                        Controls.Add(label);
                    }

                    label = labels[new Point(0, j)];

                    label.Location = new Point(LeftTopX_Pixel - 50, RowsCoords[currentShift.Height + j]);
                    label.Size = new Size(50, Cell.Height);
                    label.Text = (currentShift.Height + j).ToString();
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.BorderStyle = BorderStyle.Fixed3D;
                    label.ContextMenuStrip = contextMenu;
                }
                else
                {
                    if (labels.ContainsKey(new Point(0, j)))
                        labels.Remove(new Point(0, j));
                    else break;
                }
            }

            vScroller.Maximum = table.RowsCount - maxY;
            hScroller.Maximum = table.ColumnsCount - maxX;

            for (int x = 1; x <= maxX; x++)//table.TableWidth
            {
                var i = x;
                for (int y = 1; y <= maxY; y++)//table.TableHeight
                {
                    var j = y;
                    //if (RowsCoords[j] + table.RowsHeight[j] < this.Bottom - 30 && ColumnsCoords[i] + table.ColumnsWidth[i] < this.Right - 30)
                    TextBox textbox;
                    if (!textBoxes.ContainsKey(new Point(i, j)))
                    {
                        textbox = new TextBox();
                        textBoxes.Add(new Point(i, j), textbox);
                        textbox.BorderStyle = BorderStyle.Fixed3D;
                        Controls.Add(textbox);
                    }

                    textbox = textBoxes[new Point(i, j)];
                    textbox.Location = new Point(ColumnsCoords[currentShift.Width + i], RowsCoords[currentShift.Height + j]);
                    textbox.Width = Cell.Width;
                    textbox.Height = Cell.Height;
                    ////
                    textbox.Text = table[currentShift.Width + i, currentShift.Height + j].Data;

                    textbox.GotFocus += (s, a) =>
                    {
                        focusedCellCoords.Text = new Point(currentShift.Width + i, currentShift.Height + j).ToString();
                        currentTextBox = textbox;
                        focusedCell.Text = textbox.Text;
                    };
                    textbox.TextChanged += (s, a) =>
                    {
                        PushData(new Point(currentShift.Width + i, currentShift.Height + j), textbox.Text);
                        currentTextBox = textbox;
                        focusedCell.Text = textbox.Text;
                    };
                }
            }
            //for (int x = maxX + 1; x <= table.TableWidth; x++)
            //{
            //    var i = x;
            //    if (labels.ContainsKey(new Point(i, 0)))
            //        for (int y = maxY + 1; y <= table.TableHeight; y++)
            //        {
            //            var j = y;
            //            if (textBoxes.ContainsKey(new Point(i, j)))
            //                textBoxes.Remove(new Point(i, j));
            //            else break;
            //        }
            //    else break;
            //}
        }

        void PushData(Point point, string text)
        {
            var needReDraw = false;
            if (point.X == table.ColumnsCount || point.Y == table.RowsCount) needReDraw = true;
            table.PushData(point, text);
            ShowMaxCoords();
            if (needReDraw) DrawTable();
        }
        void AddRow(int number)
        {
            table.AddRow(number);
            ShowMaxCoords();
            DrawTable();
        }
        void AddColumn(int number)
        {
            table.AddColumn(number);
            ShowMaxCoords();
            DrawTable();
        }
        void RemoveRow(int number)
        {
            table.RemoveRow(number);
            ShowMaxCoords();
            DrawTable();
        }
        void RemoveColumn(int number)
        {
            table.RemoveColumn(number);
            ShowMaxCoords();
            DrawTable();
        }

        void CutRow(int number)
        {
            table.CutRow(number);
            ShowMaxCoords();
            DrawTable();
        }
        void CutColumn(int number)
        {
            table.CutColumn(number);
            ShowMaxCoords();
            DrawTable();
        }

        void CopyRow(int number)
        {
            table.CopyRow(number);
            ShowMaxCoords();
            DrawTable();
        }
        void CopyColumn(int number)
        {
            table.CopyColumn(number);
            ShowMaxCoords();
            DrawTable();
        }

        void PastRow(int number)
        {
            table.PastRow(number);
            ShowMaxCoords();
            DrawTable();
        }
        void PastColumn(int number)
        {
            table.PastColumn(number);
            ShowMaxCoords();
            DrawTable();
        }

    }    
}
