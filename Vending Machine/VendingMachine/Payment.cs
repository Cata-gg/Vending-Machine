using System;
using System.Collections.Generic;
using System.Text;

namespace iQuest.VendingMachine
{
    internal class Payment
    {
        public void Pay(int id, float price)
        {
            Console.WriteLine($"Processing payment for product {id}...");
            Console.WriteLine($"Amount to pay: ${price:F2}");
            Console.WriteLine("Payment successful!");
        }
    }
}
