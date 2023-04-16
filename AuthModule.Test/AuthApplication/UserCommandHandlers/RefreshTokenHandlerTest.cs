using Auth.Application.Commands;
using Auth.Application.MediatR;
using Auth.Application.Models;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace AuthModule.Test.AuthApplication.UserHandlers
{
    public class RefreshTokenHandlerTest
    {
        private readonly Mock<IServiceWrapper> mockService;
        private readonly RefreshTokenCommand command = new() { RefreshToken = Guid.NewGuid().ToString(), };
        private readonly RefreshTokenHandler handler = new();
        public RefreshTokenHandlerTest()
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
            result.ReasonPhrase.Should().Contain(InvalidToken);
            mockService.Verify(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once());
        }

        [Fact]
        public async void HandlerAsync_ReturnsError_WhenTokenHasExpired()
        {
            //Arrange
            var user = Users[0];
            user.RefreshTokenExpireTime = DateTime.UtcNow.AddMinutes(-1);
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
        public async void HandlerAsync_ReturnsAccessToken_WhenFor_A_ValidRefreshToken()
        {
            //Arrange
            var user = Users[0];
            user.RefreshTokenExpireTime = DateTime.UtcNow.AddMinutes(1);
            mockService.Setup(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()))
                .ReturnsAsync(user);
            mockService.Setup(s => s.AuthService.TokenManager(It.IsAny<UserModel>()))
                .ReturnsAsync(tokenModel);

            //Act
            var result = await handler.HandleAsync(command, mockService.Object);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.data.Should().Be(tokenModel.AccessToken);
            mockService.Verify(s => s.UserRepo.FindOneByPredicate(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once());
            mockService.Verify(s => s.AuthService.TokenManager(It.IsAny<UserModel>()), Times.Once());
        }
    }
}
