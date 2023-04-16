using Auth.Application.Models;
using Auth.Application.RepositoryContracts;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

namespace AuthModule.Test.AuthApplication.AuthSetup
{
    public class AuthSetupTests
    {
        private readonly Mock<IUserRepo> _userRepoMock;
        private readonly IOptions<AuthConfigData> _configData;
        private readonly Auth.Application.AuthServices.AuthSetup _authSetup;
        public AuthSetupTests()
        {
            _userRepoMock = new Mock<IUserRepo>();
            _configData = new Mock<IOptions<AuthConfigData>>().Object;
            _authSetup = new Auth.Application.AuthServices.AuthSetup(_userRepoMock.Object, _configData);
        }

        [Fact]
        public void PasswordManager_Should_Hash_Password()
        {
            // Arrange
            var password = "password";
            var user = Users[0];

            // Act
            _authSetup.PasswordManager(password, user);

            // Assert
            user.PasswordHash.Should().NotBeNullOrEmpty();
            user.PasswordSalt.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void VerifyPassword_Should_Return_True_If_Password_Matches()
        {
            // Arrange
            var password = "password";
            var user = Users[0];
            _authSetup.PasswordManager(password, user);

            // Act
            var result = _authSetup.VerifyPassword(password, user);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_Should_Return_False_If_Password_Does_Not_Match()
        {
            // Arrange
            var password = "password";
            var user = Users[0];
            _authSetup.PasswordManager(password, user);

            // Act
            var result = _authSetup.VerifyPassword("wrongpassword", user);

            // Assert
            result.Should().BeFalse();
        }

        //[Fact]
        //public async Task TokenManager_Should_Return_Tokens_And_Update_User()
        //{
        //    // Arrange
        //    var user = Users[0];
        //    _userRepoMock.Setup(x => x.Update(It.IsAny<UserModel>()))
        //        .ReturnsAsync(new Utilities.Responses.KOActionResult());

        //    // Act
        //    var result = await _authSetup.TokenManager(user);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.AccessToken.Should().NotBeNullOrEmpty();
        //    result.RefreshToken.Should().NotBeNullOrEmpty();
        //    user.RefreshToken.Should().NotBeNullOrEmpty();
        //    user.RefreshTokenExpireTime.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(_configData.Value.RefreshTokenLifespanInMins),TimeSpan.FromSeconds(1));
        //    _userRepoMock.Verify(x => x.Update(user), Times.Once);
        //}
    }
}
