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
        #region Fields
        int width;
        int index;
        bool cycle;
        string clearString;
        string[] items;
        #endregion

        #region Properties
        /// <summary>
        /// The right side of the element
        /// </summary>
        public int Right => X + width - 1;

        public override int Width => width;

        /// <summary>
        /// How many items are in the spinner
        /// </summary>
        public int ItemCount => items.Length;

        /// <summary>
        /// The currently selected item
        /// </summary>
        public string SelectedItem => items[index];
        #endregion

        #region Constructors
        public Spinner(int x, int y, string[] items, bool cycle, int index = 0) : base(x, y, false)
        {
            this.items = items;
            this.index = index;
            this.cycle = cycle;

            foreach (string s in items)
                width = width > s.Length ? width : s.Length;

            width += 2;

            StringBuilder sb = new StringBuilder(width);
            for (int i = 0; i < width; i++)
                sb.Append(' ');

            clearString = sb.ToString();
        }
        #endregion

        #region Methods
        public override void Toggle(bool redraw = true)
        {
            base.Toggle();

            if (redraw)
                Draw();
        }

        /// <summary>
        /// Goes to the next item on the list of items. 
        /// If the spinner is at the end and it set to cycle, goes back to the beginning.
        /// Optionally redraws the spinner after chaning items.
        /// </summary>
        /// <param name="redraw">Optionally redraw the spinner</param>
        public void Next(bool redraw = true)
        {
            if (!cycle && index == ItemCount + 1)
                return;

            index = (index + 1) % ItemCount;

            if (redraw)
                Draw();
        }

        /// <summary>
        /// Goes to the previous item on the list of items. 
        /// If the spinner is at the beginning and it set to cycle, goes back to the end.
        /// Optionally redraws the spinner after chaning items.
        /// </summary>
        /// <param name="redraw">Optionally redraw the spinner</param>
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

        public override void Move(int x, int y, bool redraw = true)
        {
            if (redraw)
                Hide();

            base.Move(x, y);

            if (redraw)
                Draw();
        }
        #endregion

        #region Draw Methods
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

        /// <summary>
        /// Removes the item from the console output
        /// </summary>
        public void Hide()
        {
            Painter.Write(clearString, X, Y);
        }
        #endregion
    }
}
