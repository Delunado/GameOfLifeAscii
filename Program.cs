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
        }

        static int WIDTH = 16, HEIGHT = 16;
        const char LIVE_CELL_CHAR = '#', DEAD_CELL_CHAR = '*';

        static void Main(string[] args)
        {
            char[,] table = CreateGameTable(HEIGHT, WIDTH); //BE CAREFUL: FIRST Y, SECOND X

            //We create 10 initial points
            for (int i = -5; i < 5; i++)
            {
                Point p = new Point(WIDTH / 2 + i, HEIGHT / 2);
                AliveCell(table, p);
            }

            MainMenu(table);
        }

        //******MENU METHODS******
        static void MainMenu(char[,] table)
        {
            ShowMenu(table);
            Console.Write("\n> ");

            bool runMenu = true;
            while (runMenu)
            {
                int option = 0;
                option = TryParseInt(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        Console.Clear();
                        GameLoop(table);
                        ShowMenu(table);
                        break;
                    case 2:
                        AddCellOnPointMenu(table);
                        ShowMenu(table);
                        break;
                    case 3:
                        EliminateCellOnPointMenu(table);
                        ShowMenu(table);
                        break;
                    case 4:
                        ResizeTable();
                        table = CreateGameTable(HEIGHT, WIDTH);
                        ShowMenu(table);
                        break;
                    case 5:
                        Console.WriteLine("Bye!");
                        runMenu = false;
                        break;
                }

                if (runMenu)
                {
                    Console.Write("\n> ");
                }
            }
        }

        static void ShowMenu(char[,] table)
        {
            Console.Clear();
            ShowTabletop(table);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n");
            Console.WriteLine("1. Start Game");
            Console.WriteLine("2. Add Live Cell");
            Console.WriteLine("3. Remove Live Cell");
            Console.WriteLine("4. Resize Table. It erases all the live cells!");
            Console.WriteLine("5. Exit");
        }

        //******GAME METHODS******
        /// <summary>
        /// Creates and returns the Game Table with a specific height and width
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <returns>The Game Table</returns>
        static char[,] CreateGameTable(int height, int width)
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

        /// <summary>
        /// Draw the Game Table in the console.
        /// </summary>
        /// <param name="table">Game Table</param>
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

        /// <summary>
        /// Apply the game rules on the given table, and return a table with the changes. 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
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

        static void GameLoop(char[,] table)
        {
            while (true)
            {
                Console.CursorVisible = false;
                ShowTabletop(table);
                System.Threading.Thread.Sleep(100);
                Console.Clear();
                table = GameOfLife(table);
            }
        }

        static void AddCellOnPointMenu(char[,] table)
        {
            int x = 0, y = 0;

            do
            {
                Console.WriteLine("Add a live cell on a valid point (TABLE SIZE: {0} x {1}).", WIDTH, HEIGHT);

                Console.Write("X: ");
                x = TryParseInt(Console.ReadLine());

                Console.Write("Y: ");
                y = TryParseInt(Console.ReadLine());

            } while (!((x >= 0 && x < WIDTH) && (y >= 0 && y < HEIGHT)));

            AliveCell(table, new Point(x, y));
        }

        static void EliminateCellOnPointMenu(char[,] table)
        {
            int x = 0, y = 0;

            do
            {
                Console.WriteLine("Kill a live cell on a valid point (TABLE SIZE: {0} x {1}).", WIDTH, HEIGHT);

                Console.Write("X: ");
                x = TryParseInt(Console.ReadLine());

                Console.Write("Y: ");
                y = TryParseInt(Console.ReadLine());

            } while (!((x >= 0 && x < WIDTH) && (y >= 0 && y < HEIGHT)));

            KillCell(table, new Point(x, y));
        }

        static void ResizeTable()
        {
            int x = 0, y = 0;

            do
            {
                Console.WriteLine("Resize the table. Min: 1 - Max: 32. Actual Size: {0} x {1}", WIDTH, HEIGHT);
                Console.Write("X: ");

                if (int.TryParse(Console.ReadLine(), out int optx))
                {
                    x = optx;
                }

                Console.Write("Y: ");
                if (int.TryParse(Console.ReadLine(), out int opty))
                {
                    y = opty;
                }

            } while (!((x > 0 && x <= 32) && (y > 0 && y <= 32)));

            WIDTH = x;
            HEIGHT = x;
        }

        //******GAME RULES******
        /// <summary>
        /// Check if a certain cell in a point will become live or not.
        /// </summary>
        /// <param name="table">Game Table</param>
        /// <param name="auxTable">Aux Table which will replace the Game Table</param>
        /// <param name="p">Cell Point</param>
        static void CheckBirth(char[,] table, char[,] auxTable, Point p)
        {
            if (NeighboursNumber(table, p) == 3)
            {
                AliveCell(auxTable, p);
            }
        }

        /// <summary>
        /// Check if a certain cell in a point will die or not.
        /// </summary>
        /// <param name="table">Game Table</param>
        /// <param name="auxTable">Aux Table which will replace the Game Table</param>
        /// <param name="p">Cell Point</param>
        static void CheckDeath(char[,] table, char[,] auxTable, Point p)
        {
            int neighbours = NeighboursNumber(table, p);

            if (neighbours != 2 && neighbours != 3)
            {
                KillCell(auxTable, p);
            }
        }

        /// <summary>
        /// Check all neighbours cells and returns the number of living neighbours cells around a point.
        /// </summary>
        /// <param name="table">Game Table</param>
        /// <param name="p">Point to check around</param>
        /// <returns>Number of neigbour living cells around a point</returns>
        static int NeighboursNumber(char[,] table, Point p)
        {
            int neighbours = 0;

            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    if (!(x == 0 && y == 0))
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

        /// <summary>
        /// Set alive a cell in a certain point.
        /// </summary>
        /// <param name="table">Game Table</param>
        /// <param name="p">Cell point</param>
        static void AliveCell(char[,] table, Point p)
        {
            table[p.Y(), p.X()] = LIVE_CELL_CHAR;
        }

        /// <summary>
        /// Set death a cell in a certain point.
        /// </summary>
        /// <param name="table">Game Table</param>
        /// <param name="p">Cell point</param>
        static void KillCell(char[,] table, Point p)
        {
            table[p.Y(), p.X()] = DEAD_CELL_CHAR;
        }

        //******AUX METHODS******
        /// <summary>
        /// Creates a copy of the param table. 
        /// </summary>
        /// <param name="table">The table you want to copy</param>
        /// <returns>A copy of the table</returns>
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

        /// <summary>
        /// Returns the state (alive/dead) of a cell in a certain point.
        /// </summary>
        /// <param name="table">Game Table</param>
        /// <param name="p">Cell point</param>
        /// <returns>A char (alive/dead)</returns>
        static char GetCellState(char[,] table, Point p)
        {
            return table[p.Y(), p.X()];
        }

        static int TryParseInt(string strInt)
        {
            int number = 0;
            if (int.TryParse(strInt, out int opt))
            {
                number = opt;
            } else
            {
                number = -1;
            }

            return number;
        }

    }
}
