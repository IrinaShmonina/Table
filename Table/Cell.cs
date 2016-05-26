﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class Cell
    {
        public const int defaultWidth = 30;
        public const int defaultHeigth = 15;
        public int ColumnNumber;
        public int RowNumber;
        private int Height;
        private int Width;
        public string data;

        public Cell(int columnNumber, int rowNumber, int height = defaultHeigth, int width = defaultWidth)
        {
            this.ColumnNumber = columnNumber;
            this.RowNumber = rowNumber;
            this.Height = height;
            this.Width = width;
        }
        public void PushData(string x)
        {
            data = x;
        }
        //добавил
        public Cell SetNewCoords(int x, int y)
        {
            ColumnNumber = x;
            RowNumber = y;
            return this;
        }
        public Cell SetNewSize(int width, int heigth)
        {
            Width = width;
            Height = heigth;
            return this;
        }
        
    }
}
