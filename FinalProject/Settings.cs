//Name:    Shawn Carter
//Date:    11/17/2020
//File:    Collection.cs
//Purpose: Stores a list of settings, stored as key-value pairs.

using System;
using System.Collections.Generic;
using System.IO;

namespace FinalProject
{
    public delegate void ChangeHandler(string key);
    public static class Settings
    {
        #region Events
        /// <summary>
        /// Occurs when a value for a key is either created or changed
        /// </summary>
        public static event ChangeHandler ValueChanged;
        #endregion

        #region Class Fields
        private static Dictionary<string, string> values;
        #endregion

        #region Static Constructor
        static Settings()
        {
            values = new Dictionary<string, string>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets an int value for a given key.
        /// Throws an exception if the value cannot be parsed into an int, or if the key does not exist.
        /// </summary>
        /// <param name="key">Name of the setting</param>
        /// <returns>Integer value of a key</returns>
        public static int GetInt(string key)
        {
            if (!values.ContainsKey(key))
                throw new Exception(key + " does not exist as a setting.");

            string value = values[key];
            int result;

            if (!int.TryParse(value, out result))
                throw new Exception("Value of " + value + " is not an int.");

            return result;
        }

        /// <summary>
        /// Gets a bool value for a given key.
        /// Throws an exception if the value cannot be parsed into an bool, or if the key does not exist.
        /// </summary>
        /// <param name="key">Name of the setting</param>
        /// <returns>Boolean value of a key</returns>
        public static bool GetBool(string key)
        {
            if (!values.ContainsKey(key))
                throw new Exception(key + " does not exist as a setting.");

            string value = values[key];
            bool result;

            if (!bool.TryParse(value, out result))
                throw new Exception("Value of " + value + " is not a bool.");

            return result;
        }

        /// <summary>
        /// Gets a char value for a given key.
        /// Throws an exception if the value cannot be parsed into an int, or if the key does not exist.
        /// </summary>
        /// <param name="key">Name of the setting</param>
        /// <returns>Character value of a key</returns>
        public static char GetChar(string key)
        {
            if (!values.ContainsKey(key))
                throw new Exception(key + " does not exist as a setting.");

            string value = values[key];
            char result;

            if (!char.TryParse(value, out result))
                throw new Exception("Value of " + value + " is not a char.");

            return result;
        }

        /// <summary>
        /// Gets a value for a key
        /// Throws an exception if the key does not exist.
        /// </summary>
        /// <param name="key">Name of the setting</param>
        /// <returns>Value for the key</returns>
        public static string GetString(string key)
        {
            if (!values.ContainsKey(key))
                throw new Exception(key + " does not exist as a setting.");

            return values[key];
        }

        /// <summary>
        /// Sets a value for a setting. Creates a new setting if doesn't exist already.
        /// </summary>
        /// <param name="key">Name of the setting</param>
        public static void SetValue(string key, string value)
        {
            values[key] = value;

            ValueChanged.Invoke(key);
        }

        /// <summary>
        /// Loads a settings file from a given path.
        /// If the file does not exist, a default file will be created, then loaded.
        /// </summary>
        /// <param name="fileName">Path to the file</param>
        public static void LoadSettingsFile(string fileName)
        {
            values.Clear();

            if (!File.Exists(fileName))
                CreateDefaultFile(fileName);

            using (StreamReader reader = new StreamReader(File.OpenRead(fileName)))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] split = line.Split(':');
                    if (split.Length != 2)
                        continue;
                    SetValue(split[0], split[1]);
                }
            }
        }

        /// <summary>
        /// Creates the default settings file at a location.
        /// </summary>
        /// <param name="fileName">Path to the file</param>
        public static void CreateDefaultFile(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(File.OpenWrite(fileName)))
            {
                writer.WriteLine("mute_key:m");
                writer.WriteLine("speed_up_key:]");
                writer.WriteLine("speed_down_key:[");
                writer.WriteLine("is_muted:False");
                writer.WriteLine("speed:50");
                writer.WriteLine("do_slow_open:True");
            }
        }

        /// <summary>
        /// Saves the setting to a file.
        /// </summary>
        /// <param name="fileName">Path to the file</param>
        public static void WriteToFile(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(File.OpenWrite(fileName)))
            {
                foreach (string s in values.Keys)
                {
                    writer.WriteLine("{0}:{1}", s, values[s]);
                }
            }
        }
        #endregion
    }
}