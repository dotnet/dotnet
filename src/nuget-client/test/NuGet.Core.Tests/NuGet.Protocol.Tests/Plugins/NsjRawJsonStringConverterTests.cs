// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using Newtonsoft.Json;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class NsjRawJsonStringConverterTests
    {
        [Theory]
        [InlineData("{\"Value\":{\"a\":\"b\"}}", "{\"a\":\"b\"}")]
        [InlineData("{\"Value\":{}}", "{}")]
        [InlineData("{\"Value\":{\"x\":{\"y\":\"z\",\"n\":1}}}", "{\"x\":{\"y\":\"z\",\"n\":1}}")]
        [InlineData("{\"Value\":{\"a\":1,\"b\":true,\"c\":null}}", "{\"a\":1,\"b\":true,\"c\":null}")]
        [InlineData("{\"Value\":null}", null)]
        public void Read_ValidInput_ReturnsExpected(string json, string expected)
        {
            var result = JsonConvert.DeserializeObject<Wrapper>(json);

            Assert.Equal(expected, result.Value);
        }

        [Theory]
        [InlineData("{\"Value\":\"a string\"}")]
        [InlineData("{\"Value\":42}")]
        [InlineData("{\"Value\":true}")]
        [InlineData("{\"Value\":[1,2,3]}")]
        public void Read_NonObjectToken_Throws(string json)
        {
            Assert.Throws<JsonSerializationException>(() => JsonConvert.DeserializeObject<Wrapper>(json));
        }

        [Theory]
        [InlineData("{\"a\":\"b\"}", "{\"Value\":{\"a\":\"b\"}}")]
        [InlineData("{}", "{\"Value\":{}}")]
        [InlineData("{\"x\":{\"y\":\"z\"}}", "{\"Value\":{\"x\":{\"y\":\"z\"}}}")]
        [InlineData("{\"a\":1,\"b\":true}", "{\"Value\":{\"a\":1,\"b\":true}}")]
        [InlineData(null, "{\"Value\":null}")]
        public void Write_ValidInput_ProducesExpectedJson(string value, string expectedJson)
        {
            var result = JsonConvert.SerializeObject(new Wrapper { Value = value });

            Assert.Equal(expectedJson, result);
        }

        [Theory]
        [InlineData("not json")]
        [InlineData("[1,2,3]")]
        [InlineData("42")]
        [InlineData("\"a string\"")]
        public void Write_NonObjectValue_Throws(string value)
        {
            Assert.ThrowsAny<JsonException>(() => JsonConvert.SerializeObject(new Wrapper { Value = value }));
        }

        private sealed class Wrapper
        {
            [JsonConverter(typeof(NsjRawJsonStringConverter))]
            public string Value { get; set; }
        }
    }
}
