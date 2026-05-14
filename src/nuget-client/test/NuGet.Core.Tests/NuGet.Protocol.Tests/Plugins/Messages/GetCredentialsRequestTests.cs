// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;
using Newtonsoft.Json;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class GetCredentialsRequestTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageSourceRepository(string packageSourceRepository)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetCredentialsRequest(
                    packageSourceRepository,
                    HttpStatusCode.Unauthorized));

            Assert.Equal("packageSourceRepository", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForUndefinedStatusCode()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetCredentialsRequest(
                    packageSourceRepository: "a",
                    statusCode: (HttpStatusCode)int.MinValue));

            Assert.Equal("statusCode", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var request = new GetCredentialsRequest(
                packageSourceRepository: "a",
                statusCode: HttpStatusCode.Unauthorized);

            Assert.Equal("a", request.PackageSourceRepository);
            Assert.Equal(HttpStatusCode.Unauthorized, request.StatusCode);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var request = new GetCredentialsRequest(
                packageSourceRepository: "a",
                statusCode: HttpStatusCode.Unauthorized);

            var json = TestUtilities.Serialize(request);

            Assert.Equal("{\"PackageSourceRepository\":\"a\",\"StatusCode\":\"Unauthorized\"}", json);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObject()
        {
            var json = "{\"PackageSourceRepository\":\"a\",\"StatusCode\":\"Unauthorized\"}";
            var request = JsonSerializationUtilities.Deserialize<GetCredentialsRequest>(json);

            Assert.Equal("a", request.PackageSourceRepository);
            Assert.Equal(HttpStatusCode.Unauthorized, request.StatusCode);
        }

        [Theory]
        [InlineData("{\"StatusCode\":\"Unauthorized\"}")]
        [InlineData("{\"PackageSourceRepository\":null,\"StatusCode\":\"Unauthorized\"}")]
        [InlineData("{\"PackageSourceRepository\":\"\",\"StatusCode\":\"Unauthorized\"}")]
        public void JsonDeserialization_ThrowsForInvalidPackageSourceRepository(string json)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<GetCredentialsRequest>(json));

            Assert.Equal("packageSourceRepository", exception.ParamName);
        }

        [Fact]
        public void JsonDeserialization_ThrowsForMissingStatusCode()
        {
            var json = "{\"PackageSourceRepository\":\"a\"}";
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<GetCredentialsRequest>(json));

            Assert.Equal("statusCode", exception.ParamName);
        }

        [Theory]
        [InlineData("{\"PackageSourceRepository\":\"a\",\"StatusCode\":null}")]
        [InlineData("{\"PackageSourceRepository\":\"a\",\"StatusCode\":\"\"}")]
        [InlineData("{\"PackageSourceRepository\":\"a\",\"StatusCode\":\"b\"}")]
        public void JsonDeserialization_ThrowsForInvalidStatusCodeValue(string json)
        {
            Assert.Throws<JsonSerializationException>(
                () => JsonSerializationUtilities.Deserialize<GetCredentialsRequest>(json));
        }
    }
}
