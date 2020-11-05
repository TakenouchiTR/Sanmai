using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject
{
    public class TextBox
    {
        int x, y;
        int width, height;

        public TextBox(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public void WriteText(string text)
        {
            string[] words = text.Split(" ");

        }
    }
}
