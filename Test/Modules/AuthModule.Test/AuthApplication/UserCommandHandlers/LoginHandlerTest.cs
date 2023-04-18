using Auth.Application.Commands;
using Auth.Application.EventData;
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

namespace AuthModule.Test.AuthApplication.UserHandlers
{
    public class LoginHandlerTest
    {
        private readonly Mock<IServiceWrapper> mockService;
        private readonly LoginCommand command = new() { Email = Guid.NewGuid().ToString(), Password = Guid.NewGuid().ToString() };
        private readonly LoginHandler handler = new();
        public LoginHandlerTest()
        {
            mockService = new Mock<IServiceWrapper>();
        }


        [Fact]
        public async void HandlerAsync_ReturnsError_WhenEmailNotFound()
        {
            //Arrange
            mockService.Setup(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()))
                .ReturnsAsync(It.IsAny<UserModel>());

            //Act
            var result = await handler.HandleAsync(command, mockService.Object);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ReasonPhrase.Should().Contain(InvalidEmailOrPassword);
            mockService.Verify(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once());
        }

        [Fact]
        public async void HandlerAsync_ReturnsError_WhenPasswordIsInvalid()
        {
            //Arrange
            mockService.Setup(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()))
                .ReturnsAsync(Users[0]);
            mockService.Setup(s => s.AuthService.VerifyPassword(It.IsAny<string>(), It.IsAny<UserModel>()))
                .Returns(false);

            //Act
            var result = await handler.HandleAsync(command, mockService.Object);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ReasonPhrase.Should().Contain(InvalidEmailOrPassword);
            mockService.Verify(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once());
            mockService.Verify(s => s.AuthService.VerifyPassword(It.IsAny<string>(), It.IsAny<UserModel>()), Times.Once());
        }
        [Fact]
        public async void HandlerAsync_ReturnsTokens_WhenEmailIsFoundAndPasswordIsInvalid()
        {
            //Arrange
            mockService.Setup(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()))
                .ReturnsAsync(Users[0]);
            mockService.Setup(s => s.AuthService.VerifyPassword(It.IsAny<string>(), It.IsAny<UserModel>()))
                .Returns(true);
            mockService.Setup(s => s.AuthService.TokenManager(It.IsAny<UserModel>()))
                .ReturnsAsync(tokenModel);

            //Act
            var result = await handler.HandleAsync(command, mockService.Object);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(tokenModel.AccessToken);
            mockService.Verify(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once());
            mockService.Verify(s => s.AuthService.VerifyPassword(It.IsAny<string>(), It.IsAny<UserModel>()), Times.Once());
            mockService.Verify(s => s.AuthService.TokenManager(It.IsAny<UserModel>()), Times.Once());
        }
    }
}
