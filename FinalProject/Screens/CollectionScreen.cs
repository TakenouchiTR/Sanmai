using System;
using FinalProject.UI;
using FinalProject.IO;

namespace FinalProject.Screens
{
    public class CollectionScreen : Screen
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
        public override void Setup()
        {
            textBox.DrawBorder();
            Painter.Write("Press esc to return to the title screen", Console.WindowWidth / 2 - 20, 
                Console.WindowHeight - 2, ConsoleColor.White);

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
        public override bool Play()
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
                        //This line fixes an issue with the title screen
                        //  It appears that using a non-character key causes issues with writing to the console
                        //  This line will be invisible, as the background and forground are both black.
                        Painter.Write("This fixes a problem", 0, 0);
                        return false;
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
                showcase.Open();
                drawn = true;
            }
            else if (showcase.Opened && !Collection.PrizeStatus[id])
            {
                showcase.Close();
            }

            showcase.Prize = Collection.Prizes[id];

            //Draws the showcase if the the player has both the previous and current prizes
            if (showcase.Opened && !drawn)
                showcase.Draw();

            //Draws text to the textbox
            textBox.WriteText("---" + (id + 1) + "---", TextAlign.Center);

            if (showcase.Opened)
            {
                //Makes the first letter of the name upper case
                string name = char.ToUpper(showcase.Prize.Name[0]) + showcase.Prize.Name.Substring(1);

                textBox.WriteText("Prize: " + name);
                textBox.WriteText("Value: " + showcase.Prize.Price);
                textBox.WriteText();
                textBox.WriteText(showcase.Prize.Description);
            }
            else
            {
                textBox.WriteText("This prize has not been won yet.", TextAlign.Center);
            }
        }
        #endregion
    }
}
