using Auth.Application.CommandAndHandlers.Helper;
using Auth.Application.MediatR;
using Auth.Application.Models;
using Auth.Application.QueryAndHandlers;
using FluentAssertions;
using Moq;

namespace AuthModule.Test.AuthApplication.UserQueryHandlers
{
    public class AllUserHandlerTests
    {
        private static List<UserModel> usersModel = Users;
        private readonly Mock<IServiceWrapper> mockService;
        private readonly AllUserQuery query=new AllUserQuery(); 
        private readonly AllUserHandler handler = new AllUserHandler(); 
        public AllUserHandlerTests()
        {
            mockService= new Mock<IServiceWrapper>();
        }
        [Fact]
        public async Task HandlerAsync_ShouldReturnAllUsers()
        {
            // Arrange
            
            
            mockService.Setup(s => s.UserRepo.GetAll()).ReturnsAsync(usersModel);
            
            // Act
            var result = await handler.HandlerAsync(query, mockService.Object);

            // Assert
            result.Should().NotBeNull();
            result.data.Should().NotBeNull();
            result.data.Should().BeEquivalentTo(usersModel.ConvertUserList());
        }

    }
}