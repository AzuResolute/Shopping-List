using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListManager
{
    class Program
    {
        static void Main(string[] args)
        {
            User.Load();
            Console.ForegroundColor = ConsoleColor.White;

            if(User.SaveLoadMode == false)  User.InitialBasket();

            WelcomeScreen welcome = new WelcomeScreen();
            MenuDisplay mainDisplay = new MenuDisplay();
            ListDisplay listDisplay = new ListDisplay();

            mainDisplay.FrameBuild();
            listDisplay.FrameBuild();
            listDisplay.DisplayCurrentList(ItemsList.UserShoppingList);
            while(User.ProgramRun == true)
            {
                string input = mainDisplay.OptionInstructions();
                mainDisplay.Action(input,mainDisplay,listDisplay);
            }
            User.Quit(mainDisplay);
        }
    }
}
