using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Domain;

namespace GUI
{

    class AppForm : Form
    {
        private Size currentShift;
        private ITable table;
        private MenuStrip mainMenu;

        private TextBox focusedCellData;
        private TextBox focusedCellFormula;
        private TextBox focusedCellCoords;

        private VScrollBar vScroller;
        private HScrollBar hScroller;

        private Button serialize;
        private Button deserialize;

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
            this.table = table;
            this.Text = "Электронная Таблица";
            this.MinimumSize = new Size(800, 600);
            //this.AutoSize = true;
            this.WindowState = FormWindowState.Maximized;
            this.SizeChanged += (sender, args) =>
            {
                DrawTable();
            };

            DoubleBuffered = true;

            mainMenu = new MenuStrip();
            var menuItems = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("Файл",null,
                    new ToolStripMenuItem[]
                    {
                        new ToolStripMenuItem("Сохранить", null,(s,a) => SerializeData()),
                        new ToolStripMenuItem("Загрузить", null,(s,a) => DeserializeData()),                        
                    }
                    ),
                new ToolStripMenuItem("Редактирование",null,
                    new ToolStripMenuItem[]
                    {
                    }
                    ),
                new ToolStripMenuItem("О программе", null,(sender,args) => MessageBox.Show("Электронная таблица, версия 1.0\nРазработчики:\nЕрмаков Степан\nШмонина Ирина\nЛевшин Михаил","О программе"))

            };

            mainMenu.Items.AddRange(menuItems);
            Controls.Add(mainMenu);

            focusedCellCoords = new TextBox();
            focusedCellCoords.Location = new Point(LeftTopX_Pixel, LeftTopY_Pixel - 50);
            focusedCellCoords.Size = new Size(90, 20);
            focusedCellCoords.Enabled = false;
            focusedCellCoords.TextAlign = HorizontalAlignment.Center;
            Controls.Add(focusedCellCoords);

            focusedCellData = new TextBox();
            focusedCellData.Location = new Point(focusedCellCoords.Right + 10, focusedCellCoords.Top);
            focusedCellData.Size = new Size(200, 20);
            focusedCellData.Enabled = false;
            Controls.Add(focusedCellData);

            focusedCellFormula = new TextBox();
            focusedCellFormula.Location = new Point(focusedCellData.Right + 10, focusedCellCoords.Top);
            focusedCellFormula.Size = new Size(200, 20);
            focusedCellFormula.Enabled = false;
            Controls.Add(focusedCellFormula);


            vScroller = new VScrollBar();
            vScroller.Dock = DockStyle.Right;
            vScroller.Width = 20;
            vScroller.Height = 200;
            vScroller.Minimum = 0;
            vScroller.Maximum = table.RowsCount;
            vScroller.Scroll += (sender, args) =>
            {
                currentShift = new Size(currentShift.Width, vScroller.Value);
                if (vScroller.Value > vScroller.Maximum - 10)
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
                currentShift = new Size(hScroller.Value, currentShift.Height);
                if (hScroller.Value > hScroller.Maximum - 10)
                    table.Resize(0.5, 0);
                DrawTable();
            };
            Controls.Add(hScroller);

            serialize = new Button();
            serialize.Location = new Point(570, 30);
            serialize.Size = new Size(90, 40);
            serialize.Text = "Сохранить данные";
            serialize.Click += (sender, args) =>
            {
                SerializeData();
            };
            Controls.Add(serialize);

            deserialize = new Button();
            deserialize.Location = new Point(670, 30);
            deserialize.Size = new Size(90, 40);
            deserialize.Text = "Загрузить данные";
            deserialize.Click += (sender, args) =>
            {
                DeserializeData();
            };
            Controls.Add(deserialize);

            ColumnsCoords = GetShiftedColumnsCoords(currentShift.Width);
            RowsCoords = GetShiftedRowsCoords(currentShift.Height);
            SetMaxXMaxY();

            textBoxes = new Dictionary<Point, TextBox>();
            labels = new Dictionary<Point, Label>();

            #region
            for (int x = 1; x <= maxX; x++)
            {
                var i = x;
                var contextMenu = new ContextMenuStrip();
                ToolStripMenuItem addColumn = new ToolStripMenuItem("Добавить столбец", null, (s, a) => { AddColumn(currentShift.Width + i); });
                ToolStripMenuItem removeColumn = new ToolStripMenuItem("Удалить столбец", null, (s, a) => { RemoveColumn(currentShift.Width + i); });

                ToolStripMenuItem cutColumn = new ToolStripMenuItem("Вырезать столбец", null, (s, a) => { CutColumn(currentShift.Width + i); });
                ToolStripMenuItem copyColumn = new ToolStripMenuItem("Копировать столбец", null, (s, a) => { CopyColumn(currentShift.Width + i); });
                ToolStripMenuItem pastColumn = new ToolStripMenuItem("Вставить столбец", null, (s, a) => { PastColumn(currentShift.Width + i); });
                contextMenu.Items.AddRange(new[] { addColumn, removeColumn, cutColumn, copyColumn, pastColumn });

                var label = new Label();
                labels.Add(new Point(i, 0), label);
                Controls.Add(label);

                label.Location = new Point(ColumnsCoords[currentShift.Width + i], LeftTopY_Pixel - 20);
                label.Size = new Size(Cell.Width, 20);
                label.Text = GetLettersFromNumber(currentShift.Width + i);
                label.BorderStyle = BorderStyle.Fixed3D;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.ContextMenuStrip = contextMenu;
            }

            for (int y = 1; y <= maxY; y++)
            {
                var j = y;
                var contextMenu = new ContextMenuStrip();
                ToolStripMenuItem addRow = new ToolStripMenuItem("Добавить строку", null, (s, a) => { AddRow(currentShift.Height + j); });
                ToolStripMenuItem removeRow = new ToolStripMenuItem("Удалить строку", null, (s, a) => { RemoveRow(currentShift.Height + j); });

                ToolStripMenuItem cutRow = new ToolStripMenuItem("Вырезать строку", null, (s, a) => { CutRow(currentShift.Height + j); });
                ToolStripMenuItem copyRow = new ToolStripMenuItem("Копировать строку", null, (s, a) => { CopyRow(currentShift.Height + j); });
                ToolStripMenuItem pastRow = new ToolStripMenuItem("Вставить строку", null, (s, a) => { PastRow(currentShift.Height + j); });
                contextMenu.Items.AddRange(new[] { addRow, removeRow, cutRow, copyRow, pastRow });

                var label = new Label();
                labels.Add(new Point(0, j), label);
                Controls.Add(label);

                label.Location = new Point(LeftTopX_Pixel - 50, RowsCoords[currentShift.Height + j]);
                label.Size = new Size(50, Cell.Height);
                label.Text = (currentShift.Height + j).ToString();
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.BorderStyle = BorderStyle.Fixed3D;
                label.ContextMenuStrip = contextMenu;
            }

            for (int x = 1; x <= maxX; x++)
            {
                var i = x;
                for (int y = 1; y <= maxY; y++)
                {
                    var j = y;
                    TextBox textbox = new TextBox();
                    textbox.BorderStyle = BorderStyle.Fixed3D;

                    textbox.Location = new Point(ColumnsCoords[currentShift.Width + i], RowsCoords[currentShift.Height + j]);
                    textbox.Width = Cell.Width;
                    textbox.Height = Cell.Height;

                    textbox.GotFocus += (s, a) =>
                    {
                        focusedCellCoords.Text = GetLettersFromNumber(currentShift.Width + i) + "" + (currentShift.Height + j).ToString();

                        focusedCellData.Text = table[currentShift.Width + i, currentShift.Height + j].Data;
                        focusedCellFormula.Text = table[currentShift.Width + i, currentShift.Height + j].Formula;

                    };
                    textbox.KeyDown += (s, a) =>
                        {
                            if (a.KeyCode == Keys.Enter)
                            {
                                var text = textbox.Text;
                                if (!ExpressionCalculator.IsCorrect(text))
                                {
                                    PushData(new Point(currentShift.Width + i, currentShift.Height + j), text);
                                }
                                else
                                {
                                    text = text.Remove(0, 1);
                                    PushData(new Point(currentShift.Width + i, currentShift.Height + j), ExpressionCalculator.Count(text, table.GetTable()).ToString());
                                    SetFormula(new Point(currentShift.Width + i, currentShift.Height + j), text);
                                }
                                focusedCellData.Text = table[currentShift.Width + i, currentShift.Height + j].Data;
                                focusedCellFormula.Text = table[currentShift.Width + i, currentShift.Height + j].Formula;
                            }
                        };
                    textbox.TextChanged += (s, a) =>
                    {
                        var text = textbox.Text;
                        PushData(new Point(currentShift.Width + i, currentShift.Height + j), text);
                        focusedCellData.Text = table[currentShift.Width + i, currentShift.Height + j].Data;
                        focusedCellFormula.Text = table[currentShift.Width + i, currentShift.Height + j].Formula;
                    };
                    textBoxes.Add(new Point(i, j), textbox);
                    Controls.Add(textbox);
                }
            }
            #endregion//заполнение текстбокса


            DrawTable();
        }

        public Dictionary<int, int> GetShiftedRowsCoords(int yShiftInCells)
        {
            var result = new Dictionary<int, int>();
            var yCoord = LeftTopY_Pixel;
            for (int i = yShiftInCells + 1; i <= table.RowsCount; i++)
            {
                result.Add(i, yCoord);
                yCoord += Cell.Height;
            }
            return result;
        }

        public Dictionary<int, int> GetShiftedColumnsCoords(int xShiftInCells)
        {
            var result = new Dictionary<int, int>();
            var xCoord = LeftTopX_Pixel;
            for (int i = xShiftInCells + 1; i <= table.ColumnsCount; i++)
            {
                result.Add(i, xCoord);
                xCoord += Cell.Width;
            }
            return result;
        }

        public string GetLettersFromNumber(int number)
        {
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var str = "";
            while (number != 0)
            {
                number--;
                str = alphabet[number % alphabet.Length] + str;
                number /= alphabet.Length;
            }
            return str;
        }

        public int GetNumberFromLetters(string letters)
        {
            var number = 0;
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (var i = 0; i < letters.Length; i++)
            {
                var a = (int)Math.Pow(alphabet.Length, i);
                var b = letters[letters.Length - i - 1];
                var c = alphabet.IndexOf(b) + 1;
                number += a * c;
            }
            return number;
        }
        void SetMaxXMaxY()
        {
            maxX = 0;
            maxY = 0;
            for (int x = 1; x <= table.ColumnsCount; x++)
            {
                var i = x;
                if (ColumnsCoords[currentShift.Width + i] + Cell.Width < SystemInformation.PrimaryMonitorSize.Width - 50)//SystemInformation.PrimaryMonitorSize.Width
                    maxX++;
                else break;
            }

            for (int y = 1; y <= table.RowsCount; y++)
            {
                var j = y;
                if (RowsCoords[currentShift.Height + j] + Cell.Height < SystemInformation.PrimaryMonitorSize.Height - 50)//SystemInformation.PrimaryMonitorSize.Height
                    maxY++;
                else break;
            }
        }
        void DrawTable()
        {
            vScroller.Maximum = table.RowsCount - maxY;
            hScroller.Maximum = table.ColumnsCount - maxX;

            for (int x = 1; x <= maxX; x++)
            {
                var i = x;
                Label label = labels[new Point(i, 0)];
                label.Text = GetLettersFromNumber(currentShift.Width + i);//
            }

            for (int y = 1; y <= maxY; y++)
            {
                var j = y;
                Label label = labels[new Point(0, j)];
                label.Text = (currentShift.Height + j).ToString();
            }

            for (int x = 1; x <= maxX; x++)
            {
                var i = x;
                for (int y = 1; y <= maxY; y++)
                {
                    var j = y;
                    TextBox textbox = textBoxes[new Point(i, j)];

                    if (table[currentShift.Width + i, currentShift.Height + j].Formula == "")
                    {
                        textbox.Text = table[currentShift.Width + i, currentShift.Height + j].Data;
                    }
                    else
                    {
                        textbox.Text = ExpressionCalculator.Count(table[currentShift.Width + i, currentShift.Height + j].Formula, table.GetTable()).ToString();
                    }
                }
            }
        }
        void SerializeData()
        {
            table.Serialize();
            DrawTable();
        }
        void DeserializeData()
        {
            try
            {
                table.Deserialize();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Нечего загружать!");
            }
            DrawTable();
        }
        void SetFormula(Point point, string formula)
        {
            table.SetFormula(point, formula);
            DrawTable();
        }
        void PushData(Point point, string text)
        {
            table.PushData(point, text);
            if (table[point].ChangesAnotherCell)
                DrawTable();
        }
        void AddRow(int number)
        {
            table.AddRow(number);
            DrawTable();
        }
        void AddColumn(int number)
        {
            table.AddColumn(number);
            DrawTable();
        }
        void RemoveRow(int number)
        {
            table.RemoveRow(number);
            DrawTable();
        }
        void RemoveColumn(int number)
        {
            table.RemoveColumn(number);
            DrawTable();
        }

        void CutRow(int number)
        {
            table.CutRow(number);
            DrawTable();
        }
        void CutColumn(int number)
        {
            table.CutColumn(number);
            DrawTable();
        }

        void CopyRow(int number)
        {
            table.CopyRow(number);
            DrawTable();
        }
        void CopyColumn(int number)
        {
            table.CopyColumn(number);
            DrawTable();
        }

        void PastRow(int number)
        {
            table.PastRow(number);
            DrawTable();
        }
        void PastColumn(int number)
        {
            table.PastColumn(number);
            DrawTable();
        }

    }
}
