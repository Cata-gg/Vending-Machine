using System;
using System.Collections.Generic;
using System.Text;


namespace iQuest.VendingMachine
{
    internal class ProductRepository : IProductRepository
    {
        private readonly List<Product> products = new List<Product>
        {
            new Product { ColumnId = 1, Name = "Cola",   Price = 1.50, Quantity = 10 },
            new Product { ColumnId = 2, Name = "Water",  Price = 1.00, Quantity = 5  },
            new Product { ColumnId = 3, Name = "Chips",  Price = 2.00, Quantity = 8  },
        };

        public IEnumerable<Product> GetAll() => products;

        public Product GetByColumn(int columnId)
        {
            foreach (Product product in products)
            {
                if (product.ColumnId == columnId)
                {
                    return product;
                }
            }

            throw new InvalidColumnException();
        }
    }
}
