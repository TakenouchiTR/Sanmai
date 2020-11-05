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
        int textLine;
        BorderType borderType;

        public int Right => x + width - 1;

        public TextBox(int x, int y, int width, int height, BorderType borderType)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.borderType = borderType;
            textLine = 0;
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

        public void WriteText(string text, TextAlign align = TextAlign.Left)
        {
            if (text == null)
                return;

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
                if (textLine > height - 2)
                    return;

                line.Append(words[index++]);

                while (index < words.Length && line.Length + words[index].Length < maxLength)
                    line.Append(" " + words[index++]);

                Console.CursorTop = y + 1 + textLine;
                Console.CursorLeft = align == TextAlign.Left ? x + 1 :
                                     align == TextAlign.Center ? x + width / 2 - line.Length / 2 : Right - line.Length;

                Console.WriteLine(line.ToString());

                line.Clear();
                textLine++;
            }
        }

        public void WriteText()
        {
            textLine++;
        }

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
    }

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
}
