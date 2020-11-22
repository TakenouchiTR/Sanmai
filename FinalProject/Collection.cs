//Name:    Shawn Carter
//Date:    11/09/2020
//File:    Collection.cs
//Purpose: This class tracks information about which prizes the user has and has not collected.

using System.IO;

namespace FinalProject
{
    public static class Collection
    {
        #region Class Fields
        private static bool[] prizeStatus;
        private static Prize[] prizes;
        #endregion

        #region Properties
        /// <summary>
        /// The list of bools representing which prizes have been collected
        /// </summary>
        public static bool[] PrizeStatus => prizeStatus;

        /// <summary>
        /// The list of prizes
        /// </summary>
        public static Prize[] Prizes => prizes;

        /// <summary>
        /// How long the list of prizes is.
        /// Returns 0 if the list is null.
        /// </summary>
        public static int Count => prizes != null ? prizes.Length : 0;
        #endregion

        #region Methods
        /// <summary>
        /// Loads all of the prize files (.prz) in a folder.
        /// Files are assigned to an index in the list depending on their file name.
        /// Files that have names that are num numbers, ids that are too large, or ids that are too small will be skipped.
        /// </summary>
        /// <param name="folder">Path to the directory</param>
        /// <param name="category">Category for the folder</param>
        public static void LoadPrizeFolder(string folder, PrizeCategory category)
        {
            string[] files = Directory.GetFiles(folder, "*.prz");
            if (files.Length == 0)
                return;

            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                int id;

                //Ensures that the filename is in the correct format
                if (!int.TryParse(fileName, out id))
                    continue;

                //Makes sure that the id is a valid value
                if (id >= Count || id < 0)
                    continue;

                Prize prize = Prize.FromFile(file, category, id);

                prizes[id] = prize;
            }
        }

        /// <summary>
        /// Loads the status of a collection from a file.
        /// Generates a new default file if it doesn't exist.
        /// </summary>
        /// <param name="file">File path</param>
        public static void LoadCollectionFile(string file)
        {
            //Generates a new file if the file doesn't exist
            if (!File.Exists(file))
                CreateCollectionFile(file, 30);

            using (BinaryReader reader = new BinaryReader(File.OpenRead(file)))
            {
                int count = reader.ReadInt32();
                int data = 0;
                prizes = new Prize[count];
                prizeStatus = new bool[count];

                int mask = 1;
                for (int i = 0; i < count; i++)
                {
                    //Reads four bytes at the start, and after four have been read.
                    if (i % 32 == 0)
                    {
                        data = reader.ReadInt32();
                        mask = 1;
                    }

                    //Sets the collection status of an ID depending on the current bit
                    prizeStatus[i] = (mask & data) != 0;
                    mask <<= 1;
                }
            }
        }

        /// <summary>
        /// Creates a default collection file.
        /// The file will default to having no collected prizes
        /// </summary>
        /// <param name="file">File path</param>
        /// <param name="count">Number of items in the collection</param>
        public static void CreateCollectionFile(string file, int count)
        {
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(file)))
            {
                writer.Write(count);
                for (int i = 0; i <= count / 32; i++)
                {
                    writer.Write(0);
                }
            }
        }

        /// <summary>
        /// Writes the current collection to a file
        /// </summary>
        /// <param name="file">File path</param>
        public static void WriteToFile(string file)
        {
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(file)))
            {
                writer.Write(prizes.Length);

                int data = 0;
                int mask = 1;
                for (int i = 0; i < prizes.Length; i++)
                {
                    if (prizeStatus[i])
                        data = (mask | data);
                    mask <<= 1;

                    //Writes the current data to the file after setting all the bits in the data int, or when the
                    //  collection has been looped through entirely.
                    if ((i + 1) % 32 == 0 || i == prizes.Length - 1)
                    {
                        writer.Write(data);
                        data = 0;
                        mask = 1;
                    }
                }
            }
        }
        #endregion
    }
}
