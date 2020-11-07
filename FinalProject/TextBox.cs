using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FinalProject
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

        #region Draw Methods
        /// <summary>
        /// Draws the borders of the textbox.
        /// If used correctly, this should only be run once as nothing should draw over it.
        /// </summary>
        public void DrawBorder()
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;

            char[] borderSet = BORDER_PIECES[(int)borderType];

            //Top Edge
            Console.CursorLeft = x;
            Console.CursorTop = y;

            Console.Write(borderSet[(int)BorderPiece.TopLeft]);
            for (int i = 1; i < width - 1; i++)
                Console.Write(borderSet[(int)BorderPiece.Horizontal]);
            Console.Write(borderSet[(int)BorderPiece.TopRight]);

            //Bottom Edge
            Console.CursorLeft = x;
            Console.CursorTop = y + height - 1;

            Console.Write(borderSet[(int)BorderPiece.BottomLeft]);
            for (int i = 1; i < width - 1; i++)
                Console.Write(borderSet[(int)BorderPiece.Horizontal]);
            Console.Write(borderSet[(int)BorderPiece.BottomRight]);

            //Left Edge
            Console.CursorTop = y + 1;

            for (int i = 1; i < height - 1; i++)
            {
                Console.CursorLeft = x;
                Console.WriteLine(borderSet[(int)BorderPiece.Vertical]);
            }

            //Right Edge
            Console.CursorTop = y + 1;

            for (int i = 1; i < height - 1; i++)
            {
                Console.CursorLeft = Right;
                Console.WriteLine(borderSet[(int)BorderPiece.Vertical]);
            }

            Console.ForegroundColor = oldColor;
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
            if (text == null)
                return;

            //Saves the cursor position and color so that it can be reset later
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;

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
                if (textLine > height - 2)
                    return;

                //Repeatedly adds words to a line until everything is added, or there is no more room
                line.Append(words[index++]);
                while (index < words.Length && line.Length + words[index].Length < maxLength)
                    line.Append(" " + words[index++]);

                //Positions the text depending on text alignment
                Console.CursorTop = y + 1 + textLine;
                Console.CursorLeft = align == TextAlign.Left ? x + 1 :
                                     align == TextAlign.Center ? x + width / 2 - line.Length / 2 : Right - line.Length;

                //Writes the text to the screen
                if (writeTime == 0)
                    Console.WriteLine(line.ToString());
                else
                {
                    foreach (char c in line.ToString())
                    {
                        Console.Write(c);
                        Thread.Sleep(writeTime);
                    }
                    Console.WriteLine();
                }

                line.Clear();
                textLine++;
            }

            Console.ForegroundColor = oldColor;
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

            Console.CursorTop = y + 1;
            for (int i = 1; i < height - 1; i++)
            {
                Console.CursorLeft = x + 1;
                Console.WriteLine(sb.ToString());
            }

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
        DoubleLine
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
