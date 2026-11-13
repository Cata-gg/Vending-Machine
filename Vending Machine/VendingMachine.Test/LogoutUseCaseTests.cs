
using iQuest.VendingMachine.Authentication;
using iQuest.VendingMachine.UseCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace VendingMachine.Test
{
    [TestClass]
    public class LogoutUseCaseTests
    {
        private Mock<IAuthenticationService> authenticationMock;

        [TestInitialize]
        public void Setup()
        {
            authenticationMock = new Mock<IAuthenticationService>();
        }

        [TestMethod]
        public void HavingAuthenticatedUser_WhenLogOut_ThenReturnsTrue()
        {
            authenticationMock
                .Setup(x => x.IsUserAuthenticated)
                .Returns(true);

            LogoutUseCase useCase = new LogoutUseCase(
                authenticationMock.Object
            );

            bool result = useCase.CanExecute;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HavingNotAuthenticatedUser_WhenLogOut_ThenReturnsFalse()
        {
            authenticationMock
                .Setup(x => x.IsUserAuthenticated)
                .Returns(false);

            LogoutUseCase useCase = new LogoutUseCase(
                authenticationMock.Object
            );

            bool result = useCase.CanExecute;

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HavingAuthenticatedUser_WhenLogOut_ThenCallsLogoutOnce()
        {
            authenticationMock
                .Setup(x => x.IsUserAuthenticated)
                .Returns(true);

            LogoutUseCase useCase = new LogoutUseCase(
                authenticationMock.Object
            );

            useCase.Execute();

            authenticationMock.Verify(
                x => x.Logout(),
                Times.Once()
            );
        }

        [TestMethod]
        public void HavingNullAuthenticationService_WhenCreatingLogoutUseCase_ThenThrowsArgumentNullException()
        {
            IAuthenticationService authenticationService = null;

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                LogoutUseCase useCase = new LogoutUseCase(authenticationService);
            });
        }

        [TestMethod]
        public void HavingLogoutUseCase_WhenGettingName_ThenReturnsLogout()
        {
            LogoutUseCase useCase = new LogoutUseCase(
                authenticationMock.Object
            );

            string result = useCase.Name;

            Assert.AreEqual("logout", result);
        }

        [TestMethod]
        public void HavingLogoutUseCase_WhenGettingDescription_ThenReturnsCorrectDescription()
        {
            LogoutUseCase useCase = new LogoutUseCase(
                authenticationMock.Object
            );

            string result = useCase.Description;

            Assert.AreEqual("Restrict access to administration section.", result);
        }
    }
}
