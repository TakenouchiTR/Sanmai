using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FinalProject
{
    public class CollectionScreen
    {
        private int id;
        private Door showcase;
        private TextBox textBox;

        public CollectionScreen()
        {
            showcase = new Door(Console.WindowWidth / 2 - Door.DOOR_WIDTH / 2, 4, 0);
            textBox = new TextBox(Console.WindowWidth / 2 - 30, Door.DOOR_HEIGHT + 9, 
                61, 9, BorderType.DoubleLine);
            id = 0;
        }

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

        public bool Play()
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
                        return true;
                }
            }
        }

        private void ChangePrize(int amount)
        {
            id += amount;
            bool drawn = false;

            if (id < 0)
                id += Collection.Count;
            else if (id >= Collection.Count)
                id -= Collection.Count;

            textBox.ClearText();

            //Checks if the shocase doors need to be open or closed.
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

        public void Hide()
        {
            showcase.Hide();
            textBox.Hide();
            Console.Clear();
        }
    }
}
