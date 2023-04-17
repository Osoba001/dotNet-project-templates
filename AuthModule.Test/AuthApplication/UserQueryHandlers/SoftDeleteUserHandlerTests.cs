using Auth.Application.CommandAndHandlers.Helper;
using Auth.Application.MediatR;
using Auth.Application.Models;
using Auth.Application.QueryAndHandlers;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace AuthModule.Test.AuthApplication.UserQueryHandlers
{
    public class SoftDeleteUserHandlerTests
    {
        private static List<UserModel> usersModel = Users;
        private readonly Mock<IServiceWrapper> mockService;
        private readonly SoftDeleteUserQuery query = new();
        private readonly SoftDeletedUserHandler handler = new();
        public SoftDeleteUserHandlerTests()
        {
            mockService = new Mock<IServiceWrapper>();
        }
        [Fact]
        public async Task HandlerAsync_ShouldReturnAllUsers()
        {
            // Arrange


            mockService.Setup(s => s.UserRepo.IgnorQueryFilter(It.IsAny<Expression<Func<UserModel,bool>>>()))
                .ReturnsAsync(usersModel);

            // Act
            var result = await handler.HandlerAsync(query, mockService.Object);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(usersModel.ConvertUserList());
        }

    }
}