using iQuest.VendingMachine;
using iQuest.VendingMachine.Authentication;
using iQuest.VendingMachine.PresentationLayer;
using iQuest.VendingMachine.UseCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace VendingMachine.Test
{
    [TestClass]
    public class BuyUseCaseTests
    {
        [TestMethod]
        public void HavingNotAuthenticatedUser_WhenCheckingCanExecute_ThenReturnsTrue()
        {
            Mock<IBuyView> buyViewMock = new Mock<IBuyView>();

            Mock<IProductRepository> repositoryMock =
                new Mock<IProductRepository>();

            Mock<IPayment> paymentMock = new Mock<IPayment>();

            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            authenticationMock
                .Setup(x => x.IsUserAuthenticated)
                .Returns(false);

            BuyUseCase useCase = new BuyUseCase(
                buyViewMock.Object,
                repositoryMock.Object,
                paymentMock.Object,
                authenticationMock.Object
            );

            bool result = useCase.CanExecute;

            Assert.IsTrue(result);
        }
        [TestMethod]
        public void HavingAuthenticatedUser_WhenCheckingCanExecute_ThenReturnsFalse()
        {
            Mock<IBuyView> buyViewMock = new Mock<IBuyView>();

            Mock<IProductRepository> repositoryMock =
                new Mock<IProductRepository>();

            Mock<IPayment> paymentMock = new Mock<IPayment>();

            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            authenticationMock
                .Setup(x => x.IsUserAuthenticated)
                .Returns(true);

            BuyUseCase useCase = new BuyUseCase(
                buyViewMock.Object,
                repositoryMock.Object,
                paymentMock.Object,
                authenticationMock.Object
            );

            bool result = useCase.CanExecute;

            Assert.IsFalse(result);
        }
        [TestMethod]
        public void HavingAvailableProduct_WhenExecutingBuy_ThenDispensesProduct()
        {
            Mock<IBuyView> buyViewMock = new Mock<IBuyView>();

            Mock<IProductRepository> repositoryMock =
                new Mock<IProductRepository>();

            Mock<IPayment> paymentMock = new Mock<IPayment>();

            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            Product product = new Product
            {
                ColumnId = 1,
                Name = "Cola",
                Price = 2.5,
                Quantity = 5
            };

            buyViewMock
                .Setup(x => x.RequestProduct())
                .Returns(1);

            repositoryMock
                .Setup(x => x.GetByColumn(1))
                .Returns(product);

            BuyUseCase useCase = new BuyUseCase(
                buyViewMock.Object,
                repositoryMock.Object,
                paymentMock.Object,
                authenticationMock.Object
            );

            useCase.Execute();

            paymentMock.Verify(
                x => x.Pay(1, 2.5f),
                Times.Once()
            );

            buyViewMock.Verify(
                x => x.DispenseProduct("Cola"),
                Times.Once()
            );
        }
        [TestMethod]
        public void HavingAvailableProduct_WhenExecutingBuy_ThenDecreasesQuantity()
        {
            Mock<IBuyView> buyViewMock = new Mock<IBuyView>();

            Mock<IProductRepository> repositoryMock =
                new Mock<IProductRepository>();

            Mock<IPayment> paymentMock = new Mock<IPayment>();

            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            Product product = new Product
            {
                ColumnId = 1,
                Name = "Cola",
                Price = 2,
                Quantity = 5
            };

            buyViewMock
                .Setup(x => x.RequestProduct())
                .Returns(1);

            repositoryMock
                .Setup(x => x.GetByColumn(1))
                .Returns(product);

            BuyUseCase useCase = new BuyUseCase(
                buyViewMock.Object,
                repositoryMock.Object,
                paymentMock.Object,
                authenticationMock.Object
            );

            useCase.Execute();

            Assert.AreEqual(4, product.Quantity);
        }
        [TestMethod]
        public void HavingInsufficientStock_WhenExecutingBuy_ThenDoesNotProcessPayment()
        {
            Mock<IBuyView> buyViewMock = new Mock<IBuyView>();

            Mock<IProductRepository> repositoryMock =
                new Mock<IProductRepository>();

            Mock<IPayment> paymentMock = new Mock<IPayment>();

            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            Product product = new Product
            {
                ColumnId = 1,
                Name = "Cola",
                Price = 2,
                Quantity = 0
            };

            buyViewMock
                .Setup(x => x.RequestProduct())
                .Returns(1);

            repositoryMock
                .Setup(x => x.GetByColumn(1))
                .Returns(product);

            BuyUseCase useCase = new BuyUseCase(
                buyViewMock.Object,
                repositoryMock.Object,
                paymentMock.Object,
                authenticationMock.Object
            );

            useCase.Execute();

            paymentMock.Verify(
                x => x.Pay(It.IsAny<int>(), It.IsAny<float>()),
                Times.Never()
            );

            buyViewMock.Verify(
                x => x.DispenseProduct(It.IsAny<string>()),
                Times.Never()
            );

            Assert.AreEqual(0, product.Quantity);
        }
        [TestMethod]
        public void HavingCancelledPurchase_WhenExecutingBuy_ThenDoesNotProcessPayment()
        {
            Mock<IBuyView> buyViewMock = new Mock<IBuyView>();

            Mock<IProductRepository> repositoryMock =
                new Mock<IProductRepository>();

            Mock<IPayment> paymentMock = new Mock<IPayment>();

            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            buyViewMock
                .Setup(x => x.RequestProduct())
                .Throws(new CancelException());

            BuyUseCase useCase = new BuyUseCase(
                buyViewMock.Object,
                repositoryMock.Object,
                paymentMock.Object,
                authenticationMock.Object
            );

            useCase.Execute();

            repositoryMock.Verify(
                x => x.GetByColumn(It.IsAny<int>()),
                Times.Never()
            );

            paymentMock.Verify(
                x => x.Pay(It.IsAny<int>(), It.IsAny<float>()),
                Times.Never()
            );

            buyViewMock.Verify(
                x => x.DispenseProduct(It.IsAny<string>()),
                Times.Never()
            );
        }
        [TestMethod]
        public void HavingInvalidColumn_WhenExecutingBuy_ThenDoesNotProcessPayment()
        {
            Mock<IBuyView> buyViewMock = new Mock<IBuyView>();

            Mock<IProductRepository> repositoryMock =
                new Mock<IProductRepository>();

            Mock<IPayment> paymentMock = new Mock<IPayment>();

            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            buyViewMock
                .Setup(x => x.RequestProduct())
                .Returns(99);

            repositoryMock
                .Setup(x => x.GetByColumn(99))
                .Throws(new InvalidColumnException());

            BuyUseCase useCase = new BuyUseCase(
                buyViewMock.Object,
                repositoryMock.Object,
                paymentMock.Object,
                authenticationMock.Object
            );

            useCase.Execute();

            repositoryMock.Verify(
                x => x.GetByColumn(99),
                Times.Once()
            );

            paymentMock.Verify(
                x => x.Pay(It.IsAny<int>(), It.IsAny<float>()),
                Times.Never()
            );

            buyViewMock.Verify(
                x => x.DispenseProduct(It.IsAny<string>()),
                Times.Never()
            );
        }
        [TestMethod]
        public void HavingNullBuyView_WhenCreatingBuyUseCase_ThenThrowsArgumentNullException()
        {
            Mock<IProductRepository> repositoryMock =
                new Mock<IProductRepository>();

            Mock<IPayment> paymentMock =
                new Mock<IPayment>();

            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                BuyUseCase useCase = new BuyUseCase(
                    null,
                    repositoryMock.Object,
                    paymentMock.Object,
                    authenticationMock.Object
                );
            });
        }
        [TestMethod]
        public void HavingNullProductRepository_WhenCreatingBuyUseCase_ThenThrowsArgumentNullException()
        {

            Mock<IBuyView> buyViewMock =
                new Mock<IBuyView>();

            Mock<IPayment> paymentMock =
                new Mock<IPayment>();

            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                BuyUseCase useCase = new BuyUseCase(
                    buyViewMock.Object,
                    null,
                    paymentMock.Object,
                    authenticationMock.Object
                );
            });
        }
        [TestMethod]
        public void HavingNullPayment_WhenCreatingBuyUseCase_ThenThrowsArgumentNullException()
        {
            Mock<IBuyView> buyViewMock =
                new Mock<IBuyView>();

            Mock<IProductRepository> repositoryMock =
                new Mock<IProductRepository>();

            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                BuyUseCase useCase = new BuyUseCase(
                    buyViewMock.Object,
                    repositoryMock.Object,
                    null,
                    authenticationMock.Object
                );
            });
        }
        [TestMethod]
        public void HavingNullAuthenticationService_WhenCreatingBuyUseCase_ThenThrowsArgumentNullException()
        {
 
            Mock<IBuyView> buyViewMock =
                new Mock<IBuyView>();

            Mock<IProductRepository> repositoryMock =
                new Mock<IProductRepository>();

            Mock<IPayment> paymentMock =
                new Mock<IPayment>();

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                BuyUseCase useCase = new BuyUseCase(
                    buyViewMock.Object,
                    repositoryMock.Object,
                    paymentMock.Object,
                    null
                );
            });
        }
        [TestMethod]
        public void HavingFailedPayment_WhenExecutingBuy_ThenDoesNotDispenseProduct()
        {
            Mock<IBuyView> buyViewMock = new Mock<IBuyView>();

            Mock<IProductRepository> repositoryMock =
                new Mock<IProductRepository>();

            Mock<IPayment> paymentMock = new Mock<IPayment>();

            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            Product product = new Product
            {
                ColumnId = 1,
                Name = "Cola",
                Price = 2,
                Quantity = 5
            };

            buyViewMock
                .Setup(x => x.RequestProduct())
                .Returns(1);

            repositoryMock
                .Setup(x => x.GetByColumn(1))
                .Returns(product);

            paymentMock
                .Setup(x => x.Pay(1, 2f))
                .Throws(new Exception("Payment failed"));

            BuyUseCase useCase = new BuyUseCase(
                buyViewMock.Object,
                repositoryMock.Object,
                paymentMock.Object,
                authenticationMock.Object
            );

            Assert.ThrowsException<Exception>(() => useCase.Execute());

            buyViewMock.Verify(
                x => x.DispenseProduct(It.IsAny<string>()),
                Times.Never()
            );

            Assert.AreEqual(5, product.Quantity);
        }
        [TestMethod]
        public void HavingAuthenticatedAdmin_WhenExecutingBuy_ThenThrowsInvalidOperationException()
        {
            Mock<IBuyView> buyViewMock = new Mock<IBuyView>();

            Mock<IProductRepository> repositoryMock =
                new Mock<IProductRepository>();

            Mock<IPayment> paymentMock = new Mock<IPayment>();

            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            authenticationMock
                .Setup(x => x.IsUserAuthenticated)
                .Returns(true);

            BuyUseCase useCase = new BuyUseCase(
                buyViewMock.Object,
                repositoryMock.Object,
                paymentMock.Object,
                authenticationMock.Object
            );

            Assert.ThrowsException<InvalidOperationException>(
                () => useCase.Execute()
            );

            buyViewMock.Verify(
                x => x.RequestProduct(),
                Times.Never()
            );

            paymentMock.Verify(
                x => x.Pay(It.IsAny<int>(), It.IsAny<float>()),
                Times.Never()
            );
        }
        [TestMethod]
        public void HavingBuyUseCase_WhenCheckingName_ThenReturnsBuy()
        {
            Mock<IBuyView> buyViewMock = new Mock<IBuyView>();
            Mock<IProductRepository> repositoryMock = new Mock<IProductRepository>();
            Mock<IPayment> paymentMock = new Mock<IPayment>();
            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            BuyUseCase useCase = new BuyUseCase(
                buyViewMock.Object,
                repositoryMock.Object,
                paymentMock.Object,
                authenticationMock.Object
            );

            string result = useCase.Name;

            Assert.AreEqual("buy", result);
        }
        [TestMethod]
        public void HavingBuyUseCase_WhenCheckingDescription_ThenReturnsCorrectDescription()
        {
            Mock<IBuyView> buyViewMock = new Mock<IBuyView>();
            Mock<IProductRepository> repositoryMock = new Mock<IProductRepository>();
            Mock<IPayment> paymentMock = new Mock<IPayment>();
            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            BuyUseCase useCase = new BuyUseCase(
                buyViewMock.Object,
                repositoryMock.Object,
                paymentMock.Object,
                authenticationMock.Object
            );

            string result = useCase.Description;

            Assert.AreEqual("Buy a product", result);
        }
    }
}