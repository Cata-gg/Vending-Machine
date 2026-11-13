
using System;
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
            try
            {
                int column = buyView.RequestProduct();

                Product product = productRepository.GetByColumn(column);

                if (product == null)
                {
                    throw new InvalidColumnException();
                }

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
                buyView.ShowError(ex.Message);
            }
            catch (InvalidColumnException ex)
            {
                buyView.ShowError(ex.Message);
            }
            catch (InsufficientStockException ex)
            {
                buyView.ShowError(ex.Message);
            }
        }
    }
}
