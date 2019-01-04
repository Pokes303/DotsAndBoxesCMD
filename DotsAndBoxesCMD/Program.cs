using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

/* Dots and Boxes in CMD by Pokes
 * Feel free to edit this project, but no to republish it
 * because it took many hours.
 * For more info about this project, see https://github.com/pokes303/DotsAndBoxesCMD
 */

namespace DotsAndBoxesCMD
{
    public struct Vector
    {
        public Vector(int VecX, int VecY)
        {
            x = VecX;
            y = VecY;
        }
        public int x { get; set; }
        public int y { get; set; }

        public static Vector Generic {
            get
            {
                return new Vector(-100, -100);
            }
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y);
        }
        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y);
        }
        public static bool operator ==(Vector a, Vector b)
        {
            return (a.x == b.x && a.y == b.y);
        }
        public static bool operator !=(Vector a, Vector b)
        {
            return (a.x != b.x || a.y != b.y);
        }
        public static bool operator >(Vector a, Vector b)
        {
            return (a.x > b.x || a.y > b.y);
        }
        public static bool operator <(Vector a, Vector b)
        {
            return (a.x < b.x || a.y < b.y);
        }
    }
    public class Player
    {
        //public bool CPU = false
        public int Boxes = 0;
    }
    class Program
    {
        public int GridSize = 3;

        public List<Player> Players = new List<Player>();
        public int PlayerTurn = 0;
        public ConsoleColor PlayerTurnColor
        {
            get
            {
                switch (PlayerTurn)
                {
                    case 0:
                        return ConsoleColor.Cyan;
                    case 1:
                        return ConsoleColor.Green;
                    case 2:
                        return ConsoleColor.Yellow;
                    case 3:
                        return ConsoleColor.Red;
                    case 4:
                        return ConsoleColor.Magenta;
                }
                return ConsoleColor.White;
            }
        }
        public string PlayerColorName
        {
            get
            {
                switch (PlayerTurn)
                {
                    case 0:
                        return "Blue";
                    case 1:
                        return "Green";
                    case 2:
                        return "Yellow";
                    case 3:
                        return "Red";
                    case 4:
                        return "Pink";
                }
                return "Null";
            }
        }
        public int PlayerBoxes = 0;

        public ConsoleColor[,] HorWires, VerWires, BoxWires;

        public Vector SelectedPoint = Vector.Generic, AttachedPoint = Vector.Generic;
        Vector[] VectorCoords = { new Vector(0, 1), new Vector(1, 0), new Vector(0, -1), new Vector(-1, 0) };
        public byte Errors = 0;
        public string[] ErrorMessages = { " ERROR: Input a whole number bigger than 1 and lesser than ",
            " ERROR: The selected points are not in a near position",
            " ERROR: The selected points cannot be the same",
            " ERROR: The selected wire is alredy written"};
        //"ERROR: Input a whole number bigger than 1 and lesser than " + GridSize
        static void Main(string[] args)
        {
            Program prg = new Program();
            bool[] InitialErrors = { false, false };
            int PlayerNumber = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(" Dots and Boxes in CMD");
                Console.WriteLine("    By Pokes (2019)");
                Console.WriteLine();
                if (InitialErrors[0] || InitialErrors[1])
                {
                    if (InitialErrors[0])
                        Console.Write(" ERROR: The number of the players need\n to be a number between 1~5: ");
                    else
                        Console.Write(" ERROR: The size of the grid need to be\n a number between 2~99");
                    InitialErrors[0] = false;
                    InitialErrors[1] = false;
                }
                Console.Write(" Welcome to ");
                foreach (char CharTemp in "Dots and Boxes!")
                {
                    Console.ForegroundColor = prg.PlayerTurnColor;
                    prg.PlayerTurn = (prg.PlayerTurn < 4) ? ++prg.PlayerTurn : 0;
                    Console.Write(CharTemp);
                }
                prg.PlayerTurn = 0;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(" Select\n how many players will play {1~5}: ");
                try
                {
                    PlayerNumber = Convert.ToInt32(Console.ReadLine());
                    if (PlayerNumber < 1 || PlayerNumber > 5)
                        throw new Exception("So many players");
                }
                catch (Exception)
                {
                    InitialErrors[0] = true;
                    PlayerNumber = 0;
                    continue;
                }
                for (int i = 0; i < PlayerNumber; i++)
                    prg.Players.Add(new Player());
                Console.WriteLine();
                Console.Write(" Select the size of the grid {2~99} (If the\n grid is too big it wont write correctly): ");
                try
                {
                    prg.GridSize = Convert.ToInt32(Console.ReadLine());
                    if (prg.GridSize < 2 || prg.GridSize > 99)
                        throw new Exception("Grid is too big");
                }
                catch (Exception)
                {
                    InitialErrors[1] = true;
                    continue;
                }
                break;
            }
            prg.HorWires = new ConsoleColor[prg.GridSize - 1, prg.GridSize];
            prg.VerWires = new ConsoleColor[prg.GridSize, prg.GridSize - 1];
            prg.BoxWires = new ConsoleColor[prg.GridSize - 1, prg.GridSize - 1];
            while (true)
            {
                Console.Clear();
                Console.WriteLine(" Dots and Boxes in CMD");
                Console.WriteLine("    By Pokes (2019)");
                Console.WriteLine();
                if (prg.PlayerBoxes >= Math.Pow(prg.GridSize - 1, 2))
                    break;
                int TempPlayerTurn = prg.PlayerTurn;
                prg.PlayerTurn = 0;
                for (int i = 0; i < prg.Players.Count; i++)
                {
                    if (i != TempPlayerTurn)
                        Console.Write(" " + (i + 1) + "; ");
                    else
                    {
                        Console.Write(" ");
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write((i + 1) + ";");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                    }
                    Console.ForegroundColor = prg.PlayerTurnColor;
                    Console.Write(prg.PlayerColorName);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" - Boxes: " + prg.Players[prg.PlayerTurn].Boxes);
                    prg.PlayerTurn++;
                }
                prg.PlayerTurn = TempPlayerTurn;
                Console.WriteLine();
                prg.GenerateGird(prg);
                Console.WriteLine();
                if (prg.Errors != 0)
                {
                    Console.WriteLine(prg.ErrorMessages[prg.Errors - 1] + ((prg.Errors == 1) ? prg.GridSize.ToString() : ""));
                    prg.Errors = 0;
                }
                try
                {
                    if (prg.SelectedPoint == Vector.Generic)
                    {
                        Console.Write(" Select a point in X axis: ");
                        prg.SelectedPoint.x = Convert.ToInt32(prg.CheckArgument(Console.ReadLine()));
                        Console.Write(" Select a point in Y axis: ");
                        prg.SelectedPoint.y = Convert.ToInt32(prg.CheckArgument(Console.ReadLine()));
                    }
                    else
                    {
                        Console.Write(" Select a near point in X axis: ");
                        prg.AttachedPoint.x = Convert.ToInt32(prg.CheckArgument(Console.ReadLine()));
                        Console.Write(" Select a near point in Y axis: ");
                        prg.AttachedPoint.y = Convert.ToInt32(prg.CheckArgument(Console.ReadLine()));

                        if (prg.SelectedPoint == prg.AttachedPoint)
                        {
                            prg.Errors = 3;
                            throw new Exception ("Same points");
                        }
                        bool CheckNearVectors = false;
                        foreach (Vector VectorTemp in prg.VectorCoords)
                        {
                            if (prg.AttachedPoint - prg.SelectedPoint == VectorTemp)
                            {
                                CheckNearVectors = true;
                                if (prg.SelectedPoint > prg.AttachedPoint)
                                {
                                    Vector TempPoint = prg.AttachedPoint;
                                    prg.AttachedPoint = prg.SelectedPoint;
                                    prg.SelectedPoint = TempPoint;
                                }
                                if (VectorTemp.x != 0)
                                {
                                    if (prg.HorWires[prg.SelectedPoint.x, prg.SelectedPoint.y] == ConsoleColor.Black)
                                        prg.HorWires[prg.SelectedPoint.x, prg.SelectedPoint.y] = prg.PlayerTurnColor;
                                    else
                                    {
                                        prg.Errors = 4;
                                        throw new Exception("Wire alredy written");
                                    }
                                    if (prg.SelectedPoint.y > 0 &&
                                    prg.HorWires[prg.SelectedPoint.x, prg.SelectedPoint.y - 1] != ConsoleColor.Black &&
                                    prg.VerWires[prg.SelectedPoint.x, prg.SelectedPoint.y - 1] != ConsoleColor.Black &&
                                    prg.VerWires[prg.AttachedPoint.x, prg.AttachedPoint.y - 1] != ConsoleColor.Black)
                                    {
                                        prg.BoxWires[prg.SelectedPoint.x, prg.SelectedPoint.y - 1] = prg.PlayerTurnColor;
                                        prg.Players[prg.PlayerTurn].Boxes++;
                                    }
                                    if (prg.SelectedPoint.y < prg.GridSize - 1 &&
                                    prg.HorWires[prg.SelectedPoint.x, prg.SelectedPoint.y + 1] != ConsoleColor.Black &&
                                    prg.VerWires[prg.AttachedPoint.x, prg.AttachedPoint.y] != ConsoleColor.Black &&
                                    prg.VerWires[prg.SelectedPoint.x, prg.SelectedPoint.y] != ConsoleColor.Black)
                                    {
                                        prg.BoxWires[prg.AttachedPoint.x - 1, prg.AttachedPoint.y] = prg.PlayerTurnColor;
                                        prg.Players[prg.PlayerTurn].Boxes++;
                                    }
                                }
                                else if (VectorTemp.y != 0)
                                {
                                    if (prg.VerWires[prg.SelectedPoint.x, prg.SelectedPoint.y] == ConsoleColor.Black)
                                        prg.VerWires[prg.SelectedPoint.x, prg.SelectedPoint.y] = prg.PlayerTurnColor;
                                    else
                                    {
                                        prg.Errors = 4;
                                        throw new Exception("Wire alredy written");
                                    }
                                    if (prg.SelectedPoint.x > 0 &&
                                    prg.VerWires[prg.SelectedPoint.x - 1, prg.SelectedPoint.y] != ConsoleColor.Black &&
                                    prg.HorWires[prg.SelectedPoint.x - 1, prg.SelectedPoint.y] != ConsoleColor.Black &&
                                    prg.HorWires[prg.SelectedPoint.x - 1, prg.AttachedPoint.y] != ConsoleColor.Black)
                                    {
                                        prg.BoxWires[prg.SelectedPoint.x - 1, prg.SelectedPoint.y] = prg.PlayerTurnColor;
                                        prg.Players[prg.PlayerTurn].Boxes++;
                                    }
                                    if (prg.SelectedPoint.x < prg.GridSize - 1 &&
                                    prg.VerWires[prg.SelectedPoint.x + 1, prg.SelectedPoint.y] != ConsoleColor.Black &&
                                    prg.HorWires[prg.SelectedPoint.x, prg.SelectedPoint.y] != ConsoleColor.Black &&
                                    prg.HorWires[prg.AttachedPoint.x, prg.AttachedPoint.y] != ConsoleColor.Black)
                                    {
                                        prg.BoxWires[prg.SelectedPoint.x, prg.SelectedPoint.y] = prg.PlayerTurnColor;
                                        prg.Players[prg.PlayerTurn].Boxes++;
                                    }
                                }
                            }
                        }
                        if (!CheckNearVectors)
                        {
                            prg.Errors = 2;
                            throw new Exception("Not in near position");
                        }
                        prg.SelectedPoint = Vector.Generic;
                        prg.AttachedPoint = Vector.Generic;
                        int BoxesTemp = 0;
                        foreach (Player PlayerTemp in prg.Players)
                            BoxesTemp += PlayerTemp.Boxes;
                        if (BoxesTemp <= prg.PlayerBoxes)
                            prg.PlayerTurn = (prg.PlayerTurn >= prg.Players.Count - 1) ? 0 : prg.PlayerTurn + 1;
                        else
                            prg.PlayerBoxes = BoxesTemp;
                    }
                }
                catch (Exception)
                {
                    prg.SelectedPoint = Vector.Generic;
                    prg.AttachedPoint = Vector.Generic;
                }
            }
            Console.WriteLine("     [Score list]");
            List<int> WinnersTemp = new List<int>();
            int TopNumber = 0;
            for (int i = 0; i < prg.Players.Count; i++)
            {
                if (prg.Players[i].Boxes > TopNumber)
                {
                    TopNumber = prg.Players[i].Boxes;
                    WinnersTemp.Clear();
                    WinnersTemp.Add(i);
                }
                else if (prg.Players[i].Boxes == TopNumber)
                    WinnersTemp.Add(i);
            }
            bool Winner = false;
            prg.PlayerTurn = 0;
            for (int i = 0; i < prg.Players.Count; i++)
            {
                foreach(int NumberTemp in WinnersTemp)
                {
                    if (NumberTemp == i)
                    {
                        Winner = true;
                        break;
                    }
                }
                if (!Winner)
                    Console.Write(" #" + (i + 1) + "; ");
                else
                {
                    Console.Write(" ");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write("#" + (i + 1) + ";");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                    Winner = false;
                }
                Console.ForegroundColor = prg.PlayerTurnColor;
                Console.Write(prg.PlayerColorName);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" - Boxes: " + prg.Players[prg.PlayerTurn].Boxes);
                prg.PlayerTurn++;
            }
            Console.WriteLine();
            Console.WriteLine("Game ended! Press any key to exit or [R] to restart the game");
            ConsoleKeyInfo EndKey = Console.ReadKey();
            if (EndKey.Key == ConsoleKey.R)
                Main(new string[0]);
        }
        private void GenerateGird(Program prg)
        {
            string Point = "0", HorWire = "═══";
            int CountY = 1;
            for (int y = 0; y < GridSize; y++)
            {
                Console.Write("   ");
                for (int x = 0; x < GridSize; x++)
                {
                    if (SelectedPoint != Vector.Generic)
                    {
                        foreach (Vector VectorTemp in prg.VectorCoords)
                            SelectColorIf(SelectedPoint == new Vector(x, y) + VectorTemp, ConsoleColor.Black, ConsoleColor.Gray);
                        SelectColorIf(SelectedPoint == new Vector(x, y), ConsoleColor.Black, ConsoleColor.White);
                    }
                    Console.Write(Point);
                    if (SelectedPoint != Vector.Generic)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }

                    if (x < GridSize - 1)
                    {
                        if (prg.HorWires[x, y] != ConsoleColor.Black)
                            Console.ForegroundColor = prg.HorWires[x, y];
                        Console.Write(HorWire);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                Console.Write("   " + CountY);
                CountY++;
                Console.WriteLine();
                if (y > GridSize - 2)
                {
                    Console.WriteLine();
                    for (int c = 1; c <= GridSize; c++)
                    {
                        if (c < 10)
                            Console.Write("   " + c);
                        else
                            Console.Write("  " + c);
                    }

                    Console.WriteLine("  x\\y");
                    return;
                }
                Console.Write("   ");
                for (int z = 0; z < GridSize; z++)
                {
                    if (prg.VerWires[z, y] != ConsoleColor.Black)
                        Console.ForegroundColor = prg.VerWires[z, y];
                    Console.Write("║");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (z < GridSize - 1)
                    {
                        if (prg.BoxWires[z, y] != ConsoleColor.Black)
                        {
                            Console.ForegroundColor = prg.BoxWires[z, y];
                            Console.Write(" ■ ");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        else
                            Console.Write("   ");
                    }
                }
                Console.WriteLine();
            }
        }
        private void SelectColorIf(bool Operation, ConsoleColor TextColor, ConsoleColor BackColor)
        {
            if (!Operation)
                return;
            Console.ForegroundColor = TextColor;
            Console.BackgroundColor = BackColor;
        }
        private string CheckArgument(string arg)
        {
            Program prg = new Program();
            switch (arg)
            {
                default:
                    try
                    {
                        int temp = Convert.ToInt32(arg) - 1;
                        if (temp < 0 || temp > GridSize - 1)
                        {
                            Errors = 1;
                            throw new Exception("Point not valid");
                        }
                        return temp.ToString();
                    }
                    catch (Exception)
                    {
                        return "#";
                    }
                case "restart":
                case "-r":
                    Main(new string[0]);
                    return "#";
                case "exit":
                case "-e":
                    Environment.Exit(0);
                    return "#";
            }
        }
    }
}
