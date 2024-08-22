using System.Text;
using NuGet.Protocol.Plugins;
using NuGet.Protocol.Plugins.Tests;
using Xunit;

namespace NuGet.Protocol.Tests.Plugins
{
    public class GetAuthenticationCredentialsResponseTests
    {
        [Theory]
        [InlineData("user", "pass", "msg", null, MessageResponseCode.Success)]
        [InlineData("user", "pass", "msg", new string[] { "basic" }, MessageResponseCode.Success)]
        [InlineData("user", "pass", "msg", new string[] { "basic", "digest" }, MessageResponseCode.Success)]
        public void AJsonSerialization_ReturnsCorrectJson(
            string username,
            string password,
            string message,
            string[] authenticationTypes,
            MessageResponseCode messageResponseCode
            )
        {

            var authTypesBuilder = new StringBuilder();
            if (authenticationTypes != null)
            {
                authTypesBuilder.Append("\",\"AuthenticationTypes\":[\"");
                authTypesBuilder.Append(string.Join("\",\"", authenticationTypes));
                authTypesBuilder.Append("\"]");
            }
            else
            {
                authTypesBuilder.Append("\"");
            }

            var expectedJson =
                "{\"Username\":\"" + username
                + "\",\"Password\":\"" + password
                + "\",\"Message\":\"" + message
                + authTypesBuilder.ToString()
                + ",\"ResponseCode\":\"" + messageResponseCode + "\"}";

            var response = new GetAuthenticationCredentialsResponse(
                username,
                password,
                message,
                authenticationTypes,
                messageResponseCode);

            var actualJson = TestUtilities.Serialize(response);
            Assert.Equal(expectedJson, actualJson);
        }

        [Theory]
        [InlineData("{\"Username\":\"user\",\"Password\":\"pass\",\"Message\":\"msg\",\"AuthenticationTypes\":[\"basic\",\"digest\"],\"ResponseCode\":\"Success\"}", "user", "pass", "msg", new string[] { "basic", "digest" }, MessageResponseCode.Success)]
        public void AJsonDeserialization_ReturnsCorrectObject(
            string json,
            string username,
            string password,
            string message,
            string[] authenticationTypes,
            MessageResponseCode messageResponseCode)
        {
            var response = JsonSerializationUtilities.Deserialize<GetAuthenticationCredentialsResponse>(json);
            Assert.Equal(response.Username, username);
            Assert.Equal(response.Password, password);
            Assert.Equal(response.Message, message);
            Assert.Equal(response.AuthenticationTypes, authenticationTypes);
            Assert.Equal(response.ResponseCode, messageResponseCode);

        }
    }
}
