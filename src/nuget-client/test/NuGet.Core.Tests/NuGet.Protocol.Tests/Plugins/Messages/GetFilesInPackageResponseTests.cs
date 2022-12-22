// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class GetFilesInPackageResponseTests
    {
        [Fact]
        public void Constructor_ThrowsForUndefinedResponseCode()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetFilesInPackageResponse((MessageResponseCode)int.MaxValue, new[] { "a" }));

            Assert.Equal("responseCode", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullCopiedFilesWhenResponseCodeIsSuccess()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetFilesInPackageResponse(MessageResponseCode.Success, files: null));

            Assert.Equal("files", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForEmptyCopiedFilesWhenResponseCodeIsSuccess()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetFilesInPackageResponse(MessageResponseCode.Success, new string[] { }));

            Assert.Equal("files", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var response = new GetFilesInPackageResponse(MessageResponseCode.Success, new[] { "a" });

            Assert.Equal(MessageResponseCode.Success, response.ResponseCode);
            Assert.Equal(new[] { "a" }, response.Files);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var response = new GetFilesInPackageResponse(MessageResponseCode.Success, new[] { "a" });

            var json = TestUtilities.Serialize(response);

            Assert.Equal("{\"Files\":[\"a\"],\"ResponseCode\":\"Success\"}", json);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObjectForSuccess()
        {
            var json = "{\"Files\":[\"a\"],\"ResponseCode\":\"Success\"}";
            var response = JsonSerializationUtilities.Deserialize<GetFilesInPackageResponse>(json);

            Assert.Equal(MessageResponseCode.Success, response.ResponseCode);
            Assert.Equal(new[] { "a" }, response.Files);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObjectForNotFound()
        {
            var json = "{\"ResponseCode\":\"NotFound\"}";
            var response = JsonSerializationUtilities.Deserialize<GetFilesInPackageResponse>(json);

            Assert.Equal(MessageResponseCode.NotFound, response.ResponseCode);
            Assert.Null(response.Files);
        }

        [Theory]
        [InlineData("{\"ResponseCode\":null}")]
        [InlineData("{\"ResponseCode\":\"\"}")]
        [InlineData("{\"ResponseCode\":\"b\"}")]
        public void JsonDeserialization_ThrowsForInvalidResponseCode(string json)
        {
            Assert.Throws<JsonSerializationException>(
                () => JsonSerializationUtilities.Deserialize<GetFilesInPackageResponse>(json));
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("{\"ResponseCode\":\"Success\"}")]
        [InlineData("{\"Files\":null,\"ResponseCode\":\"Success\"}")]
        [InlineData("{\"Files\":[],\"ResponseCode\":\"Success\"}")]
        public void JsonDeserialization_ThrowsForInvalidCopiedFiles(string json)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<GetFilesInPackageResponse>(json));

            Assert.Equal("files", exception.ParamName);
        }
    }
}
