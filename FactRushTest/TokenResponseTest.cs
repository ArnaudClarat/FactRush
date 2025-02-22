using FactRush.Models;

namespace FactRushTest
{
    public class TokenResponseTests
    {
        [Fact]
        public void TokenResponse_Should_Initialize_With_Default_Values()
        {
            // Arrange: Create a new TokenResponse instance without setting any properties.
            var tokenResponse = new TokenResponse();

            // Assert: Verify that the default values match the expected ones.
            Assert.Equal(0, tokenResponse.ResponseCode);
            Assert.Equal("", tokenResponse.ResponseMessage);
            Assert.Equal("", tokenResponse.Token);
        }

        [Fact]
        public void TokenResponse_Should_Set_ResponseCode()
        {
            // Arrange: Create a new TokenResponse instance.
            var tokenResponse = new TokenResponse
            {
                // Act: Set the ResponseCode property.
                ResponseCode = 200
            };

            // Assert: Verify that the ResponseCode property returns the expected value.
            Assert.Equal(200, tokenResponse.ResponseCode);
        }

        [Fact]
        public void TokenResponse_Should_Set_ResponseMessage()
        {
            // Arrange: Create a new TokenResponse instance.
            var tokenResponse = new TokenResponse
            {
                // Act: Set the ResponseMessage property.
                ResponseMessage = "Success"
            };

            // Assert: Verify that the ResponseMessage property returns the expected value.
            Assert.Equal("Success", tokenResponse.ResponseMessage);
        }

        [Fact]
        public void TokenResponse_Should_Set_Token()
        {
            // Arrange: Create a new TokenResponse instance.
            var tokenResponse = new TokenResponse
            {
                // Act: Set the Token property.
                Token = "abcd1234"
            };

            // Assert: Verify that the Token property returns the expected value.
            Assert.Equal("abcd1234", tokenResponse.Token);
        }
    }
}
