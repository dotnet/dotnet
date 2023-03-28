// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Xunit;

namespace Microsoft.Deployment.DotNet.Releases.Tests
{
    public class ReleaseTypeConverterTests
    {
        public class TestObject
        {
            public ReleaseType ReleaseType
            {
                get;
                set;
            }
        }

        [Theory]
        [InlineData("LTs", ReleaseType.LTS)]
        [InlineData("sts", ReleaseType.STS)]
        public void ItIsCaseInsenitive(string releaseTypeValue, ReleaseType expectedReleaseType)
        {
            var json = $@"{{""ReleaseType"":""{releaseTypeValue}""}}";
            var testObject = JsonSerializer.Deserialize<TestObject>(json, SerializerOptions.Default);

            Assert.Equal(expectedReleaseType, testObject.ReleaseType);
        }

        [Theory]
        [InlineData("X63")]
        [InlineData("")]
        [InlineData(null)]
        public void ItReturnsUnknownIfParsingFails(string releaseTypeValue)
        {
            var json = $@"{{""ReleaseType"":""{releaseTypeValue}""}}";
            var testObject = JsonSerializer.Deserialize<TestObject>(json, SerializerOptions.Default);

            Assert.Equal(ReleaseType.Unknown, testObject.ReleaseType);
        }
    }
}
