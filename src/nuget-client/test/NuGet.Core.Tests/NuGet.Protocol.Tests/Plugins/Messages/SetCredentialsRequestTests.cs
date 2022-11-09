// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class SetCredentialsRequestTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageSourceRepository(string packageSourceRepository)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new SetCredentialsRequest(
                    packageSourceRepository,
                    proxyUsername: "a",
                    proxyPassword: "b",
                    username: "c",
                    password: "d"));

            Assert.Equal("packageSourceRepository", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("b")]
        public void Constructor_InitializesProperties(string argument)
        {
            var request = new SetCredentialsRequest(
                packageSourceRepository: "a",
                proxyUsername: argument,
                proxyPassword: argument,
                username: argument,
                password: argument);

            Assert.Equal("a", request.PackageSourceRepository);
            Assert.Equal(argument, request.ProxyUsername);
            Assert.Equal(argument, request.ProxyPassword);
            Assert.Equal(argument, request.Username);
            Assert.Equal(argument, request.Password);
        }

        [Theory]
        [InlineData("a", null, null, null, null, "{\"PackageSourceRepository\":\"a\"}")]
        [InlineData("a", "b", "c", "d", "e", "{\"PackageSourceRepository\":\"a\",\"Password\":\"e\",\"ProxyPassword\":\"c\",\"ProxyUsername\":\"b\",\"Username\":\"d\"}")]
        public void JsonSerialization_ReturnsCorrectJson(
            string packageSourceRepository,
            string proxyUsername,
            string proxyPassword,
            string username,
            string password,
            string expectedJson)
        {
            var request = new SetCredentialsRequest(
                packageSourceRepository,
                proxyUsername,
                proxyPassword,
                username,
                password);

            var actualJson = TestUtilities.Serialize(request);

            Assert.Equal(expectedJson, actualJson);
        }

        [Theory]
        [InlineData("{\"PackageSourceRepository\":\"a\"}", "a", null, null, null, null)]
        [InlineData("{\"PackageSourceRepository\":\"a\",\"Password\":\"b\",\"ProxyPassword\":\"c\",\"ProxyUsername\":\"d\",\"Username\":\"e\"}", "a", "d", "c", "e", "b")]
        public void JsonDeserialization_ReturnsCorrectObject(
            string json,
            string packageSourceRepository,
            string proxyUsername,
            string proxyPassword,
            string username,
            string password)
        {
            var request = JsonSerializationUtilities.Deserialize<SetCredentialsRequest>(json);

            Assert.Equal(packageSourceRepository, request.PackageSourceRepository);
            Assert.Equal(proxyUsername, request.ProxyUsername);
            Assert.Equal(proxyPassword, request.ProxyPassword);
            Assert.Equal(username, request.Username);
            Assert.Equal(password, request.Password);
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("{\"PackageSourceRepository\":null}")]
        [InlineData("{\"PackageSourceRepository\":\"\"}")]
        public void JsonDeserialization_ThrowsForInvalidPackageSourceRepository(string json)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<SetCredentialsRequest>(json));

            Assert.Equal("packageSourceRepository", exception.ParamName);
        }
    }
}
