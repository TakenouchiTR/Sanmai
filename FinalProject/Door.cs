using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject
{
    public class Door
    {
        public const int DOOR_WIDTH = 21;
        public const int DOOR_HEIGHT = 9;

        private int x, y;
        private int frame;
        private int number;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Number { get => number; set => number = value; }
        public int Frame { get => frame; set => frame = value; }

        private readonly string[] Display = new string[]
        {
            "|\\        |        /|",
            "| \\       |       / |",
            "|  \\      |      /  |",
            "|   \\     |     /   |",
            "|    >         <    |",
            "|   /     |     \\   |",
            "|  /      |      \\  |",
            "| /       |       \\ |",
            "|/        |        \\|",
        };

        public Door(int x, int y, int number)
        {
            X = x;
            Y = y;
            Number = number;
            Frame = 0;
        }
        public void Draw()
        {
            Console.CursorTop = y;
            foreach (string s in Display)
            {
                Console.CursorLeft = x;
                Console.WriteLine(s);
            }

            Console.CursorLeft = x + DOOR_WIDTH / 2;
            Console.CursorTop = y + DOOR_HEIGHT / 2;
            Console.Write(Number);

            Console.CursorLeft = 0;
            Console.CursorTop = Y + DOOR_HEIGHT;
        }
    }
}
