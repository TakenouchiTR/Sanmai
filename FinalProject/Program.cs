using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace FinalProject
{
    class Program
    {
        //Code for preventing resizing, shamelessly taken from 
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

        static void Main(string[] args)
        {
            //Prevents the window from being reized, which will mess with the display
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);

            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Black;

            bool playing = true;
            Game game = new Game();

            game.Setup();
            
            while (playing)
            {
                playing = game.Play();

                if (playing)
                    game.Reset();
            }
        }
    }
}
