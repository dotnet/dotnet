// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.Json;
using FluentAssertions;
using NuGet.Protocol.Converters;
using Xunit;

namespace NuGet.Protocol.Tests.Converters
{
    public class ServiceIndexEntryStringOrArrayConverterTests
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            Converters = { new ServiceIndexEntryStringOrArrayConverter() }
        };

        [Theory]
        [InlineData("\"foo\"", "\"foo\"")]
        [InlineData("[\"foo\",\"bar\"]", "[\"foo\",\"bar\"]")]
        [InlineData("[]", "[]")]
        [InlineData("[\"foo\",42,true,\"bar\"]", "[\"foo\",\"bar\"]")]
        [InlineData("42", "[]")]
        public void ReadThenWrite_ProducesExpectedOutput(string input, string expectedOutput)
        {
            var value = JsonSerializer.Deserialize<string[]>(input, Options);
            var result = JsonSerializer.Serialize(value, Options);

            result.Should().Be(expectedOutput);
        }
    }
}
