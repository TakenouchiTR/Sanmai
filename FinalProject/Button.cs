using System;
using System.Collections.Generic;
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
            ConsoleColor oldBackColor = Console.BackgroundColor;
            ConsoleColor oldFrontColor = Console.ForegroundColor;

            Console.BackgroundColor = Active ? ACTIVE_COLOR : INACTIVE_COLOR;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.CursorLeft = x;
            Console.CursorTop = y;
            Console.Write(" " + text + " ");

            Console.BackgroundColor = oldBackColor;
            Console.ForegroundColor = oldFrontColor;
        }

        /// <summary>
        /// Removes the button by covering it with white space
        /// </summary>
        public void Hide()
        {
            ConsoleColor oldBackColor = Console.BackgroundColor;
            ConsoleColor oldFrontColor = Console.ForegroundColor;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.CursorLeft = x;
            Console.CursorTop = y;

            for (int i = 0; i < text.Length + 2; i++)
                Console.Write(" ");

            Console.BackgroundColor = oldBackColor;
            Console.ForegroundColor = oldFrontColor;
        }
        #endregion
    }
}
