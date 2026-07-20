// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class GetServiceIndexResponseTests
    {
        [Fact]
        public void Constructor_ThrowsForUndefinedResponseCode()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetServiceIndexResponse((MessageResponseCode)int.MaxValue, "{}"));

            Assert.Equal("responseCode", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullServiceIndexWhenResponseCodeIsSuccess()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new GetServiceIndexResponse(MessageResponseCode.Success, serviceIndexJson: (string?)null!));

            Assert.Equal("serviceIndexJson", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var serviceIndexJson = "{\"a\":\"b\"}";
            var response = new GetServiceIndexResponse(MessageResponseCode.Success, serviceIndexJson);

            Assert.Equal(MessageResponseCode.Success, response.ResponseCode);
            Assert.Equal(serviceIndexJson, response.ServiceIndexJson);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var response = new GetServiceIndexResponse(MessageResponseCode.Success, "{\"a\":\"b\"}");

            var json = TestUtilities.Serialize(response);

            Assert.Equal("{\"ResponseCode\":\"Success\",\"ServiceIndex\":{\"a\":\"b\"}}", json);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObjectForSuccess()
        {
            var json = "{\"ResponseCode\":\"Success\",\"ServiceIndex\":{\"a\":\"b\"}}";
            var response = JsonSerializationUtilities.Deserialize<GetServiceIndexResponse>(json)!;

            Assert.Equal(MessageResponseCode.Success, response.ResponseCode);
            Assert.Equal("{\"a\":\"b\"}", response.ServiceIndexJson);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObjectForNotFound()
        {
            var json = "{\"ResponseCode\":\"NotFound\"}";
            var response = JsonSerializationUtilities.Deserialize<GetServiceIndexResponse>(json)!;

            Assert.Equal(MessageResponseCode.NotFound, response.ResponseCode);
            Assert.Null(response.ServiceIndexJson);
        }

        [Theory]
        [InlineData("{\"ResponseCode\":null}")]
        [InlineData("{\"ResponseCode\":\"\"}")]
        [InlineData("{\"ResponseCode\":\"b\"}")]
        public void JsonDeserialization_ThrowsForInvalidResponseCode(string json)
        {
            Assert.Throws<JsonSerializationException>(
                () => JsonSerializationUtilities.Deserialize<GetServiceIndexResponse>(json));
        }

        [Theory]
        [InlineData("{}", typeof(ArgumentNullException))]
        [InlineData("{\"ResponseCode\":\"Success\"}", typeof(ArgumentNullException))]
        [InlineData("{\"ResponseCode\":\"Success\",\"ServiceIndex\":null}", typeof(ArgumentNullException))]
        public void JsonDeserialization_ThrowsForInvalidServiceIndex(string json, Type exceptionType)
        {
            var exception = Assert.Throws(
                exceptionType,
                () => JsonSerializationUtilities.Deserialize<GetServiceIndexResponse>(json));

            if (exception is ArgumentNullException)
            {
                Assert.Equal("serviceIndexJson", ((ArgumentNullException)exception).ParamName);
            }
        }
    }
}
