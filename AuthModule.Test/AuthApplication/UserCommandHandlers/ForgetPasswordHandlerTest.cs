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
    public class ForgetPasswordHandlerTest
    {
        private readonly Mock<IServiceWrapper> mockService;
        private readonly ForgetPasswordCommand command = new ForgetPasswordCommand() { Email = "email@gmail.com" };
        private readonly ForgetPasswordHandler handler = new();
        public ForgetPasswordHandlerTest()
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
        public async void HandlerAsync_ReturnsError_WhenRecoverPinIsNotSave()
        {
            //Arrange
            mockService.Setup(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()))
                .ReturnsAsync(Users[0]);

            var failedActionResult = new KOActionResult();
            failedActionResult.AddError("Error");
            mockService.Setup(s => s.UserRepo.Update(It.IsAny<UserModel>()))
                .ReturnsAsync(failedActionResult);

            //Act
            var result = await handler.HandleAsync(command, mockService.Object);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            mockService.Verify(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once());
        }

        [Fact]
        public async void HandlerAsync_ReturnsSuccess_WhenUserIsFoundAndRecoverPinIsSave()
        {
            //Arrange
            mockService.Setup(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()))
                .ReturnsAsync(Users[0]);

            var SuccessActionResult = new KOActionResult();
            mockService.Setup(s => s.UserRepo.Update(It.IsAny<UserModel>()))
                .ReturnsAsync(SuccessActionResult);

            //Act
            var result = await handler.HandleAsync(command, mockService.Object);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            mockService.Verify(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once());
            mockService.Verify(s => s.UserRepo.Update(It.IsAny<UserModel>()), Times.Once);
        }
    }
}
