using iQuest.VendingMachine.PresentationLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace iQuest.VendingMachine.UseCases
{
    internal class LookUseCase : IUseCase
    {
        private readonly ShelfView shelfView;
        private readonly ProductRepository productRepository;

        public string Name => "look";
        public string Description => "Display products on shelf";
        public bool CanExecute => true;

        public LookUseCase(ShelfView shelfView, ProductRepository productRepository)
        {
            this.shelfView = shelfView ?? throw new ArgumentNullException(nameof(shelfView));
            this.productRepository = productRepository ?? throw new ArgumentException(nameof(productRepository));
        }

        public void Execute()
        {
            IEnumerable<Product> products = productRepository.GetAll();
            shelfView.DisplayProducts(products);
        }
    }
}

