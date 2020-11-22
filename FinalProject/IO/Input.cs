//Name:    Shawn Carter
//Date:    11/17/2020
//File:    Input.cs
//Purpose: Serves as a way to handle all user input. -----------------------------Add more stuff later 

using System;

namespace FinalProject.IO
{
    public static class Input
    {
        private static char muteKey, speedUpKey, speedDownKey;

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
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.KeyChar == muteKey)
                Settings.SetValue("is_muted", (!Settings.GetBool("is_muted")).ToString());
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

            return key;
        }
    }
}
