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
        private Mock<IBuyView> buyViewMock;
        private Mock<IProductRepository> repositoryMock;
        private Mock<IPayment> paymentMock;
        private Mock<IAuthenticationService> authenticationMock;

        [TestInitialize]
        public void Setup()
        {
            buyViewMock = new Mock<IBuyView>();
            repositoryMock = new Mock<IProductRepository>();
            paymentMock = new Mock<IPayment>();
            authenticationMock = new Mock<IAuthenticationService>();
        }

        [TestMethod]
        public void HavingNotAuthenticatedUser_WhenTryingToBuy_ThenReturnsTrue()
        {
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
        public void HavingAuthenticatedUser_WhenTryingToBuy_ThenReturnsFalse()
        {
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
        public void HavingAvailableProduct_WhenBuyingProduct_ThenDispensesProduct()
        {
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
        public void HavingAvailableProduct_WhenBuyingProduct_ThenDecreasesQuantity()
        {
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
        public void HavingInsufficientStock_WhenBuyingProduct_ThenDoesNotProcessPayment()
        {
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

            buyViewMock.Verify(
                x => x.ShowError(It.Is<string>(message => !string.IsNullOrWhiteSpace(message))),
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

            Assert.AreEqual(0, product.Quantity);
        }

        [TestMethod]
        public void HavingCancelledPurchase_WhenCancellingPurchase_ThenDoesNotProcessPayment()
        {
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

            buyViewMock.Verify(
                x => x.ShowError(It.Is<string>(message => !string.IsNullOrWhiteSpace(message))),
                Times.Once()
            );

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
        public void HavingInvalidColumn_WhenSelectingProduct_ThenDoesNotProcessPayment()
        {
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

            buyViewMock.Verify(
                x => x.ShowError(It.Is<string>(message => !string.IsNullOrWhiteSpace(message))),
                Times.Once()
            );

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
        public void HavingFailedPayment_WhenBuyingProduct_ThenDoesNotDispenseProduct()
        {
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
        public void HavingBuyUseCase_WhenGettingName_ThenReturnsBuy()
        {
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
        public void HavingBuyUseCase_WhenGettingDescription_ThenReturnsCorrectDescription()
        {
            BuyUseCase useCase = new BuyUseCase(
                buyViewMock.Object,
                repositoryMock.Object,
                paymentMock.Object,
                authenticationMock.Object
            );

            string result = useCase.Description;

            Assert.AreEqual("Buy a product", result);
        }

        [TestMethod]
        public void HavingNullProduct_WhenBuyingProduct_ThenDoesNotProcessPayment()
        {
            buyViewMock
                .Setup(x => x.RequestProduct())
                .Returns(99);

            repositoryMock
                .Setup(x => x.GetByColumn(99))
                .Returns((Product)null);

            BuyUseCase useCase = new BuyUseCase(
                buyViewMock.Object,
                repositoryMock.Object,
                paymentMock.Object,
                authenticationMock.Object
            );

            useCase.Execute();

            buyViewMock.Verify(
                x => x.ShowError(It.Is<string>(message => !string.IsNullOrWhiteSpace(message))),
                Times.Once()
            );

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

    }
}
