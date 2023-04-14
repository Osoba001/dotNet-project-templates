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

namespace AuthModule.Test.AuthApplication.Handlers
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
            mockService=new Mock<IServiceWrapper>();
        }
        [Fact]
        public async Task HandleAsync_UserNotFound_ReturnsError()
        {
            // Arrange
            mockService.Setup(s => s.UserRepo.FindById(It.IsAny<Guid>()))
                       .ReturnsAsync((UserModel)null);

            // Act
            var result = await handler.HandleAsync(command, mockService.Object);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ReasonPhrase.Should().Contain("User not found.");
        }

        [Fact]
        public async Task HandleAsync_OldPasswordIncorrect_ReturnsError()
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
            result.ReasonPhrase.Should().Contain("Old password is not correct.");
        }

        //[Fact]
        //public async Task HandleAsync_ValidCommand_ReturnsSuccess()
        //{
        //    //Arrange
        //    mockService.Setup(s => s.UserRepo.FindById(It.IsAny<Guid>()))
        //               .ReturnsAsync(Users[0]);
        //    mockService.Setup(s => s.AuthService.VerifyPassword("password" + 0, Users[0]))
        //               .Returns(true);
        //    mockService.Setup(s => s.UserRepo.Update(Users[0]))
        //        .ReturnsAsync(new Utilities.Responses.KOActionResult());

        //    // Act
        //    var result = await handler.HandleAsync(command, mockService.Object);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.IsSuccess.Should().BeTrue();
        //}
    }

}
