
using iQuest.VendingMachine.Authentication;
using iQuest.VendingMachine.PresentationLayer;
using iQuest.VendingMachine.UseCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace VendingMachine.Test
{
    [TestClass]
    public class LoginUseCaseTests
    {
        private Mock<IAuthenticationService> authenticationMock;
        private Mock<ILoginView> loginViewMock;

        [TestInitialize]
        public void Setup()
        {
            authenticationMock = new Mock<IAuthenticationService>();
            loginViewMock = new Mock<ILoginView>();
        }

        [TestMethod]
        public void HavingNotAuthenticatedUser_WhenLogIn_ThenReturnsTrue()
        {
            authenticationMock
                .Setup(x => x.IsUserAuthenticated)
                .Returns(false);

            LoginUseCase useCase = new LoginUseCase(
                authenticationMock.Object,
                loginViewMock.Object
            );

            bool result = useCase.CanExecute;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HavingAuthenticatedUser_WhenLogIn_ThenReturnsFalse()
        {
            authenticationMock
                .Setup(x => x.IsUserAuthenticated)
                .Returns(true);

            LoginUseCase useCase = new LoginUseCase(
                authenticationMock.Object,
                loginViewMock.Object
            );

            bool result = useCase.CanExecute;

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HavingValidPassword_WhenLogIn_ThenCallsAuthenticationService()
        {
            string password = "supercalifragilisticexpialidocious";

            loginViewMock
                .Setup(x => x.AskForPassword())
                .Returns(password);

            LoginUseCase useCase = new LoginUseCase(
                authenticationMock.Object,
                loginViewMock.Object
            );

            useCase.Execute();

            authenticationMock.Verify(
                x => x.Login(password),
                Times.Once()
            );
        }

        [TestMethod]
        public void HavingNullAuthenticationService_WhenCreatingLoginUseCase_ThenThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                LoginUseCase useCase = new LoginUseCase(
                    null,
                    loginViewMock.Object
                );
            });
        }

        [TestMethod]
        public void HavingNullLoginView_WhenCreatingLoginUseCase_ThenThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                LoginUseCase useCase = new LoginUseCase(
                    authenticationMock.Object,
                    null
                );
            });
        }

        [TestMethod]
        public void HavingLoginUseCase_WhenGettingName_ThenReturnsLogin()
        {
            LoginUseCase useCase = new LoginUseCase(
                authenticationMock.Object,
                loginViewMock.Object
            );

            string result = useCase.Name;

            Assert.AreEqual("login", result);
        }

        [TestMethod]
        public void HavingLoginUseCase_WhenGettingDescription_ThenReturnsCorrectDescription()
        {
            LoginUseCase useCase = new LoginUseCase(
                authenticationMock.Object,
                loginViewMock.Object
            );

            string result = useCase.Description;

            Assert.AreEqual("Get access to administration section.", result);
        }
    }
}
