﻿using System;
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
        private TextBox focusedCell;
        private TextBox focusedCellCoords;
        private TextBox currentTextBox;
        private TextBox MaxXCoord;
        private TextBox MaxYCoord;
        
        private Dictionary<Point, TextBox> textBoxes;
        private Dictionary<Point, Label> labels;
        public Dictionary<int, int> RowsCoords;
        public Dictionary<int, int> ColumnsCoords;
        const int startXcoord = 30;
        const int startYcoord = 100;

        public AppForm(Table table)
        {
            //System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            this.table = table;
            this.Text = "Электронная Таблица";
            this.MinimumSize = new Size(800, 600);
            //this.AutoSize = true;
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
                    if (currentTextBox!=null)
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
            //foreach (var e in labels.Values)
            //{
            //    Controls.Remove(e);
            //}
            //CellsCoords = table.GetShiftedCellsCoords(startXcoord, startYcoord);
            RowsCoords = table.GetShiftedRowsCoords(startYcoord);
            ColumnsCoords = table.GetShiftedColumnsCoords(startXcoord);

            for (int x = 1; x <= table.TableWidth; x++)
                if (ColumnsCoords[x] < this.Right - 20)
                {
                    var i = x;

                    var contextMenu = new ContextMenuStrip();
                    ToolStripMenuItem insertColumn = new ToolStripMenuItem("Вставить столбец", null, (s, a) => { InsertColumn(i); });
                    ToolStripMenuItem removeColumn = new ToolStripMenuItem("Удалить столбец", null, (s, a) => { RemoveColumn(i); });
                    ToolStripMenuItem changeColumnWidth = new ToolStripMenuItem("Изменить ширину столбца", null, (s, a) => { ChangeColumnWidth(i); });
                    contextMenu.Items.AddRange(new[] { insertColumn, removeColumn, changeColumnWidth });

                    Label label;
                    if (!labels.ContainsKey(new Point(i, 0)))
                    {
                        label = new Label();
                        labels.Add(new Point(i, 0), label);
                        Controls.Add(label);
                    }
                    else
                        label = labels[new Point(i, 0)];

                    label.Location = new Point(ColumnsCoords[i], startYcoord - 20);
                    label.Size = new Size(table.ColumnsWidth[i], 20);
                    label.Text = i.ToString();
                    label.BorderStyle = BorderStyle.Fixed3D;
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.ContextMenuStrip = contextMenu;

                }
                else break;

            for (int y = 1; y <= table.TableHeight; y++)
                if (RowsCoords[y] < this.Bottom - 20)
                {
                    var j = y;

                    var contextMenu = new ContextMenuStrip();
                    ToolStripMenuItem insertRow = new ToolStripMenuItem("Вставить строку", null, (s, a) => { InsertRow(j); });
                    ToolStripMenuItem removeRow = new ToolStripMenuItem("Удалить строку", null, (s, a) => { RemoveRow(j); });
                    ToolStripMenuItem changeRowHeight = new ToolStripMenuItem("Изменить высоту строки", null, (s, a) => { ChangeRowHeight(j); });
                    contextMenu.Items.AddRange(new[] { insertRow, removeRow, changeRowHeight });

                    Label label;
                    if (!labels.ContainsKey(new Point(0, j)))
                    {
                        label = new Label();
                        labels.Add(new Point(0, j), label);
                        Controls.Add(label);
                    }
                    else
                        label = labels[new Point(0, j)];

                    label.Location = new Point(startXcoord - 30, RowsCoords[j]);
                    label.Size = new Size(30, table.RowsHeight[j]);
                    label.Text = j.ToString();
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.BorderStyle = BorderStyle.Fixed3D;
                    label.ContextMenuStrip = contextMenu;
                }
                else break;

            //foreach (var e in textBoxes.Values)
            //{
            //    Controls.Remove(e);
            //}
            //Parallel.For(1, table.TableWidth, x =>
            {
                for (int x = 1; x <= table.TableWidth; x++)
                {
                    if (ColumnsCoords[x] < this.Right - 20)
                    {
                        for (int y = 1; y <= table.TableHeight; y++)
                        {
                            if (RowsCoords[y] < this.Bottom - 20)
                            {
                                var i = x;
                                var j = y;
                                TextBox textbox;
                                if (!textBoxes.ContainsKey(new Point(i, j)))
                                {
                                    textbox = new TextBox();
                                    textBoxes.Add(new Point(i, j), textbox);
                                    Controls.Add(textbox);
                                }
                                else
                                    textbox = textBoxes[new Point(i, j)];

                                textbox.Location = new Point(ColumnsCoords[i], RowsCoords[j]);
                                textbox.Width = table.ColumnsWidth[i];
                                textbox.Height = table.RowsHeight[j];
                                textbox.Text = table[i, j].Data;
                                textbox.BorderStyle = BorderStyle.Fixed3D;
                                textbox.GotFocus += (s, a) =>
                                {
                                    focusedCellCoords.Text = new Point(i, j).ToString();
                                    currentTextBox = textbox;
                                    focusedCell.Text = textbox.Text;
                                };
                                textbox.TextChanged += (s, a) =>
                                {
                                    PushData(new Point(i, j), textbox.Text);
                                    currentTextBox = textbox;
                                    focusedCell.Text = textbox.Text;
                                };

                            }
                            else break;
                        }


                    }
                    else break;
                }
            }//);
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
            table.ChangeRowHeight(number, 30);
            ShowMaxCoords();
            DrawTable();
        }
        void ChangeColumnWidth(int number)
        {
            table.ChangeColumnWidth(number, 30);
            ShowMaxCoords();
            DrawTable();
        }

    }
}
