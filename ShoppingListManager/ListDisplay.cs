using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListManager
{
    internal class ListDisplay : Display
    {
//Display Fields
        internal static int DisplayIntWidth = setCenterWindows[0] + genBuffer - LeftAdjustment + 1;
        internal static int DisplayWidth = setCenterWindows[0] - genBuffer + LeftAdjustment;
        internal static int[] TitlePosHeight = { 0, 3 };
        internal static int[] MainPosHeight = { 3, 45 - MenuDisplay.StatusPosHeight[1] -1 }; /*Status Bar will be below List to make room*/
        internal static bool Descending = false;
        internal static string Order = "Ascending";
        internal static int listSelectionNum = 1;
        internal static List<Item> currentList = ItemsList.UserShoppingList;

//Header Fields
        internal readonly string TitleMessage = $"Showing All Items:   {ItemsList.UserShoppingList.Count} ({ItemsList.UserShoppingList.Count - ItemsList.UserPurchasedList.Count} pending; {ItemsList.UserPurchasedList.Count} purchased)   Sorting: {ItemsList.CurrentSorting}";
        internal readonly int HeaderRow = MenuDisplay.MenuPosHeight[0];
        internal readonly int ListRow = MenuDisplay.MenuPosHeight[0] + 1;
        internal const int itemNumCol = 2;
        internal const int NameCol = itemNumCol + 4;
        internal const int CatCol = NameCol + 30;
        internal const int PrioCol = CatCol + 12;
        internal const int DatAddCol = PrioCol + 12;
        internal const int DatPurCol = DatAddCol + 12;

//Box Builders
        internal void FrameBuild()
        {
            BoxCreator(DisplayWidth, TitlePosHeight[1], DisplayIntWidth, TitlePosHeight[0], "", TitleMessage);
            BoxCreator(DisplayWidth, MainPosHeight[1], DisplayIntWidth, MainPosHeight[0]);

            foreach (Item.itemProp i in Item.itemPropList)
            {
                ListInput(i, "header", HeaderRow);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

//Used to create headers
        internal void ListInput(Item.itemProp prop, string sentence, int yAxis)
        {
            int xAxis = 0;
            switch (prop)
            {
                case Item.itemProp.ItemNum: xAxis = DisplayIntWidth + itemNumCol; Console.ForegroundColor = ConsoleColor.Blue;
                    if(sentence == "header") sentence = "#"; break;
                case Item.itemProp.Name: xAxis = DisplayIntWidth + NameCol; Console.ForegroundColor = ConsoleColor.Cyan;
                    if (sentence == "header") sentence = Item.itemProp.Name.ToString(); break;
                case Item.itemProp.Category: xAxis = DisplayIntWidth + CatCol; Console.ForegroundColor = ConsoleColor.Blue;
                    if (sentence == "header") sentence = Item.itemProp.Category.ToString(); break;
                case Item.itemProp.Priority: xAxis = DisplayIntWidth + PrioCol; Console.ForegroundColor = ConsoleColor.Cyan;
                    if (sentence == "header") sentence = Item.itemProp.Priority.ToString(); break;
                case Item.itemProp.Date_Added: xAxis = DisplayIntWidth + DatAddCol; Console.ForegroundColor = ConsoleColor.Blue;
                    if (sentence == "header") sentence = "Date Added"; break;
                case Item.itemProp.Purchased: xAxis = DisplayIntWidth + DatPurCol; Console.ForegroundColor = ConsoleColor.Cyan;
                    if (sentence == "header") sentence = Item.itemProp.Purchased.ToString(); break;
            }
            Console.SetCursorPosition(xAxis, yAxis);
            Console.Write(sentence);
            Console.ForegroundColor = ConsoleColor.White;
        }

//List Input & Display works in tandem to display current list
        internal void ListInput(Item item, List<Item> itemList)
        {
            if(Descending == false) item.ItemNum = itemList.IndexOf(item) + 1;
            if(Descending == true) item.ItemNum = (itemList.Count - itemList.IndexOf(item));
            Console.SetCursorPosition(DisplayIntWidth + itemNumCol, ListRow + item.ItemNum);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{itemList.IndexOf(item)+1}");

            Console.SetCursorPosition(DisplayIntWidth + NameCol, ListRow + item.ItemNum);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(item.Name);

            Console.SetCursorPosition(DisplayIntWidth + CatCol, ListRow + item.ItemNum);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(item.category.ToString());

            Console.SetCursorPosition(DisplayIntWidth + PrioCol, ListRow + item.ItemNum);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(item.priority.ToString());

            Console.SetCursorPosition(DisplayIntWidth + DatAddCol, ListRow + item.ItemNum);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(item.Date_Added.ToShortDateString());

            if (item.IsPurchased == true)
            {
                Console.SetCursorPosition(DisplayIntWidth + DatPurCol, ListRow + item.ItemNum);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(item.Date_Added.ToShortDateString());
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal void DisplayCurrentList(List<Item> list)
        {
            //Filter
            if (User.ToggledPurchasedItems == true) list = ItemsList.UserShoppingList;

            //Destruction
            string space = "";
            for (int i = 0; i < MainPosHeight[1] - MainPosHeight[0]; ++i)
            {
                Console.SetCursorPosition(DisplayIntWidth + itemNumCol, ListRow + i);
                for (int s = 0; s < DisplayWidth - genBuffer; ++s)
                {
                    space += " ";
                }
                Console.Write(space);
                space = "";
            }
            //Creation
            foreach (Item i in list)
            {
                ListInput(i,list);
            }
            //Update Header
            if (list == ItemsList.UserShoppingList) listSelectionNum = 1;
            else if (list == ItemsList.UserPurchasedList) listSelectionNum = 2;
            else listSelectionNum = 0;

            BoxCreator(DisplayWidth, TitlePosHeight[1], DisplayIntWidth, TitlePosHeight[0], "",
                $"Showing {ListSelection()}:   {list.Count} ({list.Count- ItemsList.UserPurchasedList.Count} pending; {ItemsList.UserPurchasedList.Count} purchased)   Sorting: {ItemsList.CurrentSorting}, {Order}");

            if (list == ItemsList.UserPurchasedList) User.ToggledPurchasedItems = true;
            else User.ToggledPurchasedItems = false;
        }

        internal string ListSelection()
        {
            string selection = string.Empty;
            switch(listSelectionNum)
            {
                case 1: selection = "Complete List"; currentList = ItemsList.UserShoppingList; break;
                case 2: selection = "Purchased List"; currentList = ItemsList.UserPurchasedList; break;
                default: selection = "Unknown List:"; break;
            }
            return selection;
        }
    }
}
