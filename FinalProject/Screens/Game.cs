//Name:    Shawn Carter
//Date:    11/09/2020
//File:    Game.cs
//Purpose: This is the meat of the game. An instance of this class represents the primary gameplay loop,
//           and handles all of the gameplay and display logic.

using System;
using System.Threading;
using FinalProject.UI;
using FinalProject.IO;

namespace FinalProject.Screens
{
    public class Game : Screen
    {
        #region Constants
        private const ConsoleKey RIGHT_KEY = ConsoleKey.RightArrow;
        private const ConsoleKey LEFT_KEY = ConsoleKey.LeftArrow;
        private const ConsoleKey ENTER_KEY = ConsoleKey.Enter;
        private static Random ran = new Random();
        #endregion

        #region Fields
        private int activeDoor;
        private int selectedDoor;
        private int selectedYesNo;
        private TextBox textBox;
        private Door[] doors;
        private Button[] doorButtons;
        private Button[] yesNoButtons;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a game
        /// </summary>
        public Game()
        {
            //Sets fields to initial values
            activeDoor = 0;
            selectedDoor = -1;
            doors = new Door[3];
            doorButtons = new Button[3];
            yesNoButtons = new Button[2];

            //Does some calculations to reduce work later
            int screenWidth = Console.WindowWidth;
            int padding = 5;
            int doorCount = 3;
            int doorX = screenWidth / 2 - Door.DOOR_WIDTH * doorCount / 2 - padding;
            int doorY = 2;

            //Creates the doors
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i] = new Door(doorX, doorY, i + 1);
                doorX += Door.DOOR_WIDTH + padding;
            }

            //Creates a button for each door
            for (int i = 0; i < doorButtons.Length; i++)
            {
                doorButtons[i] = new Button("Door #" + (i + 1), i == 0, doors[i].X + Door.DOOR_WIDTH / 2 - 4, doors[i].Bottom + 4);
            }

            //Creates yes/no buttons
            yesNoButtons[0] = new Button("Yes", true, doors[0].Right + 1, doors[0].Bottom + 4);
            yesNoButtons[1] = new Button("No", false, doors[1].Right + 1, doors[0].Bottom + 4);

            //Creates the textbox
            textBox = new TextBox(doors[0].X + 2, doors[0].Bottom + 8, doors[2].Right - doors[0].X - 4, 7, BorderType.DoubleLine);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets up the game for first time play.
        /// Runs methods that cannot be run during creation as they affect the display.
        /// </summary>
        public override void Setup()
        {
            GeneratePrizes();


            foreach (Door d in doors)
                d.Frame = 0;

            //Does an initial render of each item
            foreach (Door d in doors)
                d.Draw();
            textBox.DrawBorder();
        }

        /// <summary>
        /// Resets the game back to its default state
        /// </summary>
        public void Reset()
        {
            GeneratePrizes();

            selectedDoor = -1;
            activeDoor = 0;

            //Makes sure that all the door buttons are set to their correct values
            if (!doorButtons[0].Active)
                doorButtons[0].Toggle(false);
            if (doorButtons[1].Active)
                doorButtons[1].Toggle(false);
            if (doorButtons[2].Active)
                doorButtons[2].Toggle(false);
        }

        /// <summary>
        /// Plays a full round of the game
        /// </summary>
        /// <returns>True if the player wishes to play another round or not, False if the player wish to stop</returns>
        public override bool Play()
        {
            //---Intro and first door selection---//
            ShowButtons(doorButtons);

            textBox.ClearText();
            textBox.WriteText("Welcome to Let's Make a Deal!", TextAlign.Center);
            textBox.WriteText("--------------------", TextAlign.Center);
            textBox.WriteText("Please use the left and right arrow keys to select a door. " +
                "Once you've chosen a door that you like, press Enter to open it. " +
                "If you don't like what you got, you'll be given a chance to pick another door!");

            Prize prize = SelectDoor();

            //---Prize display---//
            textBox.ClearText();
            textBox.WriteText("You got a " + prize.Name + "!");
            textBox.WriteText(prize.Description);
            textBox.WriteText("This prize is worth a grand total of " + prize.Price + "!");
            textBox.WriteText();
            textBox.WriteText("If you don't want this, you can try your luck at one of the other doors!");

            //---Ask if the user wants to choose another door---//
            HideButtons(doorButtons);
            ResetYesNo();
            ShowButtons(yesNoButtons);

            if (GetYesNo())
            {
                //---Second door---//
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

            //---Game finish---//
            textBox.ClearText();
            textBox.WriteText();
            textBox.WriteText("Congratulations! I hope that you enjoy your brand new " + prize.Name + "!", TextAlign.Center);
            textBox.WriteText();
            textBox.WriteText("Would you like so see all of your potential prizes?", TextAlign.Center);

            //---Ask if the user wants to see the other prizes---//
            HideButtons(doorButtons);
            ResetYesNo();
            ShowButtons(yesNoButtons);

            if (GetYesNo())
            {
                //---Show other prizes---//
                Thread[] threads = new Thread[doors.Length];
                for (int i = 0; i < doors.Length; i++)
                {
                    int index = i;
                    threads[index] = new Thread(() => doors[index].Open());
                    threads[index].Start();
                }

                foreach (Thread t in threads)
                    t.Join();

                textBox.ClearText();
                textBox.WriteText();
                textBox.WriteText("I hope you got exactly what you wanted! Thank you for playing!", TextAlign.Center);
            }
            else
            {
                //---Don't show other prizes---//
                ResetYesNo();
                textBox.ClearText();
                textBox.WriteText();
                textBox.WriteText("Not a problem! Thank you for playing!", TextAlign.Center);
            }

            //---New round prompt---//
            textBox.WriteText();
            textBox.WriteText("Would you like to try winning another fabulous prize?", TextAlign.Center);

            bool result = GetYesNo();

            ResetYesNo();
            HideButtons(yesNoButtons);

            if (result)
            {
                Thread[] threads = new Thread[doors.Length];
                for (int i = 0; i < doors.Length; i++)
                {
                    int index = i;
                    threads[index] = new Thread(() => doors[index].Close(25));
                    threads[index].Start();
                }

                foreach (Thread t in threads)
                    t.Join();
            }

            Collection.PrizeStatus[prize.ID] = true;

            return result;
        }

        /// <summary>
        /// Allows the user to select a door
        /// </summary>
        /// <returns>The prize behind the selected door</returns>
        private Prize SelectDoor()
        {
            int xPos = Console.CursorLeft;

            //Loops until the enter key is pressed
            while (true)
            {
                switch (Input.GetKey().Key)
                {
                    case LEFT_KEY:
                        doorButtons[activeDoor].Toggle();

                        //Moves the selected button left one, wrapping if it's at the start
                        // Skips a door if it was opened already
                        activeDoor = activeDoor == 0 ? doorButtons.Length - 1 : activeDoor - 1;
                        if (!doors[activeDoor].Closed)
                            activeDoor = activeDoor == 0 ? doorButtons.Length - 1 : activeDoor - 1;

                        doorButtons[activeDoor].Toggle();
                        break;

                    case RIGHT_KEY:
                        doorButtons[activeDoor].Toggle();

                        //Moves the selected button right one, wrapping if it's at the end
                        // Skips a door if it was opened already
                        activeDoor = (activeDoor + 1) % doorButtons.Length;
                        if (!doors[activeDoor].Closed)
                            activeDoor = (activeDoor + 1) % doorButtons.Length;

                        doorButtons[activeDoor].Toggle();
                        break;

                    case ENTER_KEY:
                        doorButtons[activeDoor].Toggle();

                        //Checks if a door was opened previously; closes it if true
                        if (selectedDoor != -1) 
                            doors[selectedDoor].Close();
                        doors[activeDoor].RandomOpen();

                        selectedDoor = activeDoor;

                        doors[activeDoor].Prize.PlaySound();
                        return doors[activeDoor].Prize;

                    default:
                        //Keeps the cursor from moving if the player presses a character key
                        Console.CursorLeft = xPos;
                        break;
                }
            }
        }

        /// <summary>
        /// Displays an array of buttons
        /// </summary>
        /// <param name="buttons">Buttons to display</param>
        private void ShowButtons(Button[] buttons)
        {
            foreach (Button b in buttons)
                b.Draw();
        }

        /// <summary>
        /// Hides an array of buttons
        /// </summary>
        /// <param name="buttons">Buttons to hide</param>
        private void HideButtons(Button[] buttons)
        {
            foreach (Button b in buttons)
                b.Hide();
        }

        /// <summary>
        /// Resets the yes/no buttons back to their default state
        /// </summary>
        private void ResetYesNo()
        {
            if (selectedYesNo == 1)
            {
                yesNoButtons[0].Toggle();
                yesNoButtons[1].Toggle();
                selectedYesNo = 0;
            }
        }

        /// <summary>
        /// Lets the user select either yes or no and returns the result
        /// </summary>
        /// <returns>True if the user selected yes, False if the user selected no</returns>
        private bool GetYesNo()
        {
            int xPos = Console.CursorLeft;

            //Loops until the enter key is pressed
            while (true)
            {
                switch (Input.GetKey().Key)
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
                        //Keeps the cursor from moving if the player presses a character key
                        Console.CursorLeft = xPos;
                        break;
                }
            }
        }

        /// <summary>
        /// Generates a set of random prizes and assigned them to the doors
        /// </summary>
        private void GeneratePrizes()
        {
            //Creates an array of three prizes, one from each category
            Prize[] prizes = new Prize[3];
            prizes[0] = Prize.RandomFromFolder("Prizes\\Expensive\\", PrizeCategory.Expensive);
            prizes[1] = Prize.RandomFromFolder("Prizes\\Middle\\", PrizeCategory.Middle);
            prizes[2] = Prize.RandomFromFolder("Prizes\\Zonk\\", PrizeCategory.Zonk);

            //Shuffles the prizes
            for (int i = 0; i < 10; i++)
            {
                int pos1 = ran.Next(0, 3);
                int pos2 = ran.Next(0, 3);

                Prize temp = prizes[pos1];
                prizes[pos1] = prizes[pos2];
                prizes[pos2] = temp;
            }

            //Assigns a prize to each door
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].Prize = prizes[i];
            }
        }
        #endregion
    }
}
