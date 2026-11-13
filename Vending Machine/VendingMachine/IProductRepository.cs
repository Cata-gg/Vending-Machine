using System.Collections.Generic;

namespace iQuest.VendingMachine
{
    internal interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product GetByColumn(int columnId);
    }
}
