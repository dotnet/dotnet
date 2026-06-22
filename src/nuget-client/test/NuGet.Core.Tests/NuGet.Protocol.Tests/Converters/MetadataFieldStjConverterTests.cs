// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using FluentAssertions;
using Newtonsoft.Json;
using NuGet.Protocol.Converters;
using Xunit;
using StjSerializer = System.Text.Json.JsonSerializer;

namespace NuGet.Protocol.Tests.Converters
{
    public class MetadataFieldStjConverterTests
    {
        private class NsjWrapper
        {
            [Newtonsoft.Json.JsonConverter(typeof(MetadataFieldConverter))]
            [Newtonsoft.Json.JsonProperty("v")]
            public string? Value { get; set; }
        }

        private class StjWrapper
        {
            [System.Text.Json.Serialization.JsonConverter(typeof(MetadataFieldStjConverter))]
            [System.Text.Json.Serialization.JsonPropertyName("v")]
            public string? Value { get; set; }
        }

        private static string? DeserializeWithNsj(string json)
            => JsonConvert.DeserializeObject<NsjWrapper>($"{{\"v\":{json}}}")!.Value;

        private static string? DeserializeWithStj(string json)
            => StjSerializer.Deserialize<StjWrapper>($"{{\"v\":{json}}}")!.Value;

        [Theory]
        [InlineData("null", "")]
        [InlineData("\"Alice\"", "Alice")]
        [InlineData("\"\"", "")]
        [InlineData("\"  \"", "  ")]
        [InlineData("\"Alice, Bob\"", "Alice, Bob")]
        [InlineData("[]", "")]
        [InlineData("[\"Alice\"]", "Alice")]
        [InlineData("[\"Alice\",\"Bob\",\"Charlie\"]", "Alice, Bob, Charlie")]
        [InlineData("[\"Alice\",\"\",\"Bob\"]", "Alice, Bob")]
        [InlineData("[\"Alice\",\"  \",\"Bob\"]", "Alice, Bob")]
        [InlineData("[\"Alice\",null,\"Bob\"]", "Alice, Bob")]
        [InlineData("[null,null]", "")]
        [InlineData("[\"  \",\"\"]", "")]
        public void Read_ValidStringOrArray_Succeeds(string json, string expected)
        {
            // Act
            var stjResult = DeserializeWithStj(json);
            var nsjResult = DeserializeWithNsj(json);

            // Assert
            stjResult.Should().Be(expected);
            nsjResult.Should().Be(stjResult, "STJ and NSJ must produce identical output");
        }

        [Theory]
        [InlineData("[\"Alice\",[\"nested\"],\"Bob\"]")]
        [InlineData("[\"Alice\",{},\"Bob\"]")]
        public void Read_NonConvertibleArrayElement_Throws(string json)
        {
            // Act
            Action nsjAction = () => DeserializeWithNsj(json);
            Action stjAction = () => DeserializeWithStj(json);

            // Assert
            nsjAction.Should().Throw<Exception>();
            stjAction.Should().Throw<System.Text.Json.JsonException>();
        }
    }
}

