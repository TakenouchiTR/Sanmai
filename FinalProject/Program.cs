//Name:    Shawn Carter
//Date:    11/09/2020
//File:    Program.cs
//Purpose: This class contains the entry point for the program handles the gamestate.

using System;
using System.Runtime.InteropServices;
using System.Text;
using FinalProject.Screens;
using FinalProject.IO;

namespace FinalProject
{
    class Program
    {
        #region C++ DLLs
        //Code for preventing window resizing, shamelessly taken from 
        //  https://stackoverflow.com/questions/32062219/c-sharp-is-there-a-way-to-make-a-fixed-height-width-console
        const int MF_BYCOMMAND = 0x00000000;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        //Code for running code when the window closes with the X, shamelessly taken from
        //  https://stackoverflow.com/questions/474679/capture-console-exit-c-sharp
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler closeHandler;
        #endregion

        static void Main(string[] args)
        {
            Initialize();

            bool playing = true;
            Game game = new Game();
            CollectionScreen collectionScreen = new CollectionScreen();
            
            while (playing)
            {
                Title title = new Title();
                title.Setup();
                int option = title.SelectOption();
                title.Hide();

                switch (option)
                {
                    case 0:
                        bool replay = true;
                        game.Setup();

                        while (replay)
                        {
                            replay = game.Play();

                            if (playing)
                                game.Reset();
                        }

                        game.Hide();
                        break;

                    case 1:
                        collectionScreen.Startup();
                        collectionScreen.Play();
                        collectionScreen.Hide();
                        break;
                    case 2:
                        playing = false;
                        break;
                }
            }

            Collection.WriteToFile("collection.txt");
        }

        /// <summary>
        /// Runs initialization code required to setup the game.
        /// </summary>
        private static void Initialize()
        {
            //Prevents the window from being reized, which will mess with the display
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);

            //Runs code when the window closes with the X. Used to save the collections file
            closeHandler += new EventHandler(CloseHandler);
            SetConsoleCtrlHandler(closeHandler, true);

            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;
            Painter.DefaultFrontColor = ConsoleColor.Black;

            SoundPlayer.Initialize();
            Input.Initialize();

            //Loads the files
            Settings.CreateDefaultFile("settings.txt");
            Settings.LoadSettingsFile("settings.txt");

            char c = Settings.GetChar("mute_key");

            Collection.LoadCollectionFile("collection.txt");
            for (int i = 0; i < Collection.Count; i++)
                Collection.Prizes[i] = Prize.FromFile("Prizes\\Zonk\\0.prz", PrizeCategory.Zonk, 0);

            Collection.LoadPrizeFolder("Prizes\\Zonk\\", PrizeCategory.Zonk);
            Collection.LoadPrizeFolder("Prizes\\Middle\\", PrizeCategory.Middle);
            Collection.LoadPrizeFolder("Prizes\\Expensive\\", PrizeCategory.Expensive);
        }

        /// <summary>
        /// Runs whenever the window is closed to make sure that the collection file is always saved.
        /// </summary>
        /// <param name="sig">The type of close event</param>
        /// <returns></returns>
        private static bool CloseHandler(CtrlType sig)
        {
            Collection.WriteToFile("collection.txt");

            switch (sig)
            {
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                default:
                    return false;
            }
        }
    }

    enum CtrlType
    {
        CTRL_C_EVENT = 0,
        CTRL_BREAK_EVENT = 1,
        CTRL_CLOSE_EVENT = 2,
        CTRL_LOGOFF_EVENT = 5,
        CTRL_SHUTDOWN_EVENT = 6
    }
}
