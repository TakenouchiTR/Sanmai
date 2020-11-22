//Name:    Shawn Carter
//Date:    11/22/2020
//File:    Screen.cs
//Purpose: This class serves as the framework for the different screens in the game.

using System;

namespace FinalProject.Screens
{
    public abstract class Screen
    {
        public abstract void Setup();

        public abstract bool Play();

        /// <summary>
        /// Clears the Screen from the console.
        /// </summary>
        public void Hide() 
        {
            Console.Clear();
        }
    }
}
