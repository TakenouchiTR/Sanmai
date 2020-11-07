using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FinalProject
{
    public class Door
    {
        #region Constants
        public const int DOOR_WIDTH = 29;
        public const int DOOR_HEIGHT = 9;
        private const string DOOR_TOP =    "_____________________________";
        private const string DOOR_BODY =   "│                           │";
        private const string DOOR_BOTTOM = "‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾";
        private static readonly int[] SLOW_OPEN = new int[]
        {
            50, 50, 50, -1000, -750, -750, 2000, 25, 25, 25, 
            25, 25, 25, 25, 25, 25, 25, 25, 25, 25
        };
        #endregion

        #region Class Fields
        private static Random ran = new Random();
        #endregion

        #region Fields
        private int x, y;
        private int frame;
        private int number;
        private bool drawTop;
        private bool drawBottom;
        private Prize prize;
        #endregion

        #region Properties
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Right => X + DOOR_WIDTH - 1;
        public int Bottom => Y + DOOR_HEIGHT - 1;
        public int Number { get => number; set => number = value; }
        public int Frame { get => frame; set => frame = value; }
        public bool Closed => Frame == 0;
        public Prize Prize { get => prize; set => prize = value; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a door at a certain coordinate.
        /// </summary>
        /// <param name="x">The left-most coordinate for the door</param>
        /// <param name="y">The top-most coordinate for the door</param>
        /// <param name="number">The number to display</param>
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
        #endregion

        #region Methods
        /// <summary>
        /// Plays the open animation at a given speed.
        /// </summary>
        /// <param name="speed">Time in milliseconds between frames</param>
        public void Open(int speed)
        {
            while (Frame < DOOR_WIDTH / 2)
            {
                Thread.Sleep(speed);
                Frame++;
                Draw();
            }
        }

        /// <summary>
        /// Plays the open animation at a given speed, with a chance of playing a special animation.
        /// </summary>
        /// <param name="speed">Time in milliseconds between frames</param>
        public void RandomOpen(int speed)
        {
            if (ran.Next(5) == 0)
            {
                foreach (int i in SLOW_OPEN)
                {
                    Thread.Sleep(Math.Abs(i));

                    if (i > 0)
                        frame++;
                    else
                        frame--;

                    Draw();
                }
            }
            else
                Open(speed);
        }

        /// <summary>
        /// Plays the close animation at a given speed.
        /// </summary>
        /// <param name="speed">Time in milliseconds between frames</param>
        public void Close(int speed)
        {
            while (Frame > 0)
            {
                Thread.Sleep(speed);
                Frame--;
                Draw();
            }
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Renders the door to the screen
        /// </summary>
        public void Draw()
        {
            DrawBase();

            DrawPrize();

            DrawLeftTriangle(X + 1 - Frame, ConsoleColor.Red);
            DrawLeftTriangle(X + DOOR_HEIGHT / 2 + 3 - Frame, ConsoleColor.Yellow);

            DrawRightTriangle(Right - 1 + Frame, ConsoleColor.Red);
            DrawRightTriangle(Right - DOOR_HEIGHT / 2 - 3 + Frame, ConsoleColor.Yellow);

            DrawInnerDoor();
        }

        /// <summary>
        /// Clears the door and redraws the edges
        /// </summary>
        private void DrawBase() 
        {
            //Top edge
            if (drawTop)
                Painter.Write(DOOR_TOP, x, Y - 1, ConsoleColor.White);

            //Center
            for (int i = 0; i < DOOR_HEIGHT; i++)
            {
                Console.CursorLeft = X;
                Painter.Write(DOOR_BODY, X, Y + i, ConsoleColor.White);
            }

            //Bottom edge
            if (drawBottom)
                Painter.Write(DOOR_BOTTOM, x, Bottom + 1, ConsoleColor.White);
        }

        /// <summary>
        /// Draws the prize hidden behind the door
        /// </summary>
        private void DrawPrize()
        {
            //Only draws the prize if the door isn't closed
            if (frame > 0)
                prize.Draw(X, Y, (Frame - 1) * 2 + 1);
        }

        /// <summary>
        /// Draws a left-facing triangle.
        /// </summary>
        /// <param name="leftBound">The left-most edge of the triangle</param>
        /// <param name="color">The color to draw the triangle</param>
        private void DrawLeftTriangle(int leftBound, ConsoleColor color)
        {
            //instantly returns if the triangle won't fit inside the door
            if (leftBound + DOOR_HEIGHT / 2 + 2 <= X)
                return;

            //Top slope
            int xPos = leftBound + DOOR_HEIGHT / 2;
            int yPos = Y;

            for (int i = 0; i < DOOR_HEIGHT / 2; i++)
            {
                if (xPos > X)
                    Painter.Write('╱', xPos, yPos, color);

                xPos--;
                yPos++;
            }

            //Point
            if (xPos > X)
                Painter.Write('<', xPos, yPos, color);

            xPos++;
            yPos++;

            //Bottom Slope
            for (int i = 0; i < DOOR_HEIGHT / 2; i++)
            {
                if (xPos > X)
                    Painter.Write('╲', xPos, yPos, color);

                xPos++;
                yPos++;
            }

            //Right edge
            if (xPos > X)
            {
                for (int i = 0; i < DOOR_HEIGHT; i++)
                {
                    Painter.Write('│', xPos, Y + i, color);
                }
            }
        }

        /// <summary>
        /// Draws a right-facing triangle.
        /// </summary>
        /// <param name="rightBound">The right-most edge of the triangle</param>
        /// <param name="color">The color to draw the triangle</param>
        private void DrawRightTriangle(int rightBound, ConsoleColor color)
        {
            //Instantly returns if the triangle won't appear in the door
            if (rightBound - DOOR_HEIGHT / 2 - 2 >= Right)
                return;

            //Top slope
            int xPos = rightBound - DOOR_HEIGHT / 2;
            int yPos = Y;

            for (int i = 0; i < DOOR_HEIGHT / 2; i++)
            {
                if (xPos < Right)
                    Painter.Write('╲', xPos, yPos, color);

                xPos++;
                yPos++;
            }

            //Point
            if (xPos < Right)
                Painter.Write('>', xPos, yPos, color);

            xPos--;
            yPos++;

            //Bottom slope
            for (int i = 0; i < DOOR_HEIGHT / 2; i++)
            {
                if (xPos < Right)
                    Painter.Write('╱', xPos, yPos, color);

                xPos--;
                yPos++;
            }

            //Left edge
            if (xPos < Right)
            {
                for (int i = 0; i < DOOR_HEIGHT; i++)
                {
                    Painter.Write('│', xPos, Y + i, color);
                }
            }
        }

        /// <summary>
        /// Draws the inner edges of the door, along with the door's number.
        /// </summary>
        private void DrawInnerDoor()
        {
            //Instantly return if the door is completely open
            int xPos = X + DOOR_WIDTH / 2 - Frame;
            if (xPos <= X)
                return;

            //Left door's edge
            for (int i = 0; i < DOOR_HEIGHT; i++)
                Painter.Write('│', xPos, Y + i, ConsoleColor.White);

            //Number
            Painter.Write(Number.ToString(), xPos, Y + DOOR_HEIGHT / 2, ConsoleColor.White);

            //Only draws the right door's edge if the door isn't closed
            if (Frame > 0)
            {
                xPos += frame * 2;
                for (int i = 0; i < DOOR_HEIGHT; i++)
                    Painter.Write('│', xPos, Y + i, ConsoleColor.White);
            }
        }
        #endregion
    }
}
