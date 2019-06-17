using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListManager
{
    public class Item
    {
        public static readonly Category[] catList = (Category[])Enum.GetValues(typeof(Category));
        public static readonly Priority[] prioList = (Priority[])Enum.GetValues(typeof(Priority));
        public static readonly itemProp[] itemPropList = (itemProp[])Enum.GetValues(typeof(itemProp));

//Item Properties
        public int ItemNum { get; internal set; }
        public string Name { get; internal set; }
        public Category category { get; internal set; }
        public Priority priority { get; internal set; }
        public DateTime Date_Added { get; private set; }
        public bool IsPurchased { get; internal set; }
        public DateTime Date_Purchased { get; internal set; }

//Prop Enums - Helps with logic
        public Item(string name, Category cat, Priority prio)
        {
            Name = name;
            category = cat;
            priority = prio;
            Date_Added = DateTime.Now;
            IsPurchased = false;
        }

        public enum itemProp
        {
            ItemNum,
            Name,
            Category,
            Priority,
            Date_Added,
            Purchased
        }

        public enum Priority
        {
            High,
            Medium,
            Low,
        }

        public static string PriorityOrder(Priority priority)
        {
            string order = string.Empty;
            switch (priority)
            {
                case Priority.High: order = "A"; break;
                case Priority.Medium: order = "B"; break;
                case Priority.Low: order = "C"; break;
            }
            return order;
        }

        public static string PriorityOrder(string priority)
        {
            string order = string.Empty;
            switch (priority)
            {
                case "High": order = "A"; break;
                case "Medium": order = "B"; break;
                case "Low": order = "C"; break;
            }
            return order;
        }

        public enum Category
        {
            Food,
            Clothing,
            Furniture,
            Household,
            Jewelry,
            Utilities,
            Tech,
        }

//Sorting Logic
        public string GetField(ItemSorting sorting)
        {
            return GetField(sorting, out bool notUsed);
        }

        public string GetField(ItemSorting sorting, out bool descending)
        {

            if (sorting.HasFlag(ItemSorting.Descending))
            {
                descending = true;
                sorting = sorting & ~ItemSorting.Descending;
            }
            else descending = false;
            switch (sorting)
            {
                case ItemSorting.Name: return Name;
                case ItemSorting.Category: return category.ToString();
                case ItemSorting.Priority: return priority.ToString();/*High Medium Low*/
                case ItemSorting.DateAdded: return Date_Added.ToShortDateString();
                case ItemSorting.DatePurchased: return Date_Purchased.ToShortDateString();

                default: throw new Exception($"Unknown Sorting: {sorting}");
            }
        }

//Save & Load
        internal void Serialize(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write((int)category);
            writer.Write((int)priority);
            writer.Write(Date_Added.ToBinary());
            writer.Write(IsPurchased);
            writer.Write(Date_Purchased.ToBinary());
        }

        internal Item(BinaryReader reader)
        {
            Name = reader.ReadString();
            category = (Category)reader.ReadInt32();
            priority = (Priority)reader.ReadInt32();
            Date_Added = DateTime.FromBinary(reader.ReadInt64());
            IsPurchased = reader.ReadBoolean();
            Date_Purchased = DateTime.FromBinary(reader.ReadInt64());
        }

//Defines Items
        public override bool Equals(object obj)
        {
            return obj is Item item &&
                string.Equals(Name.ToUpper(), item.Name.ToUpper());
        }
    }
}
