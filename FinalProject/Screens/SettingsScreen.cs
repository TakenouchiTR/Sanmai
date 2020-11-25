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
        private static readonly string[] DESCRIPTIONS = new string[]
        {
            "Changes the door speed; lower numbers are faster",
            "Toggles whether sounds are muted",
            "Toggles whether doors should have a chance to open slowly",
            "The hotkey to mute sounds while in game",
            "The hotkey to speed up the door while in game",
            "The hotkey to slow down the door while in game"
        };
        private static readonly string[] INSTRUCTIONS = new string[]
        {
            "Press the left or right arrow to change the value",
            "Press enter, then press any key to change the hotkey"
        };
        private static readonly Regex VALID_CHARS = new Regex(@"[a-z0-9,.\\\[\]/`*\-]");
        
        #endregion

        #region Fields
        private int index;
        private UIType prevUIType;
        private Spinner spn_speed;
        private Spinner spn_mute;
        private Spinner spn_slowOpen;
        private CharacterDisplay chr_mute;
        private CharacterDisplay chr_speedUp;
        private CharacterDisplay chr_speedDown;
        private TextBox textBox;

        private UIObject[] elements;
        #endregion

        #region Constructors
        public SettingsScreen()
        {
            index = 0;
            prevUIType = UIType.Spinner;

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

            textBox = new TextBox(Console.WindowWidth / 2 - 30, Console.WindowHeight - 10, 60, 8, BorderType.DashedHeavy);

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
            foreach (UIObject obj in elements)
                obj.Draw();

            for (int i = 0; i < LABELS.Length; i++)
                Painter.Write(LABELS[i], Console.WindowWidth / 2 - 13, 3 + i * 2, ConsoleColor.White);

            textBox.DrawBorder();
            textBox.WriteText("Settings", TextAlign.Center);
            textBox.WriteText("------------", TextAlign.Center);
            textBox.WriteText(DESCRIPTIONS[index], TextAlign.Center);
            textBox.WriteText();
            textBox.WriteText(INSTRUCTIONS[(int)prevUIType], TextAlign.Center);
            textBox.WriteText("Press esc to return to the title screen", TextAlign.Center);

            Console.SetCursorPosition(0, 0);

            while (true)
            {
                Console.SetCursorPosition(0, 0);
                ConsoleKeyInfo key = Input.GetKey(false);
                Spinner spn;

                switch (key.Key)
                {
                    //Selects the next item down
                    case ConsoleKey.DownArrow:
                        elements[index].Toggle();
                        index = (index + 1) % elements.Length;
                        elements[index].Toggle();

                        UpdateTextBox();
                        break;

                    //Selects the next item up
                    case ConsoleKey.UpArrow:
                        elements[index].Toggle();
                        index = index == 0 ? elements.Length - 1 : index - 1;
                        elements[index].Toggle();

                        UpdateTextBox();
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

        #region Draw Methods
        /// <summary>
        /// Updates the text in the text box whenever a new item is selected
        /// </summary>
        private void UpdateTextBox() 
        {
            UIType newType;

            //Gets the type of the currently selected element
            if (elements[index] is Spinner)
                newType = UIType.Spinner;
            else
                newType = UIType.CharacterDisplay;

            //Writes the description for the current setting
            textBox.ChangeLine(2);
            textBox.ClearLine();
            textBox.WriteText(DESCRIPTIONS[index], TextAlign.Center);

            //Updates the instructions if the element type changed
            if (newType != prevUIType) 
            {
                textBox.ChangeLine(4);
                textBox.ClearLine();
                textBox.WriteText(INSTRUCTIONS[(int)newType], TextAlign.Center);
                prevUIType = newType;
            }
        }

        public override void Hide()
        {
            base.Hide();

            textBox.ClearText();
        }
        #endregion
    }

    enum UIType
    {
        Spinner,
        CharacterDisplay
    }
}
