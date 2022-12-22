// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class GetServiceIndexResponseTests
    {
        [Fact]
        public void Constructor_ThrowsForUndefinedResponseCode()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetServiceIndexResponse((MessageResponseCode)int.MaxValue, JObject.Parse("{}")));

            Assert.Equal("responseCode", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullServiceIndexWhenResponseCodeIsSuccess()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new GetServiceIndexResponse(MessageResponseCode.Success, serviceIndex: null));

            Assert.Equal("serviceIndex", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var serviceIndex = JObject.Parse("{}");
            var response = new GetServiceIndexResponse(MessageResponseCode.Success, serviceIndex);

            Assert.Equal(MessageResponseCode.Success, response.ResponseCode);
            Assert.Same(serviceIndex, response.ServiceIndex);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var serviceIndex = JObject.Parse("{\"a\":\"b\"}");
            var response = new GetServiceIndexResponse(MessageResponseCode.Success, serviceIndex);

            var json = TestUtilities.Serialize(response);

            Assert.Equal("{\"ResponseCode\":\"Success\",\"ServiceIndex\":{\"a\":\"b\"}}", json);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObjectForSuccess()
        {
            var json = "{\"ResponseCode\":\"Success\",\"ServiceIndex\":{\"a\":\"b\"}}";
            var response = JsonSerializationUtilities.Deserialize<GetServiceIndexResponse>(json);

            Assert.Equal(MessageResponseCode.Success, response.ResponseCode);
            Assert.Equal("{\"a\":\"b\"}", response.ServiceIndex.ToString(Formatting.None));
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObjectForNotFound()
        {
            var json = "{\"ResponseCode\":\"NotFound\"}";
            var response = JsonSerializationUtilities.Deserialize<GetServiceIndexResponse>(json);

            Assert.Equal(MessageResponseCode.NotFound, response.ResponseCode);
            Assert.Null(response.ServiceIndex);
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
        [InlineData("{\"ResponseCode\":\"Success\",\"ServiceIndex\":\"a\"}", typeof(JsonSerializationException))]
        [InlineData("{\"ResponseCode\":\"Success\",\"ServiceIndex\":1}", typeof(JsonSerializationException))]
        public void JsonDeserialization_ThrowsForInvalidServiceIndex(string json, Type exceptionType)
        {
            var exception = Assert.Throws(
                exceptionType,
                () => JsonSerializationUtilities.Deserialize<GetServiceIndexResponse>(json));

            if (exception is ArgumentNullException)
            {
                Assert.Equal("serviceIndex", ((ArgumentNullException)exception).ParamName);
            }
        }
    }
}
