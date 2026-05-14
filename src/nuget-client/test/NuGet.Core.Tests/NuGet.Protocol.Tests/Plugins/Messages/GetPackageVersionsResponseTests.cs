// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class GetPackageVersionsResponseTests
    {
        [Fact]
        public void Constructor_ThrowsForUndefinedResponseCode()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetPackageVersionsResponse((MessageResponseCode)int.MaxValue, new[] { "a" }));

            Assert.Equal("responseCode", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullVersionsWhenResponseCodeIsSuccess()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetPackageVersionsResponse(MessageResponseCode.Success, versions: null));

            Assert.Equal("versions", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForEmptyVersionsWhenResponseCodeIsSuccess()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetPackageVersionsResponse(MessageResponseCode.Success, new string[] { }));

            Assert.Equal("versions", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var response = new GetPackageVersionsResponse(MessageResponseCode.Success, new[] { "a" });

            Assert.Equal(MessageResponseCode.Success, response.ResponseCode);
            Assert.Equal(new[] { "a" }, response.Versions);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var response = new GetPackageVersionsResponse(MessageResponseCode.Success, new[] { "a" });

            var json = TestUtilities.Serialize(response);

            Assert.Equal("{\"ResponseCode\":\"Success\",\"Versions\":[\"a\"]}", json);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObjectForSuccess()
        {
            var json = "{\"ResponseCode\":\"Success\",\"Versions\":[\"a\"]}";
            var response = JsonSerializationUtilities.Deserialize<GetPackageVersionsResponse>(json);

            Assert.Equal(MessageResponseCode.Success, response.ResponseCode);
            Assert.Equal(new[] { "a" }, response.Versions);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObjectForNotFound()
        {
            var json = "{\"ResponseCode\":\"NotFound\"}";
            var response = JsonSerializationUtilities.Deserialize<GetPackageVersionsResponse>(json);

            Assert.Equal(MessageResponseCode.NotFound, response.ResponseCode);
            Assert.Null(response.Versions);
        }

        [Theory]
        [InlineData("{\"ResponseCode\":null}")]
        [InlineData("{\"ResponseCode\":\"\"}")]
        [InlineData("{\"ResponseCode\":\"b\"}")]
        public void JsonDeserialization_ThrowsForInvalidResponseCode(string json)
        {
            Assert.Throws<JsonSerializationException>(
                () => JsonSerializationUtilities.Deserialize<GetPackageVersionsResponse>(json));
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("{\"ResponseCode\":\"Success\"}")]
        [InlineData("{\"ResponseCode\":\"Success\",\"Versions\":null}")]
        [InlineData("{\"ResponseCode\":\"Success\",\"Versions\":[]}")]
        public void JsonDeserialization_ThrowsForInvalidVersions(string json)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<GetPackageVersionsResponse>(json));

            Assert.Equal("versions", exception.ParamName);
        }
    }
}
