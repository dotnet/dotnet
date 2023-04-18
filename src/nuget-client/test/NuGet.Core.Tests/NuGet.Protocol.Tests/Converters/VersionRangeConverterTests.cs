// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class VersionRangeConverterTests
    {
        private static readonly VersionRangeConverter _converter = new VersionRangeConverter();
        private static readonly VersionRange _strictVersionRange = new VersionRange(
            minVersion: new NuGetVersion(1, 0, 0),
            maxVersion: new NuGetVersion(1, 0, 0),
            includeMinVersion: false);

        [Fact]
        public void CanConvert_ReturnsTrueForVersionRangeType()
        {
            var canConvert = _converter.CanConvert(typeof(VersionRange));

            Assert.True(canConvert);
        }

        [Fact]
        public void ReadJson_ReadsStrictVersionRange()
        {
            using (var stringReader = new StringReader($"\"{_strictVersionRange}\""))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                var actualVersionRange = _converter.ReadJson(
                    jsonReader,
                    typeof(VersionRange),
                    existingValue: null,
                    serializer: JsonSerializationUtilities.Serializer);

                Assert.Equal(_strictVersionRange, actualVersionRange);
            }
        }

        [Fact]
        public void ReadJson_ReadsWildCardVersionRange()
        {
            const string wildcard = "*";
            var v000 = new NuGetVersion(0, 0, 0);
            var expectedVersionRange = new VersionRange(v000, true, null, true, new FloatRange(NuGetVersionFloatBehavior.Major, v000), originalString: wildcard);
            using (var stringReader = new StringReader($"\"{wildcard}\""))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                var actualVersionRange = _converter.ReadJson(
                    jsonReader,
                    typeof(VersionRange),
                    existingValue: null,
                    serializer: JsonSerializationUtilities.Serializer);

                Assert.Equal(expectedVersionRange, actualVersionRange);
            }
        }

        [Fact]
        public void WriteJson_ThrowsForInvalidVersionRange()
        {
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                Assert.Throws<ArgumentException>(
                    () => _converter.WriteJson(jsonWriter, "**", JsonSerializationUtilities.Serializer));
            }
        }

        [Fact]
        public void WriteJson_WritesWildCardVersionRangeToString()
        {
            var versionRange = VersionRange.Parse("*");

            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                _converter.WriteJson(jsonWriter, versionRange, JsonSerializationUtilities.Serializer);

                Assert.Equal($"\"{versionRange.ToString()}\"", stringWriter.ToString());
            }
        }

        [Fact]
        public void WriteJson_WritesStrictVersionRangeToString()
        {
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                _converter.WriteJson(jsonWriter, _strictVersionRange, JsonSerializationUtilities.Serializer);

                Assert.Equal($"\"{_strictVersionRange.ToString()}\"", stringWriter.ToString());
            }
        }
    }
}
