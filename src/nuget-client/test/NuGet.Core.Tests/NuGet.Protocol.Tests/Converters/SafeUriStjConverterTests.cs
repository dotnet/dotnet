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
    public class SafeUriStjConverterTests
    {
        private class NsjWrapper
        {
            [Newtonsoft.Json.JsonConverter(typeof(SafeUriConverter))]
            [Newtonsoft.Json.JsonProperty("v")]
            public Uri? Value { get; set; }
        }

        private class StjWrapper
        {
            [System.Text.Json.Serialization.JsonConverter(typeof(SafeUriStjConverter))]
            [System.Text.Json.Serialization.JsonPropertyName("v")]
            public Uri? Value { get; set; }
        }

        private static Uri? DeserializeWithNsj(string json)
            => JsonConvert.DeserializeObject<NsjWrapper>($"{{\"v\":{json}}}")!.Value;

        private static Uri? DeserializeWithStj(string json)
            => StjSerializer.Deserialize<StjWrapper>($"{{\"v\":{json}}}")!.Value;

        [Theory]
        [InlineData("\"https://contoso.test/path\"", "https://contoso.test/path")]
        [InlineData("\"  https://contoso.test/path  \"", "https://contoso.test/path")]
        [InlineData("\"not a uri\"", null)]
        [InlineData("null", null)]
        [InlineData("42", null)]
        [InlineData("{}", null)]
        [InlineData("[]", null)]
        public void Read_ValidUriInput_Succeeds(string json, string? expectedUri)
        {
            // Arrange
            var expected = expectedUri is null ? null : new Uri(expectedUri);

            // Act
            var stjResult = DeserializeWithStj(json);
            var nsjResult = DeserializeWithNsj(json);

            // Assert
            stjResult.Should().Be(expected);
            nsjResult.Should().Be(stjResult);
        }
    }
}
