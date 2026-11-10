using System;
using System.Collections.Generic;
using System.Text;

using iQuest.VendingMachine.PresentationLayer;
using iQuest.VendingMachine.Authentication;

namespace iQuest.VendingMachine.UseCases
{
    internal class BuyUseCase : IUseCase
    {
        private readonly IBuyView buyView;
        private readonly IProductRepository productRepository;
        private readonly IPayment payment;
        private readonly IAuthenticationService authenticationService;

        public string Name => "buy";
        public string Description => "Buy a product";
        public bool CanExecute => !authenticationService.IsUserAuthenticated;

        public BuyUseCase(IBuyView buyView,
                          IProductRepository productRepository,
                          IPayment payment,
                          IAuthenticationService authenticationService)
        {
            this.buyView = buyView ?? throw new ArgumentNullException(nameof(buyView));
            this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            this.payment = payment ?? throw new ArgumentNullException(nameof(payment));
            this.authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        public void Execute()
        {
            if (authenticationService.IsUserAuthenticated)
            {
                throw new InvalidOperationException("Administrators cannot buy products.");
            }
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