namespace Business.Background.Moq
{
    public class TokenServiceTests
    {
        [Fact]
        public async Task GenerateJWT_Returns_User_With_Updated_Tokens()
        {
            //// Arrange
            //var userId = Guid.NewGuid();
            //var email = "test@example.com";
            //var role = "Admin";
            //var secret = "f08da7b2419340818d81c93f5ffddd77c656ed6b783d4b48861816d455e7d4a0";
            //var validIssuer = "issuer";
            //var validAudience = "audience";
            //var expires = 1;
            //var user = new User();
            //user.UpdateId();
            //user.UpdateEmail(email);
            //user.UpdateRole(role);
            //var identityConfig = new IdentityConfig
            //{
            //    Secret = secret,
            //    ValidIssuer = validIssuer,
            //    ValidAudience = validAudience,
            //    Expires = expires
            //};
            //var userServiceMock = new Mock<IUserService>();
            //userServiceMock.Setup(x => x.GetUserById(It.IsAny<Guid>()))
            //               .ReturnsAsync(user);
            //userServiceMock.Setup(x => x.SaveTokens(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            //               .Returns(Task.CompletedTask);
            //var tokenService = new TokenService(userServiceMock.Object, identityConfig);

            //// Act
            //var result = await tokenService.GenerateJWT(user);

            //// Assert
            //userServiceMock.Verify(x => x.GetUserById(userId), Times.Once);
            //userServiceMock.Verify(x => x.SaveTokens(result, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            //Assert.NotNull(result.AccessToken);
            //Assert.NotNull(result.RefreshToken);
            //Assert.Equal(email, result.Email);
            //Assert.Equal(role, result.Role);
        }
    }
}