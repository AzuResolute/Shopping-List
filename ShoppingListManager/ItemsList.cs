using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListManager
{
    class ItemsList : IEnumerable<Item>
    {
//Made them static, but in the future each list will be an instance
        public static readonly List<Item> UserShoppingList = new List<Item>(20);
        public static readonly List<Item> UserPurchasedList = new List<Item>(20);

//Sortable
        private static ItemSorting _currentSorting = ItemSorting.Name;

        public static ItemSorting CurrentSorting
        {
            get
            {
                return _currentSorting;
            }

            set
            {
                ItemSorter sorter = new ItemSorter(value);
                UserShoppingList.Sort(sorter);
                _currentSorting = value;
            }
        }

        internal class ItemSorter : IComparer<Item>
        {
            public ItemSorter(ItemSorting sorting)
            {
                Descending = sorting.HasFlag(ItemSorting.Descending);
                Sorting = sorting & ~ItemSorting.Descending;
            }

            private ItemSorting Sorting { get; set; }

            private bool Descending { get; set; }

            public int Compare(Item x, Item y)
            {
                if (Sorting == ItemSorting.Priority)
                {
                    int p = string.Compare(Item.PriorityOrder(x.GetField(Sorting)), Item.PriorityOrder(y.GetField(Sorting)));
                    return Descending ? -p : p;
                }
                else
                {
                    int r = string.Compare(x.GetField(Sorting), y.GetField(Sorting));
                    return Descending ? -r : r;
                }
            }
        }

//Forloopable
        public IEnumerator<Item> GetEnumerator()
        {
            return ((IEnumerable<Item>)UserShoppingList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Item>)UserShoppingList).GetEnumerator();
        }

//Saves & Loads
        public static void Serialize(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            //writer.Write((int)_currentSorting); /*Not Saving State*/
            writer.Write(UserShoppingList.Count);
            foreach (Item i in UserShoppingList) i.Serialize(writer);
            writer.Write(UserPurchasedList.Count);
            foreach (Item i in UserPurchasedList) i.Serialize(writer);

        }

        public ItemsList(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            int streamCount = reader.ReadInt32();
            for (int i = 0; i < streamCount; ++i)
            {
                UserShoppingList.Add(new Item(reader));
            }
            streamCount = reader.ReadInt32();
            for (int i = 0; i < streamCount; ++i)
            {
                UserPurchasedList.Add(new Item(reader));
            }
        }



    }
}
