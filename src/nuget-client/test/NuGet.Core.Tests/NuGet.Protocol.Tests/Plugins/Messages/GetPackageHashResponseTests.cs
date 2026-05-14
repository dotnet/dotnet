// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class GetPackageHashResponseTests
    {
        [Fact]
        public void Constructor_ThrowsForUndefinedResponseCode()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetPackageHashResponse((MessageResponseCode)int.MaxValue, hash: "a"));

            Assert.Equal("responseCode", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyHashWhenResponseCodeIsSuccess(string hash)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetPackageHashResponse(MessageResponseCode.Success, hash));

            Assert.Equal("hash", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var response = new GetPackageHashResponse(MessageResponseCode.Success, hash: "a");

            Assert.Equal(MessageResponseCode.Success, response.ResponseCode);
            Assert.Equal("a", response.Hash);
        }

        [Theory]
        [InlineData(MessageResponseCode.Success, "a", "{\"Hash\":\"a\",\"ResponseCode\":\"Success\"}")]
        [InlineData(MessageResponseCode.NotFound, null, "{\"ResponseCode\":\"NotFound\"}")]
        [InlineData(MessageResponseCode.Error, null, "{\"ResponseCode\":\"Error\"}")]
        public void JsonSerialization_ReturnsCorrectJson(
            MessageResponseCode responseCode,
            string hash,
            string expectedJson)
        {
            var response = new GetPackageHashResponse(responseCode, hash);

            var actualJson = TestUtilities.Serialize(response);

            Assert.Equal(expectedJson, actualJson);
        }

        [Theory]
        [InlineData("{\"Hash\":\"a\",\"ResponseCode\":\"Success\"}", MessageResponseCode.Success, "a")]
        [InlineData("{\"ResponseCode\":\"NotFound\"}", MessageResponseCode.NotFound, null)]
        [InlineData("{\"ResponseCode\":\"Error\"}", MessageResponseCode.Error, null)]
        public void JsonDeserialization_ReturnsCorrectObject(
            string json,
            MessageResponseCode responseCode,
            string hash)
        {
            var response = JsonSerializationUtilities.Deserialize<GetPackageHashResponse>(json);

            Assert.Equal(responseCode, response.ResponseCode);
            Assert.Equal(hash, response.Hash);
        }

        [Theory]
        [InlineData("{\"ResponseCode\":null}")]
        [InlineData("{\"ResponseCode\":\"\"}")]
        [InlineData("{\"ResponseCode\":\"b\"}")]
        public void JsonDeserialization_ThrowsForInvalidResponseCode(string json)
        {
            Assert.Throws<JsonSerializationException>(
                () => JsonSerializationUtilities.Deserialize<GetPackageHashResponse>(json));
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("{\"ResponseCode\":\"Success\"}")]
        [InlineData("{\"Hash\":null,\"ResponseCode\":\"Success\"}")]
        [InlineData("{\"Hash\":\"\",\"ResponseCode\":\"Success\"}")]
        public void JsonDeserialization_ThrowsForInvalidHash(string json)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<GetPackageHashResponse>(json));

            Assert.Equal("hash", exception.ParamName);
        }
    }
}
