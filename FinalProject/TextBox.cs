using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject
{
    public class TextBox
    {
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

        int x, y;
        int width, height;
        BorderType borderType;

        public int Right => x + width - 1;

        public TextBox(int x, int y, int width, int height, BorderType borderType)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.borderType = borderType;
        }

        public void DrawBorder()
        {
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
        }

        public void WriteText(string text)
        {
            string[] words = text.Split(" ");
        }
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
}
