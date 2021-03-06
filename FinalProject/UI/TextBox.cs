﻿//Name:    Shawn Carter
//Date:    11/09/2020
//File:    TextBox.cs
//Purpose: This object allows for text to be easily written to a pre-defined area of the screen.
//           Textboxes have borders, which clearly displays the outer edges of the textbox.

using System;
using System.Text;
using System.Threading;
using FinalProject.IO;

namespace FinalProject.UI
{
    public class TextBox
    {
        #region Constants
        private static char[][] BORDER_PIECES =
        {
            new char[] { '│', '─', '┌', '┐', '└', '┘' },
            new char[] { '┃', '━', '┏', '┓', '┗', '┛' },
            new char[] { '┊', '┈', '┌', '┐', '└', '┘' },
            new char[] { '┋', '┉', '┏', '┓', '┗', '┛' },
            new char[] { '┆', '┄', '┌', '┐', '└', '┘' },
            new char[] { '┇', '┅', '┏', '┓', '┗', '┛' },
            new char[] { '║', '═', '╔', '╗', '╚', '╝' }
        };
        #endregion

        #region Fields
        int x, y;
        int width, height;
        int textLine;
        BorderType borderType;
        #endregion

        #region Properties
        public int Right => x + width - 1;
        public int Bottom => y + height - 1;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a textbox.
        /// The writable space is 2 less than the width or height, as the edges are used to draw the borders.
        /// </summary>
        /// <param name="x">Coordinate for the left edge</param>
        /// <param name="y">Coordinate for the top edge</param>
        /// <param name="width">Width of the textbox</param>
        /// <param name="height">Height of the textbox</param>
        /// <param name="borderType">Design of the textbox's borders</param>
        public TextBox(int x, int y, int width, int height, BorderType borderType)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.borderType = borderType;
            textLine = 0;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Moves the active line to the row specified.
        /// No action is taken if the line number is below 0 or grater than the number of rows.
        /// </summary>
        /// <param name="lineNum"></param>
        public void ChangeLine(int lineNum)
        {
            if (lineNum < 0 || lineNum >= height - 2)
                return;

            textLine = lineNum;
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Draws the borders of the textbox.
        /// If used correctly, this should only be run once as nothing should draw over it.
        /// </summary>
        public void DrawBorder()
        {
            if (borderType == BorderType.None)
                return;

            char[] borderSet = BORDER_PIECES[(int)borderType];

            //Corners
            Painter.Write(borderSet[(int)BorderPiece.TopLeft], x, y, ConsoleColor.White);
            Painter.Write(borderSet[(int)BorderPiece.TopRight], Right, y, ConsoleColor.White);
            Painter.Write(borderSet[(int)BorderPiece.BottomRight], Right, Bottom, ConsoleColor.White);
            Painter.Write(borderSet[(int)BorderPiece.BottomLeft], x, Bottom, ConsoleColor.White);

            //Top and Bottom Edges
            for (int i = x + 1; i < Right; i++)
            {
                Painter.Write(borderSet[(int)BorderPiece.Horizontal], i, y, ConsoleColor.White);
                Painter.Write(borderSet[(int)BorderPiece.Horizontal], i, Bottom, ConsoleColor.White);
            }

            //Left and Right Edges
            for (int i = y + 1; i < Bottom; i++)
            {
                Painter.Write(borderSet[(int)BorderPiece.Vertical], x, i, ConsoleColor.White);
                Painter.Write(borderSet[(int)BorderPiece.Vertical], Right, i, ConsoleColor.White);
            }
        }

        /// <summary>
        /// Draws a line in the textbox.
        /// If a word would go past the edge, then the word will wrap to the next line.
        /// If the textbox runs out of lines, no text will be written
        /// </summary>
        /// <param name="text">Line to write</param>
        /// <param name="align">Where to position the line</param>
        /// <param name="writeTime">How many milliseconds to spend writing each character</param>
        public void WriteText(string text, TextAlign align = TextAlign.Left, int writeTime = 0)
        {
            //Instantly returns if the text is null
            if (text == null || textLine >= height - 2)
                return;

            //Splits the text into words, and skips to the next line if there is nothing to display
            string[] words = text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0)
            {
                textLine++;
                return;
            }

            StringBuilder line = new StringBuilder();
            int maxLength = width - 2;
            int index = 0;

            while (index < words.Length)
            {
                //Stops if there is no room left to write
                if (textLine >= height - 2)
                    return;

                //Repeatedly adds words to a line until everything is added, or there is no more room
                line.Append(words[index++]);
                while (index < words.Length && line.Length + words[index].Length < maxLength)
                    line.Append(" " + words[index++]);

                //Positions the text depending on text alignment
                int yPos = y + 1 + textLine;
                int xPos = align == TextAlign.Left ? x + 1 :
                           align == TextAlign.Center ? x + width / 2 - line.Length / 2 : 
                           Right - line.Length;

                //Writes the text to the screen
                if (writeTime == 0)
                    Painter.Write(line.ToString(), xPos, yPos, ConsoleColor.White);
                else
                {
                    foreach (char c in line.ToString())
                    {
                        Painter.Write(c, xPos++, yPos, ConsoleColor.White);
                        Thread.Sleep(writeTime);
                    }
                }

                line.Clear();
                textLine++;
            }
        }

        /// <summary>
        /// Skips a line in the text box
        /// </summary>
        public void WriteText()
        {
            textLine++;
        }

        /// <summary>
        /// Clears all text from the box and resets the active line to the top
        /// </summary>
        public void ClearText()
        {
            StringBuilder sb = new StringBuilder(width - 2);
            for (int i = 1; i < width - 1; i++)
                sb.Append(" ");

            for (int i = y + 1; i < Bottom; i++)
                Painter.Write(sb.ToString(), x + 1, i);

            textLine = 0;
        }

        /// <summary>
        /// Clears a line of text without going to the next line
        /// </summary>
        public void ClearLine()
        {
            if (textLine >= height - 2)
                return;

            StringBuilder sb = new StringBuilder(width - 2);
            for (int i = 0; i < width - 2; i++)
                sb.Append(' ');

            Painter.Write(sb.ToString(), x + 1, y + 1 + textLine);
        }

        /// <summary>
        /// Hides the textbox from the screen, resetting the line that the text will draw to as well.
        /// </summary>
        public void Hide()
        {
            StringBuilder sb = new StringBuilder(width);
            for (int i = 0; i < width; i++)
                sb.Append(" ");

            for (int i = 0; i < height; i++)
                Painter.Write(sb.ToString(), x, y + i);

            textLine = 0;
        }
        #endregion
    }

    #region Enums
    public enum TextAlign
    {
        Left,
        Center,
        Right
    }

    public enum BorderType
    {
        SolidLight,
        SolidHeavy,
        DottedLight,
        DottedHeavy,
        DashedLight,
        DashedHeavy,
        DoubleLine,
        None
    }

    enum BorderPiece
    {
        Vertical,
        Horizontal,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
    #endregion
}
