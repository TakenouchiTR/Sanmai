using System;
using System.Text;
using System.Threading;

namespace FinalProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            int screenWidth = Console.WindowWidth;
            int padding = 4;
            int doorCount = 3;

            int doorX = screenWidth / 2 - Door.DOOR_WIDTH * doorCount / 2 - padding;
            int doorY = 2;
            Door[] doors = new Door[3];

            for (int i = 0; i < doors.Length; i++)
            {
                doors[i] = new Door(doorX, doorY, i + 1);
                doorX += Door.DOOR_WIDTH + padding;
            }

            doors[0].Prize = Prize.FromFile("Prizes\\Zonk\\dog.txt");

            foreach (Door d in doors)
                d.Draw();

            Console.CursorTop = doors[0].Y + Door.DOOR_HEIGHT + 2;


            Console.ReadLine();
            doors[0].Open(50);
            

            Console.ReadLine();
        }
    }
}
