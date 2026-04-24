using System;
using System.Collections.Generic;
using System.Text;

namespace iQuest.VendingMachine.PresentationLayer
{
    internal class ShelfView : DisplayBase
    {
        public void DisplayProducts(IEnumerable<Product> products)
        {
            Console.WriteLine("\n--- Products ---");
            foreach (var p in products)
            {
                Console.WriteLine($"[{p.ColumnId}] {p.Name,-20} ${p.Price:F2}   ({p.Quantity} left)");
            }
            Console.WriteLine("----------------\n");
        }
    }
}
