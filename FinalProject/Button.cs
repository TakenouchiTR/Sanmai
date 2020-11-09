//Name:    Shawn Carter
//Date:    11/09/2020
//File:    Button.cs
//Purpose: This class displays a simple line of highlighted text, which will have a different
//           background color depending on whether the button is active or not.

using System;
using System.Text;

namespace FinalProject
{
    public class Button
    {
        #region Constants
        private static ConsoleColor ACTIVE_COLOR = ConsoleColor.Gray;
        private static ConsoleColor INACTIVE_COLOR = ConsoleColor.DarkGray;
        #endregion

        #region fields
        private bool active;
        private string text;
        private int x, y;
        private string hideString;
        #endregion

        #region Properties
        public bool Active => active;
        public string Text => text;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a button at a certain position
        /// </summary>
        /// <param name="text">Text to display</param>
        /// <param name="active">Whether the button is active or not</param>
        /// <param name="x">Left edge of the button</param>
        /// <param name="y">Top edge of the button</param>
        public Button(string text, bool active, int x, int y)
        {
            this.text = text;
            this.active = active;
            this.x = x;
            this.y = y;

            StringBuilder sb = new StringBuilder(text.Length + 2);
            for (int i = 0; i < text.Length + 2; i++)
                sb.Append(" ");
            hideString = sb.ToString();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Toggles the button between active and inactive
        /// </summary>
        /// <param name="redraw">Whether to automatically redraw the button.</param>
        public void Toggle(bool redraw = true)
        {
            active = !active;

            if (redraw)
                Draw();
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Draws the button
        /// </summary>
        public void Draw()
        {
            ConsoleColor backColor = Active ? ACTIVE_COLOR : INACTIVE_COLOR;

            Painter.Write(" " + text + " ", x, y, ConsoleColor.Black, backColor);
        }

        /// <summary>
        /// Removes the button by covering it with white space
        /// </summary>
        public void Hide()
        {
            Painter.Write(hideString, x, y);
        }
        #endregion
    }
}
