//Name:    Shawn Carter
//Date:    11/09/2020
//File:    Title.cs
//Purpose: This class shows the title screen, allowing the user to choose between a handful of options.

using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject
{
    public class Title
    {
        private int activeButton;
        private Button[] buttons;

        public Title()
        {
            buttons = new Button[3];

            buttons[0] = new Button("Play", true, Console.WindowWidth / 2 - 15, Console.WindowHeight - 6);
            buttons[1] = new Button("Collection", false, Console.WindowWidth / 2 - 6, Console.WindowHeight - 6);
            buttons[2] = new Button("Exit", false, Console.WindowWidth / 2 + 9, Console.WindowHeight - 6);
        }

        public void Setup()
        {
            if (!buttons[0].Active)
                buttons[0].Toggle(false);
            if (buttons[1].Active)
                buttons[1].Toggle(false);
            if (buttons[2].Active)
                buttons[2].Toggle(false);

            activeButton = 0;
        }

        public int SelectOption()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;

            foreach (Button b in buttons)
                b.Draw();

            //Temprary fix while I figure out why a button doesn't draw properly.
            //Drawing it makes it appear correctly
            buttons[0].Draw();

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        buttons[activeButton].Toggle();
                        activeButton = activeButton == 0 ? buttons.Length - 1: activeButton - 1;
                        buttons[activeButton].Toggle();
                        break;
                    case ConsoleKey.RightArrow:
                        buttons[activeButton].Toggle();
                        activeButton = activeButton == buttons.Length - 1 ? 0: activeButton + 1;
                        buttons[activeButton].Toggle();
                        break;
                    case ConsoleKey.Enter:
                        return activeButton;
                }
            }
        }

        public void Hide()
        {
            Console.Clear();
        }
    }
}
