using FinalProject.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject.UI
{
    public class CharacterDisplay : UIObject
    {
        char character;

        public char Character => character;

        public CharacterDisplay(int x, int y, char c) : base(x, y, false)
        {
            character = c;
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

        public override void Draw()
        {
            ConsoleColor backColor = Active ? ACTIVE_COLOR : INACTIVE_COLOR;

            Painter.Write("[" + character + "]", X, Y, ConsoleColor.Black, backColor);
        }
    }
}
