// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;
using System.Text.Json;
using FluentAssertions;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Protocol.Converters;
using Xunit;

namespace NuGet.Protocol.Tests.Converters
{
    public class PackageDependencyGroupStjConverterTests
    {
        private static readonly JsonSerializerOptions Options = PackageSearchJsonContext.Default.Options;

        [Fact]
        public void Read_WithIsFinalBlockFalse_UnknownPropertyDoesNotThrow()
        {
            byte[] json = Encoding.UTF8.GetBytes(
                @"{""unknownProperty"": {""nested"": true}, ""targetFramework"": ""net8.0""}");

            var reader = new Utf8JsonReader(json, isFinalBlock: false, new JsonReaderState());
            reader.Read();

            var converter = new PackageDependencyGroupStjConverter();
            var result = converter.Read(ref reader, typeof(PackageDependencyGroup), Options);

            result.Should().NotBeNull();
            result.TargetFramework.Should().Be(NuGetFramework.Parse("net8.0"));
        }
    }
}
