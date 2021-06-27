using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Task15
{
    class Printer
    {
        public static void Print(string message)
        {
            Console.WriteLine(message);
        }

        public static string Read()
        {
            return Console.ReadLine();
        }

        public static void Clear()
        {
            Console.Clear();
        }
    }
}
