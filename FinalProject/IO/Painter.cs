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
        #region Class Fields
        //Key object for accessing the Write method's lock
        private static readonly Object KEY = new Object();

        private static ConsoleColor defaultBackColor = ConsoleColor.Black;
        private static ConsoleColor defaultFrontColor = ConsoleColor.White;
        #endregion

        #region Properties
        /// <summary>
        /// The default background color to use when one isn't specified.
        /// </summary>
        public static ConsoleColor DefaultBackColor 
        { 
            get => defaultBackColor; 
            set => defaultBackColor = value; 
        }

        /// <summary>
        /// The default text color to use when one isn't specified.
        /// </summary>
        public static ConsoleColor DefaultFrontColor 
        { 
            get => defaultFrontColor; 
            set => defaultFrontColor = value; 
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Writes a line of text at a given xy coordinate with the default colors.
        /// </summary>
        /// <param name="line">Text to display</param>
        /// <param name="x">0-indexed X coordinate</param>
        /// <param name="y">0-indexed Y coordinate</param>
        public static void Write(string line, int x, int y)
        {
            Write(line, x, y, DefaultFrontColor);
        }

        /// <summary>
        /// Writes a line of text at a given xy coordinate.
        /// Specifies the text color, but uses the default background color.
        /// </summary>
        /// <param name="line">Text to display</param>
        /// <param name="x">0-indexed X coordinate</param>
        /// <param name="y">0-indexed Y coordinate</param>
        /// <param name="frontColor">Text color</param>
        public static void Write(string line, int x, int y, ConsoleColor frontColor)
        {
            Write(line, x, y, frontColor, DefaultBackColor);
        }

        /// <summary>
        /// Writes a line of text at a given xy coordinate, using user-specified text and background colors.
        /// </summary>
        /// <param name="line">Text to display</param>
        /// <param name="x">0-indexed X coordinate</param>
        /// <param name="y">0-indexed Y coordinate</param>
        /// <param name="frontColor">Text color</param>
        /// <param name="backColor">Background color</param>
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

        /// <summary>
        /// Writes a character at a given xy coordinate. with the default colors.
        /// </summary>
        /// <param name="character">Character to display</param>
        /// <param name="x">0-indexed X coordinate</param>
        /// <param name="y">0-indexed Y coordinate</param>
        public static void Write(char character, int x, int y)
        {
            Write(character, x, y, DefaultFrontColor);
        }

        /// <summary>
        /// Writes a character at a given xy coordinate.
        /// Specifies the text color, but uses the default background color.
        /// </summary>
        /// <param name="character">Character to display</param>
        /// <param name="x">0-indexed X coordinate</param>
        /// <param name="y">0-indexed Y coordinate</param>
        /// <param name="frontColor">Text color</param>
        public static void Write(char character, int x, int y, ConsoleColor frontColor)
        {
            Write(character, x, y, frontColor, DefaultBackColor);
        }

        /// <summary>
        /// Writes a character at a given xy coordinate., using user-specified text and background colors.
        /// </summary>
        /// <param name="character">Character to display</param>
        /// <param name="x">0-indexed X coordinate</param>
        /// <param name="y">0-indexed Y coordinate</param>
        /// <param name="frontColor">Text color</param>
        /// <param name="backColor">Background color</param>
        public static void Write(char character, int x, int y, ConsoleColor frontColor, ConsoleColor backColor)
        {
            Write(character.ToString(), x, y, frontColor, backColor);
        }
        #endregion
    }
}
