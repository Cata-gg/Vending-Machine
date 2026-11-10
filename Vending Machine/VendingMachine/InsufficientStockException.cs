using System;
using System.Collections.Generic;
using System.Text;

namespace iQuest.VendingMachine
{
    internal class InsufficientStockException : Exception
    {
        public InsufficientStockException()
            : base("Product is out of stock.")
        {
        }
    }
}