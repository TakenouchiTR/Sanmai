//Name:    Shawn Carter
//Date:    11/09/2020
//File:    Painter.cs
//Purpose: This class handles all of the writing done to the screen, allowing for multiple threads to write without
//           interfering with each other's colors or positions.

using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject.IO
{
    public static class Painter
    {
        private static readonly Object KEY = new Object();

        private static ConsoleColor defaultBackColor = ConsoleColor.Black;
        private static ConsoleColor defaultFrontColor = ConsoleColor.White;

        public static ConsoleColor DefaultBackColor { get => defaultBackColor; set => defaultBackColor = value; }
        public static ConsoleColor DefaultFrontColor { get => defaultFrontColor; set => defaultFrontColor = value; }

        public static void Write(string line, int x, int y)
        {
            Write(line, x, y, DefaultFrontColor);
        }

        public static void Write(string line, int x, int y, ConsoleColor frontColor)
        {
            Write(line, x, y, frontColor, DefaultBackColor);
        }

        public static void Write(string line, int x, int y, ConsoleColor frontColor, ConsoleColor backColor)
        {
            lock (KEY)
            {
                Console.ForegroundColor = frontColor;
                Console.BackgroundColor = backColor;

                Console.CursorLeft = x;
                Console.CursorTop = y;

                Console.Write(line);

                Console.ForegroundColor = DefaultFrontColor;
                Console.BackgroundColor = DefaultBackColor;
            }
        }

        public static void Write(char character, int x, int y)
        {
            Write(character, x, y, DefaultFrontColor);
        }

        public static void Write(char character, int x, int y, ConsoleColor frontColor)
        {
            Write(character, x, y, frontColor, DefaultBackColor);
        }

        public static void Write(char character, int x, int y, ConsoleColor frontColor, ConsoleColor backColor)
        {
            Write(character.ToString(), x, y, frontColor, backColor);
        }
    }
}
