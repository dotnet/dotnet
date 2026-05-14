// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class CopyFilesInPackageResponseTests
    {
        [Fact]
        public void Constructor_ThrowsForUndefinedResponseCode()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new CopyFilesInPackageResponse((MessageResponseCode)int.MaxValue, new[] { "a" }));

            Assert.Equal("responseCode", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullCopiedFilesWhenResponseCodeIsSuccess()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new CopyFilesInPackageResponse(MessageResponseCode.Success, copiedFiles: null));

            Assert.Equal("copiedFiles", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForEmptyCopiedFilesWhenResponseCodeIsSuccess()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new CopyFilesInPackageResponse(MessageResponseCode.Success, new string[] { }));

            Assert.Equal("copiedFiles", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var response = new CopyFilesInPackageResponse(MessageResponseCode.Success, new[] { "a" });

            Assert.Equal(MessageResponseCode.Success, response.ResponseCode);
            Assert.Equal(new[] { "a" }, response.CopiedFiles);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var response = new CopyFilesInPackageResponse(MessageResponseCode.Success, new[] { "a" });

            var json = TestUtilities.Serialize(response);

            Assert.Equal("{\"CopiedFiles\":[\"a\"],\"ResponseCode\":\"Success\"}", json);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObjectForSuccess()
        {
            var json = "{\"CopiedFiles\":[\"a\"],\"ResponseCode\":\"Success\"}";
            var response = JsonSerializationUtilities.Deserialize<CopyFilesInPackageResponse>(json);

            Assert.Equal(MessageResponseCode.Success, response.ResponseCode);
            Assert.Equal(new[] { "a" }, response.CopiedFiles);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObjectForNotFound()
        {
            var json = "{\"ResponseCode\":\"NotFound\"}";
            var response = JsonSerializationUtilities.Deserialize<CopyFilesInPackageResponse>(json);

            Assert.Equal(MessageResponseCode.NotFound, response.ResponseCode);
            Assert.Null(response.CopiedFiles);
        }

        [Theory]
        [InlineData("{\"ResponseCode\":null}")]
        [InlineData("{\"ResponseCode\":\"\"}")]
        [InlineData("{\"ResponseCode\":\"b\"}")]
        public void JsonDeserialization_ThrowsForInvalidResponseCode(string json)
        {
            Assert.Throws<JsonSerializationException>(
                () => JsonSerializationUtilities.Deserialize<CopyFilesInPackageResponse>(json));
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("{\"ResponseCode\":\"Success\"}")]
        [InlineData("{\"CopiedFiles\":null,\"ResponseCode\":\"Success\"}")]
        [InlineData("{\"CopiedFiles\":[],\"ResponseCode\":\"Success\"}")]
        public void JsonDeserialization_ThrowsForInvalidCopiedFiles(string json)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<CopyFilesInPackageResponse>(json));

            Assert.Equal("copiedFiles", exception.ParamName);
        }
    }
}
