using Auth.Application.MediatR;
using Auth.Application.Models;
using Auth.Application.QueryAndHandlers;
using Auth.Application.Response;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthModule.Test.AuthApplication.UserQueryHandlers
{
    public class UserByIdQueryHandlerTests
    {
        private static UserModel User = Users[0];
        private readonly Mock<IServiceWrapper> mockService;
        private readonly UserByIdQueryHadler handler = new UserByIdQueryHadler();
        private readonly UserById query = new UserById() { Id = User.Id };
        private readonly UserResponse userResponse = User;

        public UserByIdQueryHandlerTests()
        {
            mockService = new Mock<IServiceWrapper>();
        }

        [Fact]
        public async Task HandlerAsync_WhenUserDoesNotExist_ReturnsError()
        {
            // Arrange
            var mockService = new Mock<IServiceWrapper>();
            mockService.Setup(s => s.UserRepo.FindById(query.Id)).ReturnsAsync(It.IsAny<UserModel>());

            // Act
            var result = await handler.HandlerAsync(query, mockService.Object);

            // Assert
            result.Should().NotBeNull();
            result.ReasonPhrase.Should().Contain(UserNotFound);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task HandlerAsync_WhenUserExists_ReturnsUser()
        {
            // Arrange
            mockService.Setup(s => s.UserRepo.FindById(query.Id)).ReturnsAsync(User);

            // Act
            var result = await handler.HandlerAsync(query, mockService.Object);

            // Assert
            result.Should().NotBeNull();
            result.ReasonPhrase.Should().BeEmpty();
            result.Data.Should().BeEquivalentTo(userResponse);
        }


    }
}