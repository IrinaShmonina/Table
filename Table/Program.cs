using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes("ABCDEFGHIJKLMNOPQRSTUVWXYZ");//(A-65)замутить перевод из цифер в буквы на уровне представления
            //for (int k = 0;k<asciiBytes.Length;k++)
            //{
            //    Console.WriteLine(asciiBytes[k]);
            //}


            Table table = new Table();
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 7; j++)
                    table[i, j] = new Cell(i, j);


            for ( int y = 0;y<8;y++)
            {
                for (int x = 0;x<8;x++)
                {
                    try
                    {
                        Console.Write(table[x, y].ColumnNumber + "-" +table[x, y].RowNumber  + " ");
                    }
                    catch(Exception ex)
                    {
                        Console.Write("--- ");
                        continue;
                    }
                }
                Console.WriteLine("");
            }

            table.RemoveColumn(2);
            table.RemoveRow(2);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    try
                    {
                        Console.Write(table[x, y].ColumnNumber + "-" + table[x, y].RowNumber + " ");
                    }
                    catch (Exception ex)
                    {
                        Console.Write("--- ");
                        continue;
                    }
                }
                Console.WriteLine("");
            }
            table.InsertColumn(1);
            table.InsertRow(1);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    try
                    {
                        Console.Write(table[x, y].ColumnNumber + "-" + table[x, y].RowNumber + " ");
                    }
                    catch (Exception ex)
                    {
                        Console.Write("--- ");
                        continue;
                    }
                }
                Console.WriteLine("");
            }

            table.RemoveColumn(2);
            table.RemoveRow(2);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    try
                    {
                        Console.Write(table[x, y].ColumnNumber + "-" + table[x, y].RowNumber + " ");
                    }
                    catch (Exception ex)
                    {
                        Console.Write("--- ");
                        continue;
                    }
                }
                Console.WriteLine("");
            }
            table.InsertColumn(3);
            table.InsertRow(3);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    try
                    {
                        Console.Write(table[x, y].ColumnNumber + "-" + table[x, y].RowNumber + " ");
                    }
                    catch (Exception ex)
                    {
                        Console.Write("--- ");
                        continue;
                    }
                }
                Console.WriteLine("");
            }

            table.RemoveColumn(4);
            table.RemoveRow(4);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    try
                    {
                        Console.Write(table[x, y].ColumnNumber + "-" + table[x, y].RowNumber + " ");
                    }
                    catch (Exception ex)
                    {
                        Console.Write("--- ");
                        continue;
                    }
                }
                Console.WriteLine("");
            }
            table.InsertColumn(5);
            table.InsertRow(5);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    try
                    {
                        Console.Write(table[x, y].ColumnNumber + "-" + table[x, y].RowNumber + " ");
                    }
                    catch (Exception ex)
                    {
                        Console.Write("--- ");
                        continue;
                    }
                }
                Console.WriteLine("");
            }
        }
    }
}
