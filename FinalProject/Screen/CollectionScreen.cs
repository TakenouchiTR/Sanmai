using System;
using FinalProject.UI;
using FinalProject.IO;

namespace FinalProject.Screen
{
    public class CollectionScreen
    {
        #region Fields
        private int id;
        private Door showcase;
        private TextBox textBox;
        #endregion

        #region Constructors
        public CollectionScreen()
        {
            showcase = new Door(Console.WindowWidth / 2 - Door.DOOR_WIDTH / 2, 4, 0);
            textBox = new TextBox(Console.WindowWidth / 2 - 30, Door.DOOR_HEIGHT + 9, 
                61, 9, BorderType.DoubleLine);
            id = 0;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Runs code that only needs to be run once to setup the screen
        /// </summary>
        public void Startup()
        {
            textBox.DrawBorder();

            id = 0;
            showcase.Prize = Collection.Prizes[id];
            if (!Collection.PrizeStatus[id])
                showcase.Frame = 0;

            showcase.Draw();
           
            ChangePrize(0);
        }

        /// <summary>
        /// Plays the screen until the user presses escape to quit.
        /// </summary>
        /// <returns></returns>
        public void Play()
        {
            while (true)
            {
                ConsoleKeyInfo key = Input.GetKey();

                switch (key.Key)
                {
                    case ConsoleKey.RightArrow:
                        ChangePrize(1);
                        break;
                    case ConsoleKey.LeftArrow:
                        ChangePrize(-1);
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Changes the door's prize index by a given amount
        /// </summary>
        /// <param name="amount">Amount to change the index by; can be positive or negative.</param>
        private void ChangePrize(int amount)
        {
            id += amount;
            bool drawn = false;

            //Keeps the index in the valid range
            if (id < 0)
                id += Collection.Count;
            else if (id >= Collection.Count)
                id -= Collection.Count;

            textBox.ClearText();

            //Checks if the showcase doors need to be open or closed.
            if (showcase.Closed && Collection.PrizeStatus[id])
            {
                showcase.Prize = Collection.Prizes[id];
                showcase.Open(25);
                drawn = true;
            }
            else if (showcase.Opened && !Collection.PrizeStatus[id])
            {
                showcase.Close(25);
            }

            showcase.Prize = Collection.Prizes[id];

            //Draws the showcase if the the player has both the previous and current prizes
            if (showcase.Opened && !drawn)
                showcase.Draw();

            //Draws text to the textbox
            textBox.WriteText("---" + (id + 1) + "---", TextAlign.Center);
            textBox.WriteText();

            if (showcase.Opened)
            {
                textBox.WriteText(showcase.Prize.Name);
                textBox.WriteText(showcase.Prize.Description);
                textBox.WriteText(showcase.Prize.Price);
            }
            else
            {
                textBox.WriteText("This prize has not been won yet.", TextAlign.Center);
            }
        }

        /// <summary>
        /// Removes the screen's elements from the console
        /// </summary>
        public void Hide()
        {
            //I got lazy and just cleared the entire screen.
            Console.Clear();
        }
        #endregion
    }
}
