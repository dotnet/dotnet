// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentAssertions;
using Newtonsoft.Json;
using NuGet.Frameworks;
using NuGet.Protocol.Converters;
using Xunit;
using StjSerializer = System.Text.Json.JsonSerializer;

namespace NuGet.Protocol.Tests.Converters
{
    public class NuGetFrameworkStjConverterTests
    {
        private class NsjWrapper
        {
            [Newtonsoft.Json.JsonConverter(typeof(NuGetFrameworkConverter))]
            [Newtonsoft.Json.JsonProperty("v")]
            public NuGetFramework? Value { get; set; }
        }

        private class StjWrapper
        {
            [System.Text.Json.Serialization.JsonConverter(typeof(NuGetFrameworkStjConverter))]
            [System.Text.Json.Serialization.JsonPropertyName("v")]
            public NuGetFramework? Value { get; set; }
        }

        private static NuGetFramework? DeserializeWithNsj(string json)
            => JsonConvert.DeserializeObject<NsjWrapper>($"{{\"v\":{json}}}")!.Value;

        private static NuGetFramework? DeserializeWithStj(string json)
            => StjSerializer.Deserialize<StjWrapper>($"{{\"v\":{json}}}")!.Value;

        [Theory]
        [InlineData("\"net8.0\"", "net8.0")]
        [InlineData("\"net472\"", "net472")]
        [InlineData("\"\"", null)]
        [InlineData("null", null)]
        [InlineData("42", "42")]
        [InlineData("true", "True")]
        public void Read_ScalarToken_DelegatesToNuGetFrameworkParse(string json, string? expectedShortName)
        {
            // Arrange
            var expected = expectedShortName is null ? NuGetFramework.AnyFramework : NuGetFramework.Parse(expectedShortName);

            // Act
            var stjResult = DeserializeWithStj(json);
            var nsjResult = DeserializeWithNsj(json);

            // Assert
            stjResult.Should().Be(expected);
            nsjResult.Should().Be(stjResult);
        }

        [Theory]
        [InlineData("[\"net8.0\"]")]
        [InlineData("{\"framework\":\"net8.0\"}")]
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
