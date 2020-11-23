//Name:    Shawn Carter
//Date:    11/23/2020
//File:    CharacterDisplay.cs
//Purpose: This object displays a single character between square brackets

using FinalProject.IO;
using System;

namespace FinalProject.UI
{
    public class CharacterDisplay : UIObject
    {
        #region Constants
        private const ConsoleColor CHANGE_COLOR = ConsoleColor.Red;
        private const ConsoleColor NORMAL_COLOR = ConsoleColor.Black;
        #endregion

        #region Fields
        char character;
        bool isChanging;
        #endregion

        #region Properties
        /// <summary>
        /// Character that the element is displaying
        /// </summary>
        public char Character => character;

        public override int Width => 3;
        #endregion

        #region Constructors
        public CharacterDisplay(int x, int y, char c) : base(x, y, false)
        {
            character = c;
            isChanging = false;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Change the displayed character, then optionally redraw the element
        /// </summary>
        /// <param name="c">New character to display</param>
        /// <param name="redraw">Optionally redraw the CharacterDisplay</param>
        public void ChangeCharacter(char c, bool redraw = true)
        {
            character = c;
            if (redraw)
                Draw();
        }

        public override void Toggle(bool redraw = true)
        {
            base.Toggle();

            if (redraw)
                Draw();
        }

        /// <summary>
        /// Toggles between using the normal text color and the "changing" text color.
        /// Optionally redraws the element.
        /// </summary>
        /// <param name="redraw">Optionally redraw the CharacterDisplay</param>
        public void ToggleChanging(bool redraw = true)
        {
            isChanging = !isChanging;

            if (redraw)
                Draw();
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
            ConsoleColor foreColor = isChanging ? CHANGE_COLOR : NORMAL_COLOR;

            Painter.Write("[" + character + "]", X, Y, foreColor, backColor);
        }

        /// <summary>
        /// Removes the item from the console output
        /// </summary>
        public void Hide()
        {
            Painter.Write("   ", X, Y);
        }
        #endregion
    }
}
