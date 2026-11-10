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
        [TestMethod]
        public void HavingLookUseCase_WhenCheckingCanExecute_ThenReturnsTrue()
        {
            Mock<IShelfView> shelfViewMock = new Mock<IShelfView>();
            Mock<IProductRepository> repositoryMock = new Mock<IProductRepository>();

            LookUseCase useCase = new LookUseCase(
                shelfViewMock.Object,
                repositoryMock.Object
            );

            bool result = useCase.CanExecute;

            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void HavingProducts_WhenExecutingLook_ThenProductsAreDisplayed()
        {
            Mock<IShelfView> shelfViewMock = new Mock<IShelfView>();
            Mock<IProductRepository> repositoryMock = new Mock<IProductRepository>();

            List<Product> products = new List<Product>();

            repositoryMock
                .Setup(r => r.GetAll())
                .Returns(products);

            LookUseCase useCase = new LookUseCase(
                shelfViewMock.Object,
                repositoryMock.Object
            );

            useCase.Execute();

            shelfViewMock.Verify(
                s => s.DisplayProducts(products),
                Times.Once()
            );
        }
        [TestMethod]
        public void HavingNullShelfView_WhenCreatingLookUseCase_ThenThrowsArgumentNullException()
        {
            Mock<IProductRepository> repositoryMock = new Mock<IProductRepository>();

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                LookUseCase useCase = new LookUseCase(
                    null,
                    repositoryMock.Object
                );
            });
        }
        [TestMethod]
        public void HavingNullRepository_WhenCreatingLookUseCase_ThenThrowsArgumentException()
        {
            Mock<IShelfView> shelfViewMock = new Mock<IShelfView>();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                LookUseCase useCase = new LookUseCase(
                    shelfViewMock.Object,
                    null
                );
            });
        }
        [TestMethod]
        public void HavingLookUseCase_WhenCheckingName_ThenReturnsLook()
        {
            Mock<IShelfView> shelfViewMock = new Mock<IShelfView>();
            Mock<IProductRepository> repositoryMock =
                new Mock<IProductRepository>();

            LookUseCase useCase = new LookUseCase(
                shelfViewMock.Object,
                repositoryMock.Object
            );

            string result = useCase.Name;

            Assert.AreEqual("look", result);
        }
        [TestMethod]
        public void HavingLookUseCase_WhenCheckingDescription_ThenReturnsCorrectDescription()
        {
            Mock<IShelfView> shelfViewMock = new Mock<IShelfView>();
            Mock<IProductRepository> repositoryMock =
                new Mock<IProductRepository>();

            LookUseCase useCase = new LookUseCase(
                shelfViewMock.Object,
                repositoryMock.Object
            );

            string result = useCase.Description;

            Assert.AreEqual("Display products on shelf", result);
        }
    }
}