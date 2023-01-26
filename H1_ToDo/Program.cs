using H1_ToDo.Classes;
using System.Diagnostics;

namespace H1_ToDo
{
    internal class Program
    {
        static ToDoClass ToDo = new ToDoClass();
        static MenuClass Menu = new MenuClass();

        static void Main(string[] args)
        {

            // Build references and prep the program to run
            Menu.ToDo = ToDo;
            ToDo.Menu = Menu;
            ToDo.LoadToDoList();
            Menu.ShowDone = true;
            Menu.currentlySelectedLine = 0;

            Menu.CurrentViewType = MenuClass.ViewType.MainView;

            while (true)
            {
                Console.CursorVisible = false;
                Menu.ShowMenu();
                Menu.MenuSelector();
            }
        }


    }
}