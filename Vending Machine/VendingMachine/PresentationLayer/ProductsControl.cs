using System;
using System.Collections.Generic;
using System.Text;


namespace iQuest.VendingMachine.PresentationLayer
{
    internal class ProductsControl : DisplayBase
    {
        private readonly IEnumerable<Product> products;

        public ProductsControl(IEnumerable<Product> products)
        {
            this.products = products;
        }

        public void Display()
        {
            Console.WriteLine("Available products:");
            Console.WriteLine(new string('-', 79));

            foreach (var product in products)
            {
                Console.WriteLine($"[{product.Id}] {product.Name,-20} ${product.Price:F2}   ({product.Quantity} left)");
            }

            Console.WriteLine(new string('-', 79));
        }
    }
}
