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

namespace AuthModule.Test.AuthApplication.Handlers
{
    public class HardDeleteHandlerTest
    {
        private readonly Mock<IServiceWrapper> mockService;
        private readonly HardDeleteCommand command=new() { Id=Guid.NewGuid()};
        private readonly HardDeleteHandler handler=new();
        public HardDeleteHandlerTest()
        {
            mockService = new Mock<IServiceWrapper>();
        }

        [Fact]
        public async void HandlerAsync_ReturnsError_WhenUserNotFound()
        {
            //Arrange
            mockService.Setup(s => s.UserRepo.FindById(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<UserModel>());

            //Act
            var result = await handler.HandleAsync(command, mockService.Object);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ReasonPhrase.Should().Contain("User not found.");
        }
        [Fact]
        public async void HandlerAsync_ReturnsError_WhenDeleteIsNotSuccessful()
        {
            //Arrange
            mockService.Setup(s => s.UserRepo.FindById(It.IsAny<Guid>()))
                .ReturnsAsync(Users[0]);

            var failedActionResult = new KOActionResult();
            failedActionResult.AddError("Error");
            mockService.Setup(s => s.UserRepo.Delete(It.IsAny<UserModel>()))
                .ReturnsAsync(failedActionResult);

            //Act
            var result = await handler.HandleAsync(command, mockService.Object);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async void HandlerAsync_ReturnsSuccess_WhenUserIsFoundAndDeleteIsSuccessful()
        {
            //Arrange
            mockService.Setup(s => s.UserRepo.FindById(It.IsAny<Guid>()))
                .ReturnsAsync(Users[0]);

            var SuccessActionResult = new KOActionResult();
            mockService.Setup(s => s.UserRepo.Delete(It.IsAny<UserModel>()))
                .ReturnsAsync(SuccessActionResult);

            //Act
            var result = await handler.HandleAsync(command, mockService.Object);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
        }
    }
}
