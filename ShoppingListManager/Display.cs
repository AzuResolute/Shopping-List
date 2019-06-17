using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListManager
{
    internal abstract class Display
    {
//Display Fields
        internal static int[] setMaxWindows = { 160, 50 };
        internal static int[] setCenterWindows = { setMaxWindows[0]/2, setMaxWindows[1] / 2, };
        internal const int genBuffer = 2;
        internal const int LeftAdjustment = 10;

//Box Builder Process
        internal static string WriteHorBorder(int xAxis, int yAxis,int borderWidth)
        {
            Console.SetCursorPosition(xAxis, yAxis);
            string border = "";
            for (int i = 0; i < borderWidth; ++i)
            {
                border += "*";
            }
            border += "*";
            return border;
        }

        internal static string WriteVertBorder(int xAxis, int yAxis, int edge)
        {
            Console.SetCursorPosition(xAxis, yAxis);
            string border = "*";
            for (int i = 1; i < edge; ++i)
            {
            border += " ";
            }
            border += "*";
            return border;
        }

        internal void BoxCreator(int width, int height, int intWidth = 0, int intHeight = 0, string message1 = "", string message2 = "", string message3 = "")
        {
            Console.Write(WriteHorBorder(intWidth, intHeight, width));
            for (int i = 1; i < height; ++i)
            {
                Console.Write(WriteVertBorder(intWidth, i + intHeight, width));
            }
            Console.Write(WriteHorBorder(intWidth, height+intHeight, width));



            Console.SetCursorPosition((width - message1.Length)/2 + intWidth, height / 2 + intHeight - 1);
            Console.Write(message1);
            Console.SetCursorPosition((width - message2.Length) / 2 + intWidth, height / 2 + intHeight);
            Console.Write(message2);
            Console.SetCursorPosition((width - message3.Length)/2 + intWidth, height / 2 + intHeight + 1);
            Console.Write(message3);
        }


    }
}
