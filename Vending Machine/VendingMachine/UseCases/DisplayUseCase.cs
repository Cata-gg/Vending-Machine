using iQuest.VendingMachine.PresentationLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace iQuest.VendingMachine.UseCases
{
    internal class DisplayUseCase : IUseCase
    {
        private readonly MainView mainView;
        private readonly ProductRepository productRepository;
        public string Name => "display";

        public string Description => "Data inside vending";

        public bool CanExecute => true;
        public DisplayUseCase(MainView mainView, ProductRepository productRepository)
        {
            this.mainView = mainView;
            this.productRepository = productRepository;
        }

        public void Execute()
        {
            var products = productRepository.GetAll();
            var control = new ProductsControl(products);
            control.Display();
        }
    }
}
