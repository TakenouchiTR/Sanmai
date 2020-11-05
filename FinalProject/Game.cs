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

        private int activeDoor;
        private int selectedDoor;
        private TextBox textBox;
        private Door[] doors;
        private Button[] doorButtons;

        public Game()
        {
            activeDoor = 0;
            selectedDoor = -1;
            int screenWidth = Console.WindowWidth;
            int padding = 5;
            int doorCount = 3;

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
                doorButtons[i] = new Button("Door #" + (i + 1), i == 0, doors[i].X + Door.DOOR_WIDTH / 2 - 4, doors[i].Bottom + 4);
            }

            textBox = new TextBox(doors[0].X + 2, doors[0].Bottom + 8, doors[2].Right - doors[0].X - 4, 7, BorderType.DoubleLine);

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
            textBox.DrawBorder();

            Prize prize = SelectDoor();
            foreach (Button b in doorButtons)
                b.Hide();

            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                foreach (Button b in doorButtons)
                    b.Draw();
                activeDoor = (activeDoor + 1) % doorButtons.Length;
                doorButtons[activeDoor].Toggle();
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
                        doorButtons[activeDoor].Toggle();

                        activeDoor = activeDoor == 0 ? doorButtons.Length - 1 : activeDoor - 1;
                        if (!doors[activeDoor].Closed)
                            activeDoor = activeDoor == 0 ? doorButtons.Length - 1 : activeDoor - 1;

                        doorButtons[activeDoor].Toggle();
                        break;

                    case RIGHT_KEY:
                        doorButtons[activeDoor].Toggle();

                        activeDoor = (activeDoor + 1) % doorButtons.Length;
                        if (!doors[activeDoor].Closed)
                            activeDoor = (activeDoor + 1) % doorButtons.Length;

                        doorButtons[activeDoor].Toggle();
                        break;

                    case ENTER_KEY:
                        doorButtons[activeDoor].Toggle();
                        if (selectedDoor != -1)
                            doors[selectedDoor].Close(50);
                        doors[activeDoor].Open(50);
                        selectedDoor = activeDoor;
                        return doors[activeDoor].Prize;
                }
            }

            return null;
        }
    }
}
