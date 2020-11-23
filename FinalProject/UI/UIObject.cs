//Name:    Shawn Carter
//Date:    11/23/2020
//File:    UIObject.cs
//Purpose: This class hold common members for various UI elements

using System;

namespace FinalProject.UI
{
    public abstract class UIObject
    {
        #region Constants
        protected const ConsoleColor ACTIVE_COLOR = ConsoleColor.Gray;
        protected const ConsoleColor INACTIVE_COLOR = ConsoleColor.DarkGray;
        #endregion

        #region Fields
        private bool active;
        private int x;
        private int y;
        #endregion

        #region Properties
        /// <summary>
        /// Whether the element is drawn with an active or inactive background.
        /// </summary>
        public bool Active => active;

        /// <summary>
        /// The left side of the element
        /// </summary>
        public int X => x;

        /// <summary>
        /// The right side of the element
        /// </summary>
        public int Y => y;

        /// <summary>
        /// How wide the element is
        /// </summary>
        public abstract int Width 
        {
            get;
        }
        #endregion

        #region Constructors
        protected UIObject(int x, int y, bool active)
        {
            this.x = x;
            this.y = y;
            this.active = active;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Toggles the UIObject between active and inactive backgrounds.
        /// </summary>
        /// <param name="redraw">Optional: Redraw the element</param>
        public virtual void Toggle(bool redraw = true)
        {
            active = !Active;
        }

        #endregion

        #region Draw Methods
        /// <summary>
        /// Draws the element to the screen
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Moves the element to a new position, then optionally redraws it.
        /// </summary>
        /// <param name="x">New X position</param>
        /// <param name="y">New Y position</param>
        /// <param name="redraw">optionally redraw the element</param>
        public virtual void Move(int x, int y, bool redraw = true)
        {
            this.x = x;
            this.y = y;
        }
        #endregion;
    }
}
