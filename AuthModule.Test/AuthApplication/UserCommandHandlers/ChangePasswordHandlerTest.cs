using Auth.Application.Commands;
using Auth.Application.MediatR;
using Auth.Application.Models;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace AuthModule.Test.AuthApplication.UserHandlers
{
    public class ChangePasswordCommandHandlerTests
    {
        private readonly Mock<IServiceWrapper> mockService;
        ChangePasswordCommand command = new()
        {
            Id = Guid.NewGuid(),
            OldPassword = "password" + 0,
            NewPassword = "newPassword"
        };
        ChangePasswordCommandHandler handler = new();
        public ChangePasswordCommandHandlerTests()
        {
            mockService = new Mock<IServiceWrapper>();
        }
        [Fact]
        public async Task HandleAsync_WhenUserNotFound_ReturnsError()
        {
            // Arrange
            mockService.Setup(s => s.UserRepo.FindById(It.IsAny<Guid>()))
                       .ReturnsAsync(It.IsAny<UserModel>());

            // Act
            var result = await handler.HandleAsync(command, mockService.Object);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ReasonPhrase.Should().Contain(UserNotFound);
            mockService.Verify(x => x.UserRepo.FindById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ReturnReturnsError_WhenOldPasswordIncorrect()
        {
            // Arrange

            mockService.Setup(s => s.UserRepo.FindById(It.IsAny<Guid>()))
                       .ReturnsAsync(Users[0]);
            mockService.Setup(s => s.AuthService.VerifyPassword(It.IsAny<string>(), It.IsAny<UserModel>()))
                       .Returns(false);

            // Act
            var result = await handler.HandleAsync(command, mockService.Object);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ReasonPhrase.Should().Contain(InvalidPassword);
            mockService.Verify(x => x.UserRepo.FindById(It.IsAny<Guid>()), Times.Once);
            mockService.Verify(x => x.AuthService.VerifyPassword(It.IsAny<string>(), It.IsAny<UserModel>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ReturnSuccess_OldPasswordIsCorrect()
        {
            //Arrange
            mockService.Setup(s => s.UserRepo.FindById(It.IsAny<Guid>()))
                       .ReturnsAsync(Users[0]);
            mockService.Setup(s => s.AuthService.VerifyPassword(It.IsAny<string>(), It.IsAny<UserModel>()))
                       .Returns(true);
            mockService.Setup(s => s.UserRepo.Update(It.IsAny<UserModel>()))
                .ReturnsAsync(new Utilities.Responses.KOActionResult());


            // Act
            var result = await handler.HandleAsync(command, mockService.Object);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            mockService.Verify(x => x.UserRepo.FindById(It.IsAny<Guid>()), Times.Once);
            mockService.Verify(x => x.AuthService.VerifyPassword(It.IsAny<string>(), It.IsAny<UserModel>()), Times.Once);
            mockService.Verify(x => x.UserRepo.Update(It.IsAny<UserModel>()), Times.Once);
        }
    }

}
