using System.Collections.Generic;
using iQuest.VendingMachine.Authentication;
using iQuest.VendingMachine.PresentationLayer;
using iQuest.VendingMachine.UseCases;

namespace iQuest.VendingMachine
{
    internal class Bootstrapper
    {
        public void Run()
        {
            VendingMachineApplication vendingMachineApplication = BuildApplication();
            vendingMachineApplication.Run();
        }

        private static VendingMachineApplication BuildApplication()
        {   
            MainView mainView = new MainView();
            ShelfView shelfView = new ShelfView();
            LoginView loginView = new LoginView();
            BuyView buyView = new BuyView();

            AuthenticationService authenticationService = new AuthenticationService();
            ProductRepository productRepository = new ProductRepository();
            Payment payment = new Payment();

            List<IUseCase> useCases = new List<IUseCase>
            {   
                new LookUseCase(shelfView, productRepository),
                new BuyUseCase(buyView, productRepository, payment, authenticationService),
                new LoginUseCase(authenticationService, loginView),
                new LogoutUseCase(authenticationService),
            };

            return new VendingMachineApplication(useCases, mainView);
        }
    }
}