using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShoppingListManager
{
    public static class User
    {
        internal static bool ProgramRun = true;
        internal static bool ToggledPurchasedItems = false;
        internal static bool SaveLoadMode = true;

        internal static void InitialBasket()
        {
            Item Coffee = new Item("Coffee", Item.Category.Food, Item.Priority.High);
            ItemsList.UserShoppingList.Add(Coffee);
            Item Eggs = new Item("Eggs", Item.Category.Food, Item.Priority.Medium);
            ItemsList.UserShoppingList.Add(Eggs);
            Item Evaporated_Milk = new Item("Evaporated Milk", Item.Category.Food, Item.Priority.High);
            ItemsList.UserShoppingList.Add(Evaporated_Milk);
            Item Lasagna_Noodles = new Item("Lasagna Noodles", Item.Category.Food, Item.Priority.Medium);
            ItemsList.UserShoppingList.Add(Lasagna_Noodles);
            Item MacBookPro = new Item("MacBook Pro", Item.Category.Tech, Item.Priority.Low);
            ItemsList.UserShoppingList.Add(MacBookPro);
            Item ToiletPaper = new Item("Toilet Paper", Item.Category.Household, Item.Priority.High);
            ItemsList.UserShoppingList.Add(ToiletPaper);
        }

        internal static void AddNewItem(MenuDisplay menu, ListDisplay list)
        {
            string NewItemName = "";
            Item.Category NewItemCategory = new Item.Category();
            Item.Priority NewItemPriority = new Item.Priority();

            list.DisplayCurrentList(ItemsList.UserShoppingList);
            bool valid = false;
            bool duplicate = true;
            do
            {
                duplicate = false;
                menu.ActionInstructionsClear();
                menu.ActionPrompt.Clear();
                menu.ActionPrompt.Add("Enter Item Name:");
                menu.ActionInstructions(menu.ActionPrompt);
                Console.SetCursorPosition(MenuDisplay.InstDisplay + 2, menu.ActionPromptInitLine + 1);
                NewItemName = Console.ReadLine();

                /*Duplicate Detector*/
                for (int i = 0; i < ItemsList.UserShoppingList.Count; ++i)
                {

                    if (ItemsList.UserShoppingList[i].Name.ToUpper() == NewItemName.ToUpper())
                    {
                        menu.StatusUpdate();
                        menu.StatusUpdate($"{NewItemName} is already on the list. ", MenuDisplay.ActionStatus.ActionFailure);
                        duplicate = true;
                        break;
                    }
                    if (duplicate == true) break;
                }
                if (duplicate == true) continue;

                if (NewItemName.Length > ListDisplay.CatCol - ListDisplay.itemNumCol /*Name Col*/ - 2)
                {
                    menu.StatusUpdate();
                    menu.StatusUpdate($"Your item name is too long. " +
                        $"Max {ListDisplay.CatCol - ListDisplay.itemNumCol /*Name Col*/ - 2} only", MenuDisplay.ActionStatus.ActionFailure);
                    continue;
                }
                valid = true;
            }
            while (valid == false || duplicate == true);

            menu.StatusUpdate();
            menu.StatusUpdate(MenuDisplay.DefaultStatus);

            valid = false;
            while (valid == false)
            {
                menu.ActionInstructionsClear();
                menu.ActionPrompt.Clear();
                menu.ActionPrompt.Add($"Name: {NewItemName}");
                menu.ActionPrompt.Add($" ");
                for (int i = 0; i < Item.catList.Count(); ++i)
                {
                    menu.ActionPrompt.Add($"{i + 1}: {Item.catList[i]}");
                }
                menu.ActionPrompt.Add($" ");
                menu.ActionPrompt.Add("Your Choice: ");
                menu.ActionInstructions(menu.ActionPrompt);
                char charInput = Console.ReadKey().KeyChar;

                if (!int.TryParse(charInput.ToString(), out int catInput))
                {
                    menu.StatusUpdate();
                    menu.StatusUpdate($"Please enter a whole Number.", MenuDisplay.ActionStatus.ActionFailure);
                    continue;
                }

                if (catInput > 7 || catInput <= 0)
                {
                    menu.StatusUpdate();
                    menu.StatusUpdate($"Number {charInput} is out of  range.", MenuDisplay.ActionStatus.ActionFailure);
                    continue;
                }
                NewItemCategory = Item.catList[catInput - 1];
                valid = true;
            }
            menu.StatusUpdate();
            menu.StatusUpdate(MenuDisplay.DefaultStatus);

            valid = false;
            while (valid == false)
            {
                menu.ActionInstructionsClear();
                menu.ActionPrompt.Clear();
                menu.ActionPrompt.Add($"Name: {NewItemName}");
                menu.ActionPrompt.Add($"Category: {NewItemCategory.ToString()}");
                menu.ActionPrompt.Add($" ");
                for (int i = 0; i < Item.prioList.Count(); ++i)
                {
                    menu.ActionPrompt.Add($"{menu.PriorityInitial(Item.prioList[i])}: {Item.prioList[i]}");
                }
                menu.ActionPrompt.Add($" ");
                menu.ActionPrompt.Add("Your Choice: ");
                menu.ActionInstructions(menu.ActionPrompt);
                string stringInput = Console.ReadKey().KeyChar.ToString().ToUpper();


                if (stringInput != "H" && stringInput != "M" && stringInput != "L")
                {
                    menu.StatusUpdate();
                    menu.StatusUpdate($"Please Select 'H', 'M', or 'L'", MenuDisplay.ActionStatus.ActionFailure);
                    continue;
                }

                switch (stringInput)
                {
                    case "H": NewItemPriority = Item.Priority.High; break;
                    case "M": NewItemPriority = Item.Priority.Medium; break;
                    case "L": NewItemPriority = Item.Priority.Low; break;
                }
                valid = true;
            }

            menu.StatusUpdate();
            Item item = new Item(NewItemName, NewItemCategory, NewItemPriority);
            ItemsList.UserShoppingList.Add(item);
            list.DisplayCurrentList(ItemsList.UserShoppingList);
            menu.StatusUpdate($"Item {NewItemName} has been added to list.",MenuDisplay.ActionStatus.ActionSuccess);
            Save();
            menu.ActionInstructionsClear();
        }

        internal static void RemoveItem(MenuDisplay menu, ListDisplay list)
        {
            string removedItemName = "";
            list.DisplayCurrentList(ItemsList.UserShoppingList);
            IndexFilter(menu, "Which Item # do you want to remove?", out int itemNumIndex, out bool exit);
            if (exit == true) return;
            menu.StatusUpdate();
            removedItemName = ItemsList.UserShoppingList[itemNumIndex].Name;

            exit = Confirm($"Remove item '{removedItemName}' from the list? (Y/N)", menu);
            if (exit == true) return;

            if (ItemsList.UserShoppingList[itemNumIndex].IsPurchased == true)
            {
                int indexAtPurchList = ItemsList.UserPurchasedList.IndexOf(ItemsList.UserShoppingList[itemNumIndex]);
                ItemsList.UserPurchasedList.RemoveAt(indexAtPurchList);
            }
            ItemsList.UserShoppingList.RemoveAt(itemNumIndex);
            list.DisplayCurrentList(ItemsList.UserShoppingList);
            menu.StatusUpdate($"Item {removedItemName} has been removed from the list.", MenuDisplay.ActionStatus.ActionSuccess);
            Save();
            menu.ActionInstructionsClear();
        }

        internal static void EditItem(MenuDisplay menu, ListDisplay list)
        {
            string NewItemName = "";
            Item.Category NewItemCategory = new Item.Category();
            Item.Priority NewItemPriority = new Item.Priority();

            list.DisplayCurrentList(ItemsList.UserShoppingList);
            IndexFilter(menu, "Which Item # do you want to edit?", out int itemNumIndex, out bool exit);
            if (exit == true) return;
            menu.StatusUpdate();
            Item selectedItem = ItemsList.UserShoppingList[itemNumIndex];
            ItemSpecs(selectedItem, menu, list);
            /*Do not reset yet*/

            /*Name*/
            bool valid = false;
            bool duplicate = true;
            do
            {
                duplicate = false;
                menu.ActionInstructionsClear();
                menu.ActionPrompt.Clear();
                ItemSpecs(selectedItem, menu, list);
                menu.ActionPrompt.Add($" "); 
                menu.ActionPrompt.Add($"New Name: ");
                menu.ActionInstructions(menu.ActionPrompt);
                string input = Console.ReadLine();

                for (int i = 0; i < ItemsList.UserShoppingList.Count; ++i)
                {

                    if (ItemsList.UserShoppingList[i].Name == NewItemName)
                    {
                        menu.StatusUpdate();
                        menu.StatusUpdate($"{NewItemName} is already on the list. ", MenuDisplay.ActionStatus.ActionFailure);
                        duplicate = true;
                        break;
                    }
                    if (duplicate == true) break;
                }
                if (duplicate == true) continue;

                if (NewItemName.Length > ListDisplay.CatCol - ListDisplay.itemNumCol /*Name Col*/ - 2)
                {
                    menu.StatusUpdate();
                    menu.StatusUpdate($"Your item name is too long. " +
                        $"Max {ListDisplay.CatCol - ListDisplay.itemNumCol /*Name Col*/ - 2} only", MenuDisplay.ActionStatus.ActionFailure);
                    continue;
                }
                NewItemName = input;
                valid = true;
            }
            while (valid == false || duplicate == true);
            int inputSpaces = NewItemName.Count(char.IsWhiteSpace);
            if (NewItemName.Length == inputSpaces || NewItemName == string.Empty)
            {
                NewItemName = selectedItem.Name;
            }
            selectedItem.Name = NewItemName;

            menu.StatusUpdate();
            menu.StatusUpdate(MenuDisplay.DefaultStatus);

            /*Category*/
            valid = false;
            while (valid == false)
            {
                menu.ActionInstructionsClear();
                menu.ActionPrompt.Clear();
                ItemSpecs(selectedItem, menu, list);
                menu.ActionPrompt.Add($" ");
                menu.ActionPrompt.Add($"New Name: {selectedItem.Name}");
                menu.ActionPrompt.Add($" ");
                for (int i = 0; i < Item.catList.Count(); ++i)
                {
                    menu.ActionPrompt.Add($"{i + 1}: {Item.catList[i]}");
                }
                menu.ActionPrompt.Add($" ");
                menu.ActionPrompt.Add("New Category: ");
                menu.ActionInstructions(menu.ActionPrompt);
                char charInput = Console.ReadKey().KeyChar;

                if (charInput == ' ' || charInput == '\r')
                {
                    NewItemCategory = selectedItem.category;
                    valid = true;
                    break;
                }

                    if (!int.TryParse(charInput.ToString(), out int catInput))
                {
                    menu.StatusUpdate();
                    menu.StatusUpdate($"Please enter a whole Number.", MenuDisplay.ActionStatus.ActionFailure);
                    continue;
                }

                if (catInput > 7 || catInput <= 0)
                {
                    menu.StatusUpdate();
                    menu.StatusUpdate($"Number {charInput} is out of  range.", MenuDisplay.ActionStatus.ActionFailure);
                    continue;
                }

                NewItemCategory = Item.catList[catInput - 1];
                valid = true;
            }
            selectedItem.category = NewItemCategory;
            menu.StatusUpdate();
            menu.StatusUpdate(MenuDisplay.DefaultStatus);

            //Priority
            valid = false;
            while (valid == false)
            {
                menu.ActionInstructionsClear();
                menu.ActionPrompt.Clear();
                ItemSpecs(selectedItem, menu, list);
                menu.ActionPrompt.Add($" ");
                menu.ActionPrompt.Add($"New Name: {selectedItem.Name}");
                menu.ActionPrompt.Add($"New Category: {selectedItem.category}");
                menu.ActionPrompt.Add($" ");

                for (int i = 0; i < Item.prioList.Count(); ++i)
                {
                    menu.ActionPrompt.Add($"{menu.PriorityInitial(Item.prioList[i])}: {Item.prioList[i]}");
                }
                menu.ActionPrompt.Add($" ");
                menu.ActionPrompt.Add("Your Choice: ");
                menu.ActionInstructions(menu.ActionPrompt);
                string stringInput = Console.ReadKey().KeyChar.ToString().ToUpper();

                if (stringInput == " " || stringInput == "\r")
                {
                    NewItemPriority = selectedItem.priority;
                    valid = true;
                    break;
                }

                if (stringInput != "H" && stringInput != "M" && stringInput != "L")
                {
                    menu.StatusUpdate();
                    menu.StatusUpdate($"Please Select 'H', 'M', or 'L'", MenuDisplay.ActionStatus.ActionFailure);
                    continue;
                }

                switch (stringInput)
                {
                    case "H": NewItemPriority = Item.Priority.High; break;
                    case "M": NewItemPriority = Item.Priority.Medium; break;
                    case "L": NewItemPriority = Item.Priority.Low; break;
                }
                valid = true;
            }
            selectedItem.priority = NewItemPriority;

            //ifPurchased
            if (ItemsList.UserShoppingList[itemNumIndex].IsPurchased == true)
            {
                int indexAtPurchList = ItemsList.UserPurchasedList.IndexOf(ItemsList.UserShoppingList[itemNumIndex]);
                ItemsList.UserPurchasedList[indexAtPurchList].Name = NewItemName;
                ItemsList.UserPurchasedList[indexAtPurchList].category = NewItemCategory;
                ItemsList.UserPurchasedList[indexAtPurchList].priority = NewItemPriority;
            }

            //Finalize
            menu.ActionInstructionsClear();
            menu.ActionPrompt.Clear();
            menu.StatusUpdate();
            list.DisplayCurrentList(ItemsList.UserShoppingList);
            menu.StatusUpdate($"Item {selectedItem.Name} has been successfully edited.", MenuDisplay.ActionStatus.ActionSuccess);
            Save();
        }

        internal static void MarkPurchase(MenuDisplay menu, ListDisplay list)
        {
            list.DisplayCurrentList(ItemsList.UserShoppingList);
            IndexFilter(menu, "Which Item # do you want to purchase?",out int itemNumIndex, out bool exit);
            if (exit == true) return;
            menu.StatusUpdate();
            Item selectedItem = ItemsList.UserShoppingList[itemNumIndex];
            menu.StatusUpdate();
            menu.StatusUpdate(MenuDisplay.DefaultStatus);

            //Prompt Confirmation

            exit = Confirm($"Mark item '{selectedItem.Name}' as purchased? (Y/N)", menu);
            if (exit == true) return;
            menu.ActionInstructionsClear();
            menu.ActionPrompt.Clear();

            //IsPurchased already?
            if (selectedItem.IsPurchased == true)
            {
                menu.StatusUpdate();
                menu.StatusUpdate($"Item '{selectedItem.Name}' has already been purchased", MenuDisplay.ActionStatus.Neutral);
                return;
            }
            //Finalize
            selectedItem.IsPurchased = true;
            selectedItem.Date_Purchased = DateTime.Now;

            menu.ActionInstructionsClear();
            menu.ActionPrompt.Clear();
            menu.StatusUpdate();
            ItemsList.UserPurchasedList.Add(selectedItem);
            list.DisplayCurrentList(ItemsList.UserShoppingList);
            menu.StatusUpdate($"Item {selectedItem.Name} has been marked as purchased.", MenuDisplay.ActionStatus.ActionSuccess);
            Save();
        }

        internal static void SortList(MenuDisplay menu, ListDisplay list)
        {
            ItemSorting sortingMethod = new ItemSorting();
   

            ItemSorting orderMethod = new ItemSorting();
            bool valid = false;
            while (valid == false)
            {
                menu.ActionInstructionsClear();
                menu.ActionPrompt.Clear();
                for (int i = 0; i < MenuDisplay.itemSortingList.Count() - 1 /*Not counting Descending*/; ++i)
                {
                    menu.ActionPrompt.Add($"{menu.SortOptions(MenuDisplay.itemSortingList[i])}  Sort by {MenuDisplay.itemSortingList[i]}");
                }
                menu.ActionPrompt.Add($" ");
                menu.ActionPrompt.Add($"Your Choice: ");
                menu.ActionInstructions(menu.ActionPrompt);

                string stringInput = Console.ReadKey().KeyChar.ToString().ToUpper();
                sortingMethod = menu.SortOptions(stringInput, out bool validSort);
                
                valid = validSort;
            }
            menu.StatusUpdate();

            valid = false;
            while (valid == false)
            {
                menu.ActionInstructionsClear();
                menu.ActionPrompt.Clear();
                menu.ActionPrompt.Add($"Sort by {sortingMethod.ToString()}:");
                menu.ActionPrompt.Add($" ");
                menu.ActionPrompt.Add($"A  Sort Ascending");
                menu.ActionPrompt.Add($"D  Sort Descending");
                menu.ActionPrompt.Add($" ");
                menu.ActionPrompt.Add($"Your Choice: ");
                menu.ActionInstructions(menu.ActionPrompt);

                string stringInput = Console.ReadKey().KeyChar.ToString().ToUpper();
                orderMethod = menu.OrderOptions(stringInput,sortingMethod, out bool validOrder);
                valid = validOrder;
            }
            menu.StatusUpdate();

            ItemsList.ItemSorter sorter = new ItemsList.ItemSorter(orderMethod);
            ItemsList.UserShoppingList.Sort(sorter);

            menu.ActionInstructionsClear();
            menu.ActionPrompt.Clear();
            ItemsList.CurrentSorting = sortingMethod;
            list.DisplayCurrentList(ItemsList.UserShoppingList);
            menu.StatusUpdate($"Sorting changed to {sortingMethod.ToString()}, {ListDisplay.Order}", MenuDisplay.ActionStatus.ActionSuccess);
        }

        internal static void Quit(MenuDisplay menu)
        {
            menu.StatusUpdate();
            menu.StatusUpdate($"Thank You! See You Next Time!", MenuDisplay.ActionStatus.ActionSuccess);
            Console.ReadKey();
        }

        internal static int IndexFilter(MenuDisplay menu, string prompt, out int itemNumIndex, out bool exit)
        {
            exit = false;
            itemNumIndex = 0;
            bool valid = false;
            while (valid == false)
            {
                menu.ActionInstructionsClear();
                menu.ActionPrompt.Clear();
                menu.ActionPrompt.Add(prompt);
                menu.ActionPrompt.Add($" ");
                menu.ActionInstructions(menu.ActionPrompt);

                string stringInput = Console.ReadLine();
                if (!int.TryParse(stringInput, out int itemNumInput))
                {
                    menu.StatusUpdate();
                    menu.StatusUpdate($"Please enter a whole Number.", MenuDisplay.ActionStatus.ActionFailure);
                    continue;
                }

                if (itemNumInput > ItemsList.UserShoppingList.Count || itemNumInput <= 0)
                {
                    menu.StatusUpdate();
                    menu.StatusUpdate($"Number {stringInput} is out of range.", MenuDisplay.ActionStatus.ActionFailure);
                    exit = true;
                    break;
                }
                valid = true;
                itemNumIndex = itemNumInput - 1;
            }
            return itemNumIndex;
        }

        internal static void ItemSpecs(Item item, MenuDisplay menu, ListDisplay list)
        {
            menu.ActionInstructionsClear();
            menu.ActionPrompt.Add($"Name: {item.Name}");
            menu.ActionPrompt.Add($"Date Added: {item.Date_Added.ToShortDateString()}");
            menu.ActionPrompt.Add($"Category: {item.category.ToString()}");
            menu.ActionPrompt.Add($"Priority: {item.priority.ToString()}");
            if (item.IsPurchased == true) menu.ActionPrompt.Add($"Purchase Date: {item.Date_Purchased.ToShortDateString()}");
            else menu.ActionPrompt.Add("Purchase Date: Not Purchased");
            menu.ActionPrompt.Add($" ");
            menu.ActionPrompt.Add($"Leave blank to keep current value");
        }

        internal static bool Confirm(string message,MenuDisplay menu)
        {
            bool exit = false;
            bool valid = false;
            while (valid == false)
            {
                menu.ActionInstructionsClear();
                menu.ActionPrompt.Clear();
                menu.ActionPrompt.Add(message);
                menu.ActionPrompt.Add($" ");
                menu.ActionInstructions(menu.ActionPrompt);

                string stringInput = Console.ReadKey().KeyChar.ToString().ToUpper();

                if (stringInput == "Y")
                {
                    break;
                }
                else if (stringInput == "N")
                {
                    menu.StatusUpdate();
                    menu.StatusUpdate(MenuDisplay.DefaultStatus);
                    menu.ActionInstructionsClear();
                    menu.ActionPrompt.Clear();
                    exit = true;
                    break;
                }
                else
                {
                    menu.StatusUpdate();
                    menu.StatusUpdate($"Please input (Y or N)", MenuDisplay.ActionStatus.ActionFailure);
                    continue;
                }
                //valid = true;
            }
            return exit;

        }

        internal static void Save()
        {
            if (SaveLoadMode == true)
            {
                using (FileStream stream = new FileStream(Environment.CurrentDirectory + @"\class.bin", FileMode.Create))
                {
                    ItemsList.Serialize(stream);
                }
            }
        }

        internal static void Load()
        {
            if (SaveLoadMode == true)
            {
                using (FileStream stream = new FileStream(Environment.CurrentDirectory + @"\class.bin", FileMode.Open))
                {
                    ItemsList loaded = new ItemsList(stream);
                }
            }
        }
    }
}
