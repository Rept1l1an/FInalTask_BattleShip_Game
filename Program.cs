using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Numerics;

namespace BattleShip2._0
{
    public class Ship
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public int[,] Position { get; set; }
        public bool IsSunk { get; set; }

        public Ship(string name, int size)
        {
            Name = name;
            Size = size;
            Position = new int[size, 2];
            IsSunk = false;

            // Generate random starting position for the ship
            Random rand = new Random();
            int row = rand.Next(10 - size);
            int col = rand.Next(10 - size);

            // Set the position of the ship on the game board
            for (int i = 0; i < size; i++)
            {
                Position[i, 0] = row + i;
                Position[i, 1] = col;
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Ship> ships = new List<Ship>();
            ships.Add(new Ship("Carrier", 5));
            ships.Add(new Ship("Battleship", 4));
            ships.Add(new Ship("Cruiser", 3));
            ships.Add(new Ship("Submarine", 3));
            ships.Add(new Ship("Destroyer", 2));

            char[,] Board = new char[10, 10];
            char[,] BoardP1 = new char[10, 10];
            char[,] BoardP2 = new char[10, 10];
            char[,] BoardBattle1 = new char[10, 10];
            char[,] BoardBattle2 = new char[10, 10];

            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    Board[row, col] = ' ';
                    if (row == 0 || col == 0 || row == 9 || col == 9)
                    {
                        Board[row, col] = 'N';
                    }

                    BoardBattle1[row, col] = BoardBattle2[row, col] = BoardP2[row, col] = BoardP1[row, col] = Board[row, col];

                    Console.Write(Board[row, col]);
                }
                Console.WriteLine();
            }
            int ShipModel = 0;
            int Turn = 0;
            int x = 0;
            int WinnerP1 = 0;
            int WinnerP2 = 0;
            bool Menu = true, Game = false, HowTo = false;
            int ShipCount = 0;
            string[] ShipPort;
            ShipPort = new string[] { "Carrier", "Battleship", "Submarine", "Destroyer " };
            while (Menu)
            {
                Console.Clear();
                string[] opening;
                opening = new string[] { "Start", "How to play", "Exit" };
                for (int i = 0; i < 3; i++)
                {
                    if (i == x)
                    {
                        Console.WriteLine("\n |> " + opening[i] + "\n");
                        continue;
                    }
                    Console.WriteLine("\n     " + opening[i] + "\n");
                }
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.S:
                        x = x + 1;
                        if (x > 2)
                        {
                            x = 0;
                        }
                        break;
                    case ConsoleKey.W:
                        x = x - 1;
                        if (x < 0)
                        {
                            x = 2;
                        }
                        break;
                    case ConsoleKey.Enter:

                        if (x == 0)
                        {
                            Menu = false;
                            Game = true;
                        }
                        else if (x == 1)
                        {
                            HowTo = true;
                        }
                        else
                        {
                            Menu = false;
                        }
                        break;
                }
                while (HowTo)
                {
                    Console.Clear();
                    Console.WriteLine("Every player have 4 ship they are : \n 1. Carrier [5 points] \n 2. Battleship [4 points] \n 3. Submarine [3 points] \n 4. Destroyer [3 points] \n\n" +
                        "The gameplay is straightforward. Each player hides ships on a plastic grid containing vertical and horizontal space coordinates. Players take turns calling out " +
                        "row and column coordinates on the other player's grid in an attempt to identify a square that contains a ship.\n" +
                        "\nPress Enter to return main Menu");
                    ConsoleKeyInfo Exit = Console.ReadKey();
                    switch (Exit.Key)
                    {
                        case ConsoleKey.Enter:
                            HowTo = false;
                            break;
                    }
                }
            }

            x = 0;
            Console.Clear();
            while (Game)
            {
                int MoveX = 5, MoveY = 5;
                int CanonX = 1, CanonY = 1;
                int xy = 0;
                bool SetShips = true;
                bool MovementP1 = false;
                bool MovementP2 = false;
                bool Battle = false;
                int Rotate = 0;
                while (SetShips)
                {
                    Console.WriteLine("Choose the ship : \n");
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == xy)
                        {
                            Console.WriteLine(" |> " + ShipPort[i] + "\n");
                            continue;
                        }
                        Console.WriteLine("     " + ShipPort[i] + "\n");
                    }

                    ConsoleKeyInfo key = Console.ReadKey();
                    Console.Clear();
                    switch (key.Key)
                    {
                        case ConsoleKey.S:
                            xy = xy + 1;
                            if (xy > 3)
                            {
                                xy = 0;
                            }
                            break;
                        case ConsoleKey.W:
                            xy = xy - 1;
                            if (xy < 0)
                            {
                                xy = 3;
                            }
                            break;
                        case ConsoleKey.Enter:
                            SetShips = false;

                            ShipModel = xy;
                            if (ShipCount < 4)
                            {
                                MovementP1 = true;
                            }
                            else if (ShipCount < 8)
                            {
                                MovementP2 = true;
                            }
                            else
                            {
                                Battle = true;
                            }
                            break;
                    }

                    while (MovementP1)
                    {
                        Console.Clear();
                        Ship ChosenShip = ships[xy];
                        Console.WriteLine("Player 1s Board");
                        Console.WriteLine("Chosen ship: " + ChosenShip.Name + "\n");
                        for (int row = 0; row < 10; row++)
                        {
                            for (int col = 0; col < 10; col++)
                            {
                                Board[row, col] = BoardP1[row, col];
                                if (row == 0 || col == 0 || row == 9 || col == 9)
                                {
                                    Board[row, col] = BoardP1[row, col];
                                }
                            }
                        }

                        if (Rotate % 2 == 0)
                        {
                            if (ShipModel == 0)
                            {
                                if (MoveY + 2 > 8)
                                {
                                    MoveY = MoveY - 2;
                                }
                                else if (MoveY - 2 < 1)
                                {
                                    MoveY = MoveY + 2;
                                }
                                Board[MoveY - 1, MoveX] = 'C';
                                Board[MoveY - 2, MoveX] = 'C';
                                Board[MoveY, MoveX] = 'C';
                                Board[MoveY + 1, MoveX] = 'C';
                                Board[MoveY + 2, MoveX] = 'C';
                            }
                            else if (ShipModel == 1)
                            {
                                if (MoveY + 1 > 8)
                                {
                                    MoveY = MoveY - 1;
                                }
                                else if (MoveY - 2 < 1)
                                {
                                    MoveY = MoveY + 2;
                                }
                                Board[MoveY - 1, MoveX] = 'B';
                                Board[MoveY - 2, MoveX] = 'B';
                                Board[MoveY, MoveX] = 'B';
                                Board[MoveY + 1, MoveX] = 'B';
                            }
                            else if (ShipModel == 3)
                            {
                                if (MoveY + 1 > 8)
                                {
                                    MoveY = MoveY - 1;
                                }
                                else if (MoveY - 1 < 1)
                                {
                                    MoveY = MoveY + 1;
                                }
                                Board[MoveY - 1, MoveX] = 'D';
                                Board[MoveY, MoveX] = 'D';
                                Board[MoveY + 1, MoveX] = 'D';
                            }
                            else if (ShipModel == 2)
                            {
                                if (MoveY + 1 > 8)
                                {
                                    MoveY = MoveY - 1;
                                }
                                else if (MoveY - 1 < 1)
                                {
                                    MoveY = MoveY + 1;
                                }
                                Board[MoveY - 1, MoveX] = 'S';
                                Board[MoveY, MoveX] = 'S';
                                Board[MoveY + 1, MoveX] = 'S';
                            }
                        }

                        else
                        {
                            if (ShipModel == 0)
                            {
                                if (MoveX + 2 > 8)
                                {
                                    MoveX = MoveX - 2;
                                }
                                else if (MoveX - 2 < 1)
                                {
                                    MoveX = MoveX + 2;
                                }
                                Board[MoveY, MoveX - 1] = 'C';
                                Board[MoveY, MoveX - 2] = 'C';
                                Board[MoveY, MoveX] = 'C';
                                Board[MoveY, MoveX + 1] = 'C';
                                Board[MoveY, MoveX + 2] = 'C';
                            }
                            else if (ShipModel == 1)
                            {
                                if (MoveX + 1 > 8)
                                {
                                    MoveX = MoveX - 1;
                                }
                                else if (MoveX - 2 < 1)
                                {
                                    MoveX = MoveX + 2;
                                }
                                Board[MoveY, MoveX - 1] = 'B';
                                Board[MoveY, MoveX - 2] = 'B';
                                Board[MoveY, MoveX] = 'B';
                                Board[MoveY, MoveX + 1] = 'B';
                            }
                            else if (ShipModel == 3)
                            {
                                if (MoveX + 1 > 8)
                                {
                                    MoveX = MoveX - 1;
                                }
                                else if (MoveX - 1 < 1)
                                {
                                    MoveX = MoveX + 1;
                                }
                                Board[MoveY, MoveX - 1] = 'D';
                                Board[MoveY, MoveX] = 'D';
                                Board[MoveY, MoveX + 1] = 'D';
                            }
                            else if (ShipModel == 2)
                            {
                                if (MoveX + 1 > 8)
                                {
                                    MoveX = MoveX - 1;
                                }
                                else if (MoveX - 1 < 1)
                                {
                                    MoveX = MoveX + 1;
                                }
                                Board[MoveY, MoveX - 1] = 'S';
                                Board[MoveY, MoveX] = 'S';
                                Board[MoveY, MoveX + 1] = 'S';
                            }
                        }

                        for (int row = 0; row < 10; row++)
                        {
                            Console.Write("  ");
                            for (int col = 0; col < 10; col++)
                            {
                                Console.Write(Board[row, col] + " ");
                            }
                            Console.WriteLine();
                        }


                        ConsoleKeyInfo ShipMovement = Console.ReadKey();
                        switch (ShipMovement.Key)
                        {

                            case ConsoleKey.W:
                                MoveY = MoveY - 1;
                                if (MoveY < 1)
                                {
                                    MoveY = 8;
                                }
                                break;
                            case ConsoleKey.S:
                                MoveY = MoveY + 1;
                                if (MoveY > 8)
                                {
                                    MoveY = 1;
                                }
                                break;
                            case ConsoleKey.A:
                                MoveX = MoveX - 1;
                                if (MoveX < 1)
                                {
                                    MoveX = 8;
                                }
                                break;
                            case ConsoleKey.D:
                                MoveX = MoveX + 1;
                                if (MoveX > 8)
                                {
                                    MoveX = 1;
                                }
                                break;
                            case ConsoleKey.R:
                                Rotate++;
                                break;
                            case ConsoleKey.Enter:
                                ShipPort[xy] = "USED";
                                Console.Clear();

                                for (int row = 0; row < 10; row++)
                                {
                                    for (int col = 0; col < 10; col++)
                                    {
                                        BoardP1[row, col] = Board[row, col];
                                        if (row == 0 || col == 0 || row == 9 || col == 9)
                                        {
                                            BoardP1[row, col] = Board[row, col];
                                        }
                                    }
                                }

                                ShipCount++;
                                if (ShipCount > 3)
                                {
                                    ShipPort = new string[] { "Carrier", "Battleship", "Submarine", "Destroyer " };
                                }
                                MovementP1 = false;
                                break;
                        }

                    }
                    Rotate = 0;
                    while (MovementP2)
                    {
                        Console.Clear();
                        Ship ChosenShip = ships[xy];
                        Console.WriteLine("Player 2s Board");
                        Console.WriteLine("Chosen ship: " + ChosenShip.Name + "\n");
                        for (int row = 0; row < 10; row++)
                        {
                            for (int col = 0; col < 10; col++)
                            {
                                Board[row, col] = BoardP2[row, col];
                                if (row == 0 || col == 0 || row == 9 || col == 9)
                                {
                                    Board[row, col] = BoardP2[row, col];
                                }
                            }
                        }


                        if (Rotate % 2 == 0)
                        {
                            if (ShipModel == 0)
                            {
                                if (MoveY + 2 > 8)
                                {
                                    MoveY = MoveY - 2;
                                }
                                else if (MoveY - 2 < 1)
                                {
                                    MoveY = MoveY + 2;
                                }
                                Board[MoveY - 1, MoveX] = 'C';
                                Board[MoveY - 2, MoveX] = 'C';
                                Board[MoveY, MoveX] = 'C';
                                Board[MoveY + 1, MoveX] = 'C';
                                Board[MoveY + 2, MoveX] = 'C';
                            }
                            else if (ShipModel == 1)
                            {
                                if (MoveY + 1 > 8)
                                {
                                    MoveY = MoveY - 1;
                                }
                                else if (MoveY - 2 < 1)
                                {
                                    MoveY = MoveY + 2;
                                }
                                Board[MoveY - 1, MoveX] = 'B';
                                Board[MoveY - 2, MoveX] = 'B';
                                Board[MoveY, MoveX] = 'B';
                                Board[MoveY + 1, MoveX] = 'B';
                            }
                            else if (ShipModel == 3)
                            {
                                if (MoveY + 1 > 8)
                                {
                                    MoveY = MoveY - 1;
                                }
                                else if (MoveY - 1 < 1)
                                {
                                    MoveY = MoveY + 1;
                                }
                                Board[MoveY - 1, MoveX] = 'D';
                                Board[MoveY, MoveX] = 'D';
                                Board[MoveY + 1, MoveX] = 'D';
                            }
                            else if (ShipModel == 2)
                            {
                                if (MoveY + 1 > 8)
                                {
                                    MoveY = MoveY - 1;
                                }
                                else if (MoveY - 1 < 1)
                                {
                                    MoveY = MoveY + 1;
                                }
                                Board[MoveY - 1, MoveX] = 'S';
                                Board[MoveY, MoveX] = 'S';
                                Board[MoveY + 1, MoveX] = 'S';
                            }
                        }

                        else
                        {
                            if (ShipModel == 0)
                            {
                                if (MoveX + 2 > 8)
                                {
                                    MoveX = MoveX - 2;
                                }
                                else if (MoveX - 2 < 1)
                                {
                                    MoveX = MoveX + 2;
                                }
                                Board[MoveY, MoveX - 1] = 'C';
                                Board[MoveY, MoveX - 2] = 'C';
                                Board[MoveY, MoveX] = 'C';
                                Board[MoveY, MoveX + 1] = 'C';
                                Board[MoveY, MoveX + 2] = 'C';
                            }
                            else if (ShipModel == 1)
                            {
                                if (MoveX + 1 > 8)
                                {
                                    MoveX = MoveX - 1;
                                }
                                else if (MoveX - 2 < 1)
                                {
                                    MoveX = MoveX + 2;
                                }
                                Board[MoveY, MoveX - 1] = 'B';
                                Board[MoveY, MoveX - 2] = 'B';
                                Board[MoveY, MoveX] = 'B';
                                Board[MoveY, MoveX + 1] = 'B';
                            }
                            else if (ShipModel == 3)
                            {
                                if (MoveX + 1 > 8)
                                {
                                    MoveX = MoveX - 1;
                                }
                                else if (MoveX - 1 < 1)
                                {
                                    MoveX = MoveX + 1;
                                }
                                Board[MoveY, MoveX - 1] = 'D';
                                Board[MoveY, MoveX] = 'D';
                                Board[MoveY, MoveX + 1] = 'D';
                            }
                            else if (ShipModel == 2)
                            {
                                if (MoveX + 1 > 8)
                                {
                                    MoveX = MoveX - 1;
                                }
                                else if (MoveX - 1 < 1)
                                {
                                    MoveX = MoveX + 1;
                                }
                                Board[MoveY, MoveX - 1] = 'S';
                                Board[MoveY, MoveX] = 'S';
                                Board[MoveY, MoveX + 1] = 'S';
                            }
                        }

                        for (int row = 0; row < 10; row++)
                        {
                            Console.Write("  ");
                            for (int col = 0; col < 10; col++)
                            {
                                Console.Write(Board[row, col] + " ");
                            }
                            Console.WriteLine();
                        }

                        ConsoleKeyInfo ShipMovement = Console.ReadKey();
                        switch (ShipMovement.Key)
                        {
                            case ConsoleKey.W:
                                MoveY = MoveY - 1;
                                if (MoveY < 1)
                                {
                                    MoveY = 8;
                                }
                                break;
                            case ConsoleKey.S:
                                MoveY = MoveY + 1;
                                if (MoveY > 8)
                                {
                                    MoveY = 1;
                                }
                                break;
                            case ConsoleKey.A:
                                MoveX = MoveX - 1;
                                if (MoveX < 1)
                                {
                                    MoveX = 8;
                                }
                                break;
                            case ConsoleKey.D:
                                MoveX = MoveX + 1;
                                if (MoveX > 8)
                                {
                                    MoveX = 1;
                                }
                                break;
                            case ConsoleKey.R:
                                Rotate++;
                                break;
                            case ConsoleKey.Enter:
                                ShipPort[xy] = "USED";
                                Console.Clear();

                                for (int row = 0; row < 10; row++)
                                {
                                    for (int col = 0; col < 10; col++)
                                    {
                                        BoardP2[row, col] = Board[row, col];
                                        if (row == 0 || col == 0 || row == 9 || col == 9)
                                        {
                                            BoardP2[row, col] = Board[row, col];
                                        }
                                    }
                                }

                                ShipCount++;
                                MovementP2 = false;
                                break;
                        }
                    }

                    while (Battle)
                    {
                        Console.Clear();

                        Console.WriteLine();
                        if (Turn % 2 == 0)
                        {
                            for (int row = 0; row < 10; row++)
                            {
                                for (int col = 0; col < 10; col++)
                                {
                                    Board[row, col] = BoardBattle1[row, col];
                                    if (row == 0 || col == 0 || row == 9 || col == 9)
                                    {
                                        Board[row, col] = BoardBattle1[row, col];
                                    }
                                }
                            }

                            Console.ForegroundColor = ConsoleColor.Green;
                            Board[CanonY, CanonX] = '+';
                            Console.WriteLine("Player 2s Board : \n");
                            for (int row = 0; row < 10; row++)
                            {
                                Console.Write("  ");
                                for (int col = 0; col < 10; col++)
                                {
                                    if (Board[row, col] == 'X')
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                    }
                                    else if (Board[row, col] == 'O')
                                    {
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    Console.Write(Board[row, col] + " ");
                                }
                                Console.WriteLine();
                            }

                            ConsoleKeyInfo ShipMovement = Console.ReadKey();
                            switch (ShipMovement.Key)
                            {
                                case ConsoleKey.W:
                                    CanonY = CanonY - 1;
                                    if (CanonY < 1)
                                    {
                                        CanonY = 8;
                                    }
                                    break;
                                case ConsoleKey.S:
                                    CanonY = CanonY + 1;
                                    if (CanonY > 8)
                                    {
                                        CanonY = 1;
                                    }
                                    break;
                                case ConsoleKey.A:
                                    CanonX = CanonX - 1;
                                    if (CanonX < 1)
                                    {
                                        CanonX = 8;
                                    }
                                    break;
                                case ConsoleKey.D:
                                    CanonX = CanonX + 1;
                                    if (CanonX > 8)
                                    {
                                        CanonX = 1;
                                    }
                                    break;
                                case ConsoleKey.Enter:
                                    if (BoardP2[CanonY, CanonX] == 'C' || BoardP2[CanonY, CanonX] == 'D' || BoardP2[CanonY, CanonX] == 'B' || BoardP2[CanonY, CanonX] == 'S')
                                    {
                                        BoardBattle1[CanonY, CanonX] = 'X';
                                        BoardP2[CanonY, CanonX] = 'X';
                                        WinnerP1++;
                                        if (WinnerP1 > 14)
                                        {
                                            Battle = false;
                                            Game = false;
                                        }
                                    }

                                    else
                                    {
                                        BoardBattle1[CanonY, CanonX] = 'O';
                                        Turn++;
                                    }
                                    break;
                            }
                        }
                        else if (Turn % 2 == 1)
                        {
                            for (int row = 0; row < 10; row++)
                            {
                                for (int col = 0; col < 10; col++)
                                {
                                    Board[row, col] = BoardBattle2[row, col];
                                    if (row == 0 || col == 0 || row == 9 || col == 9)
                                    {
                                        Board[row, col] = BoardBattle2[row, col];
                                    }
                                }
                            }
                            Console.ForegroundColor = ConsoleColor.Green;
                            Board[CanonY, CanonX] = '+';
                            Console.WriteLine("Player 1s Board : \n");
                            for (int row = 0; row < 10; row++)
                            {
                                Console.Write("  ");
                                for (int col = 0; col < 10; col++)
                                {
                                    if (Board[row, col] == 'X')
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                    }
                                    else if (Board[row,col]== 'O')
                                    {
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                    }
                                    else
                                    {
                                        Console.ForegroundColor= ConsoleColor.White;
                                    }
                                    Console.Write(Board[row, col] + " ");
                                }
                                Console.WriteLine();
                            }

                            ConsoleKeyInfo ShipMovement = Console.ReadKey();
                            switch (ShipMovement.Key)
                            {
                                case ConsoleKey.W:
                                    CanonY = CanonY - 1;
                                    if (CanonY < 1)
                                    {
                                        CanonY = 8;
                                    }
                                    break;
                                case ConsoleKey.S:
                                    CanonY = CanonY + 1;
                                    if (CanonY > 8)
                                    {
                                        CanonY = 1;
                                    }
                                    break;
                                case ConsoleKey.A:
                                    CanonX = CanonX - 1;
                                    if (CanonX < 1)
                                    {
                                        CanonX = 8;
                                    }
                                    break;
                                case ConsoleKey.D:
                                    CanonX = CanonX + 1;
                                    if (CanonX > 8)
                                    {
                                        CanonX = 1;
                                    }
                                    break;
                                case ConsoleKey.Enter:
                                    if (BoardP1[CanonY, CanonX] == 'C' || BoardP1[CanonY, CanonX] == 'D' || BoardP1[CanonY, CanonX] == 'B' || BoardP1[CanonY, CanonX] == 'S')
                                    {
                                        Console.WriteLine("Player 1 Hitted");
                                        BoardBattle2[CanonY, CanonX] = 'X';
                                        BoardP1[CanonY, CanonX] = 'X';
                                        WinnerP2++;
                                        if (WinnerP2 > 14)
                                        {
                                            Battle = false;
                                            Game = false;
                                        }
                                    }

                                    else
                                    {
                                        BoardBattle2[CanonY, CanonX] = 'O';
                                        Turn++;
                                    }
                                    Console.Clear();
                                    break;
                            }
                        }
                    }
                }
            }
            if (WinnerP1 > WinnerP2)
            {
                Console.WriteLine("Player 1 is winner thanks for playing");
            }
            else if (WinnerP1 < WinnerP2)
            {
                Console.WriteLine("Player 2 is winner thanks for playing");
            }
        }
    }
}