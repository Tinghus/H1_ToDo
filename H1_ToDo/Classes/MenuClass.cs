using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace H1_ToDo.Classes
{
    internal class MenuClass
    {
        public ToDoClass ToDo { get; set; }
        public ViewType CurrentViewType { get; set; }
        public bool ShowDone { get; set; }
        ToDoClass.ToDoObject? currentToDo { get; set; }
        public int currentlySelectedLine { get; set; }

        public enum ViewType
        {
            MainView,
            Detailview,
            CreateView,
            EditView,
            DeleteView
        }


        public void ShowMenu()
        {
            Console.SetCursorPosition(0, 0);
            Console.Clear();

            switch (CurrentViewType)
            {
                case ViewType.MainView:
                    string filterText = ShowDone ? "off" : "on";

                    if (currentToDo == null && ToDo.ToDoList.Count > 0)
                    {
                        currentToDo = ToDo.ToDoList[currentlySelectedLine];
                    }

                    if (currentToDo != null && currentToDo.TaskIsDone == false)
                    {
                        Console.WriteLine($"Shortcuts | (f)ilter[{filterText}] | (c)reate new |    (m)ark as done    | (e)dit | (d)elete | (q)uit |");
                    }
                    else
                    {
                        Console.WriteLine($"Shortcuts | (f)ilter[{filterText}] | (c)reate new | (m)ark as incomplete | (e)dit | (d)elete | (q)uit |");
                    }
                    break;

                case ViewType.Detailview:
                    if (currentToDo.TaskIsDone)
                    {
                        Console.WriteLine("Shortcuts | (b)ack | (m)ark as incomplete | (e)dit | (d)elete | (q)uit |");
                    }
                    else
                    {
                        Console.WriteLine("Shortcuts | (b)ack | (m)ark as done | (e)dit | (d)elete | (q)uit |");
                    }
                    break;

                case ViewType.CreateView:
                    Console.WriteLine("Shortcuts | (b)ack | (q)uit |");
                    break;

                case ViewType.DeleteView:
                    Console.WriteLine("Shortcuts | (b)ack | (q)uit |");
                    break;

            }

            AddLine();
            Console.WriteLine();
            LoadViewContext(CurrentViewType);
        }

        public void LoadViewContext(ViewType currentContext)
        {
            string output = "";

            if (currentContext == ViewType.MainView)
            {
                Console.Write("Id".PadRight(5) + "Deadline".PadRight(12) + "Title".PadRight(30) + "Priority".PadRight(12) + "Done" + "\n");


                if (ToDo.ToDoList.Count == 0)
                {
                    return;
                }


                for (int i = 0; i < ToDo.ToDoList.Count; i++)
                {
                    ToDoClass.ToDoObject toDo = ToDo.ToDoList[i];

                    if (ShowDone != true && toDo.TaskIsDone)
                    {
                        continue;
                    }

                    ApplyHoverEffect(i); // Highlights the currently selected task for the user

                    // If the deadline have passed and task is not completed we change the text to red
                    if (toDo.ToDoDate < DateTime.Now.Date && toDo.TaskIsDone == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    // If task is done we set the text to green
                    if (toDo.TaskIsDone == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }

                    // Output the general task information to the console
                    output = $"{toDo.Id.ToString().PadRight(5)}{toDo.ToDoDate.ToShortDateString().PadRight(12)}{toDo.Title.PadRight(30)}{toDo.Priority.PadRight(12)}{toDo.TaskIsDone}";
                    Console.WriteLine(output);

                    // Sets the console colors. Could also be done with Reset.
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                return;
            }

            if (currentContext == ViewType.EditView)
            {
                Console.WriteLine("Original:");

                output =
                    "Id:".PadRight(30) + $"{currentToDo.Id.ToString()}\n" +
                    "Created:".PadRight(30) + currentToDo.CreatedDateTime.ToShortDateString() + "\n\n" +
                    "Title:".PadRight(30) + $"{currentToDo.Title}\n" +
                    "Priority:".PadRight(30) + $"{currentToDo.Priority}\n" +
                    "Repeating every:".PadRight(30) + $"{currentToDo.Repeat.ToString()} days\n" +
                    "Description:".PadRight(30) + $"{currentToDo.Description}\n" +
                    "Deadline:".PadRight(30) + currentToDo.ToDoDate.ToShortDateString() + "\n" +
                    "Done:".PadRight(30) + $"{currentToDo.TaskIsDone.ToString()}\n";

                Console.WriteLine(output + "\n");

                Console.WriteLine("Type in the updated values, if you want to keep the old values just leave the field blank");
                return;


            }

            if (currentContext == ViewType.DeleteView)
            {
                int startingPos = Console.WindowHeight / 2;

                string[] outputArray = {
                    $"You are currently trying to delete:",
                    currentToDo.Title ,
                    "This action is irreversible. Do you wish to continue",
                    "",
                    "y/n?"
                };

                Console.ForegroundColor = ConsoleColor.DarkRed;
                CenterMessage(outputArray, "delete"); // This method centers the text on the screen long both vertical and horizontal axis
                Console.ForegroundColor = ConsoleColor.Gray;

                return;
            }

            if (currentContext == ViewType.Detailview && currentToDo != null)
            {
                output =
                    "Id:".PadRight(30) + $"{currentToDo.Id.ToString()}\n" +
                    "Created:".PadRight(30) + currentToDo.CreatedDateTime.ToShortDateString() + "\n\n" +
                    "Title:".PadRight(30) + (currentToDo.Title.Length > 28 ? currentToDo.Title.Substring(0, 24) + "...\n" : currentToDo.Title) + "\n" +
                    "Priority:".PadRight(30) + $"{currentToDo.Priority}\n" +
                    "Repeating every:".PadRight(30) + $"{currentToDo.Repeat.ToString()} days\n" +
                    "Description:".PadRight(30) + (currentToDo.Description.Length > 30 ? currentToDo.Description.Substring(0, 30) + "...\n" : currentToDo.Description) + "\n" +
                    "Deadline:".PadRight(30) + currentToDo.ToDoDate.ToShortDateString() + "\n" +
                    "Done:".PadRight(30) + $"{currentToDo.TaskIsDone.ToString()}\n";

                Console.WriteLine(output);
                return;
            }

        }

        private void ApplyHoverEffect(int currentItem)
        {
            // Apply a highligt effect if task is currently in focus

            if (currentItem == currentlySelectedLine)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
            }

        }

        private void AddLine()
        {
            // Adds a horizontal line, used to visually seperate the menu from the information area

            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("-");
            }

            Console.WriteLine();

        }

        public void MenuSelector()
        {
            // Handles menu selection, can pass on the input to other methods

            ConsoleKeyInfo consoleKey = Console.ReadKey(true);

            switch (consoleKey.Key)
            {
                case ConsoleKey.Q:
                    Environment.Exit(0);
                    break;

                case ConsoleKey.LeftArrow:
                    CurrentViewType = ViewType.MainView;
                    ShowMenu();
                    break;

                case ConsoleKey.B:
                    CurrentViewType = ViewType.MainView;
                    ShowMenu();
                    break;

                case ConsoleKey.D:
                    CurrentViewType = ViewType.DeleteView;
                    DeleteHoveredItem();
                    return;

                case ConsoleKey.M:
                    ChangeStatusOfHoveredItem();
                    return;

                case ConsoleKey.E:
                    CurrentViewType = ViewType.EditView;
                    ShowMenu();
                    EditHoveredItem();
                    return;

                case ConsoleKey.Backspace:
                    CurrentViewType = ViewType.MainView;
                    ShowMenu();
                    break;
            }

            if (CurrentViewType == ViewType.MainView)
            {
                MenuSelectorMainView(consoleKey);
            }

        }

        private void MenuSelectorMainView(ConsoleKeyInfo consoleKey)
        {
            // Handles main menu selection. Could probably be combined with MenuSelector()

            switch (consoleKey.Key)
            {
                case ConsoleKey.UpArrow:
                    ChangeHoveredItem(-1);
                    break;

                case ConsoleKey.DownArrow:
                    ChangeHoveredItem(1);
                    break;

                case ConsoleKey.Spacebar:
                    SelectHoveredItem();
                    break;

                case ConsoleKey.Enter:
                    SelectHoveredItem();
                    break;

                case ConsoleKey.F:
                    ChangeFilterStatus();
                    FindItemToHover(0);
                    ShowMenu();
                    return;

                case ConsoleKey.C:
                    CurrentViewType = ViewType.CreateView;
                    ToDo.CreateNewToDoTask();
                    break;

                case ConsoleKey.D:
                    CurrentViewType = ViewType.DeleteView;
                    DeleteHoveredItem();
                    return;

                case ConsoleKey.E:
                    CurrentViewType = ViewType.EditView;
                    ShowMenu();
                    EditHoveredItem();
                    return;
            }
        }

        private void ChangeHoveredItem(int valueModifier)
        {
            // Changes the currently selected task

            if (valueModifier == -1 && currentlySelectedLine == 0) // We can not go below item 0
            {
                return;
            }
            if (valueModifier == 1 && currentlySelectedLine == ToDo.ToDoList.Count - 1) // We can not go past our last item
            {
                return;
            }

            currentlySelectedLine += valueModifier; // Updates the selected line. Used to determine which line to highlight
            currentToDo = ToDo.ToDoList[currentlySelectedLine];

            bool canContinue = FindItemToHover(valueModifier); // Ensure the item we are trying to hover is valid. If not it will determine the closest one

            if (!canContinue) // If no valid selection is found we return null
            {
                currentToDo = null;
                return;
            }

            ShowMenu();
        }

        private bool FindItemToHover(int valueModifier)
        {
            // When filter is set to true tasks which are marked "done" are no longer being displayed.
            // We need to make sure that we are not targetting an invisble entry

            if (valueModifier == 0)
            {
                valueModifier = 1;
            }

            bool hitLowerLimit = false, hitUpperLimit = false;

            while (currentToDo.TaskIsDone == true && ShowDone == false) // While currently selected task should not be shown
            {
                if (currentlySelectedLine > 0 && valueModifier == -1) // Keeps momentum, so if you were going up on the list it will keep going up
                {
                    currentlySelectedLine += valueModifier;
                    currentToDo = ToDo.ToDoList[currentlySelectedLine];
                }
                else if (currentlySelectedLine <= 0 && valueModifier == -1) // There is no more items in the current direction. So reset and start looking in the oposite direction
                {
                    hitLowerLimit = true;
                    currentlySelectedLine = 0;
                    valueModifier = 1;
                }

                if (currentlySelectedLine < ToDo.ToDoList.Count - 1 && valueModifier == +1) // Keeps momentum, so if you were going up on the list it will keep going up
                {
                    currentlySelectedLine += valueModifier;
                    currentToDo = ToDo.ToDoList[currentlySelectedLine];
                }
                else if (currentlySelectedLine >= ToDo.ToDoList.Count - 1 && valueModifier == +1) // There is no more items in the current direction. So reset and start looking in the oposite direction
                {
                    currentlySelectedLine = ToDo.ToDoList.Count - 1;
                    valueModifier = -1;
                    hitUpperLimit = true;
                }

                if (hitLowerLimit && hitUpperLimit) // If we have looked in both directions and have hit the limit we can assume there is no valid task to select
                {
                    return false;
                }
            }

            return true; // We have found a valid selection so we return true
        }

        private void EditHoveredItem()
        {
            ToDoClass.ToDoObject newToDo = ToDo.UpdateToDoTask(currentToDo);
            Console.Clear();

            string[] outputArray = {
                    $"Do you want to keep the changes you made to:",
                    currentToDo.Title ,
                    "",
                    "y/n?"
                };

            CenterMessage(outputArray, ""); // Centers message on screen

            ConsoleKeyInfo consoleKey = Console.ReadKey(true); // User needs to confirm that they want to save the updated task

            switch (consoleKey.Key)
            {
                case ConsoleKey.Y:
                    ToDo.ToDoList[currentlySelectedLine] = newToDo;
                    ToDo.SaveToDoList();
                    CurrentViewType = ViewType.MainView;
                    ShowMenu();
                    break;

                case ConsoleKey.N:
                    CurrentViewType = ViewType.MainView;
                    ShowMenu();
                    break;

                case ConsoleKey.B:
                    CurrentViewType = ViewType.MainView;
                    ShowMenu();
                    break;

                case ConsoleKey.Q:
                    Environment.Exit(0);
                    break;
            }

        }


        public void CenterMessage(string[] message, string? caller)
        {
            // Used to center text in the console

            int startingPos = (Console.WindowHeight / 2) - (message.Length / 2); //We start by putting the cursor in the middle of the screen. Then we move it depending on how many lines we need to write

            foreach (string messageText in message)
            {
                if (messageText == currentToDo.Title && caller == "delete") // When status is "delete" it is because we are trying to delete an entry so we need different color codes
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (caller == "delete")
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                }

                Console.SetCursorPosition((Console.WindowWidth / 2) - (messageText.Length / 2), startingPos);
                Console.WriteLine(messageText);
                startingPos++;
            }

        }

        private void DeleteHoveredItem()
        {
            CurrentViewType = ViewType.DeleteView;
            ShowMenu();

            ConsoleKeyInfo consoleKey = Console.ReadKey(true); // User needs to confirm that they want to delete the selected task

            switch (consoleKey.Key)
            {
                case ConsoleKey.Y:
                    ToDo.ToDoList.RemoveAt(currentlySelectedLine);

                    if (currentlySelectedLine > ToDo.ToDoList.Count - 1)
                    {
                        currentlySelectedLine--;
                    }

                    ToDo.SaveToDoList();
                    CurrentViewType = ViewType.MainView;
                    ShowMenu();
                    break;

                case ConsoleKey.N:
                    CurrentViewType = ViewType.MainView;
                    ShowMenu();
                    break;

                case ConsoleKey.B:
                    CurrentViewType = ViewType.MainView;
                    ShowMenu();
                    break;

                case ConsoleKey.Q:
                    Environment.Exit(0);
                    break;
            }

        }

        private void ChangeFilterStatus()
        {
            // Determines wether or not tasks that are marked "Done" will show up in the mainview

            ShowDone = ShowDone ? false : true;
        }

        private void ChangeStatusOfHoveredItem()
        {
            // Switches the selected task between done and incomplete

            currentToDo.TaskIsDone = currentToDo.TaskIsDone ? false : true;
            ToDo.SaveToDoList();
            ShowMenu();
        }
        private void SelectHoveredItem()
        {
            // Shows the details of the currently selected item.

            CurrentViewType = ViewType.Detailview;
            ShowMenu();
        }

    }
}
