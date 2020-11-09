//Name:    Shawn Carter
//Date:    11/09/2020
//File:    ImageBuffer.cs
//Purpose: This class allows the user to draw to multiple parts of an "image" before drawing the entire image to
//           the screen. The purpose is to reduce the number of Painter.Write() calls to allow for smoother multithread
//           drawing. Currently unused as it can only support a single color, which makes it very limited.

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
