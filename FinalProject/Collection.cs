//Name:    Shawn Carter
//Date:    11/09/2020
//File:    Collection.cs
//Purpose: This class tracks information about which prizes the user has and has not collected.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FinalProject
{
    public static class Collection
    {
        private static bool[] prizeStatus;
        private static Prize[] prizes;

        public static bool[] PrizeStatus => prizeStatus;
        public static Prize[] Prizes => prizes;

        /// <summary>
        /// Loads the status of a collection from a file.
        /// </summary>
        /// <param name="file">File path</param>
        public static void LoadCollectionFile(string file)
        {
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
    }
}
