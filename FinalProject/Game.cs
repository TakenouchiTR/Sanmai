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
        private static Random ran = new Random();

        private int activeDoor;
        private int selectedDoor;
        private int selectedYesNo;
        private TextBox textBox;
        private Door[] doors;
        private Button[] doorButtons;
        private Button[] yesNoButtons;

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
            yesNoButtons = new Button[2];

            for (int i = 0; i < doors.Length; i++)
            {
                doors[i] = new Door(doorX, doorY, i + 1);
                doorX += Door.DOOR_WIDTH + padding;
            }

            for (int i = 0; i < doorButtons.Length; i++)
            {
                doorButtons[i] = new Button("Door #" + (i + 1), i == 0, doors[i].X + Door.DOOR_WIDTH / 2 - 4, doors[i].Bottom + 4);
            }

            yesNoButtons[0] = new Button("Yes", true, doors[0].Right + 1, doors[0].Bottom + 4);
            yesNoButtons[1] = new Button("No", false, doors[1].Right + 1, doors[0].Bottom + 4);

            textBox = new TextBox(doors[0].X + 2, doors[0].Bottom + 8, doors[2].Right - doors[0].X - 4, 7, BorderType.DoubleLine);
        }

        public void Setup()
        {
            GeneratePrizes();

            //Does an initial render of each item
            foreach (Door d in doors)
                d.Draw();
            textBox.DrawBorder();
        }

        public void Reset()
        {
            GeneratePrizes();

            selectedDoor = -1;
            activeDoor = 0;

            if (!doorButtons[0].Active)
                doorButtons[0].Toggle(false);
            if (doorButtons[1].Active)
                doorButtons[1].Toggle(false);
            if (doorButtons[2].Active)
                doorButtons[2].Toggle(false);
        }

        public bool Play()
        {
            ShowButtons(doorButtons);

            textBox.ClearText();
            textBox.WriteText("Welcome to Let's Make a Deal!", TextAlign.Center);
            textBox.WriteText("--------------------", TextAlign.Center);
            textBox.WriteText("Please use the left and right arrow keys to select a door. " +
                "Once you've chosen a door that you like, press Enter to open it. " +
                "If you don't like what you got, you'll be given a chance to pick another door!");

            Prize prize = SelectDoor();

            textBox.ClearText();
            textBox.WriteText("You got a " + prize.Name + "!");
            textBox.WriteText(prize.Description);
            textBox.WriteText("This prize is worth a grand total of " + prize.Price + "!");
            textBox.WriteText();
            textBox.WriteText("If you don't want this, you can try your luck at one of the other doors!");

            HideButtons(doorButtons);
            ResetYesNo();
            ShowButtons(yesNoButtons);

            if (GetYesNo())
            {
                HideButtons(yesNoButtons);
                ShowButtons(doorButtons);

                activeDoor = (activeDoor + 1) % doorButtons.Length;
                doorButtons[activeDoor].Toggle();

                textBox.ClearText();
                textBox.WriteText();
                textBox.WriteText("Take your time, this is your last chance at getting something good!", TextAlign.Center);

                prize = SelectDoor();

                textBox.ClearText();
                textBox.WriteText("This time, you got a " + prize.Name + "!");
                textBox.WriteText(prize.Description);
                textBox.WriteText("This prize is worth a grand total of " + prize.Price + "!");
                textBox.WriteText();
                textBox.WriteText("I hope you like this one better than the last one!");
            }

            textBox.ClearText();
            textBox.WriteText();
            textBox.WriteText("Congratulations! I hope that you enjoy your brand new " + prize.Name + "!", TextAlign.Center);
            textBox.WriteText();
            textBox.WriteText("Would you like so see all of your potential prizes?", TextAlign.Center);

            HideButtons(doorButtons);
            ResetYesNo();
            ShowButtons(yesNoButtons);

            if (GetYesNo())
            {
                foreach (Door d in doors)
                    if (d.Closed)
                        d.Open(25);

                textBox.ClearText();
                textBox.WriteText();
                textBox.WriteText("I hope you got exactly what you wanted! Thank you for playing!", TextAlign.Center);
            }
            else
            {
                ResetYesNo();
                textBox.ClearText();
                textBox.WriteText();
                textBox.WriteText("Not a problem! Thank you for playing!", TextAlign.Center);
            }

            textBox.WriteText();
            textBox.WriteText("Would you like to try winning another fabulous prize?", TextAlign.Center);

            bool result = GetYesNo();

            ResetYesNo();
            HideButtons(yesNoButtons);

            if (result)
            {
                foreach (Door d in doors)
                    if (!d.Closed)
                        d.Close(25);
            }

            return result;
        }

        private Prize SelectDoor()
        {
            int xPos = Console.CursorLeft;

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
                        doors[activeDoor].RandomOpen(50);
                        selectedDoor = activeDoor;
                        return doors[activeDoor].Prize;

                    default:
                        Console.CursorLeft = xPos;
                        break;
                }
            }
        }

        private void ShowButtons(Button[] buttons)
        {
            foreach (Button b in buttons)
                b.Draw();
        }

        private void HideButtons(Button[] buttons)
        {
            foreach (Button b in buttons)
                b.Hide();
        }

        private void ResetYesNo()
        {
            if (selectedYesNo == 1)
            {
                yesNoButtons[0].Toggle();
                yesNoButtons[1].Toggle();
                selectedYesNo = 0;
            }
        }

        private bool GetYesNo()
        {
            int xPos = Console.CursorLeft;

            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case LEFT_KEY:
                    case RIGHT_KEY:
                        yesNoButtons[selectedYesNo].Toggle();
                        selectedYesNo = selectedYesNo == 0 ? 1 : 0;
                        yesNoButtons[selectedYesNo].Toggle();
                        break;

                    case ENTER_KEY:
                        bool result = selectedYesNo == 0;
                        return result;

                    default:
                        Console.CursorLeft = xPos;
                        break;
                }
            }
        }

        private void GeneratePrizes()
        {
            Prize[] prizes = new Prize[3];
            prizes[0] = Prize.RandomFromFolder("Prizes\\Expensive\\", Prize.PrizeCategory.Expensive);
            prizes[1] = Prize.RandomFromFolder("Prizes\\Middle\\", Prize.PrizeCategory.Middle);
            prizes[2] = Prize.RandomFromFolder("Prizes\\Zonk\\", Prize.PrizeCategory.Zonk);

            for (int i = 0; i < 10; i++)
            {
                int pos1 = ran.Next(0, 3);
                int pos2 = ran.Next(0, 3);

                Prize temp = prizes[pos1];
                prizes[pos1] = prizes[pos2];
                prizes[pos2] = temp;
            }

            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].Prize = prizes[i];
            }
        }
    }
}
