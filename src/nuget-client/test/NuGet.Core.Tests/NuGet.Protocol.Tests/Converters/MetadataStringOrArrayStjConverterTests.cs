// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using NuGet.Protocol.Converters;
using Xunit;
using StjSerializer = System.Text.Json.JsonSerializer;

namespace NuGet.Protocol.Tests.Converters
{
    public class MetadataStringOrArrayStjConverterTests
    {
        private class NsjWrapper
        {
            [Newtonsoft.Json.JsonConverter(typeof(MetadataStringOrArrayConverter))]
            [Newtonsoft.Json.JsonProperty("v")]
            public IReadOnlyList<string>? Value { get; set; }
        }

        private class StjWrapper
        {
            [System.Text.Json.Serialization.JsonConverter(typeof(MetadataStringOrArrayStjConverter))]
            [System.Text.Json.Serialization.JsonPropertyName("v")]
            public IReadOnlyList<string>? Value { get; set; }
        }

        private static IReadOnlyList<string>? DeserializeWithNsj(string json)
            => JsonConvert.DeserializeObject<NsjWrapper>($"{{\"v\":{json}}}")!.Value;

        private static IReadOnlyList<string>? DeserializeWithStj(string json)
            => StjSerializer.Deserialize<StjWrapper>($"{{\"v\":{json}}}")!.Value;

        [Theory]
        [InlineData("\"\"", null)]
        [InlineData("\"   \"", null)]
        [InlineData("\"Alice\"", new[] { "Alice" })]
        [InlineData("[]", new string[0])]
        [InlineData("[\"Alice\"]", new[] { "Alice" })]
        [InlineData("[\"Alice\",\"Bob\",\"Charlie\"]", new[] { "Alice", "Bob", "Charlie" })]
        [InlineData("[\"Alice\",\"\",\"Bob\"]", new[] { "Alice", "", "Bob" })]
        public void Read_ValidStringOrArray_Succeeds(string json, string[]? expected)
        {
            // Act
            var stjResult = DeserializeWithStj(json);
            var nsjResult = DeserializeWithNsj(json);

            // Assert
            stjResult.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
            nsjResult.Should().BeEquivalentTo(stjResult, o => o.WithStrictOrdering());
        }

        [Theory]
        [InlineData("null")]
        [InlineData("[\"Alice\",[],\"Bob\"]")]
        [InlineData("[\"Alice\",{},\"Bob\"]")]
        public void Read_NonConvertibleArrayElement_Throws(string json)
        {
            // Act
            Action stjAction = () => DeserializeWithStj(json);
            Action nsjAction = () => DeserializeWithNsj(json);

            // Assert
            stjAction.Should().Throw<System.Text.Json.JsonException>();
            nsjAction.Should().Throw<Newtonsoft.Json.JsonException>();
        }
    }
}
