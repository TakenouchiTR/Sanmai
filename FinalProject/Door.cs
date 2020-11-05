using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FinalProject
{
    public class Door
    {
        public const int DOOR_WIDTH = 29;
        public const int DOOR_HEIGHT = 9;
        private const string DOOR_TOP =    "_____________________________";
        private const string DOOR_BODY =   "│                           │";
        private const string DOOR_BOTTOM = "‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾";

        private int x, y;
        private int frame;
        private int number;
        private bool drawTop;
        private bool drawBottom;
        private Prize prize;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Right => X + DOOR_WIDTH - 1;
        public int Bottom => Y + DOOR_HEIGHT;
        public int Number { get => number; set => number = value; }
        public int Frame { get => frame; set => frame = value; }
        public Prize Prize { get => prize; set => prize = value; }

        public Door(int x, int y, int number)
        {
            X = x;
            Y = y;
            Number = number;
            Frame = 0;
            prize = new Prize();
            drawTop = true;
            drawBottom = true;
        }

        public void Open(int speed)
        {
            for (int i = 0; i < 14; i++)
            {
                Thread.Sleep(speed);
                Frame++;
                Draw();
            }
        }

        public void Draw()
        {
            Console.CursorVisible = false;
            //Saves the cursor position and color so that it can be reset later
            ConsoleColor oldColor = Console.BackgroundColor;
            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;

            DrawBase();

            DrawPrize();

            DrawLeftTriangle(X + 1 - Frame, ConsoleColor.Red);
            DrawLeftTriangle(X + DOOR_HEIGHT / 2 + 3 - Frame, ConsoleColor.Yellow);

            DrawRightTriangle(Right - 1 + Frame, ConsoleColor.Red);
            DrawRightTriangle(Right - DOOR_HEIGHT / 2 - 3 + Frame, ConsoleColor.Yellow);

            DrawInnerDoor();

            //Resets the cursor position and color 
            Console.CursorLeft = cursorX;
            Console.CursorTop = cursorY;
            Console.BackgroundColor = oldColor;
            Console.CursorVisible = true;
        }

        private void DrawBase() 
        {
            Console.CursorTop = Y;
            if (drawTop)
            {
                Console.CursorTop--;
                Console.CursorLeft = X;
                Console.WriteLine(DOOR_TOP);
            }

            for (int i = 0; i < DOOR_HEIGHT; i++)
            {
                Console.CursorLeft = X;
                Console.WriteLine(DOOR_BODY);
            }

            if (drawBottom)
            {
                Console.CursorLeft = X;
                Console.WriteLine(DOOR_BOTTOM);
            }
        }

        private void DrawPrize()
        {
            if (frame > 0)
                prize.Draw(X, Y, (Frame - 1) * 2 + 1);
        }

        private void DrawLeftTriangle(int leftBound, ConsoleColor color)
        {
            if (leftBound + DOOR_HEIGHT / 2 + 2 <= X)
                return;

            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            int xPos = leftBound + DOOR_HEIGHT / 2;
            int yPos = Y;

            for (int i = 0; i < DOOR_HEIGHT / 2; i++)
            {
                Console.CursorLeft = xPos;
                Console.CursorTop = yPos;

                if (xPos > X)
                    Console.Write('╱');

                xPos--;
                yPos++;
            }

            Console.CursorLeft = xPos;
            Console.CursorTop = yPos;

            if (xPos > X)
                Console.Write('<');

            xPos++;
            yPos++;

            for (int i = 0; i < DOOR_HEIGHT / 2; i++)
            {
                Console.CursorLeft = xPos;
                Console.CursorTop = yPos;

                if (xPos > X)
                    Console.Write('╲');

                xPos++;
                yPos++;
            }

            Console.CursorTop = Y;
            if (xPos > X)
            {
                for (int i = 0; i < DOOR_HEIGHT; i++)
                {
                    Console.CursorLeft = xPos;
                    Console.WriteLine('│');
                }
            }

            Console.ForegroundColor = oldColor;
        }

        private void DrawRightTriangle(int rightBound, ConsoleColor color)
        {
            if (rightBound - DOOR_HEIGHT / 2 - 2 >= Right)
                return;

            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            int xPos = rightBound - DOOR_HEIGHT / 2;
            int yPos = Y;

            for (int i = 0; i < DOOR_HEIGHT / 2; i++)
            {
                Console.CursorLeft = xPos;
                Console.CursorTop = yPos;

                if (xPos < Right)
                    Console.Write('╲');

                xPos++;
                yPos++;
            }

            Console.CursorLeft = xPos;
            Console.CursorTop = yPos;

            if (xPos < Right)
                Console.Write('>');

            xPos--;
            yPos++;

            for (int i = 0; i < DOOR_HEIGHT / 2; i++)
            {
                Console.CursorLeft = xPos;
                Console.CursorTop = yPos;

                if (xPos < Right)
                    Console.Write('╱');

                xPos--;
                yPos++;
            }

            Console.CursorTop = Y;
            if (xPos < Right)
            {
                for (int i = 0; i < DOOR_HEIGHT; i++)
                {
                    Console.CursorLeft = xPos;
                    Console.WriteLine('│');
                }
            }

            Console.ForegroundColor = oldColor;
        }

        private void DrawInnerDoor()
        {
            int xPos = X + DOOR_WIDTH / 2 - Frame;

            if (xPos <= X)
                return;

            Console.CursorTop = Y;
            for (int i = 0; i < DOOR_HEIGHT; i++)
            {
                Console.CursorLeft = xPos;
                Console.WriteLine('┃');
            }

            Console.CursorTop = Y + DOOR_HEIGHT / 2;
            Console.CursorLeft = xPos;
            Console.WriteLine(Number);

            if (Frame > 0)
            {
                xPos += frame * 2;
                Console.CursorTop = Y;
                for (int i = 0; i < DOOR_HEIGHT; i++)
                {
                    Console.CursorLeft = xPos;
                    Console.WriteLine('┃');
                }
            }
        }

    }
}
