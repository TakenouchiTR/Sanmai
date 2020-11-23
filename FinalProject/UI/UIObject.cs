using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject.UI
{
    public abstract class UIObject
    {
        protected const ConsoleColor ACTIVE_COLOR = ConsoleColor.Gray;
        protected const ConsoleColor INACTIVE_COLOR = ConsoleColor.DarkGray;

        private bool active;
        private int x;
        private int y;

        /// <summary>
        /// Whether the UIObject is drawn with an active or inactive background.
        /// </summary>
        public bool Active => active;

        public int X => x;

        public int Y => y;

        protected UIObject(int x, int y, bool active)
        {
            this.x = x;
            this.y = y;
            this.active = active;
        }

        /// <summary>
        /// Toggles the UIObject between active and inactive backgrounds.
        /// </summary>
        /// <param name="redraw">Optional: Redraw the element</param>
        public virtual void Toggle(bool redraw = true)
        {
            active = !Active;
        }

        public abstract void Draw();
    }
}
