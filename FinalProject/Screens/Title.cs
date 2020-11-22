//Name:    Shawn Carter
//Date:    11/09/2020
//File:    Title.cs
//Purpose: This class shows the title screen, allowing the user to choose between a handful of options.

using System;
using FinalProject.UI;
using FinalProject.IO;

namespace FinalProject.Screens
{
    public class Title
    {
        #region Class Fields
        //Title of the game, maybe I should put this in a file.
        private string[] SPLASH = new string[]
        {
            " .ooHHHoo.         dHHHHHb        HHHHb      HHH  HHHHb         dHHHH        dHHHHHb        HHHHHHHHHHHHH",
            "dHHHHHHHHHb       dHHH^HHHb       HHHHHb     HHH  HHHHHb       dHHHHH       dHHH^HHHb       HHHHHHHHHHHHH",
            "HHHP    HHH      dHH/   \\HHb      HHH HHb    HHH  HHH HHb     dHH HHH      dHH/   \\HHb           HHH     ",
            "HHH     \\HP      HHH     HHH      HHH HHHb   HHH  HHH HHH     HHH HHH      HHH     HHH           HHH     ",
            "VHHA            dHHH     HHHb     HHH  HHH   HHH  HHH HHHb   dHHH HHH     dHHH     HHHb          HHH     ",
            " \\HHH\\          HHHHHHHHHHHHH     HHH  \\HHb  HHH  HHH  HHH   HHH  HHH     HHHHHHHHHHHHH          HHH     ",
            "   \\HHHH       dHHHHHHHHHHHHHb    HHH   HHH  HHH  HHH  HHH   HHH  HHH    dHHHHHHHHHHHHHb         HHH     ",
            "     HHHHH     HHH         HHH    HHH   \\HHb HHH  HHH  HHHb dHHH  HHH    HHH         HHH         HHH     ",
            "       HHHH   dHH/         \\HHb   HHH    HHH HHH  HHH   HHH HHH   HHH   dHH/         \\HHb        HHH     ",
            "dHb     HHH   HHH           HHH   HHH    \\HHHHHH  HHH   HHHHHHH   HHH   HHH           HHH        HHH     ",
            "HHHb   dHHH  dHHH           HHHb  HHH     \\HHHHH  HHH   HHHHHHH   HHH  dHHH           HHHb       HHH     ",
            "\\HHHHHHHHH   HHHH           HHHH  HHH      HHHHH  HHH    HHHHH    HHH  HHHH           HHHH  HHHHHHHHHHHHH",
            " `\"\"\"\"\"\"'    \"\"\"\"           \"\"\"\"  \"\"\"      \"\"\"\"\"  \"\"\"    \"\"\"\"\"    \"\"\"  \"\"\"\"" +
            "           \"\"\"\"  \"\"\"\"\"\"\"\"\"\"\"\"\""
        };

        private int activeButton;
        private Button[] buttons;
        #endregion

        #region Constructor
        public Title()
        {
            buttons = new Button[3];

            buttons[0] = new Button("Play", true, Console.WindowWidth / 2 - 15, Console.WindowHeight - 2);
            buttons[1] = new Button("Collection", false, Console.WindowWidth / 2 - 6, Console.WindowHeight - 2);
            buttons[2] = new Button("Exit", false, Console.WindowWidth / 2 + 9, Console.WindowHeight - 2);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Runs code that only needs to be run once to setup the screen
        /// </summary>
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
        #endregion

        #region Draw Methods
        /// <summary>
        /// Plays the scene, allowing the player to choose an option
        /// </summary>
        /// <returns>Magic number representing the chosen option (Change to enum later)</returns>
        public int SelectOption()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;

            for (int i = 0; i < SPLASH.Length; i++)
                Painter.Write(SPLASH[i], Console.WindowWidth / 2 - SPLASH[i].Length / 2, 1 + i, ConsoleColor.White);

            foreach (Button b in buttons)
                b.Draw();

            //Temprary fix while I figure out why a button doesn't draw properly.
            //Drawing it makes it appear correctly
            buttons[0].Draw();

            while (true)
            {
                ConsoleKeyInfo key = Input.GetKey();

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

        /// <summary>
        /// Removes the screen from the console.
        /// </summary>
        public void Hide()
        {
            Console.Clear();
        }
        #endregion
    }
}
