using System;
using System.Collections.Generic;
using System.Text;

using iQuest.VendingMachine.PresentationLayer;
using iQuest.VendingMachine.Authentication;

namespace iQuest.VendingMachine.UseCases
{
    internal class BuyUseCase : IUseCase
    {
        private readonly BuyView buyView;
        private readonly ProductRepository productRepository;
        private readonly Payment payment;
        private readonly AuthenticationService authenticationService;

        public string Name => "buy";
        public string Description => "Buy a product";
        public bool CanExecute => !authenticationService.IsUserAuthenticated;

        public BuyUseCase(BuyView buyView,
                          ProductRepository productRepository,
                          Payment payment,
                          AuthenticationService authenticationService)
        {
            this.buyView = buyView ?? throw new ArgumentNullException(nameof(buyView));
            this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            this.payment = payment ?? throw new ArgumentNullException(nameof(payment));
            this.authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        public void Execute()
        {
            try
            {
                int column = buyView.RequestProduct();

                Product product = productRepository.GetByColumn(column);

                if (product.Quantity == 0)
                {
                    throw new InsufficientStockException();
                }

                payment.Pay(product.ColumnId, (float)product.Price);

                product.Quantity--;

                buyView.DispenseProduct(product.Name);
            }
            catch (CancelException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InvalidColumnException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InsufficientStockException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}