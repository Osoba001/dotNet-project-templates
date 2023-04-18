using Auth.Application.Commands;
using Auth.Application.MediatR;
using Auth.Application.Models;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Responses;

namespace AuthModule.Test.AuthApplication.UserHandlers
{
    public class SetNewPasswordHandlerTest
    {

        private readonly Mock<IServiceWrapper> mockService;
        private readonly SetNewPasswordCommand command = new() { Email = Guid.NewGuid().ToString(), Password = Guid.NewGuid().ToString(), RecoveryPin = 12345 };
        private readonly SetNewPasswordHandler handler = new();

        public SetNewPasswordHandlerTest()
        {
            mockService = new Mock<IServiceWrapper>();

        }
        [Fact]
        public async void HandlerAsync_ReturnsError_WhenUserNotFound()
        {
            //Arrange
            mockService.Setup(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()))
                .ReturnsAsync(It.IsAny<UserModel>());

            //Act
            var result = await handler.HandleAsync(command, mockService.Object);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ReasonPhrase.Should().Contain(UserNotFound);
            mockService.Verify(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once());
        }

        [Fact]
        public async void HandlerAsync_ReturnsError_WhenPinNotMatch()
        {
            //Arrange
            var user = Users[0];
            user.PasswordRecoveryPin = 11234;
            mockService.Setup(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()))
                .ReturnsAsync(user);

            //Act
            var result = await handler.HandleAsync(command, mockService.Object);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ReasonPhrase.Should().Contain(UserNotFound);
            mockService.Verify(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once());
        }

        [Fact]
        public async void HandlerAsync_ReturnsError_WhenSessionExpired()
        {
            //Arrange
            var user = Users[0];
            user.PasswordRecoveryPin = 12345;
            user.AllowSetNewPassword = DateTime.UtcNow.AddMinutes(-11);
            mockService.Setup(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()))
                .ReturnsAsync(user);

            //Act
            var result = await handler.HandleAsync(command, mockService.Object);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ReasonPhrase.Should().Contain(SessionExpired);
            mockService.Verify(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once());
        }

        [Fact]
        public async void HandlerAsync_ReturnsSuccess_WhenCommandMetAllConditions()
        {
            //Arrange
            var user = Users[0];
            user.PasswordRecoveryPin = 12345;
            user.AllowSetNewPassword = DateTime.UtcNow.AddMinutes(-9);
            mockService.Setup(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()))
                .ReturnsAsync(user);
            mockService.Setup(s => s.AuthService.PasswordManager(It.IsAny<string>(), It.IsAny<UserModel>()));
            mockService.Setup(s => s.UserRepo.Update(It.IsAny<UserModel>()))
                .ReturnsAsync(new KOActionResult());

            //Act
            var result = await handler.HandleAsync(command, mockService.Object);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.ReasonPhrase.Should().BeNullOrEmpty();
            mockService.Verify(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once());
            mockService.Verify(s => s.AuthService.PasswordManager(It.IsAny<string>(), It.IsAny<UserModel>()), Times.Once());
            mockService.Verify(s => s.UserRepo.Update(It.IsAny<UserModel>()), Times.Once());
        }
    }
}
