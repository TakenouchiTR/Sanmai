using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FinalProject
{
    public class ImageBuffer
    {
        private char[][] buffer;

        public int Width => buffer[0].Length;
        public int Height => buffer.Length;

        public ImageBuffer(int width, int height)
        {
            buffer = new char[height][];

            for (int i = 0; i < height; i++)
                buffer[i] = new char[width];
        }


        public void DrawToBuffer(int x, int y, char character)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return;

            buffer[y][x] = character;
        }
        public void DrawToBuffer(int x, int y, string text)
        {
            if (y < 0 || y >= Height)
                return;

            for (int i = 0; i < text.Length; i++, x++)
            {
                if (x < 0)
                    continue;
                if (x >= Width)
                    return;

                buffer[y][x] = text[i];
            }
        }

        public void Draw(int x, int y, ConsoleColor color)
        {
            for (int i = 0; i < buffer.Length; i++)
                Painter.Write(new string(buffer[i]), x, y + i, color);
        }
    }
}
