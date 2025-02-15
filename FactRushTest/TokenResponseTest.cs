using FactRush.Components;

namespace FactRush.Tests.Components
{
    public class TokenResponseTests
    {
        [Fact]
        public void TokenResponse_Should_Initialize_With_Default_Values()
        {
            var tokenResponse = new TokenResponse();

            Assert.Equal(0, tokenResponse.ResponseCode);
            Assert.Equal("", tokenResponse.ResponseMessage);
            Assert.Equal("", tokenResponse.Token);
        }

        [Fact]
        public void TokenResponse_Should_Set_ResponseCode()
        {
            var tokenResponse = new TokenResponse();

            tokenResponse.ResponseCode = 200;

            // Assert
            Assert.Equal(200, tokenResponse.ResponseCode);
        }

        [Fact]
        public void TokenResponse_Should_Set_ResponseMessage()
        {
            // Arrange
            var tokenResponse = new TokenResponse();

            // Act
            tokenResponse.ResponseMessage = "Success";

            // Assert
            Assert.Equal("Success", tokenResponse.ResponseMessage);
        }

        [Fact]
        public void TokenResponse_Should_Set_Token()
        {
            // Arrange
            var tokenResponse = new TokenResponse();

            // Act
            tokenResponse.Token = "abcd1234";

            // Assert
            Assert.Equal("abcd1234", tokenResponse.Token);
        }
    }
}
