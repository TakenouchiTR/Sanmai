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
            showcase.Prize = Collection.Prizes[id];
            showcase.Draw();
            textBox.DrawBorder();
            if (Collection.PrizeStatus[id])
                showcase.Open(50);
        }

        public bool Play()
        {
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.LeftArrow) 
                {

                    id += key.Key == ConsoleKey.RightArrow ? 1 : -1;

                    if (id < 0)
                        id += Collection.Count;
                    else if (id >= Collection.Count)
                        id -= Collection.Count;

                    if (showcase.Closed && Collection.PrizeStatus[id])
                    {
                        showcase.Prize = Collection.Prizes[id];
                        showcase.Open(50);
                    }
                    else if (!showcase.Closed && !Collection.PrizeStatus[id])
                    {
                        showcase.Close(50);
                        showcase.Prize = Collection.Prizes[id];
                    }
                    else if (!showcase.Closed)
                    {
                        showcase.Prize = Collection.Prizes[id];
                        showcase.Draw();
                    }


                }
            }

            return false;
        }
    }
}
