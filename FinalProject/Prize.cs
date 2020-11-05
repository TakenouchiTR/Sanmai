using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FinalProject
{
    public class Prize
    {
        public const int DISPLAY_WIDTH = 27;
        public const int DISPLAY_HEIGHT = 9;

        private string[] display;
        private string name;
        private string description;
        private string price;
        private PrizeCategory category;

        public Prize()
        {
            display = new string[]
            {
                "       _______             ",
                "      ╱ ______│            ",
                "                           ",
                "                         _ ",
                " ╱                        ╲",
                " │) _                ___ [│",
                " │_                       │",
                "  │ () │                   ",
                "   ╲__╱             ╲__╱   "
            };
        }

        public void Draw(int x, int y, int width)
        {
            int xPos = x + DISPLAY_WIDTH / 2 - width / 2 + 1;

            Console.CursorTop = y;
            for (int i = 0; i < DISPLAY_HEIGHT; i++)
            {
                Console.CursorLeft = xPos;
                Console.WriteLine(display[i].Substring(DISPLAY_WIDTH / 2 - width / 2, width));
            }
        }

        public static Prize FromFile(string file)
        {
            try
            {
                Prize result = new Prize();
                using (StreamReader reader = new StreamReader(file))
                {
                    result.display = new string[DISPLAY_HEIGHT];
                    for (int i = 0; i < DISPLAY_HEIGHT; i++)
                    {
                        result.display[i] = reader.ReadLine();
                    }

                    result.name = reader.ReadLine();
                    result.description = reader.ReadLine();
                    result.price = reader.ReadLine();
                }

                return result;
            }
            catch 
            {
                return new Prize();
            }
        }
    }

    public enum PrizeCategory
    {
        Expensive,
        Medium,
        Zonk
    }
}
