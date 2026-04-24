using System;
using System.Collections.Generic;
using System.Text;

namespace iQuest.VendingMachine.PresentationLayer
{
    
    internal class StartMenuControl : DisplayBase
    {
        public int Display()
        {
            Console.WriteLine("Press 1 to continue as guest");
            Console.WriteLine("Press 2 to log in");
            Console.Write("Your choice: ");

            while (true)
            {
                string input = Console.ReadLine();
                if (input == "1") return 1;
                if (input == "2") return 2;
                Console.WriteLine("Invalid choice. Please press 1 or 2:");
            }
        }
    }
}

