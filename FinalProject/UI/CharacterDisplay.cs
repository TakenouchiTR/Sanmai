using FinalProject.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject.UI
{
    public class CharacterDisplay : UIObject
    {
        private const ConsoleColor CHANGE_COLOR = ConsoleColor.Red;
        private const ConsoleColor NORMAL_COLOR = ConsoleColor.Black;

        char character;
        bool isChanging;

        public char Character => character;

        public override int Width => 3;

        public CharacterDisplay(int x, int y, char c) : base(x, y, false)
        {
            character = c;
            isChanging = false;
        }

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

        public void ToggleChanging(bool redraw = true)
        {
            isChanging = !isChanging;

            if (redraw)
                Draw();
        }

        public override void Draw()
        {
            ConsoleColor backColor = Active ? ACTIVE_COLOR : INACTIVE_COLOR;
            ConsoleColor foreColor = isChanging ? CHANGE_COLOR : NORMAL_COLOR;

            Painter.Write("[" + character + "]", X, Y, foreColor, backColor);
        }

        public void Hide()
        {
            Painter.Write("   ", X, Y);
        }

        public override void Move(int x, int y, bool redraw = true)
        {
            if (redraw)
                Hide();

            base.Move(x, y);

            if (redraw)
                Draw();
        }
    }
}
