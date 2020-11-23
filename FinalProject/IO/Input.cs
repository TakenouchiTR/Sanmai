//Name:    Shawn Carter
//Date:    11/17/2020
//File:    Input.cs
//Purpose: Serves as a way to handle all user input. -----------------------------Add more stuff later 

using System;

namespace FinalProject.IO
{
    public static class Input
    {
        #region Class Fields
        private static char muteKey;
        private static char speedUpKey;
        private static char speedDownKey;
        #endregion

        /// <summary>
        /// Initializes keys to their default values and sets an event listener for the Setting.ValueChanged event
        /// </summary>
        public static void Initialize()
        {
            muteKey = 'm';
            speedUpKey = ']';
            speedDownKey = '[';

            Settings.ValueChanged += Settings_ValueChanged;
        }

        /// <summary>
        /// Event for when settings changed
        /// </summary>
        /// <param name="key">Key for the setting</param>
        private static void Settings_ValueChanged(string key)
        {
            switch (key)
            {
                case "mute_key":
                    muteKey = Settings.GetChar(key);
                    break;
                case "speed_up_key":
                    speedUpKey = Settings.GetChar(key);
                    break;
                case "speed_down_key":
                    speedDownKey = Settings.GetChar(key);
                    break;
            }
        }

        /// <summary>
        /// Gets the next key press, checks if it matches a hotkey, then returns it
        /// </summary>
        /// <returns>KeyInfo of the key pressed</returns>
        public static ConsoleKeyInfo GetKey()
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;

            ConsoleKeyInfo key = Console.ReadKey();

            //Toggles whether the game is muted or not when the mute key is pressed.
            if (key.KeyChar == muteKey)
            {
                bool isMuted = !Settings.GetBool("is_muted");
                Settings.SetValue("is_muted", isMuted.ToString());
            }

            //Adjusts the door opening speed when the appropriate key is pressed.
            if (key.KeyChar == speedUpKey)
            {
                int speed = Settings.GetInt("speed");
                speed -= 5;
                if (speed < 5)
                    speed = 5;
                Settings.SetValue("speed", speed.ToString());
            }
            if (key.KeyChar == speedDownKey)
            {
                int speed = Settings.GetInt("speed");
                speed += 5;
                Settings.SetValue("speed", speed.ToString());
            }

            //Keeps the cursor in place so that characters don't write over existing UI
            Console.SetCursorPosition(x, y);
            return key;
        }
    }
}
