using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListManager
{
    internal class MenuDisplay : Display
    {
//Window Fields
        internal static int DisplayIntWidth = 0 + genBuffer;
        internal static int DisplayWidth = setCenterWindows[0] - genBuffer - LeftAdjustment - 1;
        internal static int InstDisplay = DisplayIntWidth + 2;
        internal static int[] LogoPosHeight = { 0, 3 };
        internal static int[] MenuPosHeight = { 4, 11 };
        internal static int[] ActionPosHeight = { 18, 26 };
        internal static int[] StatusPosHeight = { 45, 3 };
        internal static int[] ChoiceAction = { 20, MenuPosHeight[0] + MenuPosHeight[1] + 1 };
        internal static string DefaultStatus = "Select a Menu Option";
        internal int ActionPromptInitLine
        {
            get
            {
                return ActionPosHeight[0] + (ActionPosHeight[1] - ActionPrompt.Count()) / 2;
            }
        }
        public static char MenuChoice { get; private set; }

//Action Fields
        public static readonly Option[] optionsList = (Option[])Enum.GetValues(typeof(Option));
        public static readonly ItemSorting[] itemSortingList = (ItemSorting[])Enum.GetValues(typeof(ItemSorting));
        public enum Option
        {
            A, R, E, P, S, T, Q
        }
        internal List<string> ActionPrompt = new List<string>() {"Test","Test","Test","Test"};
        public enum ActionStatus
        {
            ActionSuccess,
            ActionFailure,
            Neutral
        }

        internal void FrameBuild()
        {
            BoxCreator(DisplayWidth, LogoPosHeight[1], DisplayIntWidth, LogoPosHeight[0],"", "Shopping List Manager");
            BoxCreator(DisplayWidth, MenuPosHeight[1], DisplayIntWidth, MenuPosHeight[0]);
            BoxCreator(DisplayWidth, ActionPosHeight[1], DisplayIntWidth, ActionPosHeight[0]);
            BoxCreator(setMaxWindows[0] - genBuffer + 1, StatusPosHeight[1], DisplayIntWidth, StatusPosHeight[0],"","Select a Menu Option");
            /*Status bar adjusted to be below both Menu and List displays*/
        }

//Action Display Group
        internal string OptionInstructions()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(ChoiceAction[0], ChoiceAction[1]);
            Console.Write("   ");
            Console.SetCursorPosition(InstDisplay, MenuPosHeight[0] + 1);
            Console.Write("Menu Options:");
            for (int i = 1; i <= optionsList.Length; ++i)
            {
                Console.SetCursorPosition(InstDisplay, MenuPosHeight[0] + i + 2);
                Console.Write($"{optionsList[i-1]} - {Action(optionsList[i - 1],"inst")}");
            }
            Console.SetCursorPosition(InstDisplay, ChoiceAction[1]);
            Console.Write("Your Choice:");
            Console.SetCursorPosition(ChoiceAction[0], ChoiceAction[1]);
            string option = Console.ReadKey(true).KeyChar.ToString().ToUpper();
            Console.Write(option);
            return option;
        }

        internal void ActionInstructions(List<string> instructions)
        {
            for (int i = 0; i < instructions.Count(); ++i)
            {
                Console.SetCursorPosition(InstDisplay + 2,ActionPromptInitLine + i);
                Console.Write(instructions[i]);
            }
        }

        public void ActionInstructionsClear()
        {
            string space = "";
            for (int i = 0; i < ActionPosHeight[1] - 1; ++i)
            {
                Console.SetCursorPosition(InstDisplay + 2, ActionPosHeight[0] + 1 + i);
                for (int s = 0; s < DisplayWidth - genBuffer - 2; ++s)
                {
                    space += " ";
                }
                Console.Write(space);
                space = "";
            }
        }


//Action Broadcaster Group
//Overrides exists for string to option and vice versa
        public string Action(Option option, string placeholder)
        {
            string action = "";
            switch(option)
            {
                case Option.A: action = "Add a new Item"; break;
                case Option.R: action = "Remove an Item"; break;
                case Option.E: action = "Edit an Item"; break;
                case Option.P: action = "Mark an Item as purchased"; break;
                case Option.S: action = "Sort the list"; break;
                case Option.T: action = "Toggle display of purchased items"; break;
                case Option.Q: action = "Quit the program"; break;
                default: action = "Error"; break;
            }
            return action;
        }

        public Option Action(string command, MenuDisplay menu, ListDisplay list)
        {
            Option option = new Option();
            switch (command)
            {

                case "A": option = Option.A; User.AddNewItem(menu, list); break;
                case "R": option = Option.R; User.RemoveItem(menu,list); break;
                case "E": option = Option.E; User.EditItem(menu,list); break;
                case "P": option = Option.P; User.MarkPurchase(menu,list); break;
                case "S": option = Option.S; User.SortList(menu,list); break;
                case "T": option = Option.T; list.DisplayCurrentList(ItemsList.UserPurchasedList); break;
                case "Q": option = Option.Q; User.ProgramRun = false; break;
                
            }
            return option;
        }

        public void StatusUpdate(string message = " ", ActionStatus status = ActionStatus.Neutral)
        {
            if (status == ActionStatus.ActionSuccess)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (status == ActionStatus.ActionFailure)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else 
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            if(message == " ") /*If Clear*/
            {
                string space = "";
                for (int i = 0; i < ActionPosHeight[1] - 1; ++i)
                {
                    Console.SetCursorPosition(InstDisplay + 2, StatusPosHeight[0] + StatusPosHeight[1]/2);
                    for (int s = 0; s < DisplayWidth - genBuffer - 2; ++s)
                    {
                        space += " ";
                    }
                    Console.Write(space);
                    space = "";
                }
            }
            BoxCreator(setMaxWindows[0] - genBuffer + 1, StatusPosHeight[1], DisplayIntWidth, StatusPosHeight[0], "",message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public string PriorityInitial(Item.Priority priority)
        {
            string initial = "";
            switch(priority)
            {
                case Item.Priority.High: initial = "H"; break;
                case Item.Priority.Medium: initial = "M"; break;
                case Item.Priority.Low: initial = "L"; break;
            }
            return initial;
        }

        public string SortOptions(ItemSorting command/*, MenuDisplay menu, List list*/)
        {
            string option = "";
            switch (command)
            {
                case ItemSorting.Name: option = "N" ; break;
                case ItemSorting.Category: option = "C" ; break;
                case ItemSorting.Priority: option = "P" ; break;
                case ItemSorting.DateAdded: option = "D"; break;
                case ItemSorting.DatePurchased: option = "B"; break;
            }
            return option;
        }

        public ItemSorting SortOptions(string command, out bool valid/*, MenuDisplay menu, List list*/)
        {
            valid = true;
            ItemSorting option = new ItemSorting();
            switch (command)
            {
                case "N": option = ItemSorting.Name; break;
                case "C": option = ItemSorting.Category; break;
                case "P": option = ItemSorting.Priority; break;
                case "D": option = ItemSorting.DateAdded; break;
                case "B": option = ItemSorting.DatePurchased; break;
                default: option = ItemSorting.Name;

                    StatusUpdate();
                    StatusUpdate($"Please Select one of the options above", ActionStatus.ActionFailure);
                    valid = false;
                    break;
            }
            return option;
        }

        public string OrderOptions(string command, ItemSorting sorting/*, MenuDisplay menu, List list*/)
        {
            string option = "";
            switch (command)
            {
                case "A": option = "ascending"; break;
                case "D": option = "descending"; break;
            }
            return option;
        }

        public ItemSorting OrderOptions(string command, ItemSorting sorting, out bool valid)
        {
            valid = true;
            ItemSorting option = new ItemSorting();
            switch (command)
            {
                case "A": option = sorting; ListDisplay.Descending = false; ListDisplay.Order = "Ascending"; break;
                case "D": option = sorting; ListDisplay.Descending = true; ListDisplay.Order = "Descending"; break;
                default:
                    option = (sorting); /*Placeholder because it will not activate*/
                    StatusUpdate();
                    StatusUpdate($"Please Select one of the options above", ActionStatus.ActionFailure);
                    valid = false;
                    break;
            }
            return option;
        }


    }
}
