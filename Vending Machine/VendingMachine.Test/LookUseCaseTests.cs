
using iQuest.VendingMachine;
using iQuest.VendingMachine.PresentationLayer;
using iQuest.VendingMachine.UseCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace VendingMachine.Test
{
    [TestClass]
    public class LookUseCaseTests
    {
        private Mock<IShelfView> shelfViewMock;
        private Mock<IProductRepository> repositoryMock;

        [TestInitialize]
        public void Setup()
        {
            shelfViewMock = new Mock<IShelfView>();
            repositoryMock = new Mock<IProductRepository>();
        }

        [TestMethod]
        public void HavingLookUseCase_WhenLook_ThenReturnsTrue()
        {
            LookUseCase useCase = new LookUseCase(
                shelfViewMock.Object,
                repositoryMock.Object
            );

            bool result = useCase.CanExecute;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HavingProducts_WhenLookingAtProducts_ThenProductsAreDisplayed()
        {
            List<Product> products = new List<Product>
            {
                new Product
                {
                    ColumnId = 1,
                    Name = "Cola",
                    Price = 2.5,
                    Quantity = 5
                }
            };

            repositoryMock
                .Setup(r => r.GetAll())
                .Returns(products);

            LookUseCase useCase = new LookUseCase(
                shelfViewMock.Object,
                repositoryMock.Object
            );

            useCase.Execute();

            repositoryMock.Verify(
                r => r.GetAll(),
                Times.Once()
            );

            shelfViewMock.Verify(
                s => s.DisplayProducts(products),
                Times.Once()
            );
        }

        [TestMethod]
        public void HavingEmptyRepository_WhenLookingAtProducts_ThenDisplaysEmptyList()
        {
            List<Product> products = new List<Product>();

            repositoryMock
                .Setup(r => r.GetAll())
                .Returns(products);

            LookUseCase useCase = new LookUseCase(
                shelfViewMock.Object,
                repositoryMock.Object
            );

            useCase.Execute();

            repositoryMock.Verify(
                r => r.GetAll(),
                Times.Once()
            );

            shelfViewMock.Verify(
                s => s.DisplayProducts(products),
                Times.Once()
            );
        }

        [TestMethod]
        public void HavingOutOfStockProduct_WhenLookingAtProducts_ThenDisplaysProduct()
        {
            List<Product> products = new List<Product>
            {
                new Product
                {
                    ColumnId = 1,
                    Name = "Cola",
                    Price = 2.5,
                    Quantity = 0
                }
            };

            repositoryMock
                .Setup(r => r.GetAll())
                .Returns(products);

            LookUseCase useCase = new LookUseCase(
                shelfViewMock.Object,
                repositoryMock.Object
            );

            useCase.Execute();

            repositoryMock.Verify(
                r => r.GetAll(),
                Times.Once()
            );

            shelfViewMock.Verify(
                s => s.DisplayProducts(products),
                Times.Once()
            );
        }

        [TestMethod]
        public void HavingNullShelfView_WhenCreatingLookUseCase_ThenThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                LookUseCase useCase = new LookUseCase(
                    null,
                    repositoryMock.Object
                );
            });
        }

        [TestMethod]
        public void HavingNullRepository_WhenCreatingLookUseCase_ThenThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                LookUseCase useCase = new LookUseCase(
                    shelfViewMock.Object,
                    null
                );
            });
        }

        [TestMethod]
        public void HavingLookUseCase_WhenGettingName_ThenReturnsLook()
        {
            LookUseCase useCase = new LookUseCase(
                shelfViewMock.Object,
                repositoryMock.Object
            );

            string result = useCase.Name;

            Assert.AreEqual("look", result);
        }

        [TestMethod]
        public void HavingLookUseCase_WhenGettingDescription_ThenReturnsCorrectDescription()
        {
            LookUseCase useCase = new LookUseCase(
                shelfViewMock.Object,
                repositoryMock.Object
            );

            string result = useCase.Description;

            Assert.AreEqual("Display products on shelf", result);
        }
    }
}
