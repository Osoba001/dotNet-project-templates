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
using Utilities.Constants;
using Utilities.Responses;

namespace AuthModule.Test.AuthApplication.UserHandlers
{
    public class CreateUserHandlerTest
    {
        private readonly Mock<IServiceWrapper> mockService;
        CreateUserCommand command = new CreateUserCommand { Email = "john@example.com", Password = "password", Role = Role.User, UserName = "john" };
        CreateUserHandler handler = new();
        public CreateUserHandlerTest()
        {
            mockService = new Mock<IServiceWrapper>();
        }
        [Fact]
        public async Task HandleAsync_ReturnError_WhenEmailAlreadyExists()
        {
            // Arrange
            //var service = Mock.Of<IServiceWrapper>(s => s.UserRepo.FindOneByPredicate(It.IsAny<Func<UserModel, bool>>()) == Task.FromResult(existingUser));
            mockService.Setup(x => x.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()))
                .ReturnsAsync(Users[0]);

            // Act
            var result = await handler.HandleAsync(command, mockService.Object);

            // Assert
            result.Should().NotBeNull();
            result.ReasonPhrase.Should().Contain(EmailAlreadyExist);
            mockService.Verify(x => x.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once());
        }

        [Fact]
        public async Task HandleAsync_ReturnToken_WhenUserCreatedSuccessfully()
        {
            // Arrange
            mockService.Setup(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()))
                .ReturnsAsync(It.IsAny<UserModel>());
            mockService.Setup(s => s.UserRepo.AddAndReturn(It.IsAny<UserModel>()))
                .ReturnsAsync(new KOActionResult<UserModel> { Item = Users[0] });
            mockService.Setup(s => s.AuthService.TokenManager(It.IsAny<UserModel>()))
                .ReturnsAsync(tokenModel);

            var handler = new CreateUserHandler();

            // Act
            var result = await handler.HandleAsync(command, mockService.Object);

            // Assert
            result.Should().NotBeNull();
            var resToken = result.Data as string;
            resToken.Should().NotBeNull();
            resToken.Should().BeEquivalentTo(tokenModel.AccessToken);
            result.ReasonPhrase.Should().BeNullOrEmpty();
            mockService.Verify(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once());
            mockService.Verify(s => s.UserRepo.AddAndReturn(It.IsAny<UserModel>()), Times.Once);
            mockService.Verify(s => s.AuthService.TokenManager(It.IsAny<UserModel>()), Times.Once);
        }
    }
}
