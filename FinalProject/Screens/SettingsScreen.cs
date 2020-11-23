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
        private static readonly string[] LABELS = new string[]
        {
            "Door Speed:",
            "Muted:",
            "Do Slow Openings:",
            "Mute Hotkey:",
            "Speed Up Hotkey: ",
            "Speed Down Hotkey: "
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

        private int index;
        private Spinner spn_speed;
        private Spinner spn_mute;
        private Spinner spn_slowOpen;
        private CharacterDisplay chr_mute;
        private CharacterDisplay chr_speedUp;
        private CharacterDisplay chr_speedDown;

        private UIObject[] elements;

        public SettingsScreen()
        {
            index = 0;

            spn_speed = new Spinner(20, 1, new string[]
            {
                "5",
                "10",
                "20",
                "30",
                "40",
                "50",
            });
            spn_mute = new Spinner(20, 3, new string[] { "True", "False" });
            spn_slowOpen = new Spinner(20, 5, new string[] { "True", "False" });
            chr_mute = new CharacterDisplay(20, 7, 'm');
            chr_speedUp = new CharacterDisplay(20, 9, ']');
            chr_speedDown = new CharacterDisplay(20, 11, '[');

            elements = new UIObject[]
            {
                spn_speed,
                spn_mute,
                spn_slowOpen,
                chr_mute,
                chr_speedUp,
                chr_speedDown
            };

            elements[0].Toggle(false);

            Console.SetCursorPosition(0, 0);
        }

        public override void Setup()
        {
            spn_speed.SetIndexToItem(Settings.GetString("speed"), false);
            spn_mute.SetIndexToItem(Settings.GetString("is_muted"), false);
            spn_mute.SetIndexToItem(Settings.GetString("do_slow_open"), false);
            chr_mute.ChangeCharacter(Settings.GetChar("mute_key"), false);
            chr_speedUp.ChangeCharacter(Settings.GetChar("speed_up_key"), false);
            chr_speedDown.ChangeCharacter(Settings.GetChar("speed_down_key"), false);
        }

        public override bool Play()
        {
            foreach (UIObject obj in elements)
                obj.Draw();

            for (int i = 0; i < LABELS.Length; i++)
                Painter.Write(LABELS[i], 1, 1 + i * 2, ConsoleColor.White);

            Console.SetCursorPosition(0, 0);

            while (true)
            {
                ConsoleKeyInfo key = Input.GetKey();
                Spinner spn;

                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        elements[index].Toggle();
                        index = (index + 1) % elements.Length;
                        elements[index].Toggle();
                        break;

                    case ConsoleKey.UpArrow:
                        elements[index].Toggle();
                        index = index == 0 ? elements.Length - 1 : index - 1;
                        elements[index].Toggle();
                        break;

                    case ConsoleKey.Enter:
                        if (!(elements[index] is CharacterDisplay))
                            break;

                        CharacterDisplay cd = (CharacterDisplay)elements[index];
                        cd.ToggleChanging();

                        char c = ' ';

                        while (!VALID_CHARS.IsMatch(c.ToString()))
                            c = Input.GetKey().KeyChar;

                        cd.ToggleChanging(false);
                        cd.ChangeCharacter(c);

                        Settings.SetValue(KEYS[index], c.ToString());
                        break;

                    case ConsoleKey.LeftArrow:
                        if (!(elements[index] is Spinner))
                            break;

                        spn = (Spinner)elements[index];

                        spn.Prev();

                        Settings.SetValue(KEYS[index], spn.SelectedItem);
                        break;

                    case ConsoleKey.RightArrow:
                        if (!(elements[index] is Spinner))
                            break;

                        spn = (Spinner)elements[index];

                        spn.Next();

                        Settings.SetValue(KEYS[index], spn.SelectedItem);
                        break;
                }
            }
            
            return true;
        }
    }
}
