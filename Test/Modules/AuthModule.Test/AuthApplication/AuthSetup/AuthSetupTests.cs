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
            AuthConfigData authData=new AuthConfigData() {
                AccessTokenLifespanInMins = 20,
                RefreshTokenLifespanInMins=30,
                SecretKey="dhufjijkfooblfl;pdpff0rf85-jbm3-8y"
            };
            var mock = new Mock<IOptions<AuthConfigData>>();
            mock.Setup(m=>m.Value).Returns(authData);
            _configData=mock.Object;
            _authSetup = new Auth.Application.AuthServices.AuthSetup(_userRepoMock.Object, _configData);
        }

        [Fact]
        public void PasswordManager_Should_HashPassword()
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
        public void VerifyPassword_ReturnTrue_WhenPasswordMatches()
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
        public void VerifyPassword_ReturnFalse_WhenPasswordDoesNotMatch()
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

        [Fact]
        public async Task TokenManager_ReturnTokens_And_Update_User()
        {
            // Arrange
            var user = Users[0];
            _userRepoMock.Setup(x => x.Update(It.IsAny<UserModel>()))
                .ReturnsAsync(new Utilities.Responses.KOActionResult());

            // Act
            var result = await _authSetup.TokenManager(user);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
            user.RefreshToken.Should().NotBeNullOrEmpty();
            user.RefreshTokenExpireTime.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(_configData.Value.RefreshTokenLifespanInMins), TimeSpan.FromSeconds(1));
            _userRepoMock.Verify(x => x.Update(user), Times.Once);
        }
    }
}
