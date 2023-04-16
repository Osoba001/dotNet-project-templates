using Auth.Application.Commands;
using Auth.Application.MediatR;
using Auth.Application.Models;
using FluentAssertions;
using FluentAssertions.Extensions;
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
    public class RecoveryPasswordHandlerTest
    {
        private readonly Mock<IServiceWrapper> mockService;
        private readonly RecoveryPasswordCommand command = new() { Email = Guid.NewGuid().ToString(), RecoveryPin = 12345 };
        private readonly RecoveryPasswordHandler handler = new();
        public RecoveryPasswordHandlerTest()
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
            result.ReasonPhrase.Should().Contain(UserNotFound);
            mockService.Verify(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once());
        }

        [Fact]
        public async void HandlerAsync_ReturnsSessionExpired_WhenPinCreationTimeIsNotLessThan10Mins()
        {
            //Arrange
            var user = Users[0];
            user.RecoveryPinCreationTime = DateTime.UtcNow.AddMinutes(-10);
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
        public async void HandlerAsync_ReturnsIncorrectPin_WhenPinIsNotCorrect()
        {
            //Arrange
            var user = Users[0];
            user.RecoveryPinCreationTime = DateTime.UtcNow.AddMinutes(-9);
            user.PasswordRecoveryPin = 12245;

            mockService.Setup(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()))
                .ReturnsAsync(user);

            //Act
            var result = await handler.HandleAsync(command, mockService.Object);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ReasonPhrase.Should().Contain(IncorrectPin);
            mockService.Verify(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once());
        }

        [Fact]
        public async void HandlerAsync_ReturnsSuccess_WhenPinAndCreationTimeAreInorder()
        {
            //Arrange
            var user = Users[0];
            user.RecoveryPinCreationTime = DateTime.UtcNow.AddMinutes(-9);
            user.PasswordRecoveryPin = 12345;
            mockService.Setup(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()))
                .ReturnsAsync(user);
            mockService.Setup(s => s.UserRepo.Update(It.IsAny<UserModel>()))
               .ReturnsAsync(new KOActionResult());

            //Act
            var result = await handler.HandleAsync(command, mockService.Object);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.ReasonPhrase.Should().BeNullOrEmpty();
            mockService.Verify(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once());
            mockService.Verify(s => s.UserRepo.Update(It.IsAny<UserModel>()), Times.Once());
        }
    }
}
