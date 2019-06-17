using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListManager
{
    [Flags]
    public enum ItemSorting
    {
        Name = 0x01,
        Category = 0x02,
        Priority = 0x04,
        DateAdded = 0x08,
        DatePurchased = 0x10,
        Descending = 0x20
    }
}
