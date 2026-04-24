using System;
using System.Collections.Generic;
using System.Text;


namespace iQuest.VendingMachine
{
    internal class ProductRepository
    {
        private readonly List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Cola",   Price = 1.50, Quantity = 10 },
            new Product { Id = 2, Name = "Water",  Price = 1.00, Quantity = 5  },
            new Product { Id = 3, Name = "Chips",  Price = 2.00, Quantity = 8  },
        };

        public IEnumerable<Product> GetAll() => products;
    }
}
