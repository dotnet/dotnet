// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class MonitorNuGetProcessExitResponseTests
    {
        [Fact]
        public void Constructor_ThrowsForUndefinedResponseCode()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new MonitorNuGetProcessExitResponse((MessageResponseCode)int.MinValue));

            Assert.Equal("responseCode", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesResponseCodeProperty()
        {
            var response = new MonitorNuGetProcessExitResponse(MessageResponseCode.Success);

            Assert.Equal(MessageResponseCode.Success, response.ResponseCode);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var response = new MonitorNuGetProcessExitResponse(MessageResponseCode.Success);

            var json = TestUtilities.Serialize(response);

            Assert.Equal("{\"ResponseCode\":\"Success\"}", json);
        }

        [Theory]
        [InlineData("{\"ResponseCode\":\"Success\"}", MessageResponseCode.Success)]
        [InlineData("{\"ResponseCode\":\"NotFound\"}", MessageResponseCode.NotFound)]
        [InlineData("{\"ResponseCode\":\"Error\"}", MessageResponseCode.Error)]
        public void JsonDeserialization_ReturnsCorrectObject(string json, MessageResponseCode responseCode)
        {
            var response = JsonSerializationUtilities.Deserialize<MonitorNuGetProcessExitResponse>(json);

            Assert.Equal(responseCode, response.ResponseCode);
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("{\"ResponseCode\":null}")]
        [InlineData("{\"ResponseCode\":\"\"}")]
        [InlineData("{\"ResponseCode\":\"abc\"}")]
        public void JsonDeserialization_ThrowsForInvalidResponseCode(string json)
        {
            Assert.Throws<JsonSerializationException>(
                () => JsonSerializationUtilities.Deserialize<MonitorNuGetProcessExitResponse>(json));
        }
    }
}
