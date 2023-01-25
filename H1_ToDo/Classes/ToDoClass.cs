using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace H1_ToDo.Classes
{
    internal class ToDoClass
    {
        public List<ToDoObject> ToDoList = new List<ToDoObject>();
        public MenuClass Menu;

        public string ShowToDo(ToDoObject toDo)
        {


            return "";
        }

        public void LoadToDoList()
        {
            string jsonData = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "todo.dat");
            ToDoList = JsonSerializer.Deserialize<List<ToDoObject>>(jsonData);
        }

        public void SaveToDoList()
        {
            string jsonData = JsonSerializer.Serialize(ToDoList);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "todo.dat", jsonData);
        }

        public int FindAvailableID()
        {
            if (ToDoList.Count == 0)
            {
                return 0;
            }

            int nextId = ToDoList.Max(id => id.Id) + 1;

            return nextId;
        }

        public void CreateNewToDoTask()
        {

            if (ToDoList.Count > 15)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("You have hit the limit of currently active tasks");
                Console.WriteLine("Press any key to get back to the main menu");
                Console.ReadKey();

                Menu.CurrentViewType = MenuClass.ViewType.MainView;
                Menu.ShowMenu();
                return;
            }

            ToDoObject newToDo = new ToDoObject();
            string title, description, priority, repeatString;
            DateTime toDoDate;



            Menu.ShowMenu();
            Console.CursorVisible = true;

            Console.WriteLine("Creating new task: \n");

            Console.WriteLine("Title: ");
            Console.WriteLine("Description: ");
            Console.WriteLine("Done by (yyyy-mm-dd)/today:");
            Console.WriteLine("Priority (y/n):");
            Console.WriteLine("Repeat every (days):");

            while (true)
            {
                Console.SetCursorPosition(28, 5);
                title = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(title))
                {
                    newToDo.Title = title;
                    break;
                }
            }

            while (true)
            {
                Console.SetCursorPosition(28, 6);
                description = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(description))
                {
                    newToDo.Description = description;
                    break;
                }
            }

            while (true)
            {
                Console.SetCursorPosition(28, 7);
                string toDoDateString = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(toDoDateString) && toDoDateString.ToLower() == "today")
                {
                    newToDo.ToDoDate = DateTime.Now;
                    break;
                }

                if (DateTime.TryParseExact(toDoDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out toDoDate))
                {
                    newToDo.ToDoDate = toDoDate;
                    break;
                }
            }

            while (true)
            {
                Console.SetCursorPosition(28, 8);
                priority = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(priority))
                {
                    if (priority == "y" || priority == "n")
                    {
                        newToDo.Priority = priority;
                        break;
                    }
                }
            }

            while (true)
            {
                Console.SetCursorPosition(28, 9);
                repeatString = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(repeatString))
                {
                    int repeat;
                    if (int.TryParse(repeatString, out repeat))
                    {
                        newToDo.Repeat = repeat;
                        break;
                    }

                }
            }

            newToDo.CreatedDateTime = DateTime.Now;
            newToDo.Id = FindAvailableID();
            newToDo.TaskIsDone = false;

            ToDoList.Add(newToDo);
            SaveToDoList();

            Menu.CurrentViewType = MenuClass.ViewType.MainView;
            Menu.ShowMenu();

        }

        public ToDoObject UpdateToDoTask(ToDoObject oldToDo)
        {
            ToDoObject newToDo = new ToDoObject();
            string title, description, priority, repeatString;
            DateTime toDoDate;



            Menu.ShowMenu();
            Console.CursorVisible = true;

            Console.WriteLine("New Values: \n");

            Console.WriteLine("Title: ");
            Console.WriteLine("Description: ");
            Console.WriteLine("Done by (yyyy-mm-dd)/today:");
            Console.WriteLine("Priority (y/n):");
            Console.WriteLine("Repeat every (days):");

            while (true)
            {
                Console.SetCursorPosition(28, 17);
                title = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(title))
                {
                    newToDo.Title = title;
                    break;
                }

                Console.SetCursorPosition(28, 17);
                Console.Write(oldToDo.Title);
                newToDo.Title = oldToDo.Title;
                break;

            }

            while (true)
            {
                Console.SetCursorPosition(28, 18);
                description = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(description))
                {
                    newToDo.Description = description;
                    break;
                }

                Console.SetCursorPosition(28, 18);
                Console.Write(oldToDo.Description);
                newToDo.Description = oldToDo.Description;
                break;
            }

            while (true)
            {
                Console.SetCursorPosition(28, 19);
                string toDoDateString = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(toDoDateString) && toDoDateString.ToLower() == "today")
                {
                    newToDo.ToDoDate = DateTime.Now;
                    break;
                }

                if (DateTime.TryParseExact(toDoDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out toDoDate))
                {
                    newToDo.ToDoDate = toDoDate;
                    break;
                }

                Console.SetCursorPosition(28, 19);
                Console.Write(oldToDo.ToDoDate.ToShortDateString());
                newToDo.ToDoDate = oldToDo.ToDoDate;
                break;
            }

            while (true)
            {
                Console.SetCursorPosition(28, 20);
                priority = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(priority))
                {
                    if (priority == "y" || priority == "n")
                    {
                        newToDo.Priority = priority;
                        break;
                    }
                }

                Console.SetCursorPosition(28, 20);
                Console.Write(oldToDo.Priority);
                newToDo.Priority = oldToDo.Priority;
                break;
            }

            while (true)
            {
                Console.SetCursorPosition(28, 21);
                repeatString = Console.ReadLine();
                bool parseError = false;

                if (!string.IsNullOrWhiteSpace(repeatString))
                {
                    int repeat;
                    if (int.TryParse(repeatString, out repeat))
                    {
                        newToDo.Repeat = repeat;
                        break;
                    }
                    else
                    {
                        parseError = true;
                    }
                }

                if (!parseError)
                {
                    Console.SetCursorPosition(28, 21);
                    Console.Write(oldToDo.Repeat.ToString());
                    newToDo.Repeat = oldToDo.Repeat;
                    break;
                }


            }

            newToDo.TaskIsDone = oldToDo.TaskIsDone;

            return newToDo;
        }


        public class ToDoObject
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime ToDoDate { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public bool TaskIsDone { get; set; }
            public string Priority { get; set; }
            public long Repeat { get; set; }
        }
    }
}
