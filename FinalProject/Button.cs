using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject
{
    public class Button
    {
        private static ConsoleColor ACTIVE_COLOR = ConsoleColor.Gray;
        private static ConsoleColor INACTIVE_COLOR = ConsoleColor.DarkGray;

        private bool active;
        private string text;
        private int x, y;

        public bool Active => active;
        public string Text => text;

        public Button(string text, bool active, int x, int y)
        {
            this.text = text;
            this.active = active;
            this.x = x;
            this.y = y;
        }

        public void Toggle(bool redraw = true)
        {
            active = !active;

            if (redraw)
                Draw();
        }

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
    }
}
