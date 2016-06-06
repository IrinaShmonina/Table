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
            vScroller.Maximum = table.TableHeight;
            vScroller.Scroll += (sender, args) =>
                {
                    currentShift.Height = vScroller.Value;
                    if (vScroller.Value > 0.9 * vScroller.Maximum)
                        table.Resize(0, 1);
                    //MaxYCoord.Text = currentShift.Height.ToString();
                    //MaxXCoord.Text = vScroller.Maximum.ToString();
                    DrawTable();

                };
            Controls.Add(vScroller);

            hScroller = new HScrollBar();
            hScroller.Dock = DockStyle.Bottom;
            hScroller.Width = 200;
            hScroller.Height = 20;
            hScroller.Minimum = 0;
            hScroller.Maximum = table.TableWidth;
            hScroller.Scroll += (sender, args) =>
                {
                    currentShift.Width = hScroller.Value;
                    if (hScroller.Value > 0.9 * hScroller.Maximum)
                        table.Resize(1,0);
                    //MaxXCoord.Text = currentShift.Width.ToString();
                    //MaxYCoord.Text = hScroller.Maximum.ToString();
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
            for (int x = 1; x <= table.TableWidth; x++)
            {
                var i = x;
                if (ColumnsCoords[currentShift.Width + i] + table.GetColumnWidth(currentShift.Width + i) < this.Right - 50)
                {
                    maxX++;
                    var contextMenu = new ContextMenuStrip();
                    ToolStripMenuItem insertColumn = new ToolStripMenuItem("Вставить столбец", null, (s, a) => { InsertColumn(currentShift.Width + i); });
                    ToolStripMenuItem removeColumn = new ToolStripMenuItem("Удалить столбец", null, (s, a) => { RemoveColumn(currentShift.Width + i); });
                    ToolStripMenuItem changeColumnWidth = new ToolStripMenuItem("Изменить ширину столбца", null, (s, a) => { ChangeColumnWidth(currentShift.Width + i); });
                    contextMenu.Items.AddRange(new[] { insertColumn, removeColumn, changeColumnWidth });

                    Label label;
                    if (!labels.ContainsKey(new Point(i, 0)))
                    {
                        label = new Label();
                        labels.Add(new Point(i, 0), label);
                        Controls.Add(label);
                    }

                    label = labels[new Point(i, 0)];

                    label.Location = new Point(ColumnsCoords[currentShift.Width + i], LeftTopY_Pixel - 20);
                    label.Size = new Size(table.GetColumnWidth(currentShift.Width + i), 20);
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


            for (int y = 1; y <= table.TableHeight; y++)
            {
                var j = y;
                if (RowsCoords[currentShift.Height + j] + table.GetRowHeight(currentShift.Height + j) < this.Bottom - 50)
                {
                    maxY++;
                    var contextMenu = new ContextMenuStrip();
                    ToolStripMenuItem insertRow = new ToolStripMenuItem("Вставить строку", null, (s, a) => { InsertRow(currentShift.Height + j); });
                    ToolStripMenuItem removeRow = new ToolStripMenuItem("Удалить строку", null, (s, a) => { RemoveRow(currentShift.Height + j); });
                    ToolStripMenuItem changeRowHeight = new ToolStripMenuItem("Изменить высоту строки", null, (s, a) => { ChangeRowHeight(currentShift.Height + j); });
                    contextMenu.Items.AddRange(new[] { insertRow, removeRow, changeRowHeight });

                    Label label;
                    if (!labels.ContainsKey(new Point(0, j)))
                    {
                        label = new Label();
                        labels.Add(new Point(0, j), label);
                        Controls.Add(label);
                    }

                    label = labels[new Point(0, j)];

                    label.Location = new Point(LeftTopX_Pixel - 50, RowsCoords[currentShift.Height + j]);
                    label.Size = new Size(50, table.GetRowHeight(currentShift.Height + j));
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

            vScroller.Maximum = table.TableHeight - maxY;
            hScroller.Maximum = table.TableWidth - maxX;

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
                    textbox.Width = table.GetColumnWidth(currentShift.Width + i);
                    textbox.Height = table.GetRowHeight(currentShift.Height + j);
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
            for (int x = maxX + 1; x <= table.TableWidth; x++)
            {
                var i = x;
                if (labels.ContainsKey(new Point(i, 0)))
                    for (int y = maxY + 1; y <= table.TableHeight; y++)
                    {
                        var j = y;
                        if (textBoxes.ContainsKey(new Point(i, j)))
                            textBoxes.Remove(new Point(i, j));
                        else break;
                    }
                else break;
            }
        }

        void PushData(Point point, string text)
        {
            var needReDraw = false;
            if (point.X == table.TableWidth || point.Y == table.TableHeight) needReDraw = true;
            table.PushData(point, text);
            ShowMaxCoords();
            if (needReDraw) DrawTable();
        }
        void InsertRow(int number)
        {
            table.InsertRow(number);
            ShowMaxCoords();
            DrawTable();
        }
        void InsertColumn(int number)
        {
            table.InsertColumn(number);
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
        void ChangeRowHeight(int number)
        {
            table.ChangeRowHeight(number, 40);
            ShowMaxCoords();
            DrawTable();
        }
        void ChangeColumnWidth(int number)
        {
            table.ChangeColumnWidth(number, 60);
            ShowMaxCoords();
            DrawTable();
        }

    }
    //void DrawTable(Size shift)
    //    {
    //        //foreach (var e in labels.Values)
    //        //{
    //        //    Controls.Remove(e);
    //        //}
    //        //CellsCoords = table.GetShiftedCellsCoords(startXcoord, startYcoord);
    //        RowsCoords = table.GetShiftedRowsCoords(startYcoord);
    //        ColumnsCoords = table.GetShiftedColumnsCoords(startXcoord);
    //        var maxX = 0;
    //        var maxY = 0;
    //        for (int x = 1; x <= table.TableWidth; x++)
    //        {
    //            var i = x;
    //            if (ColumnsCoords[i] + table.ColumnsWidth[i] < this.Right - 50)
    //            {
    //                maxX++;
    //                var contextMenu = new ContextMenuStrip();
    //                ToolStripMenuItem insertColumn = new ToolStripMenuItem("Вставить столбец", null, (s, a) => { InsertColumn(i); });
    //                ToolStripMenuItem removeColumn = new ToolStripMenuItem("Удалить столбец", null, (s, a) => { RemoveColumn(i); });
    //                ToolStripMenuItem changeColumnWidth = new ToolStripMenuItem("Изменить ширину столбца", null, (s, a) => { ChangeColumnWidth(i); });
    //                contextMenu.Items.AddRange(new[] { insertColumn, removeColumn, changeColumnWidth });

    //                Label label;
    //                if (!labels.ContainsKey(new Point(i, 0)))
    //                {
    //                    label = new Label();
    //                    labels.Add(new Point(i, 0), label);
    //                    Controls.Add(label);
    //                }

    //                label = labels[new Point(i, 0)];

    //                label.Location = new Point(ColumnsCoords[i], startYcoord - 20);
    //                label.Size = new Size(table.ColumnsWidth[i], 20);
    //                label.Text = i.ToString();
    //                label.BorderStyle = BorderStyle.Fixed3D;
    //                label.TextAlign = ContentAlignment.MiddleCenter;
    //                label.ContextMenuStrip = contextMenu;
    //            }
    //            else
    //            {
    //                if (labels.ContainsKey(new Point(i, 0)))
    //                    labels.Remove(new Point(i, 0));
    //                else
    //                    break;
    //            }
    //        }


    //        for (int y = 1; y <= table.TableHeight; y++)
    //        {
    //            var j = y;
    //            if (RowsCoords[j] + table.RowsHeight[j] < this.Bottom - 50)
    //            {
    //                maxY++;
    //                var contextMenu = new ContextMenuStrip();
    //                ToolStripMenuItem insertRow = new ToolStripMenuItem("Вставить строку", null, (s, a) => { InsertRow(j); });
    //                ToolStripMenuItem removeRow = new ToolStripMenuItem("Удалить строку", null, (s, a) => { RemoveRow(j); });
    //                ToolStripMenuItem changeRowHeight = new ToolStripMenuItem("Изменить высоту строки", null, (s, a) => { ChangeRowHeight(j); });
    //                contextMenu.Items.AddRange(new[] { insertRow, removeRow, changeRowHeight });

    //                Label label;
    //                if (!labels.ContainsKey(new Point(0, j)))
    //                {
    //                    label = new Label();
    //                    labels.Add(new Point(0, j), label);
    //                    Controls.Add(label);
    //                }

    //                label = labels[new Point(0, j)];

    //                label.Location = new Point(startXcoord - 30, RowsCoords[j]);
    //                label.Size = new Size(30, table.RowsHeight[j]);
    //                label.Text = j.ToString();
    //                label.TextAlign = ContentAlignment.MiddleCenter;
    //                label.BorderStyle = BorderStyle.Fixed3D;
    //                label.ContextMenuStrip = contextMenu;
    //            }
    //            else
    //            {
    //                if (labels.ContainsKey(new Point(0, j)))
    //                    labels.Remove(new Point(0, j));
    //                else break;
    //            }
    //        }
    //        for (int x = 1; x <= maxX; x++)//table.TableWidth
    //        {
    //            var i = x;
    //            for (int y = 1; y <= maxY; y++)//table.TableHeight
    //            {
    //                var j = y;
    //                //if (RowsCoords[j] + table.RowsHeight[j] < this.Bottom - 30 && ColumnsCoords[i] + table.ColumnsWidth[i] < this.Right - 30)
    //                {
    //                    TextBox textbox;
    //                    if (!textBoxes.ContainsKey(new Point(i, j)))
    //                    {
    //                        textbox = new TextBox();
    //                        textBoxes.Add(new Point(i, j), textbox);
    //                        textbox.BorderStyle = BorderStyle.Fixed3D;
    //                        Controls.Add(textbox);
    //                    }

    //                    textbox = textBoxes[new Point(i, j)];
    //                    textbox.Location = new Point(ColumnsCoords[i], RowsCoords[j]);
    //                    textbox.Width = table.ColumnsWidth[i];
    //                    textbox.Height = table.RowsHeight[j];
    //                    textbox.Text = table[i, j].Data;

    //                    textbox.GotFocus += (s, a) =>
    //                    {
    //                        focusedCellCoords.Text = new Point(i, j).ToString();
    //                        currentTextBox = textbox;
    //                        focusedCell.Text = textbox.Text;
    //                    };
    //                    textbox.TextChanged += (s, a) =>
    //                    {
    //                        PushData(new Point(i, j), textbox.Text);
    //                        currentTextBox = textbox;
    //                        focusedCell.Text = textbox.Text;
    //                    };
    //                }
    //                //else
    //                //{
    //                //    if (textBoxes.ContainsKey(new Point(i, j)))
    //                //        textBoxes.Remove(new Point(i, j));
    //                //    else 
    //                //    break;
    //                //}
    //            }
    //        }
    //        for (int x = maxX + 1; x <= table.TableWidth; x++)
    //        {
    //            var i = x;
    //            if (labels.ContainsKey(new Point(i, 0)))
    //                for (int y = maxY + 1; y <= table.TableHeight; y++)
    //                {
    //                    var j = y;
    //                    if (textBoxes.ContainsKey(new Point(i, j)))
    //                        textBoxes.Remove(new Point(i, j));
    //                    else break;
    //                }
    //            else break;
    //        }
    //    }
}
