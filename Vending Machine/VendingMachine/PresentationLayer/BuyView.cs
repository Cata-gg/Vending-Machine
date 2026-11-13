
using System;
using System.Collections.Generic;
using System.Text;

namespace iQuest.VendingMachine.PresentationLayer
{
    internal class BuyView : DisplayBase, IBuyView
    {
        public int RequestProduct()
        {
            Console.Write("Enter product column (0 to cancel): ");

            int column;

            while (!int.TryParse(Console.ReadLine(), out column))
            {
                Console.Write("Invalid input. Enter a number: ");
            }

            if (column == 0)
            {
                throw new CancelException();
            }

            return column;
        }

        public void DispenseProduct(string productName)
        {
            Console.WriteLine($"Dispensing {productName}...");
        }

        public void ShowError(string message)
        {
            Console.WriteLine(message);
        }
    }
}
