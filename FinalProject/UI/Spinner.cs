//Name:    Shawn Carter
//Date:    11/23/2020
//File:    Spinner.cs
//Purpose: The spinner contains an array of strings and allows the user to choose which item to select.

using FinalProject.IO;
using System;
using System.Text;

namespace FinalProject.UI
{
    public class Spinner : UIObject
    {
        int width;
        int index;
        bool cycle;
        string clearString;
        string[] items;

        public int Right => X + width - 1;

        public int ItemCount => items.Length;

        public string SelectedItem => items[index];

        public Spinner(int x, int y, string[] items, int index = 0) : base(x, y, false)
        {
            this.items = items;
            this.index = index;
            cycle = true;

            foreach (string s in items)
                width = width > s.Length ? width : s.Length;

            width += 2;

            StringBuilder sb = new StringBuilder(width);
            for (int i = 0; i < width; i++)
                sb.Append(' ');

            clearString = sb.ToString();
        }

        public override void Toggle(bool redraw = true)
        {
            base.Toggle();

            if (redraw)
                Draw();
        }

        public void Next(bool redraw = true)
        {
            if (!cycle && index == ItemCount + 1)
                return;

            index = (index + 1) % ItemCount;

            if (redraw)
                Draw();
        }

        public void Prev(bool redraw = true)
        {
            if (!cycle && index == 0)
                return;

            index--;
            if (index < 0)
                index = ItemCount - 1;

            if (redraw)
                Draw();
        }

        public void SetIndexToItem(string item, bool redraw = true)
        {
            for (int i = 0; i < ItemCount; i++)
            {
                if (items[i] == item) 
                {
                    index = i;
                    if (redraw)
                        Draw();
                    return;
                }
            }
        }

        public override void Draw()
        {
            ConsoleColor backColor = Active ? ACTIVE_COLOR : INACTIVE_COLOR;

            Painter.Write(clearString, X, Y, Painter.DefaultFrontColor, backColor);

            if (index > 0 || (index == 0 && cycle))
                Painter.Write('<', X, Y, ConsoleColor.Black, backColor);
            if (index < ItemCount - 1 || (index == ItemCount - 1 && cycle))
                Painter.Write('>', Right, Y, ConsoleColor.Black, backColor);

            int xPos = X + (width - 2) / 2 - SelectedItem.Length / 2 + 1;

            Painter.Write(SelectedItem, xPos, Y, ConsoleColor.Black, backColor);
        }

        public void Hide()
        {
            Painter.Write(clearString, X, Y);
        }
    }
}
