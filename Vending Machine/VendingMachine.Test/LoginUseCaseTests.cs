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
        [TestMethod]
        public void HavingNotAuthenticatedUser_WhenCheckingCanExecute_ThenReturnsTrue()
        {
            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            Mock<ILoginView> loginViewMock =
                new Mock<ILoginView>();

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
        public void HavingAuthenticatedUser_WhenCheckingCanExecute_ThenReturnsFalse()
        {
            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            Mock<ILoginView> loginViewMock =
                new Mock<ILoginView>();

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
        public void HavingValidPassword_WhenExecutingLogin_ThenCallsAuthenticationService()
        {
            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            Mock<ILoginView> loginViewMock =
                new Mock<ILoginView>();

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
            Mock<ILoginView> loginViewMock = new Mock<ILoginView>();

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
            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                LoginUseCase useCase = new LoginUseCase(
                    authenticationMock.Object,
                    null
                );
            });
        }
        [TestMethod]
        public void HavingLoginUseCase_WhenCheckingName_ThenReturnsLogin()
        {
            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            Mock<ILoginView> loginViewMock =
                new Mock<ILoginView>();

            LoginUseCase useCase = new LoginUseCase(
                authenticationMock.Object,
                loginViewMock.Object
            );

            string result = useCase.Name;

            Assert.AreEqual("login", result);
        }
        [TestMethod]
        public void HavingLoginUseCase_WhenCheckingDescription_ThenReturnsCorrectDescription()
        {
            Mock<IAuthenticationService> authenticationMock =
                new Mock<IAuthenticationService>();

            Mock<ILoginView> loginViewMock =
                new Mock<ILoginView>();

            LoginUseCase useCase = new LoginUseCase(
                authenticationMock.Object,
                loginViewMock.Object
            );

            string result = useCase.Description;

            Assert.AreEqual("Get access to administration section.", result);
        }


    }

}