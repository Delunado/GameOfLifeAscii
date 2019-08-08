using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeAscii
{
    class Program
    {
        struct Point
        {
            int x, y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public int X() { return x; }
            public int Y() { return y; }

            void setPoint(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        const int WIDTH = 16, HEIGHT = 16;
        const char LIVE_CELL_CHAR = '#', DEAD_CELL_CHAR = '*';

        static readonly Random random = new Random();

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            List<Point> liveCellsPoints = new List<Point>();

            char[,] table = CreateTabletop(HEIGHT, WIDTH); //TENER EN CUENTA QUE PRIMERO VA LA Y Y LUEGO LA X

            Console.Read();

            for (int i = -5; i < 5; i++)
            {
                Point p = new Point(WIDTH / 2 + i, HEIGHT / 2);
                AliveCell(table, p);
            }

            //Principal loop
            while (true)
            {
                ShowTabletop(table);
                System.Threading.Thread.Sleep(100);
                Console.Clear();
                table = GameOfLife(table);
            }
        }

        static char[,] CreateTabletop(int height, int width)
        {
            char[,] table = new char[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    table[y, x] = DEAD_CELL_CHAR;
                }
            }

            return table;
        }

        static void ShowTabletop(char[,] table)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                Console.WriteLine();
                for (int x = 0; x < WIDTH; x++)
                {
                    if (table[y, x].Equals(LIVE_CELL_CHAR))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    } else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }

                    Console.Write(table[y, x]);
                    Console.Write(" ");
                }
            }
        }

        static char[,] GameOfLife(char[,] table)
        {
            char[,] auxTable = CopyTable(table);

            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    Point point = new Point(x, y);
                    if (GetCellState(table, point) == DEAD_CELL_CHAR)
                    {
                        CheckBirth(table, auxTable, point);
                    } else
                    {
                        CheckDeath(table, auxTable, point);
                    }
                }
            }

            return auxTable;
        }

        static void CheckBirth(char[,] table, char[,] auxTable, Point p)
        {
            int neighbours = 0;

            Point pointIzq = new Point(p.X() - 1, p.Y());
            if (ExistPoint(table, pointIzq))
            {
                if (GetCellState(table, pointIzq) == LIVE_CELL_CHAR)
                    neighbours++;
            }

            Point pointDer = new Point(p.X() + 1, p.Y());
            if (ExistPoint(table, pointDer))
            {
                if (GetCellState(table, pointDer) == LIVE_CELL_CHAR)
                    neighbours++;
            }

            Point pointAbj = new Point(p.X(), p.Y() + 1);
            if (ExistPoint(table, pointAbj))
            {
                if (GetCellState(table, pointAbj) == LIVE_CELL_CHAR)
                    neighbours++;
            }

            Point pointArr = new Point(p.X(), p.Y() - 1);
            if (ExistPoint(table, pointArr))
            {
                if (GetCellState(table, pointArr) == LIVE_CELL_CHAR)
                    neighbours++;
            }

            Point pointIzqArr = new Point(p.X() - 1, p.Y() - 1);
            if (ExistPoint(table, pointIzqArr))
            {
                if (GetCellState(table, pointIzqArr) == LIVE_CELL_CHAR)
                    neighbours++;
            }

            Point pointIzqAbj = new Point(p.X() - 1, p.Y() + 1);
            if (ExistPoint(table, pointIzqAbj))
            {
                if (GetCellState(table, pointIzqAbj) == LIVE_CELL_CHAR)
                    neighbours++;
            }

            Point pointDerArr = new Point(p.X() + 1, p.Y() - 1);
            if (ExistPoint(table, pointDerArr))
            {
                if (GetCellState(table, pointDerArr) == LIVE_CELL_CHAR)
                    neighbours++;
            }

            Point pointDerAbj = new Point(p.X() + 1, p.Y() + 1);
            if (ExistPoint(table, pointDerAbj))
            {
                if (GetCellState(table, pointDerAbj) == LIVE_CELL_CHAR)
                    neighbours++;
            }

            if (neighbours == 3)
            {
                AliveCell(auxTable, p);
            }
        }

        static void CheckDeath(char[,] table, char[,] auxTable, Point p)
        {
            int neighbours = 0;

            Point pointIzq = new Point(p.X() - 1, p.Y());
            if (ExistPoint(table, pointIzq))
            {
                if (GetCellState(table, pointIzq) == LIVE_CELL_CHAR)
                    neighbours++;
            }

            Point pointDer = new Point(p.X() + 1, p.Y());
            if (ExistPoint(table, pointDer))
            {
                if (GetCellState(table, pointDer) == LIVE_CELL_CHAR)
                    neighbours++;
            }

            Point pointAbj = new Point(p.X(), p.Y() + 1);
            if (ExistPoint(table, pointAbj))
            {
                if (GetCellState(table, pointAbj) == LIVE_CELL_CHAR)
                    neighbours++;
            }

            Point pointArr = new Point(p.X(), p.Y() - 1);
            if (ExistPoint(table, pointArr))
            {
                if (GetCellState(table, pointArr) == LIVE_CELL_CHAR)
                    neighbours++;
            }

            Point pointIzqArr = new Point(p.X() - 1, p.Y() - 1);
            if (ExistPoint(table, pointIzqArr))
            {
                if (GetCellState(table, pointIzqArr) == LIVE_CELL_CHAR)
                    neighbours++;
            }

            Point pointIzqAbj = new Point(p.X() - 1, p.Y() + 1);
            if (ExistPoint(table, pointIzqAbj))
            {
                if (GetCellState(table, pointIzqAbj) == LIVE_CELL_CHAR)
                    neighbours++;
            }

            Point pointDerArr = new Point(p.X() + 1, p.Y() - 1);
            if (ExistPoint(table, pointDerArr))
            {
                if (GetCellState(table, pointDerArr) == LIVE_CELL_CHAR)
                    neighbours++;
            }

            Point pointDerAbj = new Point(p.X() + 1, p.Y() + 1);
            if (ExistPoint(table, pointDerAbj))
            {
                if (GetCellState(table, pointDerAbj) == LIVE_CELL_CHAR)
                    neighbours++;
            }

            if (neighbours != 2 && neighbours != 3)
            {
                KillCell(auxTable, p);
            }
        }

        static int NeighboursNumber(char[,] table, Point p)
        {
            int neighbours = 0;

            for (int y = 1; y > -2; y--)
            {
                for (int x = - 1; x < 2; x++)
                {
                    if (x != 0 && y != 0)
                    {
                        Point point = new Point(p.X() - x, p.Y() - y);
                        if (ExistPoint(table, point))
                        {
                            if (GetCellState(table, point) == LIVE_CELL_CHAR)
                                neighbours++;
                        }
                    }
                }
            }

            return neighbours;
        }

        static char[,] CopyTable(char[,] table)
        {
            char[,] auxTable = new char[HEIGHT, WIDTH];

            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    Point p = new Point(x, y);
                    if (GetCellState(table, p) == LIVE_CELL_CHAR)
                    {
                        AliveCell(auxTable, p);
                    } else
                    {
                        KillCell(auxTable, p);
                    }
                }
            }

            return auxTable;
        }

        /// <summary>
        /// Check if a Point is in the table.
        /// </summary>
        /// <param name="table">Game Table</param>
        /// <param name="p">Point</param>
        /// <returns>True if the point exists, false if not</returns>
        static bool ExistPoint(char[,] table, Point p)
        {
            if ((p.X() >= 0 && p.X() < WIDTH && (p.Y() >= 0 && p.Y() < HEIGHT)))
            {
                return true;
            }

            return false;
        }

        static void AliveCell(char[,] table, Point p)
        {
            table[p.Y(), p.X()] = LIVE_CELL_CHAR;
        }

        static void KillCell(char[,] table, Point p)
        {
            table[p.Y(), p.X()] = DEAD_CELL_CHAR;
        }

        static char GetCellState(char[,] table, Point p)
        {
            return table[p.Y(), p.X()];
        }
    }
}
