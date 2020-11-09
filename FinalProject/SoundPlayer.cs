//Name:    Shawn Carter
//Date:    11/09/2020
//File:    SoundPlayer.cs
//Purpose: This class allows for a series of system sounds to be played in their own thread, preventing the
//           game from being halted while the sounds are playing.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FinalProject
{
    public static class SoundPlayer
    {
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
            if (soundData.Length % 3 != 0)
                return;

            new Thread(new ThreadStart(() =>
            {
                for (int i = 0; i < soundData.Length; i += 3)
                {
                    Console.Beep(soundData[i], soundData[i + 1]);
                    if (soundData[i + 2] != 0)
                        Thread.Sleep(soundData[i + 2]);
                }
            })).Start();
            
        }
    }
}
