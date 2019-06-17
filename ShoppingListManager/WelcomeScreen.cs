using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingListManager
{
    internal class WelcomeScreen : Display
    {
        internal static readonly int pause = 3;
        internal static readonly string[] message =
            { "Welcome to the Shopping List Manager",
        $"You have {ItemsList.UserShoppingList.Count} items on your list"};

        internal WelcomeScreen(int height = 13)
        {
            BoxCreator(setMaxWindows[0] - genBuffer, height, genBuffer, 0,message[0],"",message[1]);
            Thread.Sleep(1000 * pause);
            Console.Clear();
        }
    }
}
