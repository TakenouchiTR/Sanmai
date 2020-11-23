//Name:    Shawn Carter
//Date:    11/09/2020
//File:    Button.cs
//Purpose: This class displays a simple line of highlighted text, which will have a different
//           background color depending on whether the button is active or not.

using System;
using System.Text;
using FinalProject.IO;

namespace FinalProject.UI
{
    public class Button : UIObject
    {
        #region Fields
        private string text;
        private string hideString;
        #endregion

        #region Properties
        /// <summary>
        /// The text displayed in the button.
        /// </summary>
        public string Text => text;

        public override int Width => text.Length;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a button at a certain position
        /// </summary>
        /// <param name="text">Text to display</param>
        /// <param name="active">Whether the button is active or not</param>
        /// <param name="x">Left edge of the button</param>
        /// <param name="y">Top edge of the button</param>
        public Button(string text, bool active, int x, int y) : base(x, y, active)
        {
            this.text = text;

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
        public override void Toggle(bool redraw = true)
        {
            base.Toggle();

            if (redraw)
                Draw();
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Draws the button
        /// </summary>
        public override void Draw()
        {
            ConsoleColor backColor = Active ? ACTIVE_COLOR : INACTIVE_COLOR;

            Painter.Write(" " + text + " ", X, Y, ConsoleColor.Black, backColor);
        }

        /// <summary>
        /// Removes the button by covering it with white space
        /// </summary>
        public void Hide()
        {
            Painter.Write(hideString, X, Y);
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
    }
}
