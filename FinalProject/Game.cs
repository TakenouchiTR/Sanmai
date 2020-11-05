using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject
{
    public class Game
    {
        private const ConsoleKey RIGHT_KEY = ConsoleKey.RightArrow;
        private const ConsoleKey LEFT_KEY = ConsoleKey.LeftArrow;
        private const ConsoleKey ENTER_KEY = ConsoleKey.Enter;

        private int activeButton;
        private Door[] doors;
        private Button[] doorButtons;

        public Game()
        {
            int screenWidth = Console.WindowWidth;
            int padding = 5;
            int doorCount = 3;
            activeButton = 0;

            int doorX = screenWidth / 2 - Door.DOOR_WIDTH * doorCount / 2 - padding;
            int doorY = 2;
            doors = new Door[3];
            doorButtons = new Button[3];

            for (int i = 0; i < doors.Length; i++)
            {
                doors[i] = new Door(doorX, doorY, i + 1);
                doorX += Door.DOOR_WIDTH + padding;
            }

            for (int i = 0; i < doorButtons.Length; i++)
            {
                doorButtons[i] = new Button("Door #" + (i + 1), i == 0, doors[i].X + Door.DOOR_WIDTH / 2 - 4, doors[i].Bottom + 2);
            }

            doors[0].Prize = Prize.FromFile("Prizes\\Middle\\tv.txt");
            doors[1].Prize = Prize.FromFile("Prizes\\Expensive\\truck.txt");
            doors[2].Prize = Prize.FromFile("Prizes\\Zonk\\dog.txt");
        }

        public void Play()
        {
            //Does an initial render of each item
            foreach (Door d in doors)
                d.Draw();
            foreach (Button b in doorButtons)
                b.Draw();

            Prize prize = SelectDoor();
            foreach (Button b in doorButtons)
                b.Hide();

            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                foreach (Button b in doorButtons)
                    b.Draw();
                activeButton = (activeButton + 1) % doorButtons.Length;
                doorButtons[activeButton].Toggle();
                prize = SelectDoor();
            }
        }

        private Prize SelectDoor()
        {
            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case LEFT_KEY:
                        doorButtons[activeButton].Toggle();

                        activeButton = activeButton == 0 ? doorButtons.Length - 1 : activeButton - 1;
                        if (!doors[activeButton].Closed)
                            activeButton = activeButton == 0 ? doorButtons.Length - 1 : activeButton - 1;

                        doorButtons[activeButton].Toggle();
                        break;

                    case RIGHT_KEY:
                        doorButtons[activeButton].Toggle();

                        activeButton = (activeButton + 1) % doorButtons.Length;
                        if (!doors[activeButton].Closed)
                            activeButton = (activeButton + 1) % doorButtons.Length;

                        doorButtons[activeButton].Toggle();
                        break;

                    case ENTER_KEY:
                        doorButtons[activeButton].Toggle();
                        doors[activeButton].Open(50);
                        return doors[activeButton].Prize;
                }
            }

            return null;
        }
    }
}
