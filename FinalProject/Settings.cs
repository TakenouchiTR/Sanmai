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
        public static event ChangeHandler ValueChanged;
        private static Dictionary<string, string> values;

        static Settings()
        {
            values = new Dictionary<string, string>();
        }

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

        public static string GetString(string key)
        {
            if (!values.ContainsKey(key))
                throw new Exception(key + " does not exist as a setting.");

            return values[key];
        }

        public static void SetValue(string key, string value)
        {
            values[key] = value;

            ValueChanged.Invoke(key);
        }

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
                    SetValue(split[0], split[1]);
                }
            }
        }

        public static void CreateDefaultFile(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(File.OpenWrite(fileName)))
            {
                writer.WriteLine("mute_key:m");
                writer.WriteLine("speed_up_key:]");
                writer.WriteLine("speed_down_key:[");
                writer.WriteLine("is_muted:false");
                writer.WriteLine("speed:50");
            }
        }

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
    }
}