using iQuest.VendingMachine.PresentationLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace iQuest.VendingMachine.UseCases
{
    internal class LookUseCase : IUseCase
    {
        private readonly IShelfView shelfView;
        private readonly IProductRepository productRepository;

        public string Name => "look";
        public string Description => "Display products on shelf";
        public bool CanExecute => true;

        public LookUseCase(IShelfView shelfView, IProductRepository productRepository)
        {
            this.shelfView = shelfView ?? throw new ArgumentNullException(nameof(shelfView));
            this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public void Execute()
        {
            IEnumerable<Product> products = productRepository.GetAll();
            shelfView.DisplayProducts(products);
        }
    }
}

