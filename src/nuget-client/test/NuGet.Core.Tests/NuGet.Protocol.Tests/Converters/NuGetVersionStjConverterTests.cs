// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentAssertions;
using Newtonsoft.Json;
using NuGet.Protocol.Converters;
using NuGet.Versioning;
using Xunit;
using StjSerializer = System.Text.Json.JsonSerializer;

namespace NuGet.Protocol.Tests.Converters
{
    public class NuGetVersionStjConverterTests
    {
        private class NsjWrapper
        {
            [Newtonsoft.Json.JsonConverter(typeof(NuGetVersionConverter))]
            [Newtonsoft.Json.JsonProperty("v")]
            public NuGetVersion? Value { get; set; }
        }

        private class StjWrapper
        {
            [System.Text.Json.Serialization.JsonConverter(typeof(NuGetVersionStjConverter))]
            [System.Text.Json.Serialization.JsonPropertyName("v")]
            public NuGetVersion? Value { get; set; }
        }

        private static NuGetVersion? DeserializeWithNsj(string json)
            => JsonConvert.DeserializeObject<NsjWrapper>($"{{\"v\":{json}}}")!.Value;

        private static NuGetVersion? DeserializeWithStj(string json)
            => StjSerializer.Deserialize<StjWrapper>($"{{\"v\":{json}}}")!.Value;

        [Theory]
        [InlineData("\"1.2.3\"", "1.2.3")]
        [InlineData("\"1.0.0-beta.1\"", "1.0.0-beta.1")]
        [InlineData("null", null)]
        [InlineData("42", "42.0.0")]
        public void Read_ValidVersionString_Succeeds(string json, string? expectedVersion)
        {
            // Arrange
            var expected = expectedVersion is null ? null : NuGetVersion.Parse(expectedVersion);

            // Act
            var stjResult = DeserializeWithStj(json);
            var nsjResult = DeserializeWithNsj(json);

            // Assert
            stjResult.Should().BeEquivalentTo(expected);
            nsjResult.Should().BeEquivalentTo(stjResult);
        }

        [Theory]
        [InlineData("true")]
        [InlineData("[\"1.0.0\"]")]
        [InlineData("{\"version\":\"1.0.0\"}")]
        public void Read_InvalidToken_Throws(string json)
        {
            // Act
            var stjAct = () => DeserializeWithStj(json);
            var nsjAct = () => DeserializeWithNsj(json);

            // Assert
            stjAct.Should().Throw<System.Text.Json.JsonException>();
            nsjAct.Should().Throw<System.Exception>();
        }
    }
}
