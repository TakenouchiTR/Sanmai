using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FinalProject
{
    public class Prize
    {
        #region Constants
        public const int DISPLAY_WIDTH = 27;
        public const int DISPLAY_HEIGHT = 9;
        #endregion

        #region Class Fields
        private static Random ran = new Random();
        #endregion

        #region Fields
        private string[] display;
        private string name;
        private string description;
        private string price;
        private PrizeCategory category;
        #endregion

        #region Properties
        public string Name => name;
        public string Description => description;
        public string Price => price;
        public PrizeCategory Category => category;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a dummy Prize.
        /// The constructor should not be used outside of testing purposes.
        /// </summary>
        public Prize()
        {
            display = new string[]
            {
                "This is a placeholder item ",
                "                           ",
                "If you are seeing this,    ",
                "please check your file path",
                "and make sure that it is   ",
                "written correctly.         ",
                "                           ",
                "And that you actually have ",
                "it set to copy the file.   "
            };
        }
        #endregion

        #region Factory Methods
        /// <summary>
        /// Loads a Prize from a file.
        /// </summary>
        /// <param name="file">Path to the prize file</param>
        /// <param name="category">Category of the prize</param>
        /// <returns>Prize generated from the file if successful; default prize if unsuccessful</returns>
        public static Prize FromFile(string file, PrizeCategory category)
        {
            try
            {
                Prize result = new Prize();
                using (StreamReader reader = new StreamReader(file))
                {
                    result.display = new string[DISPLAY_HEIGHT];
                    StringBuilder line = new StringBuilder();
                    for (int i = 0; i < DISPLAY_HEIGHT; i++)
                    {
                        line.Append(reader.ReadLine());

                        while (line.Length < DISPLAY_WIDTH)
                            line.Append(' ');

                        result.display[i] = line.ToString().Substring(0, DISPLAY_WIDTH);
                        line.Clear();
                    }

                    result.name = reader.ReadLine();
                    result.description = reader.ReadLine();
                    result.price = reader.ReadLine();
                    result.category = category;
                }

                return result;
            }
            catch
            {
                return new Prize();
            }
        }

        /// <summary>
        /// Loads a Prize from a random file in a folder.
        /// </summary>
        /// <param name="folder">Folder ONLY containing the prize files.</param>
        /// <param name="category">Category of the prize</param>
        /// <returns>Prize generated from the file if successful; default prize if unsuccessful</returns>
        public static Prize RandomFromFolder(string folder, PrizeCategory category)
        {
            try
            {
                string[] files = Directory.GetFiles(folder);

                string file = files[ran.Next(0, files.Length)];

                return FromFile(file, category);
            }
            catch
            {
                return new Prize();
            }
        }
        #endregion

        #region Draw Method
        /// <summary>
        /// Draws the prize at a location with a certain width.
        /// The image will draw from the center to match how doors open.
        /// </summary>
        /// <param name="x">Left-edge of the image</param>
        /// <param name="y">Top-edge of the image</param>
        /// <param name="width">Number of the center columns to draw</param>
        public void Draw(int x, int y, int width)
        {
            //Saves the cursor position and color so that it can be reset later
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;

            int xPos = x + DISPLAY_WIDTH / 2 - width / 2 + 1;

            Console.CursorTop = y;
            for (int i = 0; i < DISPLAY_HEIGHT; i++)
            {
                Console.CursorLeft = xPos;
                int length = display[i].Length;
                Console.WriteLine(display[i].Substring(DISPLAY_WIDTH / 2 - width / 2, width));
            }

            Console.ForegroundColor = oldColor;
        }
        #endregion
    }

    #region Enums
    public enum PrizeCategory
    {
        Expensive,
        Middle,
        Zonk
    }
    #endregion
}
