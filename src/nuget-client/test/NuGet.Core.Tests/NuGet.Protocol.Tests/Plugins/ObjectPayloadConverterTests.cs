// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class ObjectPayloadConverterTests
    {
        [Theory]
        [InlineData("{\"Value\":{\"a\":1}}")]
        [InlineData("{\"Value\":{\"b\":\"c\"}}")]
        public void ReadJson_ReturnsJObjectForObjectToken(string json)
        {
            var result = JsonConvert.DeserializeObject<Wrapper>(json);

            Assert.IsType<JObject>(result.Value);
        }

        [Theory]
        [InlineData("{\"Value\":\"not an object\"}")]
        [InlineData("{\"Value\":42}")]
        [InlineData("{\"Value\":true}")]
        [InlineData("{\"Value\":[1,2,3]}")]
        public void ReadJson_ThrowsForNonObjectToken(string json)
        {
            Assert.Throws<JsonSerializationException>(() => JsonConvert.DeserializeObject<Wrapper>(json));
        }

        private sealed class Wrapper
        {
            [JsonConverter(typeof(ObjectPayloadConverter))]
            public object Value { get; set; }
        }
    }
}

