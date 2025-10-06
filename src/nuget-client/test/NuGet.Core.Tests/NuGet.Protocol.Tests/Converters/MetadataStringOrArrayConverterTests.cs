// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.IO;
using FluentAssertions;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using Xunit;

namespace NuGet.Protocol.Tests.Converters
{
    public class MetadataStringOrArrayConverterTests
    {
        [Fact]
        public void ReadJson_StringPropertyValue_CreatesSingleItemArray()
        {
            // Arrange
            string jsonProperty = "'a'";

            using var stringReader = new StringReader(jsonProperty);
            using var jsonReader = new JsonTextReader(stringReader);

            jsonReader.Read();
            var converter = new MetadataStringOrArrayConverter();

            // Act
            var result = converter.ReadJson(jsonReader, typeof(string), null, JsonSerializationUtilities.Serializer) as string[];

            // Assert
            result.Should()
                .NotBeNullOrEmpty()
                .And.HaveCount(1)
                .And.Contain("a");
        }

        [Fact]
        public void ReadJson_MultipleItemArrayPropertyValue_CreatesIdenticalArray()
        {
            // Arrange
            string jsonProperty = "['a', 'b', 'c']";

            using var stringReader = new StringReader(jsonProperty);
            using var jsonReader = new JsonTextReader(stringReader);

            jsonReader.Read();
            var converter = new MetadataStringOrArrayConverter();

            // Act
            var result = converter.ReadJson(jsonReader, typeof(string[]), null, JsonSerializationUtilities.Serializer) as string[];

            // Assert
            result.Should()
                .NotBeNullOrEmpty()
                .And.HaveCount(3)
                .And.ContainInOrder("a", "b", "c");
        }
    }
}
