//Name:    Shawn Carter
//Date:    11/23/2020
//File:    SettingsScreen.cs
//Purpose: This screen allows the user to change the game's settings.
//           Likely to not make it into the game since the settings are fairly limited

using FinalProject.IO;
using FinalProject.UI;
using System;
using System.Text.RegularExpressions;

namespace FinalProject.Screens
{
    public class SettingsScreen : Screen
    {
        #region Constants
        private static readonly string[] LABELS = new string[]
        {
            "Door Speed:",
            "Muted:",
            "Do Slow Openings:",
            "Mute Hotkey:",
            "Speed Up Hotkey:",
            "Speed Down Hotkey:"
        };
        private static readonly string[] KEYS = new string[]
        {
            "speed",
            "is_muted",
            "do_slow_open",
            "mute_key",
            "speed_up_key",
            "speed_down_key"
        };
        private static readonly Regex VALID_CHARS = new Regex(@"[a-z0-9,.\\\[\]/`*\-]");
        #endregion

        #region Fields
        private int index;
        private Spinner spn_speed;
        private Spinner spn_mute;
        private Spinner spn_slowOpen;
        private CharacterDisplay chr_mute;
        private CharacterDisplay chr_speedUp;
        private CharacterDisplay chr_speedDown;

        private UIObject[] elements;
        #endregion

        #region Constructors
        public SettingsScreen()
        {
            index = 0;

            spn_speed = new Spinner(0, 0, new string[]
            {
                "5",
                "10",
                "20",
                "30",
                "40",
                "50",
            }, false);
            spn_mute = new Spinner(0, 0, new string[] { "True", "False" }, true);
            spn_slowOpen = new Spinner(0, 0, new string[] { "True", "False" }, true);
            chr_mute = new CharacterDisplay(0, 0, 'm');
            chr_speedUp = new CharacterDisplay(0, 0, ']');
            chr_speedDown = new CharacterDisplay(0, 0, '[');

            elements = new UIObject[]
            {
                spn_speed,
                spn_mute,
                spn_slowOpen,
                chr_mute,
                chr_speedUp,
                chr_speedDown
            };

            for (int i = 0; i < elements.Length; i++)
            {
                int xPos = Console.WindowWidth / 2 + 13 - elements[i].Width;
                elements[i].Move(xPos, 3 + i * 2, false);
            }

            elements[0].Toggle(false);

            Console.SetCursorPosition(0, 0);
        }
        #endregion

        #region Methods
        public override void Setup()
        {
            spn_speed.SetIndexToItem(Settings.GetString("speed"), false);
            spn_mute.SetIndexToItem(Settings.GetString("is_muted"), false);
            spn_slowOpen.SetIndexToItem(Settings.GetString("do_slow_open"), false);
            chr_mute.ChangeCharacter(Settings.GetChar("mute_key"), false);
            chr_speedUp.ChangeCharacter(Settings.GetChar("speed_up_key"), false);
            chr_speedDown.ChangeCharacter(Settings.GetChar("speed_down_key"), false);
        }

        public override bool Play()
        {
            Painter.Write("Press esc to return to the title screen", Console.WindowWidth / 2 - 20,
                Console.WindowHeight - 2, ConsoleColor.White);

            foreach (UIObject obj in elements)
                obj.Draw();

            for (int i = 0; i < LABELS.Length; i++)
                Painter.Write(LABELS[i], Console.WindowWidth / 2 - 13, 3 + i * 2, ConsoleColor.White);

            Console.SetCursorPosition(0, 0);

            while (true)
            {
                ConsoleKeyInfo key = Input.GetKey(false);
                Spinner spn;

                switch (key.Key)
                {
                    //Selects the next item down
                    case ConsoleKey.DownArrow:
                        elements[index].Toggle();
                        index = (index + 1) % elements.Length;
                        elements[index].Toggle();
                        break;

                    //Selects the next item up
                    case ConsoleKey.UpArrow:
                        elements[index].Toggle();
                        index = index == 0 ? elements.Length - 1 : index - 1;
                        elements[index].Toggle();
                        break;

                    //If the player selects a Character Display, allow them to enter a new character
                    case ConsoleKey.Enter:
                        if (!(elements[index] is CharacterDisplay))
                            break;

                        CharacterDisplay cd = (CharacterDisplay)elements[index];
                        cd.ToggleChanging();

                        char c = ' ';

                        //Loops until the player enters a valid character (alphanumeric + most punctuation)
                        while (!VALID_CHARS.IsMatch(c.ToString()))
                            c = Input.GetKey().KeyChar;

                        cd.ToggleChanging(false);
                        cd.ChangeCharacter(c);

                        Settings.SetValue(KEYS[index], c.ToString());
                        break;

                    //Is a spinner is selected, goes to the spinner's previous item
                    case ConsoleKey.LeftArrow:
                        if (!(elements[index] is Spinner))
                            break;

                        spn = (Spinner)elements[index];

                        spn.Prev();

                        Settings.SetValue(KEYS[index], spn.SelectedItem);
                        break;

                    //If a spinner is selected, goes to the spinner's next item
                    case ConsoleKey.RightArrow:
                        if (!(elements[index] is Spinner))
                            break;

                        spn = (Spinner)elements[index];

                        spn.Next();

                        Settings.SetValue(KEYS[index], spn.SelectedItem);
                        break;

                    case ConsoleKey.Escape:
                        //This line fixes an issue with the title screen
                        //  It appears that using a non-character key causes issues with writing to the console
                        //  This line will be invisible, as the background and forground are both black.
                        Painter.Write("This fixes a problem", 0, 0);
                        return false;
                }
            }
        }
        #endregion;
    }
}
