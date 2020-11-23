//Name:    Shawn Carter
//Date:    11/09/2020
//File:    SoundPlayer.cs
//Purpose: This class allows for a series of system sounds to be played in their own thread, preventing the
//           game from being halted while the sounds are playing.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FinalProject.IO
{
    public static class SoundPlayer
    {
        #region Class Fields
        private static bool isMuted;
        #endregion

        #region Methods
        public static void Initialize()
        {
            Settings.ValueChanged += Settings_ValueChanged;
            isMuted = false;
        }

        /// <summary>
        /// Event for when settings changed
        /// </summary>
        /// <param name="key">Key for the setting</param>
        private static void Settings_ValueChanged(string key)
        {
            if (key == "is_muted")
                isMuted = Settings.GetBool(key);
        }

        /// <summary>
        /// Plays a series of sounds in its own thread, allowing gameplay to continue.
        /// </summary>
        /// <param name="soundData">
        /// Series of sounds to play. Data should be constructed in groups of three, made up of (in order):
        /// n:   Sound frequency (hz)
        /// n+1: Sound duration (ms)
        /// n+2: Delay until next sound (ms)
        /// </param>
        public static void PlaySounds(int[] soundData)
        {
            //Returns early if the data array isn't a valid length, or if the game is muted.
            if (!isMuted || soundData.Length % 3 != 0 || soundData.Length == 0)
                return;

            //Runs the sound player in a thread, allowing long sets of sounds to play without
            // locking up the game.
            new Thread(new ThreadStart(() =>
            {
                for (int i = 0; i < soundData.Length; i += 3)
                {
                    Console.Beep(soundData[i], soundData[i + 1]);

                    //Sleeps if the delay is greater than 0ms
                    if (soundData[i + 2] > 0)
                        Thread.Sleep(soundData[i + 2]);
                }
            })).Start();
        }
        #endregion
    }
}
