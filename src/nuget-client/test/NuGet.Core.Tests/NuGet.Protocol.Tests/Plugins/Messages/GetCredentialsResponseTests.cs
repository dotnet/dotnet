// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class GetCredentialsResponseTests
    {
        [Fact]
        public void Constructor_ThrowsForUndefinedResponseCode()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetCredentialsResponse(
                    (MessageResponseCode)int.MinValue,
                    username: "a",
                    password: "b"));

            Assert.Equal("responseCode", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("a")]
        public void Constructor_AllowsAnyUsername(string username)
        {
            new GetCredentialsResponse(MessageResponseCode.Success, username, password: "b");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("a")]
        public void Constructor_AllowsAnyPassword(string password)
        {
            new GetCredentialsResponse(MessageResponseCode.Success, username: "a", password: password);
        }

        [Theory]
        [InlineData(null)]
        // double arrays to work around C# weirdness with passing arrays as the only argument to a 'params' parameter
        [InlineData(new object[] { new string[0] })]
        [InlineData(new object[] { new[] { "a" } })]
        [InlineData(new object[] { new[] { "a", "b" } })]
        public void Constructor_AllowsAnyAuthTypes(string[] authTypes)
        {
            new GetCredentialsResponse(MessageResponseCode.Success, username: "a", password: "b", authenticationTypes: authTypes);
        }

        [Theory]
        [InlineData(MessageResponseCode.Success, "a", "b", new[] { "basic" })]
        [InlineData(MessageResponseCode.NotFound, null, null, null)]
        public void Constructor_InitializesProperties(
            MessageResponseCode responseCode,
            string username,
            string password,
            string[] authTypes)
        {
            var response = new GetCredentialsResponse(responseCode, username, password, authTypes);

            Assert.Equal(responseCode, response.ResponseCode);
            Assert.Equal(username, response.Username);
            Assert.Equal(password, response.Password);
            Assert.Equal(authTypes, response.AuthenticationTypes);
        }

        [Theory]
        [InlineData(MessageResponseCode.NotFound, null, null, null, "{\"ResponseCode\":\"NotFound\"}")]
        [InlineData(MessageResponseCode.Success, "a", "b", null, "{\"Password\":\"b\",\"ResponseCode\":\"Success\",\"Username\":\"a\"}")]
        [InlineData(MessageResponseCode.Success, "a", "b", new[] { "basic", "negotiate" }, "{\"Password\":\"b\",\"ResponseCode\":\"Success\",\"Username\":\"a\",\"AuthenticationTypes\":[\"basic\",\"negotiate\"]}")]
        public void JsonSerialization_ReturnsCorrectJson(
            MessageResponseCode responseCode,
            string username,
            string password,
            string[] authTypes,
            string expectedJson)
        {
            var response = new GetCredentialsResponse(responseCode, username, password, authTypes);

            var actualJson = TestUtilities.Serialize(response);

            Assert.Equal(expectedJson, actualJson);
        }

        [Theory]
        [InlineData("{\"ResponseCode\":\"NotFound\"}", MessageResponseCode.NotFound, null, null, null)]
        [InlineData("{\"Password\":\"a\",\"ResponseCode\":\"Success\",\"Username\":\"b\"}", MessageResponseCode.Success, "b", "a", null)]
        [InlineData("{\"Password\":\"a\",\"ResponseCode\":\"Success\",\"Username\":\"b\",\"AuthenticationTypes\":[\"negotiate\",\"NTLM\"]}", MessageResponseCode.Success, "b", "a", new[] { "negotiate", "NTLM" })]
        public void JsonDeserialization_ReturnsCorrectObject(
            string json,
            MessageResponseCode responseCode,
            string username,
            string password,
            string[] authTypes)
        {
            var response = JsonSerializationUtilities.Deserialize<GetCredentialsResponse>(json);

            Assert.Equal(responseCode, response.ResponseCode);
            Assert.Equal(username, response.Username);
            Assert.Equal(password, response.Password);
            Assert.Equal(authTypes, response.AuthenticationTypes);
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("{\"Password\":\"a\",\"Username\":\"cc\"}")]
        [InlineData("{\"Password\":\"a\",\"ResponseCode\":null,\"Username\":\"c\"}")]
        [InlineData("{\"Password\":\"a\",\"ResponseCode\":\"\",\"Username\":\"c\"}")]
        [InlineData("{\"Password\":\"a\",\"ResponseCode\":\"b\",\"Username\":\"c\"}")]
        public void JsonDeserialization_ThrowsForInvalidResponseCode(string json)
        {
            Assert.Throws<JsonSerializationException>(
                () => JsonSerializationUtilities.Deserialize<GetCredentialsResponse>(json));
        }
    }
}
