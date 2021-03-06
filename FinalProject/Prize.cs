﻿//Name:    Shawn Carter
//Date:    11/09/2020
//File:    Prize.cs
//Purpose: This object represents a prize that the player can win. All information regarding the prize, such as it's name, 
//           category, etc. are stored within the object.

using System;
using System.IO;
using System.Text;
using FinalProject.IO;

namespace FinalProject
{
    public class Prize
    {
        #region Constants
        public const int DISPLAY_WIDTH = 27;
        public const int DISPLAY_HEIGHT = 9;
        public readonly int[][] SOUNDS = new int[][]
        {
            new int[]
            {
                2000, 75, 0,
                1500, 75, 0,
                2000, 75, 0,
                1500, 75, 0,
            },
            new int[]
            {
                750, 200, 0,
                750, 200, 0,
                750, 200, 0
            },
            new int[] { 300, 700, 0 }
        };
        #endregion

        #region Class Fields
        private static Random ran = new Random();
        #endregion

        #region Fields
        private int id;
        private string[] display;
        private string name;
        private string description;
        private string price;
        private PrizeCategory category;
        #endregion

        #region Properties
        /// <summary>
        /// Name of the prize
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Description of the prize
        /// </summary>
        public string Description => description;

        /// <summary>
        /// Price of the prize
        /// </summary>
        public string Price => price;

        /// <summary>
        /// The category of the prize
        /// </summary>
        public PrizeCategory Category => category;
        public int ID => id;
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
        public static Prize FromFile(string file, PrizeCategory category, int id)
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
                    result.id = id;
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
        /// Prize files must have the extension .prz.
        /// </summary>
        /// <param name="folder">Folder cotaining .prz files.</param>
        /// <param name="category">Category of the prize</param>
        /// <returns>Prize generated from the file if successful; default prize if unsuccessful</returns>
        public static Prize RandomFromFolder(string folder, PrizeCategory category)
        {
            try
            {
                string[] files = Directory.GetFiles(folder, "*.prz");
                if (files.Length == 0)
                    return new Prize();

                string file = files[ran.Next(0, files.Length)];

                return FromFile(file, category, int.Parse(Path.GetFileNameWithoutExtension(file)));
            }
            catch
            {
                return new Prize();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Plays the sound associated with the prize's category.
        /// </summary>
        public void PlaySound()
        {
            SoundPlayer.PlaySounds(SOUNDS[(int)Category]);
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
            int xPos = x + DISPLAY_WIDTH / 2 - width / 2 + 1;

            for (int i = 0; i < DISPLAY_HEIGHT; i++)
            {
                string substring = display[i].Substring(DISPLAY_WIDTH / 2 - width / 2, width);
                Painter.Write(substring, xPos, y + i, ConsoleColor.White);
            }
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
