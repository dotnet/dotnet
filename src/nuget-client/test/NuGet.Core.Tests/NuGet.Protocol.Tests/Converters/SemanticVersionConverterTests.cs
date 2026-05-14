// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using Xunit;

namespace NuGet.Protocol.Tests
{
    using SemanticVersion = Versioning.SemanticVersion;

    public class SemanticVersionConverterTests
    {
        private static readonly SemanticVersionConverter _converter = new SemanticVersionConverter();
        private static readonly SemanticVersion _version = new SemanticVersion(major: 1, minor: 2, patch: 3, releaseLabel: "a", metadata: "b");

        [Fact]
        public void CanConvert_ReturnsTrueForSemanticVersionType()
        {
            var canConvert = _converter.CanConvert(typeof(SemanticVersion));

            Assert.True(canConvert);
        }

        [Fact]
        public void CanConvert_ReturnsFalseForNonSemanticVersionType()
        {
            var canConvert = _converter.CanConvert(typeof(DateTime));

            Assert.False(canConvert);
        }

        [Fact]
        public void ReadJson_ThrowsForInvalidVersion()
        {
            using (var stringReader = new StringReader("1.2"))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                var exception = Assert.Throws<ArgumentException>(() => _converter.ReadJson(
                    jsonReader,
                    typeof(SemanticVersion),
                    existingValue: null,
                    serializer: JsonSerializationUtilities.Serializer));

                Assert.Equal("value", exception.ParamName);
            }
        }

        [Fact]
        public void ReadJson_ReadsSemanticVersion()
        {
            using (var stringReader = new StringReader($"\"{_version.ToString()}\""))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                var actualVersion = _converter.ReadJson(
                    jsonReader,
                    typeof(SemanticVersion),
                    existingValue: null,
                    serializer: JsonSerializationUtilities.Serializer);

                Assert.Equal(_version, actualVersion);
            }
        }

        [Fact]
        public void WriteJson_ThrowsForInvalidVersion()
        {
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                Assert.Throws<InvalidCastException>(
                    () => _converter.WriteJson(jsonWriter, "1.2", JsonSerializationUtilities.Serializer));
            }
        }

        [Fact]
        public void WriteJson_WritesSemanticVersionToString()
        {
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                _converter.WriteJson(jsonWriter, _version, JsonSerializationUtilities.Serializer);

                Assert.Equal($"\"{_version.ToString()}\"", stringWriter.ToString());
            }
        }
    }
}
