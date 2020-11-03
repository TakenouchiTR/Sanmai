using System;
using System.Threading;

namespace FinalProject
{
    class Program
    {
        static void Main(string[] args)
        {
            int screenWidth = Console.WindowWidth;
            int padding = 2;
            int doorCount = 3;

            int doorX = screenWidth / 2 - Door.DOOR_WIDTH * doorCount / 2 - padding;
            int doorY = 2;
            Door[] doors = new Door[3];

            for (int i = 0; i < doors.Length; i++)
            {
                doors[i] = new Door(doorX, doorY, i + 1);
                doorX += Door.DOOR_WIDTH + padding;
            }

            foreach (Door d in doors)
                d.Draw();
        }
    }
}
