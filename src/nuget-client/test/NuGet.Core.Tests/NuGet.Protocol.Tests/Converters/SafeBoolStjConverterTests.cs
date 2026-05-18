// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentAssertions;
using Newtonsoft.Json;
using NuGet.Protocol.Converters;
using Xunit;
using StjSerializer = System.Text.Json.JsonSerializer;

namespace NuGet.Protocol.Tests.Converters
{
    public class SafeBoolStjConverterTests
    {
        private class NsjWrapper
        {
            [Newtonsoft.Json.JsonConverter(typeof(SafeBoolConverter))]
            [Newtonsoft.Json.JsonProperty("v")]
            public bool Value { get; set; }
        }

        private class StjWrapper
        {
            [System.Text.Json.Serialization.JsonConverter(typeof(SafeBoolStjConverter))]
            [System.Text.Json.Serialization.JsonPropertyName("v")]
            public bool Value { get; set; }
        }

        private static bool DeserializeWithNsj(string json)
            => JsonConvert.DeserializeObject<NsjWrapper>($"{{\"v\":{json}}}")!.Value;

        private static bool DeserializeWithStj(string json)
            => StjSerializer.Deserialize<StjWrapper>($"{{\"v\":{json}}}")!.Value;

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("null", false)]
        [InlineData("1", true)]
        [InlineData("0", false)]
        [InlineData("\"true\"", true)]
        [InlineData("\"True\"", true)]
        [InlineData("\"false\"", false)]
        [InlineData("\"invalid\"", false)]
        [InlineData("\"  true  \"", true)]
        [InlineData("\"  \"", false)]
        [InlineData("{}", false)]
        [InlineData("[]", false)]
        public void Read_ValidBoolInput_Succeeds(string json, bool expected)
        {
            // Act
            var stjResult = DeserializeWithStj(json);
            var nsjResult = DeserializeWithNsj(json);

            // Assert
            stjResult.Should().Be(expected);
            nsjResult.Should().Be(stjResult);
        }
    }
}
