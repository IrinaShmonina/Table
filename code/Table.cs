using System;
using System.Collections.Generic;
using System.Linq;

namespace model
{
    public class Table
    {
        public List<Cell> cells = new List<Cell>();
        public int countRows;
        public int countColumns;
        public String alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public Table(int countRows, int countColumns)
        {
            this.countRows = countRows;
            this.countColumns = countColumns;
            for (var i = 0; i < countRows; i++)
                for (var j = 0; j < countColumns; j++)
                    cells.Add(new Cell("", ""));
        }

        public void increaseColumnCount()
        {
            for (var i = 0; i < countRows; i++)
                cells.Insert(countColumns + i * countRows, new Cell("", ""));
            countColumns++;
        }

        public void decreaseColumnCount()
        {
            for (var i = 0; i < countRows; i++)
                cells.RemoveAt(countColumns - 1 + i * countColumns);
            countColumns--;
        }

        public void increaseRowsCount()
        {
            for (var i = 0; i < countColumns; i++)
                cells.Add(new Cell("", ""));
            countRows++;
        }

        public void decreaseRowsCount()
        {
            for (var i = 0; i < countColumns; i++)
                cells.Add(new Cell("", ""));
            countRows++;
        }

        public Cell this[char c, int x]
        {
            get
            { 
                return cells[x - 1 + alphabet.IndexOf(c) * countColumns];
            }
        }

        public void setCellText(char c, int x, String text)
        {
            this[c, x].setText(text);
        }

        public override string ToString()
        {
            var s = "";
            for (var x = 0; x < countColumns; x++)
            {
                foreach (var c in alphabet.Take(countRows))
                    s += this[c, x+1].ToString();
                s += "\n";
            }
            return s;
        }
    }
}